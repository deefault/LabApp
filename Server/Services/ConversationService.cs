using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Services
{
	public class ConversationService
	{
		private readonly AppDbContext _db;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		public ConversationService(AppDbContext db, IMapper mapper, IUserService userService)
		{
			_db = db;
			_mapper = mapper;
			_userService = userService;
		}

		public async Task<Conversation> GetOrCreateAsync(int? assignmentId, int otherUserId, ConversationType type)
		{
			var conversation = _db.Conversations
				.Include(x => x.Users)
					.ThenInclude(x => x.User)
				.FirstOrDefault(x => x.Type == type &&
				                     (assignmentId == null || x.AssignmentId == assignmentId.Value) &&
				                     x.Users.Select(y => y.UserId).Contains(_userService.UserId) &&
				                     x.Users.Select(y => y.UserId).Contains(otherUserId)
				);
			if (conversation == null)
			{
				var created = DateTime.UtcNow;
				conversation = _db.Conversations.Add(new Conversation
				{
					Type = type, Inserted = created, AssignmentId = assignmentId
				}).Entity;
				_db.UserConversation.Add(new UserConversation {Conversation = conversation, UserId = otherUserId});
				_db.UserConversation.Add(new UserConversation
					{Conversation = conversation, UserId = _userService.UserId});
				await _db.SaveChangesAsync();
			}

			return conversation;
		}

		public Message AddMessage(int conversationId, Message message)
		{
			message.ConversationId = conversationId;
			message.Sent = DateTime.UtcNow;
			message.UserId = _userService.UserId;

			return _db.Add(message).Entity;
		}

		public async Task<IEnumerable<Message>> GetMessages(int conversationId, Paging paging = default)
		{
			return (await _db.Messages.Where(x => x.ConversationId == conversationId).OrderByDescending(x => x.Sent)
				.ToPagedListAsync(paging));
		}

		public void ReadMessage(UserConversation userConversation, Message message)
		{
			userConversation.LastReadMessage = message.Sent;
		}

		public async Task<int> CountNewAsync(UserConversation conversation)
		{
			return await CountNew(conversation).CountAsync();
		}
		
		private IQueryable<Message> CountNew(UserConversation conversation)
		{
			return _db.Messages.Where(x => x.ConversationId == conversation.ConversationId &&
			                                          x.Sent > conversation.LastReadMessage &&
			                                          x.UserId != _userService.UserId);
		}

		public Conversation GetById(int id)
		{
			return _db.Conversations
				.Include(x => x.Users)
					.ThenInclude(x => x.User)
				.FirstOrDefault(x => x.Id == id);
		}

		public dynamic ListAsync(int userId)
		{
			return _db.UserConversation.Where(x => x.UserId == userId)
				.Select(x => new
				{
					Conversation = x.Conversation,
					LastMessage = _db.Messages.Where(y => y.ConversationId == x.ConversationId)
						.OrderByDescending(y => y.Sent).FirstOrDefault(),
					LastReadMessageDate = x.LastReadMessage,
					CountNew = CountNew(x).Count()
				})
				.ToList();
		}
	}
}
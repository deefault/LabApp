using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers
{
	public class ChatController : BaseCommonController
	{
		private readonly AppDbContext _db;
		private readonly ConversationService _conversationService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ChatController(AppDbContext db, ConversationService conversationService, IUserService userService, IMapper mapper)
		{
			_db = db;
			_conversationService = conversationService;
			_userService = userService;
			_mapper = mapper;
		}

		[HttpPost("{id}")]
		[ProducesResponseType(typeof(MessageDto), 200)]
		public async Task<IActionResult> AddMessage(int id, MessageDto message)
		{
			// TODO: attachments
			Conversation conversation = _conversationService.GetById(id);
			if (conversation == null) return NotFound();
			UserConversation userConv = conversation.Users.FirstOrDefault(x => x.UserId == _userService.UserId);
			if (userConv == null) return Forbid();
			
			Message messageResult = _conversationService.AddMessage(id, new Message { Text = message.Text});
			userConv.LastReadMessage = messageResult.Sent;
			await _db.SaveChangesAsync();

			return Ok(_mapper.Map<MessageDto>(messageResult));
		}
		
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ConversationDto), 200)]
		public IActionResult GetConversation(int id)
		{
			Conversation conversation = _conversationService.GetById(id);
			if (conversation == null) return NotFound();
			if (conversation.Users.All(x => x.UserId != _userService.UserId)) return Forbid();
			
			return Ok(_mapper.Map<ConversationDto>(conversation));
		}
		
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<ConversationDto>), 200)]
		public IActionResult GetConversations()
		{
			var conversation = _conversationService.ListAsync(_userService.UserId);
			// TODO
			return Ok(_mapper.Map<ConversationDto>(conversation));
		}
		
		[HttpGet("{id}/messages")]
		[ProducesResponseType(typeof(IEnumerable<MessageDto>), 200)]
		public async Task<IActionResult> GetMessages(int id)
		{
			Conversation conversation = _conversationService.GetById(id);
			if (conversation == null) return NotFound();
			if (conversation.Users.All(x => x.UserId != _userService.UserId)) return Forbid();

			IEnumerable<Message> messages = await _conversationService.GetMessages(id);

			return Ok(_mapper.Map<IEnumerable<MessageDto>>(messages.Reverse()));
		}
		
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Data.QueryModels;
using LabApp.Server.Hubs;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using LabApp.Shared.Enums;
using LabApp.Shared.EventBus.Events;
using LabApp.Shared.EventBus.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Services
{
    public class ConversationService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IHubContext<CommonHub, ICommonHubClient> _hub;

        public ConversationService(AppDbContext db, IMapper mapper, IUserService userService, IEventBus eventBus,
            IHubContext<CommonHub, ICommonHubClient> hub)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _eventBus = eventBus;
            _hub = hub;
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

        public async Task<Message> AddMessageAsync(UserConversation conversation, Message message)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            message.ConversationId = conversation.ConversationId;
            message.Sent = DateTime.UtcNow;
            message.UserId = _userService.UserId;
            conversation.LastReadMessage = message.Sent;
            _db.Add(message);
            await _db.SaveChangesAsync();

            _eventBus.Publish(new NewMessageEvent
            {
                MessageId = message.Id, Users = GetAllOtherUsers(conversation), UserId = _userService.UserId,
                ConversationId = conversation.ConversationId, Text = message.Text
            });

            await transaction.CommitAsync();

            // foreach (int userId in GetAllOtherUsers(conversation)) TODO: DELETE
            // {
            //     _hub.Clients.Client(userId.ToString())?.NewMessage(message.UserId, message.Id, message.Text,
            //         conversation.ConversationId, conversation.User.FullName);
            // }

            return message;
        }

        public async Task<IEnumerable<Message>> GetMessages(int conversationId, Paging paging = default)
        {
            return (await _db.Messages.Where(x => x.ConversationId == conversationId).OrderByDescending(x => x.Sent)
                .ToPagedListAsync(paging));
        }

        public IEnumerable<int> GetAllOtherUsers(UserConversation conversation)
        {
            return conversation.Conversation.Users.Where(x => x.UserId != conversation.UserId)
                .Select(x => x.UserId).ToList();
        }

        public void ReadMessage(UserConversation userConversation, Message message)
        {
            userConversation.LastReadMessage = message.Sent;
        }

        public async Task<int> ReadAllMessagesAsync(UserConversation userConversation)
        {
            var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);
            var count = await CountNewAsync(userConversation);
            userConversation.LastReadMessage = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return count;
        }

        public async Task<int> CountNewAsync(UserConversation conversation)
        {
            return await CountNew(conversation).CountAsync();
        }

        public async Task<int> CountNewAsync()
        {
            var userId = _userService.UserId;

            return await _db.UserConversation.Where(x => x.UserId == userId)
                .SelectMany(x => x.Conversation.Messages,
                    (c, m) => new {LastRead = c.LastReadMessage, Sent = m.Sent})
                .Where(x => x.Sent > x.LastRead).CountAsync();
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
                .Include(x => x.Users.Where(y => y.UserId == _userService.UserId))
                .ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Id == id);
        }
        
        public async Task<IEnumerable<ConversationWithLastMessage>> ListAsync(int userId)
        {
            return await _db.UserConversation.Where(x => x.UserId == userId)
                .Select(x => new
                {
                    UserConversation = x,
                    LastMessaage = x.Conversation.Messages.OrderByDescending(y => y.Sent).FirstOrDefault()
                })
                .Select(x => new ConversationWithLastMessage
                {
                    Id = x.UserConversation.Conversation.Id,
                    Name = x.UserConversation.Conversation.Name,
                    NewMessages = x.UserConversation.Conversation.Messages.Count(m =>
                        m.UserId != _userService.UserId && m.Sent > x.UserConversation.LastReadMessage),
                    LastMessage = x.LastMessaage,
                    LastMessageUser = x.LastMessaage.User,
                    Type = x.UserConversation.Conversation.Type,
                })
                .ToListAsync();
        }
        
        public UserConversation GetUserConversation(int id, bool includeUsers = false)
        {
            var result = _db.UserConversation
                .Include(x => x.Conversation);

            if (includeUsers) result.ThenInclude(x => x.Users);

            return result.FirstOrDefault(x => x.ConversationId == id && x.UserId == _userService.UserId);
        }
    }
}
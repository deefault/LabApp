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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabApp.Server.Controllers
{
	[Authorize]
	public class ChatController : BaseCommonController
	{
		private readonly ConversationService _conversationService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ChatController(ConversationService conversationService, IUserService userService, IMapper mapper)
		{
			_conversationService = conversationService;
			_userService = userService;
			_mapper = mapper;
		}

		[HttpPost("{id}")]
		[ProducesResponseType(typeof(MessageDto), 200)]
		public async Task<IActionResult> AddMessage(int id, MessageDto message)
		{
			UserConversation conversation = _conversationService.GetUserConversation(id);
			if (conversation == null) return Forbid();
			
			Message messageResult = await _conversationService.AddMessageAsync(conversation, new Message { Text = message.Text});

			return Ok(_mapper.Map<MessageDto>(messageResult));
		}
		
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ConversationDto), 200)]
		public IActionResult GetConversation(int id)
		{
			UserConversation conversation = _conversationService.GetUserConversation(id);
			if (conversation == null) return Forbid();

			var result = _mapper.Map<ConversationDto>(conversation.Conversation);
			result.LastReadMessage = conversation.LastReadMessage;

			return Ok(result);
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
			UserConversation conversation = _conversationService.GetUserConversation(id);
			if (conversation == null) return Forbid();

			IEnumerable<Message> messages = await _conversationService.GetMessages(id);

			return Ok(_mapper.Map<IEnumerable<MessageDto>>(messages.Reverse()));
		}
		
		[HttpGet("{id}/messages/read")]
		[ProducesResponseType(typeof(int), 200)]
		public async Task<IActionResult> ReadMessages(int id)
		{
			UserConversation conversation = _conversationService.GetUserConversation(id);
			if (conversation == null) return Forbid();
			
			int res = await _conversationService.ReadAllMessagesAsync(conversation);

			return Ok(res);
		}
	}
}
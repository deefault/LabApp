using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using LabApp.Server.Controllers.Internal;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services;
using LabApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LabApp.Server.Controllers
{
	public class AuthenticationController : BaseCommonController
	{
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userRepository;
		private readonly IUserService _userService;
		private readonly IAuthService _authService;
		private readonly HistoryService _historyService;

		public AuthenticationController(IConfiguration configuration, IUserRepository userRepository,
			IUserService userService, IAuthService authService, HistoryService historyService)
		{
			_configuration = configuration;
			_userRepository = userRepository;
			_userService = userService;
			_authService = authService;
			_historyService = historyService;
		}

		[HttpPost]
		[AllowAnonymous]
		[Produces("text/plain")]
		public IActionResult Token([FromForm] string username, [FromForm] string password)
		{
			UserIdentity userIdentity = _userRepository.FindIdentityByEmail(username); //TODO email+phone
			if (userIdentity == null) return BadRequest("Вы не зарегистрированы");
			if (!_userService.ValidatePassword(password, userIdentity)) return BadRequest("Неверный пароль");

			string token = _authService.Authenticate(userIdentity);
			_historyService.UserLogged(userIdentity.User);

			return Ok(token);
		}
	}
}
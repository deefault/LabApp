using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using LabApp.Server.Data;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Repositories;
using LabApp.Server.Services.Interfaces;
using LabApp.Shared.Dto;
using Microsoft.AspNetCore.Http;
using LabApp.Shared.Enums;

namespace LabApp.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        private User _currentUser;

        public UserService(IMapper mapper, AppDbContext db, IHttpContextAccessor accessor,
            IUserRepository userRepository, PasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _db = db;
            _httpContextAccessor = accessor;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [NotNull]
        public User CurrentUser => _currentUser ??= GetCurrentUser();

        [NotNull]
        public int UserId => _currentUser?.UserId ?? GetCurrentUserId();

        public bool ValidatePassword(string value, UserIdentity identity) =>
            _passwordHasher.HashPassword(value, identity.PasswordSalt) == identity.PasswordHash;

        public User RegisterUser(UserRegisterDto userRegisterDto, UserType type)
        {
            UserIdentity userIdentity = _mapper.Map<UserIdentity>(userRegisterDto);
            User user = MapUser(userRegisterDto, type);
            userIdentity.Role = type;
            userIdentity.IsVerified = true; //TODO: verification by phone or email
            userIdentity.PasswordSalt = _passwordHasher.CreateSalt();
            userIdentity.PasswordHash =
                _passwordHasher.HashPassword(userRegisterDto.Password, userIdentity.PasswordSalt);

            return _userRepository.Add(userIdentity, user);
        }

        private User MapUser(UserRegisterDto userRegisterDto, UserType type)
        {
            return type switch
            {
                UserType.Student => _mapper.Map<UserRegisterDto, Student>(userRegisterDto),
                UserType.Teacher => _mapper.Map<UserRegisterDto, Teacher>(userRegisterDto),
                UserType.Admin => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        /// <exception cref="NullReferenceException"></exception>
        private User GetCurrentUser()
        {
            int id = GetCurrentUserId();
            User user = _userRepository.GetById(id);

            return user;
        }
        
        /// <exception cref="NullReferenceException"></exception>
        private int GetCurrentUserId()
        {
            ClaimsPrincipal httpContextUser = _httpContextAccessor.HttpContext?.User;
            Claim idClaim = httpContextUser?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim?.Value, out int id) || id == 0)
                throw new NullReferenceException();
            
            return id;
        }
    }
}
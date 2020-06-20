using System;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto;
using LabApp.Shared.Enums;

namespace LabApp.Server.Services.Interfaces
{
    public interface IUserService
    {
        /// <exception cref="NullReferenceException"></exception>
        public User CurrentUser { get; }

        /// <exception cref="NullReferenceException"></exception>
        /// <summary>Current user Id</summary>
        int UserId { get;}

        bool ValidatePassword(string value, UserIdentity identity);
        
        User RegisterUser(UserRegisterDto userRegisterDto, UserType type);
    }
}
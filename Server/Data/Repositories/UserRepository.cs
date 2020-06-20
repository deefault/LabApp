using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using LabApp.Server.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UserIdentity = LabApp.Server.Data.Models.UserIdentity;

namespace LabApp.Server.Data.Repositories
{
	public interface IUserRepository
	{
		bool EmailExists(string email);

		UserIdentity FindIdentityByEmail(string email);

		User FindUserByEmail(string email);

		IQueryable<User> Users();

		User GetById(int id, params Expression<Func<User, object>>[] includes);

		void AddProfilePhoto(User user, string image, string thumb = null);

		User Add(UserIdentity userIdentity, User user);
	}


	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _db;

		public UserRepository(AppDbContext context)
		{
			_db = context;
		}

		public bool EmailExists(string email)
		{
			email = email?.ToLower();
			return _db.UserIdentities.AsNoTracking()
				.Any(x => email == x.Email);
		}

		public UserIdentity FindIdentityByEmail(string email)
		{
			email = email?.ToLower();
			return _db.UserIdentities.Include(x => x.User).AsNoTracking().SingleOrDefault(x => email == x.Email);
		}

		public User FindUserByEmail(string email)
		{
			email = email?.ToLower();
			return _db.UserIdentities.AsNoTracking()
				.Where(x => email == x.Email).Select(x => x.User)
				.SingleOrDefault();
		}

		public IQueryable<User> Users()
		{
			throw new NotImplementedException();
		}

		public IQueryable<User> Users(Expression<Func<User, bool>> where)
		{
			return _db.Set<User>().AsQueryable()
				.Where(where);
			//.Include(x=>x.Photo);
		}

		public User GetById(int id, params Expression<Func<User, object>>[] includes)
		{
			return _db.Set<User>().AsQueryable()
				.AddIncludes(includes)
				.SingleOrDefault(x => x.UserId == id);
		}

		public void AddProfilePhoto(User user, string image, string thumb = null)
		{
			var photo = new UserPhoto {User = user, Url = image, ThumbnailUrl = thumb};
			_db.UserPhotos.Add(photo);
			user.MainPhoto = photo;
			_db.Users.Update(user);
			_db.SaveChanges();
		}

		public User Add(UserIdentity userIdentity, User user)
		{
			using IDbContextTransaction transaction = _db.Database.BeginTransaction();
			try
			{
				UserIdentity addedIdentity = _db.UserIdentities.Add(userIdentity).Entity;
				_db.SaveChanges();
				user.UserId = addedIdentity.UserId;
				user.UserIdentity = addedIdentity;
				User addedUser = _db.Users.Add(user).Entity;
				_db.SaveChanges();
				transaction.Commit();

				return addedUser;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
		}
	}
}
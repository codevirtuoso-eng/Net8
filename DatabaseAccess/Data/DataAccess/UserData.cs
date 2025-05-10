using DatabaseAccess.Data.Context;
using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLibrary.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DatabaseAccess.Data.DataAccess
{
	public class UserData : IUserData
	{
		private readonly MainAppDbContext _mainAppDbContext;
		private readonly ILogger<UserData> _logger;

		public UserData(MainAppDbContext mainAppDbContext, ILogger<UserData> logger)
		{
			_mainAppDbContext = mainAppDbContext;
			_logger = logger;
			_logger.LogDebug("NLog injected into UserData");
		}

		public async Task<List<AspNetUserDAO>> GetAllUsers()
		{
			_logger.LogInformation($"GetAllUsers was called");

			var query = _mainAppDbContext.AspNetUsers.AsQueryable();

			var aspNetUserDaoList = await query.ToListAsync();

			return aspNetUserDaoList;
		}

		public async Task<AspNetUserDAO> Login(string userName, string userPassword)
		{
			_logger.LogInformation($"Login was called with userName: {userName} userPassword: {userPassword}");

			var query = _mainAppDbContext.AspNetUsers.AsQueryable();

			query = query.Where(a => a.UserName == userName);
			query = query.Where(a => a.UserPassword == userPassword);

			AspNetUserDAO aspNetUserDAO = await query.FirstOrDefaultAsync();

			return aspNetUserDAO;
		}

		public async Task<List<AspNetRoleDAO>> GetUserRoles(string userId)
		{
			_logger.LogInformation($"GetUserRoles was called with userId: {userId}");

			var roleList = await (from u in _mainAppDbContext.AspNetUserRoles
							join r in _mainAppDbContext.AspNetRoles on u.RoleId equals r.Id
							where u.UserId == userId
							select r).ToListAsync();

			List<AspNetRoleDAO> roleDAOList = new List<AspNetRoleDAO>();

			foreach (var r in roleList)
			{
				AspNetRoleDAO role = new AspNetRoleDAO()
				{
					Id = r.Id,
					Name = r.Name
				};

				roleDAOList.Add(role);
			}            

			return roleDAOList;
		}

		public async Task<List<AspNetUserClaimDAO>> GetUserClaims(string userId)
		{
			_logger.LogInformation($"GetUserClaims was called with userId: {userId}");

			var query = _mainAppDbContext.AspNetUserClaims.AsQueryable();

			query = query.Where(a => a.UserId == userId);

			List<AspNetUserClaimDAO> userClaimDAOList = await query.ToListAsync<AspNetUserClaimDAO>();

			return userClaimDAOList;
		}

	}
}

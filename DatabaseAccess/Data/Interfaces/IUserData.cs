using DatabaseAccess.Data.EntityModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Data.Interfaces
{
	public interface IUserData
	{
		public Task<AspNetUserDAO> Login(string userName, string userPassword);

		public Task<List<AspNetUserDAO>> GetAllUsers();
		public Task<List<AspNetRoleDAO>> GetUserRoles(string userId);

		public Task<List<AspNetUserClaimDAO>> GetUserClaims(string userId);
	}
}

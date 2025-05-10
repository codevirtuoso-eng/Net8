using SharedLibrary.DTO;
using SharedLibrary.DTO.AspNetUser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAppAPI.ApiFunctions
{
	public interface IUsersFunctions
	{
		public Task<AspNetUserResponseDTO> Login(LoginRequestDTO loginRequestDTO);

		public Task<List<AspNetUserResponseDTO>> GetAllUsers();

		public Task<AspNetUserRoleResponseDTO> GetUserRoles(string userId);

		public Task<AspNetUserClaimResponseDTO> GetUserClaims(string userId);
	}
}

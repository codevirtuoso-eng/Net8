using DatabaseAccess.Data.EntityModels;
using DatabaseAccess.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Common.Models;
using SharedLibrary.DTO;
using SharedLibrary.DTO.AspNetUser;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAppAPI.Models;

namespace WebAppAPI.ApiFunctions
{
	public class UsersFunctions : IUsersFunctions
	{
		private IUserData _userData;
		private IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<UsersFunctions> _logger;

		public UsersFunctions(IUserData orderData, ILogger<UsersFunctions> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_userData = orderData;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
			_logger.LogDebug("NLog injected into OrdersFunctions");
		}

		public async Task<AspNetUserResponseDTO> Login(LoginRequestDTO loginRequestDTO)
		{
			_logger.LogInformation($"Login was called with loginRequestDTO: {loginRequestDTO}");

			var aspNetUserResponseDTO = new AspNetUserResponseDTO();
			var aspNetUserDAO = await _userData.Login(loginRequestDTO.UserName, loginRequestDTO.UserPassword);

			// Not a good practice to pass lower level objects when returning web API data - separation of concerns
			// Convert DAO data to DTO data
			if (aspNetUserDAO != null)
			{
				aspNetUserResponseDTO.Id = aspNetUserDAO.Id;
				aspNetUserResponseDTO.UserName = aspNetUserDAO.UserName;

				var token = GenerateToken(loginRequestDTO.UserName, aspNetUserDAO.Id);

                aspNetUserResponseDTO.Token = new JwtSecurityTokenHandler().WriteToken(token.Result);
				aspNetUserResponseDTO.Expiration = token.Result.ValidTo;

				// Add cookie to hold the access token and username to the response
				// This will return a response containing the necessary authorization and identity information
				_httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", aspNetUserResponseDTO.Token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
				_httpContextAccessor.HttpContext.Response.Cookies.Append("X-Username", loginRequestDTO.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            }

            return aspNetUserResponseDTO; 
		}

		public async Task<List<AspNetUserResponseDTO>> GetAllUsers()
		{
			_logger.LogInformation($"GetAllUsers was called");

			var aspNetUserResponseDtoList = new List<AspNetUserResponseDTO>();

			var userDaoList = await _userData.GetAllUsers();

			foreach (var user in userDaoList)
			{
				AspNetUserResponseDTO aspNetUserResponseDTO = new AspNetUserResponseDTO()
				{
					UserName = user.UserName,
					Id = user.Id
				};
				aspNetUserResponseDtoList.Add(aspNetUserResponseDTO);
			}

			return aspNetUserResponseDtoList;
		}

		public async Task<AspNetUserRoleResponseDTO> GetUserRoles(string userId)
		{
			_logger.LogInformation($"GetUserRoles was called with userId: {userId}");

			List<AspNetRoleDAO> userRoleDAOList = await _userData.GetUserRoles(userId);

			AspNetUserRoleResponseDTO aspNetUserRoleResponseDTO = new AspNetUserRoleResponseDTO();

			// Not a good practice to pass lower level objects when returning web API data - separation of concerns
			// Convert DAO data to DTO data
			foreach (var role in userRoleDAOList)
			{
				var dtoObj = new AspNetRole();
				dtoObj.Id = role.Id;
				dtoObj.Name = role.Name;
				aspNetUserRoleResponseDTO.RoleList.Add(dtoObj);
			}

			return aspNetUserRoleResponseDTO;
		}

		public async Task<AspNetUserClaimResponseDTO> GetUserClaims(string userId)
		{
			_logger.LogInformation($"GetUserClaims was called with userId: {userId}");

			List<AspNetUserClaimDAO> userClaimDAOList = await _userData.GetUserClaims(userId);

			AspNetUserClaimResponseDTO aspNetUserClaimResponseDTO = new AspNetUserClaimResponseDTO();

			// Not a good practice to pass lower level objects when returning web API data - separation of concerns
			// Convert DAO data to DTO data
			foreach (var daoData in userClaimDAOList)
			{
				var dtoObj = new AspNetUserClaim();
				dtoObj.Id = daoData.Id;
				dtoObj.UserId = daoData.UserId;
				dtoObj.ClaimType = daoData.ClaimType;
				dtoObj.ClaimValue = daoData.ClaimValue;
				aspNetUserClaimResponseDTO.ClaimList.Add(dtoObj);
			}

			return aspNetUserClaimResponseDTO;
		}

		private async Task<JwtSecurityToken> GenerateToken(string userName, string userId)
		{
			var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userName),
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // this guarantees the token is unique
				};

			var userRoleDAOList = await _userData.GetUserRoles(userId);

			foreach (var userRole in userRoleDAOList)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole.Name));
			}

			var userClaimDAOList = await _userData.GetUserClaims(userId);

			foreach (var userClaim in userClaimDAOList)
			{
				authClaims.Add(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
			}

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: DateTime.Now.AddMinutes(30),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			await ManualLogin(userName, authClaims);

			return token;
		}

		private async Task ManualLogin(string userName, List<Claim> claimList)
		{
			var jwtIdentity = new JwtIdentity(true, userName, CookieAuthenticationDefaults.AuthenticationScheme);

			var claimsIdentity = new ClaimsIdentity(jwtIdentity);
			claimsIdentity.AddClaims(claimList);

			//var context = _httpContextAccessor.HttpContext.Request.GetOwinContext();
			//var authenticationManager = context.Authentication;
			//authenticationManager.SignIn(claims);


			var authProperties = new AuthenticationProperties
			{
				//AllowRefresh = <bool>,
				// Refreshing the authentication session should be allowed.

				//ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
				// The time at which the authentication ticket expires. A 
				// value set here overrides the ExpireTimeSpan option of 
				// CookieAuthenticationOptions set with AddCookie.

				IsPersistent = true,
				// Whether the authentication session is persisted across 
				// multiple requests. When used with cookies, controls
				// whether the cookie's lifetime is absolute (matching the
				// lifetime of the authentication ticket) or session-based.

				//IssuedUtc = <DateTimeOffset>,
				// The time at which the authentication ticket was issued.

				//RedirectUri = <string>
				// The full path or absolute URI to be used as an http 
				// redirect response value.
			};

			 await _httpContextAccessor.HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);
		}
	}
}

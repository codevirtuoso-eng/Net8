using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MvcWebApplication.ViewFunctions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MvcWebApplication
{
	public class Startup
	{
		private IWebHostEnvironment _env;
		private string _contentRootPath;

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
			_contentRootPath = env.ContentRootPath;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// JWT authentication configuration
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;  // Saves the JWT access token in the current HttpContext
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["JWT:Issuer"],
					ValidAudience = Configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
				};
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						// option to extract access token from incoming request cookies
						if (context.Request.Cookies.ContainsKey("X-Access-Token"))
						{
							context.Token = context.Request.Cookies["X-Access-Token"];
							if (!context.Request.Headers.ContainsKey("Authorization"))
							{
								context.Request.Headers.Add("Authorization", "Bearer " + context.Token);
							}
						}
						return Task.CompletedTask;
					}
				};
			})
			.AddCookie(options =>
			{
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.IsEssential = true;
				options.SlidingExpiration = true;
			});

			// Definition for HttpClient using 
			services.AddHttpClient("LocalClient")
			.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				UseCookies = false // tells web API to not store cookies
			});

			// In case you need to inject HttpContext into any classes
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddTransient<IOrdersViewFunctions, OrdersViewFunctions>();
			services.AddTransient<IHomeViewFunctions, HomeViewFunctions>();
			services.AddTransient<IMenuListingViewFunctions, MenuListingViewFunctions>();

			services.AddControllersWithViews(); // MVC controllers
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			// we are not going to use the default exception page
			//if (_env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}

			// Enable the Exception Handler Middleware to catch unhandled exceptions
			app.UseExceptionHandler("/Error/Unhandled");
			// Enable Exception Handler to handle HTTP status code errors    
			app.UseStatusCodePagesWithReExecute("/Error/Status", "?statusCode={0}");

			app.Use(async (context, next) =>
			{
				if (context.Request.Cookies.ContainsKey("X-Access-Token"))
				{
					var accessToken = context.Request.Cookies["X-Access-Token"];
					if (!context.Request.Headers.ContainsKey("Authorization"))
					{
						context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
					}
					//context.Request.Headers.Add("Authorization", "Bearer " + accessToken);
				}

				await next();
			});

			// OWASP
			app.UseHsts();
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllers();
			});
		}
	}
}

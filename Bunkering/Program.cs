using Bunkering.Access;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rotativa.AspNetCore;
using System.Configuration;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationContext>();

builder.Services.Services();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<ErrorHandlingOptions>(builder.Configuration.GetSection("ErrorHandling"));
builder.Services.AddControllers();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
	var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
	o.SaveToken = true;
	o.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidAudience = builder.Configuration["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Key)
	};
}).AddCookie(config =>
	{
		config.Cookie.HttpOnly = true;
		config.Cookie.Name = "Cookies";
		config.LoginPath = "/Account/Login";
		config.LogoutPath = "/Account/LogOff";
		config.CookieManager = new ChunkingCookieManager();
		config.ExpireTimeSpan = TimeSpan.FromHours(1);
		config.SlidingExpiration = true;
	});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(PolicyConstants.COMPANY, policy => policy.RequireRole(RoleConstants.COMPANY));
	options.AddPolicy(PolicyConstants.STAFF, policy => policy.RequireRole(RoleConstants.REVIEWER, RoleConstants.SUPERVISOR));
	options.AddPolicy(PolicyConstants.ADMIN, policy => policy.RequireRole(RoleConstants.SUPER_ADMIN));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "CVC API",
		Description = "Authorized Tasks API using JWT Authentication and refresh Tokens",
		TermsOfService = new Uri("https://nmdpra.gov.ng"),
		Contact = new OpenApiContact
		{
			Name = "Contact us",
			Url = new Uri("https://nmdpra.gov.ng")
		},
		License = new OpenApiLicense
		{
			Name = "Nigerian Midstream and Downstream Petroleum Regulatory Authority (NMDPRA)",
			Url = new Uri("https://nmdpra.gov.ng")
		}
	});

	//options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");

	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});

	//options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
	options.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
	var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var path = Path.Combine(AppContext.BaseDirectory, xml);
	options.IncludeXmlComments(path);
});

//builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
//builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


RotativaConfiguration.Setup(builder.Environment.WebRootPath);

app.UseSwagger();
app.UseSwaggerUI(x =>
{
	x.SwaggerEndpoint("/swagger/v1/swagger.json", "CVC Api");
});

app.UseCors(x => x
	.AllowAnyMethod()
	.AllowAnyHeader()
	.SetIsOriginAllowed(origin => true) // allow any origin
	.AllowCredentials());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}/{option?}");

var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

using (var scope = scopedFactory.CreateScope())
{
	var _db = scope.ServiceProvider.GetService<ApplicationContext>();
	_db.Database.EnsureCreated();

	var service = scope.ServiceProvider.GetService<Seeder>();

	await service.CreateRoles();
	await service.CreateAdmin();
	await service.CreateStates();
	await service.CreateDefaultFcailityTypes();
	await service.CreateAppTypes();
	await service.CreateVesselType();
	await service.CreateProducts();
	await service.CreateLocations();
	await service.CreateTankMeasurementTypes();
	
}
app.Run();

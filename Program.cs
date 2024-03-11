using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using heystock.data;
using heystock.Identity;
using heystock.Interfaces;
using heystock.Repositories;
using heystock.hubs;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        //ValidIssuer = config["JwtSettings:Issuer"],
        //ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };

    x.Events = new JwtBearerEvents
      {
          OnMessageReceived = context =>
          {
              var accessToken = context.Request.Query["access_token"];

              // If the request is for our hub...
              var path = context.HttpContext.Request.Path;
              if (!string.IsNullOrEmpty(accessToken) &&
                  (path.StartsWithSegments("/loginHub")))
              {
                  // Read the token out of the query string
                  context.Token = accessToken;
              }
              return Task.CompletedTask;
          }
      };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityData.AdminUserPolicyName, p => 
        p.RequireClaim(IdentityData.AdminUserClaimName, "True"));
});

builder.Services.AddControllers();
/* builder.Services.AddScoped<IChatRepository, ChatRepository>(); */
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var dbConnectionString = builder.Configuration.GetConnectionString("defaultDb");

builder.Services.AddDbContext<DbDataContext>(opt => opt.UseMySql(
    dbConnectionString, ServerVersion.AutoDetect(dbConnectionString))); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// SWAGGER THINGS


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please provide a valid token.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            Array.Empty<string>()
        }
    });

    /*
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>{
        {"Bearer", Enumerable.Empty<string>()},
    });*/
});

builder.Services.AddSignalR();

var app = builder.Build();

// MORE SWAGGER
// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger";
    });
//}


app.UseHttpsRedirection();


app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.MapHub<LoginHub>("/loginHub");
app.Run();

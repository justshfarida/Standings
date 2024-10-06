using Standings.Application.Automappers;
using Standings.Persistence.Registrations;
using Standings.Infrastructure.Registerations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog;
using MentorApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;
using FluentValidation.AspNetCore;
using FluentValidation;
using Standings.Application.Validations.StudentValid;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureService();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Standings Final API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer
//(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateAudience = true,//tokunumuzu kim/hansi origin islede biler
        ValidateIssuer = true, //tokunu kim palylayir
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //tokenin ozel keyi

        ValidAudience = builder.Configuration["Token:Audience"],

        ValidIssuer = builder.Configuration["Token:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

        //token omru qeder islemesi ucun
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };


});

// Serilogu configure ele
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Logları günlük fayla yazır
    .CreateLogger();

// Serilog-u ASP.NET Core üçün istifadə edirik
builder.Host.UseSerilog();
// Add FluentValidation and scan for validators in the assembly
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentCreateDTOValidator>();

var app = builder.Build();
// Verilənlər bazasını yaradın və seed data əlavə edin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager=services.GetRequiredService<RoleManager<Role>>();

        // Seed data çağırılır
        await context.SeedData(userManager, roleManager);   
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();  // Marşrutlama burada əlavə olunur
app.ConfigureExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
// Marşrutları qeyd edin
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Controller-ləri marşrutlayır
});
app.Run();

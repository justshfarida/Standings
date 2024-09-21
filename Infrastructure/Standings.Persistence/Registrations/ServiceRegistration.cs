using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Standings.Application.Interfaces.IRepositories;
using Standings.Application.Interfaces.IServices;
using Standings.Application.Interfaces.IUnitOfWorks;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Infrastructure.Implementations.Services;
using Standings.Persistence.Configurations;
using Standings.Persistence.Contexts;
using Standings.Persistence.Implementations.Repositories;
using Standings.Persistence.Implementations.Services;
using Standings.Persistence.Implementations.UnitOfWorks;

namespace Standings.Persistence.Registrations
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            //sql connection and context registration
            //bu islesin deye Configuration clasinda namespaceden .Configuration sildim
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            //Identity Registration
            services.AddIdentity<User, Role>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //Repository Registrations 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();  
            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IResultService, ResultService>();


        }
    }
}

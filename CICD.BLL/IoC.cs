using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BLL
{
    public static class IoC
    {
        public static void BindDataLayer(IServiceCollection services, string connectionString)
        {
            DAL.IoC.BindDapper(services, connectionString);

            services.AddScoped<DAL.Mappers.IConfiguration, DAL.Mappers.Configuration>();
            services.AddScoped<DAL.Mappers.IUser, DAL.Mappers.User>();
            services.AddScoped<DAL.Mappers.IProject, DAL.Mappers.Project>();
            services.AddScoped<DAL.Mappers.IBranch, DAL.Mappers.Branch>();
            services.AddScoped<DAL.Mappers.ICommit, DAL.Mappers.Commit>();
            services.AddScoped<DAL.Interfaces.IConfiguration, DAL.Configuration>();
            services.AddHttpClient<DAL.GitHub>();
            services.AddScoped<DAL.Interfaces.IGitHub, DAL.GitHub>();
            services.AddScoped<DAL.Interfaces.IUser, DAL.User>();
            services.AddScoped<DAL.Interfaces.IProject, DAL.Project>();
            services.AddScoped<DAL.Interfaces.ICommit, DAL.Commit>();
            services.AddScoped<DAL.Interfaces.IBranch, DAL.Branch>();
            services.AddScoped<DAL.Interfaces.IProjectTransaction, DAL.ProjectTransaction>();
        }
    }
}

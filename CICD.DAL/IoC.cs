using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DAL
{
    public static class IoC
    {
        public static void BindDapper(IServiceCollection services, string connectionString)
        {
            services.AddScoped<Model.CICDContext>(serviceProvider =>
            {
                return new Model.CICDContext(connectionString);
            });
        }
    }
}

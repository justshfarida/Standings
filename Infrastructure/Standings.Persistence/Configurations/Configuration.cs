using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Standings.Persistence.Configurations
{
    public static class Configuration
    {
        public static string ConnectionString {
            get
            {
                var configurationManager = new ConfigurationManager();
                // Adjust the path to the directory containing appsettings.json
                var projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Standings.API"));
                configurationManager.SetBasePath(projectDirectory);
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("DefaultConnection");
            } }
    }
}

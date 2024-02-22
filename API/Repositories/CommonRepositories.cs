using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;


namespace API.Repositories
{
    public class CommonRepositories
    {
        protected NpgsqlConnection connection;



        public CommonRepositories()
        {
                IConfiguration myConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();


                connection = new NpgsqlConnection(myConfig.GetConnectionString("DefaultConnection"));
        }
    }
}

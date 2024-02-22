using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Npgsql;

namespace API.Repositories
{
    public class CityRepositories : CommonRepositories, ICityRepositories
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public bool isAuthenticated
        {
            get
            {
                var session = _httpContextAccessor.HttpContext.Session;
                return session.GetInt32("isAuthenticated") == 1;
            }
        }

        public CityRepositories(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        public List<City> FetchAllcity()
        {
            var CityList = new List<City>();
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT ct.c_cityid, cu.c_userid, cu.c_username, ct.c_cityname, ct.c_type, ct.c_city_facility, ct.c_city_photo, ct.c_stateid, ts.c_statename, ct.c_date FROM public.t_citytask ct JOIN public.t_srs_user cu ON ct.c_userid = cu.c_userid JOIN public.t_state ts ON ct.c_stateid = ts.c_stateid WHERE ct.c_userid = @userid", connection);
                cmd.Parameters.AddWithValue("@userid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var city = new City
                    {
                        c_cityid = reader.GetInt32(0),
                        c_cityname = reader.GetString(3),
                        c_type = reader.GetString(4),
                        c_city_facility = reader.GetString(5),
                        c_city_photo = reader.GetString(6),
                        State = new State
                        {
                            c_stateid = reader.GetInt32(7),
                            c_statename = reader.GetString(8)
                        },
                        c_date = reader.GetDateTime(9),
                        user = new User
                        {
                            c_userid = reader.GetInt32(1),
                            c_username = reader.GetString(2)
                        }

                    };
                    CityList.Add(city);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }
            return CityList;
        }
    }
}
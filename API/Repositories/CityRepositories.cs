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
                Console.WriteLine("id : " + _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
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


        List<State> ICityRepositories.GetAllstate()
        {
            List<State> states = new List<State>();

            using (NpgsqlCommand command = new NpgsqlCommand("SELECT c_stateid, c_statename FROM t_state;", connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(new State
                        {
                            c_stateid = reader.GetInt32(0),
                            c_statename = reader.GetString(1)
                        });
                    }
                }
            }
            connection.Close();
            return states;
        }

        public bool AddCity(City city)
        {
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("INSERT INTO public.t_citytask (c_cityname, c_type, c_city_facility, c_city_photo, c_stateid, c_userid, c_date) VALUES (@cityname, @type, @facility, @photo, @stateid, @userid, @date)", connection);

                cmd.Parameters.AddWithValue("@userid", _httpContextAccessor.HttpContext.Session.GetInt32("userid"));
                cmd.Parameters.AddWithValue("@cityname", city.c_cityname);
                cmd.Parameters.AddWithValue("@type", city.c_type);
                cmd.Parameters.AddWithValue("@facility", city.c_city_facility);
                cmd.Parameters.AddWithValue("@photo", city.c_city_photo);
                cmd.Parameters.AddWithValue("@stateid", city.c_stateid);
                cmd.Parameters.AddWithValue("@date", city.c_date);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateCity(City city)
        {
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("UPDATE public.t_citytask SET c_cityname = @cityname, c_type = @type, c_city_facility = @facility, c_city_photo = @photo, c_stateid = @stateid, c_date = @date WHERE c_cityid = @cityid", connection);

                cmd.Parameters.AddWithValue("@cityid", city.c_cityid);
                cmd.Parameters.AddWithValue("@cityname", city.c_cityname);
                cmd.Parameters.AddWithValue("@type", city.c_type);
                cmd.Parameters.AddWithValue("@facility", city.c_city_facility);
                cmd.Parameters.AddWithValue("@photo", city.c_city_photo);
                cmd.Parameters.AddWithValue("@stateid", city.c_stateid);
                cmd.Parameters.AddWithValue("@date", city.c_date);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public City FetchByCityId(int cityId)
        {
            City city = null;
            try
            {
                connection.Open();
                var cmd = new NpgsqlCommand("SELECT ct.c_cityid, ct.c_cityname, ct.c_type, ct.c_city_facility, ct.c_city_photo, ct.c_stateid, ts.c_statename, ct.c_date FROM public.t_citytask ct JOIN public.t_state ts ON ct.c_stateid = ts.c_stateid WHERE ct.c_cityid = @cityid", connection);
                cmd.Parameters.AddWithValue("@cityid", cityId);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    city = new City
                    {
                        c_cityid = reader.GetInt32(0),
                        c_cityname = reader.GetString(1),
                        c_type = reader.GetString(2),
                        c_city_facility = reader.GetString(3),
                        c_city_photo = reader.GetString(4),
                        State = new State
                        {
                            c_stateid = reader.GetInt32(5),
                            c_statename = reader.GetString(6)
                        },
                        c_date = reader.GetDateTime(7)
                    };
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
            return city;
        }





    }
}
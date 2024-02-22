using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using API.Models;
using API.Repositories;
//using system jgiejgi

namespace API.Repositories
{
    public class UserRepositories : CommonRepositories, IUserRepositories
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepositories(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public User GetOne(int id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            connection.Open();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT * FROM public.t_srs_user WHERE c_userid = @c_userid";
            cmd.Parameters.AddWithValue("c_userid", id);
            NpgsqlDataReader sdr = cmd.ExecuteReader();
            var user = new User();
            while (sdr.Read())
            {
                user.c_userid = Convert.ToInt32(sdr["c_userid"]);
                user.c_username = sdr["c_username"].ToString();
                user.c_useremail = sdr["c_useremail"].ToString();
                user.c_userpassword = sdr["c_userpassword"].ToString();

            }
            connection.Close();
            sdr.Close();
            return user;
        }
        public bool Register(User user)
        {
            if (IsEmailExists(user.c_useremail))
            {
                return false;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.c_userpassword);

            using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.t_srs_user(c_username, c_useremail, c_userpassword) VALUES (@name, @email, @password)", connection))
            {
                command.Parameters.AddWithValue("name", user.c_username);
                command.Parameters.AddWithValue("email", user.c_useremail);
                command.Parameters.AddWithValue("password", hashedPassword);
                connection.Open();
                if (command.ExecuteNonQuery() > 0)
                {
                    connection.Close();
                    return true;
                }
            }
            connection.Close();
            return false;
        }
        // Update the login method in the repository to accept email and password parameters
        public User Login(string email, string password)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT c_userid, c_username,c_useremail, c_userpassword FROM public.t_srs_user WHERE c_useremail = @email";
            cmd.Parameters.AddWithValue("@email", email);

            User user = null;
            connection.Open();
            NpgsqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string storedPasswordHash = reader["c_userpassword"].ToString();
                if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                {
                    user = new User()
                    {
                        c_userid = Convert.ToInt32(reader["c_userid"]),
                        c_username = reader["c_username"].ToString(),
                        c_useremail = reader["c_useremail"].ToString(),
                        c_userpassword = null

                    };
                    var session = _httpContextAccessor.HttpContext.Session;
                    session.SetString("username", reader["c_username"].ToString());
                    session.SetInt32("userid", Convert.ToInt32(reader["c_userid"]));

                }



            }
            connection.Close();
            return user;
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            // Use BCrypt.Net for password verification
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }


        public void Update(User user)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "Update public.t_srs_user SET c_username=@c_username,c_useremail=@c_email,c_userpassword=@c_password WHERE c_userid=@c_userid ;";
            cmd.Parameters.AddWithValue("@c_userid", user.c_userid);
            cmd.Parameters.AddWithValue("@c_username", user.c_username);
            cmd.Parameters.AddWithValue("@c_email", user.c_useremail);
            cmd.Parameters.AddWithValue("@c_password", user.c_userpassword);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public bool IsEmailExists(string email)
        {

            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT COUNT(*) FROM public.t_srs_user WHERE c_useremail = @email", connection))
            {
                command.Parameters.AddWithValue("email", email);
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }


        }







    }
}
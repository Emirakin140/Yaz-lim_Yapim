using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YazılımYapım.Enitty;

namespace YazılımYapım.DataAccess
{
    public class UserProvider
    {
        private readonly string _connectionString;
        
        public UserProvider(string connectionString)
        {
            _connectionString = connectionString;
        }
        public UserProvider()
        {
            _connectionString = @"Server=EMIR\SQLEXPRESS;Database=Sozluk;Trusted_Connection=True;";
        }
        public List<Users> GetAllUsers()
        {
            var users = new List<Users>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT id, kullanici_adi, sifre, is_valid FROM Users";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new Users
                    {
                        Id = (int)reader["id"],
                        UserName = reader["kullanici_adi"].ToString(),
                        Password = reader["sifre"].ToString(),
                        IsValid= (bool)reader["is_valid"]
                    });
                }
            }

            return users;
        }
        public Users GetUserByUsername(string kullaniciAdi)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT id, kullanici_adi, sifre, is_valid FROM Users WHERE kullanici_adi = @kadi";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Users
                    {
                        Id = (int)reader["id"],
                        UserName = reader["kullanici_adi"].ToString(),
                        Password = reader["sifre"].ToString(),
                        IsValid = (bool)reader["is_valid"]
                    };
                }
            }

            return null;
        }
        public bool UpdatePassword(string kullaniciAdi, string yeniSifre)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string updateQuery = @"
                    UPDATE users 
                    SET sifre = @yeniSifre 
                    WHERE kullanici_adi = @kadi";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.Add("@yeniSifre", SqlDbType.NVarChar, 255).Value = yeniSifre;
                    cmd.Parameters.Add("@kadi", SqlDbType.NVarChar, 50).Value = kullaniciAdi;

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool CreateUser(string kullaniciAdi, string sifre, bool isValid = true)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO users (kullanici_adi, sifre, is_valid) 
                          VALUES (@kadi, @sifre, @valid)", conn))
            {
                cmd.Parameters.Add("@kadi", SqlDbType.NVarChar, 50).Value = kullaniciAdi;
                cmd.Parameters.Add("@sifre", SqlDbType.NVarChar, 255).Value = sifre;
                cmd.Parameters.Add("@valid", SqlDbType.Bit).Value = isValid ? 1 : 0;

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}

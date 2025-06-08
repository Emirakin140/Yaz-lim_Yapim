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
    public class WordProvider
    {
        private readonly string _connectionString;

        public WordProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public WordProvider()
        {
            _connectionString = @"Server=EMIR\SQLEXPRESS;Database=Sozluk;Trusted_Connection=True;";
        }
        public List<Word> GetAllWords()
        {
            var list = new List<Word>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       "SELECT id, ingilizce, turkce, pictures, zorluk, is_valid " +
                       "FROM word " +
                       "WHERE is_valid = 1 " +
                       "ORDER BY ingilizce", conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Word
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            EngWordName = reader["ingilizce"].ToString(),
                            TurWordName = reader["turkce"].ToString(),
                            Picture = reader["pictures"] == DBNull.Value
                                          ? null : reader["pictures"].ToString(),
                            Zorluk = reader["zorluk"] == DBNull.Value
                                          ? null : reader["zorluk"].ToString(),
                            IsValid = Convert.ToBoolean(reader["is_valid"])
                        });
                    }
                }
            }
            return list;
        }

        public List<Word> SearchWords(string keyword)
        {
            var list = new List<Word>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       @"SELECT id, ingilizce, turkce, pictures, zorluk, is_valid 
                 FROM word 
                 WHERE is_valid = 1 
                   AND (ingilizce LIKE @kw OR turkce LIKE @kw) 
                 ORDER BY ingilizce", conn))
            {
                cmd.Parameters.Add("@kw", SqlDbType.NVarChar, 100).Value = $"%{keyword}%";
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Word
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            EngWordName = reader["ingilizce"].ToString(),
                            TurWordName = reader["turkce"].ToString(),
                            Picture = reader["pictures"] == DBNull.Value
                                          ? null : reader["pictures"].ToString(),
                            Zorluk = reader["zorluk"] == DBNull.Value
                                          ? null : reader["zorluk"].ToString(),
                            IsValid = Convert.ToBoolean(reader["is_valid"])
                        });
                    }
                }
            }
            return list;
        }
        public bool AddWord(Word word)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       @"INSERT INTO word (ingilizce, turkce, pictures, zorluk) 
                 VALUES (@ing, @turk, @pic, @zorluk)", conn))
            {
                cmd.Parameters.Add("@ing", SqlDbType.NVarChar, 100).Value = word.EngWordName;
                cmd.Parameters.Add("@turk", SqlDbType.NVarChar, 100).Value = word.TurWordName;
                cmd.Parameters.Add("@pic", SqlDbType.NVarChar, 255).Value =
                    string.IsNullOrEmpty(word.Picture) ? (object)DBNull.Value : word.Picture;
                cmd.Parameters.Add("@zorluk", SqlDbType.NVarChar, 50).Value =
                    string.IsNullOrEmpty(word.Zorluk) ? (object)DBNull.Value : word.Zorluk;

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public bool UpdateWord(Word word)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       @"UPDATE word
                 SET ingilizce = @ing,
                     turkce    = @turk,
                     pictures  = @pic,
                     zorluk    = @zorluk
                 WHERE id = @id", conn))
            {
                cmd.Parameters.Add("@ing", SqlDbType.NVarChar, 100).Value = word.EngWordName;
                cmd.Parameters.Add("@turk", SqlDbType.NVarChar, 100).Value = word.TurWordName;
                cmd.Parameters.Add("@pic", SqlDbType.NVarChar, 255).Value =
                    string.IsNullOrEmpty(word.Picture) ? (object)DBNull.Value : word.Picture;
                cmd.Parameters.Add("@zorluk", SqlDbType.NVarChar, 50).Value =
                    string.IsNullOrEmpty(word.Zorluk) ? (object)DBNull.Value : word.Zorluk;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = word.ID;

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public bool DeleteWord(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       @"UPDATE word
                 SET is_valid = 0
                 WHERE id = @id", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        public List<Word> GetWordsByIds(List<int> ids)
        {
            var list = new List<Word>();
            if (ids == null || ids.Count == 0) return list;


            string paramNames = string.Join(", ", ids.Select((id, idx) => $"@id{idx}"));
            string sql = $@"
                SELECT id, ingilizce, turkce, pictures, zorluk, is_valid
                FROM Word
                WHERE id IN ({paramNames})
                  AND is_valid = 1
                ORDER BY ingilizce";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@id{i}", ids[i]);
                }
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Word
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            EngWordName = reader["ingilizce"].ToString(),
                            TurWordName = reader["turkce"].ToString(),
                            Picture = reader["pictures"] == DBNull.Value ? null : reader["pictures"].ToString(),
                            Zorluk = reader["zorluk"] == DBNull.Value ? null : reader["zorluk"].ToString(),
                            IsValid = Convert.ToBoolean(reader["is_valid"])
                        });
                    }
                }
            }
            return list;
        }
        public List<Word> GetNewWords(int need, List<int> excludeWordIds)
        {
            var list = new List<Word>();

            string excludeClause = "";
            if (excludeWordIds != null && excludeWordIds.Count > 0)
            {
                string[] exParams = excludeWordIds.Select((id, idx) => $"@ex{idx}").ToArray();
                excludeClause = $"AND w.id NOT IN ({string.Join(",", exParams)})";
            }

            string sql = $@"
                SELECT TOP(@need) 
                    w.id, w.ingilizce, w.turkce, w.pictures, w.zorluk, w.is_valid
                FROM Word w
                LEFT JOIN WordProgress wp ON w.id = wp.WordId
                WHERE w.is_valid = 1
                  AND (wp.WordId IS NULL OR wp.CorrectStreak = 0)
                  {excludeClause}
                ORDER BY NEWID()";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@need", SqlDbType.Int).Value = need;
                if (excludeWordIds != null)
                {
                    for (int i = 0; i < excludeWordIds.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@ex{i}", excludeWordIds[i]);
                    }
                }

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Word
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            EngWordName = reader["ingilizce"].ToString(),
                            TurWordName = reader["turkce"].ToString(),
                            Picture = reader["pictures"] == DBNull.Value
                                         ? null : reader["pictures"].ToString(),
                            Zorluk = reader["zorluk"] == DBNull.Value
                                         ? null : reader["zorluk"].ToString(),
                            IsValid = Convert.ToBoolean(reader["is_valid"])
                        });
                    }
                }
            }

            return list;
        }
        public List<string> GetRandomWrongTurkishWords(int count, int correctWordId, List<int> excludeIds = null)
        {
            var results = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var excludeList = new List<int> { correctWordId };
                if (excludeIds != null)
                    excludeList.AddRange(excludeIds);

                string excludeClause = "";
                if (excludeList.Count > 0)
                {
                    string[] paramNames = excludeList.Select((id, idx) => $"@ex{idx}").ToArray();
                    excludeClause = $"AND id NOT IN ({string.Join(", ", paramNames)})";
                }

                string sql = $@"
            SELECT TOP(@count) turkce
            FROM Word
            WHERE is_valid = 1
              {excludeClause}
            ORDER BY NEWID()";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@count", SqlDbType.Int).Value = count;
                    for (int i = 0; i < excludeList.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@ex{i}", excludeList[i]);
                    }

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(reader["turkce"].ToString());
                        }
                    }
                }
            }
            return results;
        }

    }
}

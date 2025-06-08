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
    public class WordProgressProvider
    {
        
            private readonly string _connectionString;

            
            public WordProgressProvider()
            {
                _connectionString = @"Server=EMIR\SQLEXPRESS;Database=Sozluk;Trusted_Connection=True;";
             }

            
            public WordProgressProvider(string connectionString)
            {
                _connectionString = connectionString;
            }

            
            public List<WordProgress> GetDueWordProgresses(DateTime today)
            {
                var list = new List<WordProgress>();

                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(
                           @"SELECT WordProgressId, WordId, LastCorrectDate, CorrectStreak, NextDueDate
                         FROM WordProgress
                         WHERE NextDueDate <= @today
                           AND CorrectStreak < 6", conn))
                {
                    cmd.Parameters.Add("@today", SqlDbType.Date).Value = today.Date;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new WordProgress
                            {
                                WordProgressId = Convert.ToInt32(reader["WordProgressId"]),
                                WordId = Convert.ToInt32(reader["WordId"]),
                                LastCorrectDate = reader["LastCorrectDate"] == DBNull.Value
                                                      ? (DateTime?)null
                                                      : Convert.ToDateTime(reader["LastCorrectDate"]),
                                CorrectStreak = Convert.ToInt32(reader["CorrectStreak"]),
                                NextDueDate = reader["NextDueDate"] == DBNull.Value
                                                      ? (DateTime?)null
                                                      : Convert.ToDateTime(reader["NextDueDate"])
                            });
                        }
                    }
                }

                return list;
            }

           
            public WordProgress GetProgressByWordId(int wordId)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(
                           @"SELECT WordProgressId, WordId, LastCorrectDate, CorrectStreak, NextDueDate
                         FROM WordProgress
                         WHERE WordId = @wordId", conn))
                {
                    cmd.Parameters.Add("@wordId", SqlDbType.Int).Value = wordId;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new WordProgress
                            {
                                WordProgressId = Convert.ToInt32(reader["WordProgressId"]),
                                WordId = Convert.ToInt32(reader["WordId"]),
                                LastCorrectDate = reader["LastCorrectDate"] == DBNull.Value
                                                      ? (DateTime?)null
                                                      : Convert.ToDateTime(reader["LastCorrectDate"]),
                                CorrectStreak = Convert.ToInt32(reader["CorrectStreak"]),
                                NextDueDate = reader["NextDueDate"] == DBNull.Value
                                                      ? (DateTime?)null
                                                      : Convert.ToDateTime(reader["NextDueDate"])
                            };
                        }
                    }
                }
                return null;
            }

            
            public WordProgress GetOrCreateProgress(int wordId)
            {
              
                WordProgress existing = GetProgressByWordId(wordId);
                if (existing != null)
                    return existing;

               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(
                           @"INSERT INTO WordProgress (WordId, LastCorrectDate, CorrectStreak, NextDueDate)
                         VALUES (@wordId, NULL, 0, @today);
                         SELECT SCOPE_IDENTITY()", conn))
                {
                    cmd.Parameters.Add("@wordId", SqlDbType.Int).Value = wordId;
                    cmd.Parameters.Add("@today", SqlDbType.Date).Value = DateTime.Today;

                    conn.Open();
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    return new WordProgress
                    {
                        WordProgressId = newId,
                        WordId = wordId,
                        LastCorrectDate = null,
                        CorrectStreak = 0,
                        NextDueDate = DateTime.Today
                    };
                }
            }

            public bool UpdateProgress(WordProgress progress)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand(
                           @"UPDATE WordProgress
                         SET LastCorrectDate = @lastDate,
                             CorrectStreak   = @streak,
                             NextDueDate     = @nextDue
                         WHERE WordProgressId = @id", conn))
                {
                    cmd.Parameters.Add("@lastDate", SqlDbType.Date).Value =
                        progress.LastCorrectDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@streak", SqlDbType.Int).Value = progress.CorrectStreak;
                    cmd.Parameters.Add("@nextDue", SqlDbType.Date).Value =
                        progress.NextDueDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = progress.WordProgressId;

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        public List<WordProgress> GetLearnedWordProgresses()
        {
            var list = new List<WordProgress>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT WordProgressId, WordId, LastCorrectDate, CorrectStreak, NextDueDate
        FROM WordProgress
        WHERE CorrectStreak >= 6", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new WordProgress
                        {
                            WordProgressId = (int)reader["WordProgressId"],
                            WordId = (int)reader["WordId"],
                            LastCorrectDate = reader["LastCorrectDate"] == DBNull.Value
                                               ? (DateTime?)null
                                               : (DateTime)reader["LastCorrectDate"],
                            CorrectStreak = (int)reader["CorrectStreak"],
                            NextDueDate = reader["NextDueDate"] == DBNull.Value
                                               ? (DateTime?)null
                                               : (DateTime)reader["NextDueDate"]
                        });
                    }
                }
            }
            return list;
        }
    }
    }



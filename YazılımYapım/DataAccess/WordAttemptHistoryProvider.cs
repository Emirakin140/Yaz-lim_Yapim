using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YazılımYapım.DataAccess
{
    public class WordAttemptHistoryProvider
    {
        private readonly string _connectionString;

        public WordAttemptHistoryProvider()
        {
            _connectionString = @"Server=EMIR\SQLEXPRESS;Database=Sozluk;Trusted_Connection=True;";
        }

        public WordAttemptHistoryProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddAttempt(int wordId, int userId, bool isCorrect, DateTime answerDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                       @"INSERT INTO WordAttemptHistory (WordId, UserId, IsCorrect, AnswerDate)
                         VALUES (@wordId, @userId, @isCorrect, @answerDate)", conn))
            {
                cmd.Parameters.Add("@wordId", SqlDbType.Int).Value = wordId;
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@isCorrect", SqlDbType.Bit).Value = isCorrect;
                cmd.Parameters.Add("@answerDate", SqlDbType.DateTime).Value = answerDate;

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public DataTable GetReportData(
            int userId, DateTime startDate, DateTime endDate,
            string difficultyFilter, int maxRecords)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("WordId", typeof(int));
            dt.Columns.Add("EnglishWord", typeof(string));
            dt.Columns.Add("TurkishWord", typeof(string));
            dt.Columns.Add("LastDate", typeof(DateTime));
            dt.Columns.Add("CorrectCount", typeof(int));
            dt.Columns.Add("WrongCount", typeof(int));

            string sql = @"
SELECT TOP (@maxRecords)
    w.id            AS WordId,
    w.ingilizce     AS EnglishWord,
    w.turkce        AS TurkishWord,
    MAX(h.AnswerDate) AS LastDate,
    SUM(CASE WHEN h.IsCorrect = 1 THEN 1 ELSE 0 END) AS CorrectCount,
    SUM(CASE WHEN h.IsCorrect = 0 THEN 1 ELSE 0 END) AS WrongCount
FROM WordAttemptHistory h
INNER JOIN Word w       ON w.id = h.WordId
WHERE h.UserId = @userId
  AND CAST(h.AnswerDate AS date) BETWEEN @start AND @end
  AND (@difficulty = 'Tümü' OR w.zorluk = @difficulty)
GROUP BY w.id, w.ingilizce, w.turkce
ORDER BY MAX(h.AnswerDate) DESC;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@start", SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@end", SqlDbType.Date).Value = endDate.Date;
                cmd.Parameters.Add("@difficulty", SqlDbType.NVarChar, 50).Value = difficultyFilter;
                cmd.Parameters.Add("@maxRecords", SqlDbType.Int).Value = maxRecords;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int wid = Convert.ToInt32(reader["WordId"]);
                        string eng = reader["EnglishWord"].ToString();
                        string tur = reader["TurkishWord"].ToString();
                        DateTime lastDate = Convert.ToDateTime(reader["LastDate"]);
                        int corr = Convert.ToInt32(reader["CorrectCount"]);
                        int wrong = Convert.ToInt32(reader["WrongCount"]);

                        dt.Rows.Add(wid, eng, tur, lastDate, corr, wrong);
                    }
                }
            }

            return dt;
        }

        public List<DateTime> GetLastSixDates(
            int userId, int wordId, DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();

            string sql = @"
SELECT TOP(6) h.AnswerDate
FROM WordAttemptHistory h
WHERE h.UserId = @userId
  AND h.WordId = @wordId
  AND CAST(h.AnswerDate AS date) BETWEEN @start AND @end
ORDER BY h.AnswerDate DESC;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@wordId", SqlDbType.Int).Value = wordId;
                cmd.Parameters.Add("@start", SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@end", SqlDbType.Date).Value = endDate.Date;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dates.Add(Convert.ToDateTime(reader["AnswerDate"]));
                    }
                }
            }

            return dates;
        }

        public (int totalCorrect, int totalWrong) GetTotalCorrectWrong(
            int userId, DateTime startDate, DateTime endDate, string difficultyFilter)
        {
            int totCorrect = 0, totWrong = 0;

            string sql = @"
SELECT 
    SUM(CASE WHEN h.IsCorrect = 1 THEN 1 ELSE 0 END) AS TotCorrect,
    SUM(CASE WHEN h.IsCorrect = 0 THEN 1 ELSE 0 END) AS TotWrong
FROM WordAttemptHistory h
INNER JOIN Word w ON w.id = h.WordId
WHERE h.UserId = @userId
  AND CAST(h.AnswerDate AS date) BETWEEN @start AND @end
  AND (@difficulty = 'Tümü' OR w.zorluk = @difficulty);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                cmd.Parameters.Add("@start", SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@end", SqlDbType.Date).Value = endDate.Date;
                cmd.Parameters.Add("@difficulty", SqlDbType.NVarChar, 50).Value = difficultyFilter;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totCorrect = reader["TotCorrect"] == DBNull.Value
                                        ? 0
                                        : Convert.ToInt32(reader["TotCorrect"]);
                        totWrong = reader["TotWrong"] == DBNull.Value
                                        ? 0
                                        : Convert.ToInt32(reader["TotWrong"]);
                    }
                }
            }

            return (totCorrect, totWrong);
        }
        public List<(string SessionLabel, int CorrectCount, int WrongCount)>
    GetCorrectWrongBySession(
        int userId, DateTime startDate, DateTime endDate, string difficultyFilter)
        {
            var list = new List<(string, int, int)>();

            string sql = @"
SELECT 
    CAST(h.AnswerDate AS date) AS SessionDate,
    SUM(CASE WHEN h.IsCorrect = 1 THEN 1 ELSE 0 END) AS CorrectCount,
    SUM(CASE WHEN h.IsCorrect = 0 THEN 1 ELSE 0 END) AS WrongCount
FROM WordAttemptHistory h
INNER JOIN Word w ON w.id = h.WordId
WHERE h.UserId = @userId
  AND CAST(h.AnswerDate AS date) BETWEEN @start AND @end
  AND (@difficulty = 'Tümü' OR w.zorluk = @difficulty)
GROUP BY CAST(h.AnswerDate AS date)
ORDER BY SessionDate;";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@start", startDate.Date);
                cmd.Parameters.AddWithValue("@end", endDate.Date);
                cmd.Parameters.AddWithValue("@difficulty", difficultyFilter);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var date = (DateTime)reader["SessionDate"];
                        var corr = Convert.ToInt32(reader["CorrectCount"]);
                        var wrng = Convert.ToInt32(reader["WrongCount"]);
                        string lbl = date.ToString("dd.MM");  // X‑ekseni etiketi

                        list.Add((lbl, corr, wrng));
                    }
                }
            }

            return list;
        }
    }
}

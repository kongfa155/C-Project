using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.Data;
using System.Configuration;
using System.Linq;

namespace ProjectC_
{
    public class DatabaseHelper
    {
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        // 1. Lấy tất cả hoạt động (Lịch trình + Chi tiêu + Sinh nhật) cho một ngày
        public List<CalendarItem> GetAllActivitiesByDate(int userId, DateTime date)
        {
            List<CalendarItem> list = new List<CalendarItem>();
            string dateStr = date.ToString("yyyy-MM-dd");

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Sửa query: Lấy Note/Description vào chung cột Info để map vào CalendarItem.Description
                string sql = @"
            SELECT 'Schedule' as Source, Title as Content, Description as Info, TimeStart as SortTime 
            FROM Schedule WHERE UserId = @userId AND Date = @date
            UNION ALL
            SELECT 'Expense' as Source, CONCAT(Amount, ' - ', Note) as Content, Note as Info, '23:59:59' as SortTime
            FROM Transactions WHERE UserId = @userId AND Date = @date
            UNION ALL
            SELECT 'Birthday' as Source, 'Sinh nhật của bạn!' as Content, 'Ngày kỷ niệm sinh nhật' as Info, '00:00:00' as SortTime
            FROM Users WHERE Id = @userId AND MONTH(Birthday) = MONTH(@date) AND DAY(Birthday) = DAY(@date)
            ORDER BY SortTime ASC";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@date", dateStr);

                conn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new CalendarItem
                        {
                            Type = rdr["Source"].ToString(),
                            Content = rdr["Content"].ToString(),
                            Description = rdr["Info"].ToString() // Ghi chú hiển thị khi hover
                        });
                    }
                }
            }
            return list;
        }

        // 2. Lấy danh sách chi tiêu (Sử dụng GetDecimal cho chính xác)
        public List<CalendarItem> GetTransactionsByDate(int userId, DateTime date, int categoryId = -1)
        {
            List<CalendarItem> list = new List<CalendarItem>();
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Sử dụng FORMAT hoặc CAST nếu bạn muốn hiển thị số tiền đẹp ngay từ SQL
                string sql = @"SELECT 'Expense' as Source, Amount, Note as Description 
                               FROM Transactions 
                               WHERE UserId = @userId AND Date = @date";

                if (categoryId != -1) sql += " AND CategoryId = @catId";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                if (categoryId != -1) cmd.Parameters.AddWithValue("@catId", categoryId);

                conn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        decimal amount = Convert.ToDecimal(rdr["Amount"]);
                        string displayAmount;

                        // Logic đổi sang chữ 'k'
                        if (amount >= 1000 && amount % 1000 == 0)
                        {
                            displayAmount = (amount / 1000).ToString("N0") + "k";
                        }
                        else
                        {
                            displayAmount = amount.ToString("N0"); // Nếu lẻ (vd 50.500) thì hiện đủ
                        }

                        list.Add(new CalendarItem
                        {
                            Type = "Expense",
                            Content = $"{displayAmount} - {rdr["Description"]}",
                            Description = rdr["Description"].ToString()
                        });
                    }
                }
                return list;
            }
        }

        // 3. Tính tổng tài chính (Sửa DBNull và ép kiểu Decimal)
        public (decimal thu, decimal chi) GetFinancesByMonth(int userId, int month, int year, int categoryId = -1)
        {
            decimal thu = 0, chi = 0;
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Type = 1 là Thu, Type = 0 là Chi
                string sql = @"SELECT 
                        SUM(CASE WHEN Type = 1 THEN Amount ELSE 0 END) as TongThu,
                        SUM(CASE WHEN Type = 0 THEN Amount ELSE 0 END) as TongChi
                      FROM Transactions 
                      WHERE UserId = @userId AND MONTH(Date) = @month AND YEAR(Date) = @year";

                if (categoryId != -1) sql += " AND CategoryId = @catId";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                if (categoryId != -1) cmd.Parameters.AddWithValue("@catId", categoryId);

                conn.Open();
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        // Cách đọc Decimal an toàn từ DB
                        thu = rdr["TongThu"] != DBNull.Value ? Convert.ToDecimal(rdr["TongThu"]) : 0m;
                        chi = rdr["TongChi"] != DBNull.Value ? Convert.ToDecimal(rdr["TongChi"]) : 0m;
                    }
                }
            }
            return (thu, chi);
        }

        // 4. Thêm giao dịch (Amount đổi thành decimal)
        public void InsertTransaction(int userId, decimal amount, string note, int type, int categoryId, DateTime date)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "INSERT INTO Transactions (UserId, Amount, Note, Type, CategoryId, Date) " +
                             "VALUES (@uid, @amount, @note, @type, @catId, @date)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@amount", amount); // Truyền decimal trực tiếp
                cmd.Parameters.AddWithValue("@note", note);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@catId", categoryId);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Các hàm khác giữ nguyên
        public void InsertSchedule(int userId, DateTime date, string timeStart, string timeEnd, string title, string desc)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "INSERT INTO Schedule (UserId, Date, TimeStart, TimeEnd, Title, Description) " +
                             "VALUES (@uid, @date, @ts, @te, @title, @desc)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ts", timeStart);
                cmd.Parameters.AddWithValue("@te", timeEnd);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", desc);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "SELECT Id, Name FROM Categories";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetUsers()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "SELECT Id, Name FROM Users";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void CreateUser(string name, decimal balance, DateTime birthday)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "INSERT INTO Users (Name, Balance, Birthday) VALUES (@name, @balance, @birthday)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@balance", balance);
                cmd.Parameters.AddWithValue("@birthday", birthday.ToString("yyyy-MM-dd"));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
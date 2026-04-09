using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace ProjectC_
{
    public class DatabaseHelper
    {
        //Tạo nối kết với cơ sở dữ liệu
        private string connString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        //Lấy danh sách tất cả hoạt động (Lịch trình + Chi tiêu + Sinh nhật) cho một ngày

        public List<CalendarItem> GetAllActivitiesByDate(int userId, DateTime date)
        {
            List<CalendarItem> list = new List<CalendarItem>();

            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = @"
        SELECT Id, 'Schedule' as Source, Title as Content, Description as Info, TimeStart, TimeEnd
        FROM Schedule WHERE UserId = @userId AND Date = @date
        UNION ALL
        SELECT Id, 'Expense' as Source, CONCAT(Amount, ' - ', Note), Note, '23:59:59', '23:59:59'
        FROM Transactions WHERE UserId = @userId AND Date = @date
        UNION ALL
        SELECT 0 as Id, 'Birthday', 'Sinh nhật của bạn!', 'Ngày kỷ niệm', '00:00:00', '23:59:59'
        FROM Users WHERE Id = @userId 
            AND MONTH(Birthday) = MONTH(@date) 
            AND DAY(Birthday) = DAY(@date)
        ORDER BY TimeStart";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

                conn.Open();

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new CalendarItem
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            Type = rdr["Source"].ToString(),
                            Content = rdr["Content"].ToString(),
                            Description = rdr["Info"].ToString(),
                            TimeStart = rdr["TimeStart"].ToString(),
                            TimeEnd = rdr["TimeEnd"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        //Lấy danh sách chi tiêu trong một ngày, nếu có categoryId thì lọc theo category đó (categoryId = -1 nghĩa là lấy tất cả)

        public List<CalendarItem> GetTransactionsByDate(int userId, DateTime date, int categoryId = -1)
        {
            //Tạo list mới để lưu trữ thông tin
            List<CalendarItem> list = new List<CalendarItem>();
            //Tạo kết nối đến database và thực hiện truy vấn
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Truy vấn SQL để lấy chi tiêu trong ngày, nếu có categoryId thì thêm điều kiện lọc
                string sql = @"SELECT Id, 'Expense' as Source, Amount, Note as Description 
               FROM Transactions 
               WHERE UserId = @userId AND Date = @date";

                if (categoryId != -1) sql += " AND CategoryId = @catId";
                // Thực hiện câu lệnh SQL lên server và đọc dữ liệu trả về
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                //Gán giá trị vào tham số
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                // Nếu có categoryId thì gán thêm tham số
                if (categoryId != -1) cmd.Parameters.AddWithValue("@catId", categoryId);
                // Mở kết nối và đọc dữ liệu
                conn.Open();
                // Đọc từng dòng dữ liệu trả về và thêm vào list
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        decimal amount = Convert.ToDecimal(rdr["Amount"]);
                        string displayAmount;

                        // Logic đổi 000 sang chữ 'k'
                        if (amount >= 1000 && amount % 1000 == 0)
                        {
                            displayAmount = (amount / 1000).ToString("N0") + "k";
                        }
                        else
                        {
                            displayAmount = amount.ToString("N0"); // Nếu lẻ (vd 50.500) thì hiện đủ
                        }
                        // Thêm vào list với định dạng hiển thị đã xử lý
                        list.Add(new CalendarItem
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            Type = "Expense",
                            Content = $"{displayAmount} - {rdr["Description"]}",
                            Description = rdr["Description"].ToString()
                        });
                    }
                }
                return list;
            }
        }

        // Tính tổng tài chính của tháng, nếu có categoryId thì chỉ tính cho category đó (categoryId = -1 nghĩa là tính tất cả)
        public (decimal thu, decimal chi) GetFinancesByMonth(int userId, int month, int year, int categoryId = -1)
        {
            // Khởi tạo biến để lưu tổng thu và chi
            decimal thu = 0, chi = 0;
            // Tạo kết nối đến database và thực hiện truy vấn
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Type = 1 là Thu, Type = 0 là Chi
                // Sử dụng SUM với CASE để tính tổng thu và chi trong một truy vấn duy nhất
                string sql = @"SELECT 
                        SUM(CASE WHEN Type = 1 THEN Amount ELSE 0 END) as TongThu,
                        SUM(CASE WHEN Type = 0 THEN Amount ELSE 0 END) as TongChi
                      FROM Transactions 
                      WHERE UserId = @userId AND MONTH(Date) = @month AND YEAR(Date) = @year";

                if (categoryId != -1) sql += " AND CategoryId = @catId";
                // Thực hiện câu lệnh SQL lên server và đọc dữ liệu trả về
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                // Gán giá trị vào tham số
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                if (categoryId != -1) cmd.Parameters.AddWithValue("@catId", categoryId);
                // Mở kết nối và đọc dữ liệu
                conn.Open();
                // Đọc dữ liệu trả về và gán vào biến thu, chi
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        // Cách đọc Decimal an toàn từ DB, nếu không có giá trị thì trả về 0
                        thu = rdr["TongThu"] != DBNull.Value ? Convert.ToDecimal(rdr["TongThu"]) : 0m;
                        chi = rdr["TongChi"] != DBNull.Value ? Convert.ToDecimal(rdr["TongChi"]) : 0m;
                    }
                }
            }
            return (thu, chi);
        }

        //5. Thêm giao dịch mới và cập nhật Balance trong bảng Users
        public void InsertTransaction(int userId, decimal amount, string note, int type, int categoryId, DateTime date)
        {
            //Tạo nối kết đến database và thực hiện truy vấn trong một transaction để đảm bảo tính toàn vẹn dữ liệu
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Mở kết nối và bắt đầu transaction
                conn.Open();
                // Sử dụng transaction để đảm bảo rằng cả hai thao tác (thêm giao dịch và cập nhật balance) đều thành công hoặc đều thất bại
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Thêm giao dịch mới
                        string sql = "INSERT INTO Transactions (UserId, Amount, Note, Type, CategoryId, Date) " +
                             "VALUES (@uid, @amount, @note, @type, @catId, @date)";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        // Gán giá trị vào tham số, đảm bảo truyền decimal trực tiếp để tránh lỗi định dạng
                        cmd.Parameters.AddWithValue("@uid", userId);
                        cmd.Parameters.AddWithValue("@amount", amount); // Truyền decimal trực tiếp
                        cmd.Parameters.AddWithValue("@note", note);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@catId", categoryId);
                        cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();

                        // Cập nhật Balance trong bảng Users
                        // Nếu type = 1 (Thu) thì + amount, nếu type = 0 (Chi) thì - amount
                        string sqlUpdateBalance = @"UPDATE Users 
                                            SET Balance = Balance + @diff 
                                            WHERE Id = @uid";

                        decimal difference = (type == 1) ? amount : -amount;
                        // Gán giá trị vào tham số cho câu lệnh cập nhật balance
                        MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdateBalance, conn, trans);
                        cmdUpdate.Parameters.AddWithValue("@diff", difference);
                        cmdUpdate.Parameters.AddWithValue("@uid", userId);
                        cmdUpdate.ExecuteNonQuery();
                        // Xác nhận hoàn tất
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        // Nếu có lỗi, rollback
                        trans.Rollback();
                        throw e; // Đẩy lỗi ra ngoài để UI xử lý
                    }
                }
            }
        }

        // Thêm lịch trình mới cho người dùng
        public void InsertSchedule(int userId, DateTime date, string timeStart, string timeEnd, string title, string desc)
        {
            //Tạo nối kết
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Truy vấn SQL để thêm lịch trình mới
                string sql = "INSERT INTO Schedule (UserId, Date, TimeStart, TimeEnd, Title, Description) " +
                             "VALUES (@uid, @date, @ts, @te, @title, @desc)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                // Gán giá trị vào tham số, đảm bảo định dạng ngày tháng đúng với database
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

        // Xóa giao dịch và cập nhật lại Balance trong bảng Users
        public void DeleteTransaction(int transactionId, int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Trước khi xóa, lấy thông tin Amount và Type của giao dịch để tính toán lại Balance
                        string getSql = "SELECT Amount, Type FROM Transactions WHERE Id = @id AND UserId = @uid";
                        MySqlCommand getCmd = new MySqlCommand(getSql, conn, trans);
                        getCmd.Parameters.AddWithValue("@id", transactionId);
                        getCmd.Parameters.AddWithValue("@uid", userId);

                        decimal amount = 0;
                        int type = 0;

                        using (MySqlDataReader rdr = getCmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                amount = Convert.ToDecimal(rdr["Amount"]);
                                type = Convert.ToInt32(rdr["Type"]);
                            }
                        }
                        // Xóa giao dịch
                        string deleteSql = "DELETE FROM Transactions WHERE Id = @id";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn, trans);
                        deleteCmd.Parameters.AddWithValue("@id", transactionId);
                        deleteCmd.ExecuteNonQuery();

                        decimal diff = (type == 1) ? -amount : amount;
                        // Cập nhật lại Balance trong bảng Users
                        string updateSql = "UPDATE Users SET Balance = Balance + @diff WHERE Id = @uid";
                        MySqlCommand updateCmd = new MySqlCommand(updateSql, conn, trans);
                        updateCmd.Parameters.AddWithValue("@diff", diff);
                        updateCmd.Parameters.AddWithValue("@uid", userId);
                        updateCmd.ExecuteNonQuery();

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        // Xóa lịch trình, chỉ xóa nếu lịch trình thuộc về người dùng đó để đảm bảo an toàn
        public void DeleteSchedule(int scheduleId, int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                // Truy vấn SQL để xóa lịch trình
                string sql = "DELETE FROM Schedule WHERE Id = @id AND UserId = @uid";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", scheduleId);
                cmd.Parameters.AddWithValue("@uid", userId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //Lấy danh sách tất cả category để hiển thị trong ComboBox khi thêm giao dịch mới
        public DataTable GetCategories()
        {
            //Tạo nối kết đến database và thực hiện truy vấn để lấy danh sách category
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "SELECT Id, Name FROM Categories";
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                //Sử dụng DataAdapter để điền dữ liệu vào DataTable, sau đó trả về DataTable này để UI có thể sử dụng làm DataSource cho ComboBox
                adapter.Fill(dt);
                return dt;
            }
        }
        //Lấy danh sách tất cả người dùng để hiển thị trong ComboBox khi đăng nhập
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
        // Tạo người dùng mới với tên, số dư ban đầu và ngày sinh
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
        // Lấy số dư hiện tại của người dùng để hiển thị trong MainForm
        public decimal GetUserBalance(int userId)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                string sql = "SELECT Balance FROM Users WHERE Id = @uid";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                conn.Open();
                // Sử dụng ExecuteScalar để lấy giá trị Balance trực tiếp, nếu không có kết quả trả về thì mặc định là 0
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
        }
    }

}
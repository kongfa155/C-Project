using ProjectC_.Theme;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectC_
{
    public partial class Login : Form
    {
        DatabaseHelper db = new DatabaseHelper();
        public Login()
        {
            InitializeComponent();
            LoadUserList();
        }
        //Lấy tất cả người dùng
        private void LoadUserList()
        {
            DataTable dt = db.GetUsers();
            //Gắn vào combo box
            cboUsers.DataSource = dt;
            cboUsers.DisplayMember = "Name";
            cboUsers.ValueMember = "Id";
        }
        //Xử lý đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Kiểm tra người dùng đã chọn chưa
            if (cboUsers.SelectedValue != null)
            {
                //Do app desktop và không có dữ liệu quan trọng nên không cần phải xác thực danh tính
                int userId = (int)cboUsers.SelectedValue;
                MainForm main = new MainForm(userId); // Truyền ID sang MainForm
                //Ẩn form này và mở main Form lên
                this.Hide();
                main.ShowDialog();
                this.Close();
            }
        }
        //Tạo người dùng mới, dùng để quản lý chi tiêu cho thành viên trong gia đình
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ form
            string name = txtName.Text;
            decimal balance = decimal.Parse(txtBalance.Text);
            DateTime birthday = dtpBirthday.Value;
            // Tạo người dùng mới trong database
            db.CreateUser(name, balance, birthday);
            MessageBox.Show("Tạo tài khoản thành công!");
            txtName.Clear();
            txtBalance.Clear();
            dtpBirthday.Value = DateTime.Now;
            LoadUserList(); // Refresh danh sách
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //Áp dụng theme cho form
            FormThemeHelper.ApplyLoginForm(this);
        }
        // Tùy chỉnh vẽ item cho combo box người dùng
        private void cboUsers_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Nếu không có item nào, thoát
            if (e.Index < 0) return;
            // Lấy combo box và item được chọn
            ComboBox cb = (ComboBox)sender;
            // Kiểm tra xem item có đang được chọn hay không
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            // Màu nền và màu chữ tùy theo trạng thái chọn
            Color backColor = isSelected ? ColorTranslator.FromHtml("#FFF6E5") : Color.White;
            // Màu chữ luôn giống nhau
            Color textColor = ColorTranslator.FromHtml("#5C3A21");

            // Lấy tên người dùng từ DataRowView
            DataRowView row = (DataRowView)cb.Items[e.Index];
            string text = row["Name"].ToString();

            // Background
            using (SolidBrush bg = new SolidBrush(backColor))
                e.Graphics.FillRectangle(bg, e.Bounds);

            // Text
            using (SolidBrush txt = new SolidBrush(textColor))
                e.Graphics.DrawString(text, e.Font, txt, e.Bounds);

            e.DrawFocusRectangle();
        }
    }
}

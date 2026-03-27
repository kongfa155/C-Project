using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void LoadUserList()
        {
            DataTable dt = db.GetUsers();
            cboUsers.DataSource = dt;
            cboUsers.DisplayMember = "Name";
            cboUsers.ValueMember = "Id";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedValue != null)
            {
                int userId = (int)cboUsers.SelectedValue;
                MainForm main = new MainForm(userId); // Truyền ID sang MainForm
                this.Hide();
                main.ShowDialog();
                this.Close();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            decimal balance = decimal.Parse(txtBalance.Text);
            DateTime birthday = dtpBirthday.Value;

            db.CreateUser(name, balance, birthday);
            MessageBox.Show("Tạo tài khoản thành công!");
            LoadUserList(); // Refresh danh sách
        }
    }
}

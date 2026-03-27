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
    public partial class FormAddSchedule : Form
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TimeSpan TimeStart { get; private set; }
        public TimeSpan TimeEnd { get; private set; }

        public FormAddSchedule()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề!");
                return;
            }

            Title = txtTitle.Text;
            Description = txtDescription.Text;
            TimeStart = dtStart.Value.TimeOfDay;
            TimeEnd = dtEnd.Value.TimeOfDay;

            if (TimeEnd <= TimeStart)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu!");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

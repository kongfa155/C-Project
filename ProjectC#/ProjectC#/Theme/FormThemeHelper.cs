using System.Drawing;
using System.Windows.Forms;

namespace ProjectC_.Theme
{
    public static class FormThemeHelper
    {
        // Áp dụng theme cho form nhập liệu (FormAddSpend, FormAddSchedule)
        public static void ApplyInputForm(Form form)
        {
            form.BackColor = ColorTranslator.FromHtml("#FFF6E5");

            foreach (Control ctrl in form.Controls)
            {
                StyleControl(ctrl);
            }
        }
        // Hàm đệ quy để áp dụng style cho tất cả control trong form, bao gồm cả các control con
        private static void StyleControl(Control ctrl)
        {
            // Label
            if (ctrl is Label lbl)
            {
                lbl.ForeColor = ColorTranslator.FromHtml("#5C3A21");
                lbl.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            }

            // TextBox
            else if (ctrl is TextBox txt)
            {
                txt.BackColor = Color.White;
                txt.ForeColor = Color.Black;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // ComboBox
            else if (ctrl is ComboBox cbo)
            {
                cbo.BackColor = Color.White;
                cbo.FlatStyle = FlatStyle.Flat;
            }

            // DateTimePicker
            else if (ctrl is DateTimePicker dt)
            {
                dt.CalendarMonthBackground = ColorTranslator.FromHtml("#FFF6E5");
            }

            // Button
            else if (ctrl is Button btn)
            {
                StyleButton(btn);
            }
        }
        // Áp dụng theme cho form đăng nhập
        public static void ApplyLoginForm(Form form)
        {
            form.BackColor = ColorTranslator.FromHtml("#FFF6E5");

            foreach (Control ctrl in form.Controls)
            {
                StyleLoginControl(ctrl);
            }
        }
        // Hàm đệ quy để áp dụng style cho tất cả control trong form đăng nhập, bao gồm cả các control con
        private static void StyleLoginControl(Control ctrl)
        {
            // Label
            if (ctrl is Label lbl)
            {
                lbl.ForeColor = ColorTranslator.FromHtml("#5C3A21");
            }

            // ComboBox
            else if (ctrl is ComboBox cbo)
            {
                cbo.BackColor = Color.White;
                cbo.FlatStyle = FlatStyle.Flat;
                cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            // TextBox
            else if (ctrl is TextBox txt)
            {
                txt.BackColor = Color.White;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }

            // DateTimePicker
            else if (ctrl is DateTimePicker dt)
            {
                dt.CalendarMonthBackground = ColorTranslator.FromHtml("#FFF6E5");
            }

            // GroupBox
            else if (ctrl is GroupBox gb)
            {
                gb.BackColor = ColorTranslator.FromHtml("#F5DEB3");
                gb.ForeColor = ColorTranslator.FromHtml("#5C3A21");
                gb.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            }

            // Button
            else if (ctrl is Button btn)
            {
                if (btn.Text.Contains("Đăng nhập"))
                    StyleButton(btn, "#E67E22"); // cam
                else
                    StyleButton(btn, "#8B5E3C"); // nâu
            }

            // Đệ quy cho các control con
            if (ctrl.HasChildren)
            {
                foreach (Control child in ctrl.Controls)
                {
                    StyleLoginControl(child);
                }
            }
        }
        // Hàm style cho button với màu sắc tùy chọn
        private static void StyleButton(Button btn, string hex)
        {
            Color color = ColorTranslator.FromHtml(hex);

            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Font = new Font("Segoe UI", 13, FontStyle.Bold);

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(color);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = color;
            };
        }
        // Hàm style cho button với màu sắc dựa trên text
        private static void StyleButton(Button btn)
        {
            if (btn.Text.Contains("Lưu"))
            {
                btn.BackColor = ColorTranslator.FromHtml("#E67E22"); // cam
            }
            else
            {
                btn.BackColor = ColorTranslator.FromHtml("#8B5E3C"); // nâu
            }

            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(btn.BackColor);
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn.Text.Contains("Lưu"))
                    btn.BackColor = ColorTranslator.FromHtml("#E67E22");
                else
                    btn.BackColor = ColorTranslator.FromHtml("#8B5E3C");
            };
        }
    }
}

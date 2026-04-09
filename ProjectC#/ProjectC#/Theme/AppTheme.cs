using System.Drawing;
using System.Windows.Forms;

namespace ProjectC_
{
    public static class AppTheme
    {
        // Bản màu chủ đạo của ứng dụng, lấy cảm hứng từ tông màu ấm áp của gỗ và nâu đất
        public static Color Background = ColorTranslator.FromHtml("#FFF6E5");
        public static Color Panel = ColorTranslator.FromHtml("#F5DEB3");

        public static Color Primary = ColorTranslator.FromHtml("#8B5E3C");
        public static Color PrimaryDark = ColorTranslator.FromHtml("#5C3A21");

        public static Color Accent = ColorTranslator.FromHtml("#E67E22");
        public static Color AccentLight = ColorTranslator.FromHtml("#F4A261");
        public static Color Highlight = ColorTranslator.FromHtml("#FFD166");

        public static Color Text = ColorTranslator.FromHtml("#5C3A21");
        public static Color White = Color.White;

        // Hàm gọi để áp dụng vào form
        public static void ApplyTheme(Form form)
        {
            form.BackColor = Background;
            ApplyControl(form.Controls);
        }
        // Hàm đệ quy để áp dụng theme cho tất cả control trong form, bao gồm cả các control con
        private static void ApplyControl(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                // Panel
                if (ctrl is Panel)
                {
                    ctrl.BackColor = Panel;
                }

                // Label
                else if (ctrl is Label lbl)
                {
                    lbl.ForeColor = Text;
                    lbl.BackColor = Color.Transparent;
                }

                // Button
                else if (ctrl is Button btn)
                {
                    StyleButton(btn);
                }

                // ComboBox
                else if (ctrl is ComboBox cbo)
                {
                    cbo.BackColor = White;
                    cbo.ForeColor = Text;
                    cbo.FlatStyle = FlatStyle.Flat;
                }

                // ListBox
                else if (ctrl is ListBox lst)
                {
                    lst.BackColor = Background;
                    lst.ForeColor = Text;
                    lst.BorderStyle = BorderStyle.None;
                }

                // TableLayoutPanel
                else if (ctrl is TableLayoutPanel)
                {
                    ctrl.BackColor = Background;
                }

                // Recursive
                if (ctrl.HasChildren)
                    ApplyControl(ctrl.Controls);
            }
        }

        // Ham style riêng cho button để tạo hiệu ứng hover và màu sắc nổi bật hơn
        private static void StyleButton(Button btn)
        {
            btn.BackColor = Accent;
            btn.ForeColor = White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            // Hover effect
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = AccentLight;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Accent;
            };
        }
    }
}

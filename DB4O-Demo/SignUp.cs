using Db4objects.Db4o.Linq;
using System;
using System.Windows.Forms;
using Db4objects.Db4o;

namespace DB4O_Demo
{
    public partial class SignUp : Form
    {
        private IObjectContainer db4o;
        public SignUp()
        {
            InitializeComponent();
        }

        // Phương thức kiểm tra để in thông tin người dùng
        public string Check(IObjectContainer db)
        {
            db = Db4oFactory.OpenFile("DB4O_DEMO.db4o");
            string s = "";
            var result = db.Query<User>();
            foreach (User user in result)
            {
                s += user.Name + " " + user.Password + Environment.NewLine;
            }
            return s;
            db.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string confirm_pass = textBox3.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirm_pass))
            {
                db4o = Db4oFactory.OpenFile("DB4O_DEMO.db4o");
                // Kiểm tra người dùng đã tồn tại
                var existingUsers = db4o.Query<User>(u => u.Name == username);
                if (existingUsers.Any())
                {
                    MessageBox.Show("Tên người dùng đã tồn tại. Vui lòng chọn tên khác.");
                    return;
                }

                // Kiểm tra khớp mật khẩu và xác nhận mật khẩu
                if (password != confirm_pass)
                {
                    MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp.");
                    return;
                }

                // Thêm người dùng mới
                var newUser = new User(username, password);
                db4o.Store(newUser);
                db4o.Commit();
                MessageBox.Show("Đăng ký thành công!");

                // Ẩn form đăng ký và hiện form đăng nhập
                db4o.Close();
                this.Hide();
                LogIn logIn = new LogIn();
                logIn.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            db4o.Close();
            LogIn logIn = new LogIn();
            this.Hide();
            logIn.ShowDialog();
            this.Close();
        }
    }
}



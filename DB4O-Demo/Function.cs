using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Db4objects.Db4o;

namespace DB4O_Demo
{
    public partial class Function : Form
    {
        private IObjectContainer db4o;
        private string maSV, maKh, phone, hoten;
        public Function()
        {
            InitializeComponent();
        }
        private void creatTextbox()
        {
            maSV = dungeonTextBox1.Text;
            maKh = dungeonTextBox2.Text;
            phone = dungeonTextBox3.Text;
            hoten = dungeonTextBox4.Text;
        }
        private void resetTextbox()
        {
            dungeonTextBox1.Text = "";
            dungeonTextBox2.Text = "";
            dungeonTextBox3.Text = "";
            dungeonTextBox4.Text = "";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 19);
            Brush brush = Brushes.Black;
            PointF point = new PointF(158, 12);
            g.DrawString("Thông tin sinh viên", font, brush, point);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            creatTextbox();
            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(maKh)
                || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(hoten))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin trước khi thêm");
                return;
            }

            db4o = Db4oFactory.OpenFile("DB4O_DEMO.db4o");

            // Kiểm tra sinh viên có tồn tại hay chưa
            var sinhvien_list = db4o.Query<SinhVien>(sv => sv.maSV == maSV || sv.phone == phone);
            if (sinhvien_list.Any())
            {
                MessageBox.Show("Sinh viên đã tồn tại!!!");
            }
            else
            {
                // Kiểm tra mã khoa
                var khoa_list = db4o.Query<Khoa>(k => k.maKh == maKh);
                if (!khoa_list.Any())
                {
                    MessageBox.Show("Không tồn tại khoa");
                }
                else
                {
                    // Tạo mới sinh viên và lưu vào cơ sở dữ liệu
                    SinhVien sv = new SinhVien(maSV, maKh, hoten, phone);
                    db4o.Store(sv);
                    MessageBox.Show("Thêm sinh viên thành công!");
                }
            }
            db4o.Close();
            resetTextbox();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            creatTextbox();
            if (string.IsNullOrEmpty(maSV) && (string.IsNullOrEmpty(maKh)
                || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(hoten)))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên trước khi sửa");
                return;
            }

            db4o = Db4oFactory.OpenFile("DB4O_DEMO.db4o");

            // Kiểm tra sinh viên có tồn tại hay chưa
            var sinhvien_list = db4o.Query<SinhVien>(sv => sv.maSV == maSV || sv.phone == phone);
            if (!sinhvien_list.Any())
            {
                MessageBox.Show("Sinh viên không có trong danh sách, không thể sửa!!!");
            }
            else
            {
                // Kiểm tra mã khoa
                var khoa_list = db4o.Query<Khoa>(k => k.maKh == maKh);
                if (!khoa_list.Any())
                {
                    MessageBox.Show("Không tồn tại khoa");
                }
                else
                {
                    // Lấy sinh viên đầu tiên (do maSV là duy nhất nên chỉ có một)
                    SinhVien sv = sinhvien_list.First();

                    // Cập nhật thông tin sinh viên
                    sv.maKh = maKh;
                    sv.nameSV = hoten;
                    sv.phone = phone;

                    // Lưu sinh viên đã cập nhật
                    db4o.Store(sv);
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!");
                }
            }
            db4o.Close();
            resetTextbox();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            maSV = dungeonTextBox1.Text;
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng nhập mã số sinh viên cần xóa");
                return;
            }

            db4o = Db4oFactory.OpenFile("DB4O_DEMO.db4o");

            // Tìm sinh viên theo mã sinh viên
            var sinhvien_list = db4o.Query<SinhVien>(sv => sv.maSV == maSV);
            if (!sinhvien_list.Any())
            {
                MessageBox.Show("Không tìm thấy sinh viên với mã số này!");
            }
            else
            {
                // Xóa sinh viên đầu tiên (vì maSV là duy nhất)
                SinhVien sv = sinhvien_list.First();
                db4o.Delete(sv);
                MessageBox.Show("Xóa sinh viên thành công!");
            }
            db4o.Close();
            resetTextbox();
        }
    }
}

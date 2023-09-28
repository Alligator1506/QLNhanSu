using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSu
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True");

        private void FormMain_Load(object sender, EventArgs e)
        {
            LayDSNS();
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStaff fs = new FormStaff();
            fs.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin fl = new FormLogin();
            fl.Show();
            this.Hide();
        }

        private void phòngBanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDepartment fd = new FormDepartment();
            fd.Show();
            this.Hide();
        }

        private void Reset()
        {
            txtHoTen.Text = "";
            txtID.Text = "";
            txtDiaChi.Text = "";
            rdoNam.Checked = false;
            rdoNu.Checked = false;
        }

        private void LayDSNS()
        {
            string query = "SELECT NhanVien.id, NhanVien.hoten, NhanVien.gioitinh, NhanVien.ngaysinh, NhanVien.email, NhanVien.diachi, NhanVien.phone, PhongBan.phongban, ChucVu.chucvu \r\nFROM NhanVien\r\nJOIN PhongBan on PhongBan.phongban_id = NhanVien.phongban_id\r\nJOIN ChucVu on ChucVu.chucvu_id = NhanVien.chucvu_id";
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                sda.Fill(dt);

                dtgDSNV.DataSource = dt;
                dtgDSNV.Columns[0].Width = 35;
                dtgDSNV.Columns[0].HeaderText = "ID";
                dtgDSNV.Columns[1].Width = 130;
                dtgDSNV.Columns[1].HeaderText = "Họ tên";
                dtgDSNV.Columns[2].Width = 80;
                dtgDSNV.Columns[2].HeaderText = "Giới tính";
                dtgDSNV.Columns[3].Width = 80;
                dtgDSNV.Columns[3].HeaderText = "Ngày sinh";
                dtgDSNV.Columns[4].Width = 135;
                dtgDSNV.Columns[4].HeaderText = "Email";
                dtgDSNV.Columns[5].Width = 80;
                dtgDSNV.Columns[5].HeaderText = "Địa chỉ";
                dtgDSNV.Columns[6].Width = 70;
                dtgDSNV.Columns[6].HeaderText = "Phone";
                dtgDSNV.Columns[7].Width = 90;
                dtgDSNV.Columns[7].HeaderText = "Phòng ban";
                dtgDSNV.Columns[8].Width = 90;
                dtgDSNV.Columns[8].HeaderText = "Chức vụ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string searchId = txtID.Text;
            string searchTen = txtHoTen.Text;
            string searchDiaChi = txtDiaChi.Text;
            string searchGioiTinh = "";
            if (rdoNam.Checked)
            {
                searchGioiTinh = "Nam";
            }
            else if (rdoNu.Checked)
            {
                searchGioiTinh = "Nữ";
            }

            bool hasSearchCriteria = !string.IsNullOrEmpty(searchId) || !string.IsNullOrEmpty(searchTen) || !string.IsNullOrEmpty(searchDiaChi) || !string.IsNullOrEmpty(searchGioiTinh);

            try
            {
                conn.Open();

                // Xây dựng câu truy vấn SQL dựa trên các tiêu chí tìm kiếm
                string query = "SELECT NhanVien.id, NhanVien.hoten, NhanVien.gioitinh, NhanVien.ngaysinh, NhanVien.email, NhanVien.diachi, NhanVien.phone, PhongBan.phongban, ChucVu.chucvu " +
                               "FROM NhanVien " +
                               "JOIN PhongBan on PhongBan.phongban_id = NhanVien.phongban_id " +
                               "JOIN ChucVu on ChucVu.chucvu_id = NhanVien.chucvu_id " +
                               "WHERE 1 = 0 ";

                if (!string.IsNullOrEmpty(searchId))
                {
                    query += "OR NhanVien.id = @searchId ";
                }
                if (!string.IsNullOrEmpty(searchTen))
                {
                    query += "OR NhanVien.hoten LIKE @searchTen ";
                }
                if (!string.IsNullOrEmpty(searchDiaChi))
                {
                    query += "OR NhanVien.diachi LIKE @searchDiaChi ";
                }
                if (!string.IsNullOrEmpty(searchGioiTinh))
                {
                    query += "OR NhanVien.gioitinh = @searchGioiTinh ";
                }

                // Tạo một đối tượng SqlCommand và thiết lập câu truy vấn và kết nối
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    // Thiết lập giá trị tham số cho các tiêu chí tìm kiếm
                    if (!string.IsNullOrEmpty(searchId))
                    {
                        command.Parameters.AddWithValue("@searchId", searchId);
                    }
                    if (!string.IsNullOrEmpty(searchTen))
                    {
                        command.Parameters.AddWithValue("@searchTen", "%" + searchTen + "%");
                    }
                    if (!string.IsNullOrEmpty(searchDiaChi))
                    {
                        command.Parameters.AddWithValue("@searchDiaChi", "%" + searchDiaChi + "%");
                    }
                    if (!string.IsNullOrEmpty(searchGioiTinh))
                    {
                        command.Parameters.AddWithValue("@searchGioiTinh", searchGioiTinh);
                    }

                    // Tạo một DataTable mới để lưu trữ kết quả tìm kiếm
                    DataTable dt = new DataTable();

                    // Thực thi truy vấn và điền dữ liệu vào DataTable
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    sda.Fill(dt);

                    // Đặt DataTable làm nguồn dữ liệu cho DataGridView
                    dtgDSNV.DataSource = dt;
                    Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            if (!hasSearchCriteria)
            {
                LayDSNS();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo một DataTable mới từ dữ liệu trong DataGridView
            DataTable dt = (DataTable)dtgDSNV.DataSource;

            // Kiểm tra xem có dữ liệu để thống kê hay không
            if (dt != null && dt.Rows.Count > 0)
            {
                // Sử dụng StringBuilder để xây dựng nội dung bản in
                StringBuilder sb = new StringBuilder();

                // Thực hiện thao tác thống kê và xây dựng nội dung bản in
                sb.AppendLine("DANH SÁCH NHÂN VIÊN");
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine();

                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendFormat("ID: {0}", row["id"]);
                    sb.AppendLine();
                    sb.AppendFormat("Họ tên: {0}", row["hoten"]);
                    sb.AppendLine();
                    sb.AppendFormat("Giới tính: {0}", row["gioitinh"]);
                    sb.AppendLine();
                    sb.AppendFormat("Ngày sinh: {0}", row["ngaysinh"]);
                    sb.AppendLine();
                    sb.AppendFormat("Email: {0}", row["email"]);
                    sb.AppendLine();
                    sb.AppendFormat("Địa chỉ: {0}", row["diachi"]);
                    sb.AppendLine();
                    sb.AppendFormat("Phone: {0}", row["phone"]);
                    sb.AppendLine();
                    sb.AppendFormat("Phòng ban: {0}", row["phongban"]);
                    sb.AppendLine();
                    sb.AppendFormat("Chức vụ: {0}", row["chucvu"]);
                    sb.AppendLine();
                    sb.AppendLine("--------------------------------------------------");
                    sb.AppendLine();
                }

                // In nội dung bản in
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += (s, ev) =>
                {
                    ev.Graphics.DrawString(sb.ToString(), new Font("Arial", 12), Brushes.Black, ev.MarginBounds.Left, ev.MarginBounds.Top);
                };
                pd.Print();
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để thống kê và in.");
            }
        }

        private void chứcVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPosition fp = new FormPosition();
            fp.Show();
            this.Hide();
        }
    }
}

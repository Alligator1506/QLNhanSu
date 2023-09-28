using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSu
{
    public partial class FormPosition : Form
    {
        public FormPosition()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True");

        private void FormPosition_Load(object sender, EventArgs e)
        {
            LayDSCV();
        }

        private void LayDSCV()
        {
            string query = "SELECT PhongBan.phongban_id, PhongBan.phongban, ChucVu.chucvu_id, ChucVu.chucvu FROM PhongBan , ChucVu";
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();

                sda.Fill(dt);
                dtgDSCV.DataSource = dt;
                conn.Close();
                dtgDSCV.Columns[0].Width = 70;
                dtgDSCV.Columns[0].HeaderText = "ID_PB";
                dtgDSCV.Columns[1].Width = 120;
                dtgDSCV.Columns[1].HeaderText = "Phòng ban";
                dtgDSCV.Columns[2].Width = 70;
                dtgDSCV.Columns[2].HeaderText = "ID_CV";
                dtgDSCV.Columns[3].Width = 120;
                dtgDSCV.Columns[3].HeaderText = "Chức vụ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool KiemTraThongTin()
        {
            if (txtID_PB.Text == "")
            {
                MessageBox.Show("Vui lòng điền ID.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID_PB.Focus();
                return false;
            }
            if (txtID_CV.Text == "")
            {
                MessageBox.Show("Vui lòng điền ID.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID_CV.Focus();
                return false;
            }
            if (txtChucVu.Text == "")
            {
                MessageBox.Show("Vui lòng điền chức vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtChucVu.Focus();
                return false;
            }
            return true;
        }

        private void Reset()
        {
            txtChucVu.Text = "";
            txtPhongBan.Text = "";
            txtID_PB.Text = "";
            txtID_CV.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTin())
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    // Tạo câu lệnh SQL để chèn dữ liệu vào bảng PhongBan và ChucVu
                    string sqlInsertCV = "INSERT INTO ChucVu (chucvu_id, chucvu,phongban_id) VALUES (@chucvu_id, @chucvu, @phongban_id)";

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;

                        // Thực thi câu lệnh SQL thêm dữ liệu vào bảng ChucVu
                        command.CommandText = sqlInsertCV;
                        command.Parameters.AddWithValue("@chucvu_id", txtID_CV.Text);
                        command.Parameters.AddWithValue("@chucvu", txtChucVu.Text);
                        command.Parameters.AddWithValue("@phongban_id", txtID_PB.Text);
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi thêm thành công, làm mới danh sách chức vụ
                    LayDSCV();

                    MessageBox.Show("Thêm chức vụ thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung các điều khiển để người dùng nhập lại thông tin
                    Reset();

                    // Đặt focus vào điều khiển đầu tiên để người dùng nhập thông tin mới
                    txtChucVu.Focus();
                }
                catch (SqlException ex)
                {
                    // Xử lý lỗi nếu cần thiết
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn một hàng trong DataGridView chưa
            if (dtgDSCV.SelectedRows.Count > 0)
            {
                // Lấy giá trị cột "phongban" từ hàng được chọn
                //string phongban = dtgDSPB.SelectedRows[0].Cells["phongban"].Value.ToString();

                int chucvuId = Convert.ToInt32(dtgDSCV.SelectedRows[0].Cells["chucvu_id"].Value);

                // Xác nhận xóa bằng hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa chức vụ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Mở kết nối CSDL
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        // Tạo câu lệnh SQL DELETE để xóa chức vụ dựa trên ID
                        string sqlDelete = "DELETE FROM ChucVu WHERE chucvu_id = @chucvu_id";
                        using (SqlCommand command = new SqlCommand(sqlDelete, conn))
                        {
                            // Đặt giá trị tham số cho câu lệnh SQL
                            command.Parameters.AddWithValue("@chucvu_id", chucvuId);

                            // Thực thi câu lệnh SQL DELETE
                            command.ExecuteNonQuery();
                        }

                        // Đóng kết nối CSDL
                        conn.Close();

                        // Cập nhật lại danh sách chức vụ trong DataGridView
                        LayDSCV();

                        Reset();

                        MessageBox.Show("Xóa chức vụ thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        // Xử lý ngoại lệ và thông báo lỗi
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTin())
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int selectedRowIndex = dtgDSCV.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dtgDSCV.Rows[selectedRowIndex];
                    int chucvuId = Convert.ToInt32(selectedRow.Cells[2].Value);
                    // Tạo câu lệnh SQL để cập nhật thông tin trong bảng PhongBan
                    string sqlUpdate = "UPDATE ChucVu SET chucvu = @chucvu WHERE chucvu_id = @chucvu_id";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, conn))
                    {
                        // Đặt các giá trị tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@chucvu_id", chucvuId);
                        command.Parameters.AddWithValue("@chucvu", txtChucVu.Text);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi cập nhật thành công, làm mới danh sách chức vụ
                    LayDSCV();

                    MessageBox.Show("Cập nhật thông tin chức vụ thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung các điều khiển để người dùng nhập lại thông tin
                    Reset();

                    // Đặt focus vào điều khiển đầu tiên để người dùng nhập thông tin mới
                    txtID_PB.Focus();
                }
                catch (SqlException ex)
                {
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                FormMain fm = new FormMain();
                fm.Show();
                this.Hide();
            }
        }

        private void FormPosition_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dtgDSCV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgDSCV.Rows[e.RowIndex];

                // Lấy thông tin từ hàng được chọn
                string ID_PB = row.Cells[0].Value.ToString();
                string phongban = row.Cells[1].Value.ToString();
                string ID_CV = row.Cells[2].Value.ToString();                
                string chucvu = row.Cells[3].Value.ToString();

                // Hiển thị thông tin trong các điều khiển trên Form
                txtID_PB.Text = ID_PB;
                txtID_CV.Text = ID_CV;
                txtChucVu.Text = chucvu;
                txtPhongBan.Text = phongban;
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa toàn bộ dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = (DataTable)dtgDSCV.DataSource;
                    originalDataTable = dt.Copy(); // Lưu trữ dữ liệu gốc trước khi xóa

                    dt.Rows.Clear();
                    dt.AcceptChanges();

                    MessageBox.Show("Xóa toàn bộ dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private DataTable originalDataTable; // Biến lưu trữ dữ liệu gốc
        private void btn_Load_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show("Bạn muốn load lại dữ liệu?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (originalDataTable != null)
                        {
                            // Hiển thị lại dữ liệu gốc trong DataTable
                            DataTable dt = (DataTable)dtgDSCV.DataSource;
                            dt.Rows.Clear();
                            dt.Merge(originalDataTable);
                            dt.AcceptChanges();

                            MessageBox.Show("Load dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi load dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

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
    public partial class FormDepartment : Form
    {
        public FormDepartment()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True");

        private void FormDepartment_Load(object sender, EventArgs e)
        {
            LayDSPB();
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();           
        }

        private void LayDSPB()
        {
            string query = "SELECT * FROM PhongBan";
            SqlDataAdapter sda = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            try
            {
                conn.Open();

                sda.Fill(dt);
                dtgDSPB.DataSource = dt;
                conn.Close();
                dtgDSPB.Columns[0].Width = 70;
                dtgDSPB.Columns[0].HeaderText = "ID_PB";
                dtgDSPB.Columns[1].Width = 120;
                dtgDSPB.Columns[1].HeaderText = "Phòng ban";          
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
            if (txtPhongBan.Text == "")
            {
                MessageBox.Show("Vui lòng điền phòng ban.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhongBan.Focus();
                return false;
            }
            return true;
        }

        private void Reset()
        {
            txtID_PB.Text = "";
            txtPhongBan.Text = "";
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn một hàng trong DataGridView chưa
            if (dtgDSPB.SelectedRows.Count > 0)
            {
                // Lấy giá trị cột "phongban" từ hàng được chọn
                //string phongban = dtgDSPB.SelectedRows[0].Cells["phongban"].Value.ToString();

                int phongbanId = Convert.ToInt32(dtgDSPB.SelectedRows[0].Cells["phongban_id"].Value);

                // Xác nhận xóa bằng hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa phòng ban này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Mở kết nối CSDL
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        // Tạo câu lệnh SQL DELETE để xóa phòng ban dựa trên ID
                        string sqlDelete = "DELETE FROM PhongBan WHERE phongban_id = @phongban_id";
                        using (SqlCommand command = new SqlCommand(sqlDelete, conn))
                        {
                            // Đặt giá trị tham số cho câu lệnh SQL
                            command.Parameters.AddWithValue("@phongban_id", phongbanId);

                            // Thực thi câu lệnh SQL DELETE
                            command.ExecuteNonQuery();
                        }

                        // Đóng kết nối CSDL
                        conn.Close();

                        // Cập nhật lại danh sách phòng ban trong DataGridView
                        LayDSPB();

                        Reset();

                        MessageBox.Show("Xóa phòng ban thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    int selectedRowIndex = dtgDSPB.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dtgDSPB.Rows[selectedRowIndex];
                    int phongbanId = Convert.ToInt32(selectedRow.Cells[0].Value);
                    // Tạo câu lệnh SQL để cập nhật thông tin trong bảng PhongBan
                    string sqlUpdate = "UPDATE PhongBan SET phongban = @phongban WHERE phongban_id = @phongban_id";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, conn))
                    {
                        // Đặt các giá trị tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@phongban_id", phongbanId);
                        command.Parameters.AddWithValue("@phongban", txtPhongBan.Text);                     

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi cập nhật thành công, làm mới danh sách phòng ban
                    LayDSPB();

                    MessageBox.Show("Cập nhật thông tin phòng ban thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                    // Tạo câu lệnh SQL để chèn dữ liệu vào bảng PhongBan 
                    string sqlInsertPB = "INSERT INTO PhongBan (phongban_id, phongban) VALUES (@phongban_id, @phongban)";                 
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;

                        // Thực thi câu lệnh SQL thêm dữ liệu vào bảng PhongBan
                        command.CommandText = sqlInsertPB;
                        command.Parameters.AddWithValue("@phongban_id", txtID_PB.Text);
                        command.Parameters.AddWithValue("@phongban", txtPhongBan.Text);
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi thêm thành công, làm mới danh sách phòng ban
                    LayDSPB();

                    MessageBox.Show("Thêm phòng ban thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung các điều khiển để người dùng nhập lại thông tin
                    Reset();

                    // Đặt focus vào điều khiển đầu tiên để người dùng nhập thông tin mới
                    txtPhongBan.Focus();
                }
                catch (SqlException ex)
                {
                    // Xử lý lỗi nếu cần thiết
                }
            }
        }

        private void FormDepartment_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dtgDSPB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgDSPB.Rows[e.RowIndex];

                // Lấy thông tin từ hàng được chọn
                string ID = row.Cells[0].Value.ToString();
                string phongBan = row.Cells[1].Value.ToString();

                // Hiển thị thông tin trong các điều khiển trên Form
                txtID_PB.Text = ID;
                txtPhongBan.Text = phongBan;
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa toàn bộ dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = (DataTable)dtgDSPB.DataSource;
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
            DialogResult result = MessageBox.Show("Bạn muốn load lại dữ liệu?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    if (originalDataTable != null)
                    {
                        // Hiển thị lại dữ liệu gốc trong DataTable
                        DataTable dt = (DataTable)dtgDSPB.DataSource;
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

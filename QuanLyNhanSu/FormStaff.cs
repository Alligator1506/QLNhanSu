using System;
using System.Collections;
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
    public partial class FormStaff : Form
    {
        public FormStaff()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True");

        private void FormStaff_Load(object sender, EventArgs e)
        {
            LayDSNS();
            // Đổ dữ liệu vào ComboBox "Phòng ban"
            string queryPhongBan = "SELECT phongban_id, phongban FROM PhongBan";
            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandPhongBan = new SqlCommand(queryPhongBan, conn);
                SqlDataReader readerPhongBan = commandPhongBan.ExecuteReader();
                while (readerPhongBan.Read())
                {
                    int phongBanId = readerPhongBan.GetInt32(0);
                    string phongBan = readerPhongBan.GetString(1);
                    cbPhongBan.Items.Add(phongBanId.ToString() + " - " + phongBan);
                }
                readerPhongBan.Close();
            }

            string queryID_PhongBan = "SELECT phongban_id FROM PhongBan";
            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandPhongBan = new SqlCommand(queryID_PhongBan, conn);
                SqlDataReader readerPhongBan = commandPhongBan.ExecuteReader();
                while (readerPhongBan.Read())
                {
                    int phongBanId = readerPhongBan.GetInt32(0);
                    cbID_PB.Items.Add(phongBanId.ToString());
                }
                readerPhongBan.Close();
            }

            // Đổ dữ liệu vào ComboBox "Chức vụ"
            string queryChucVu = "SELECT chucvu_id, chucvu FROM ChucVu";
            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandChucVu = new SqlCommand(queryChucVu, conn);
                SqlDataReader readerChucVu = commandChucVu.ExecuteReader();
                while (readerChucVu.Read())
                {
                    int chucvuId = readerChucVu.GetInt32(0);
                    string chucVu = readerChucVu.GetString(1);
                    cbChucVu.Items.Add(chucvuId.ToString() + " - " + chucVu);
                }
                readerChucVu.Close();
            }

            string queryID_ChucVu = "SELECT chucvu_id, chucvu FROM ChucVu";
            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandChucVu = new SqlCommand(queryID_ChucVu, conn);
                SqlDataReader readerChucVu = commandChucVu.ExecuteReader();
                while (readerChucVu.Read())
                {
                    int chucvuId = readerChucVu.GetInt32(0);
                    cbID_CV.Items.Add(chucvuId.ToString());
                }
                readerChucVu.Close();
            }
        }

        private void FormStaff_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void LayDSNS()
        {
            string query = "SELECT NhanVien.id, NhanVien.hoten, NhanVien.gioitinh, NhanVien.ngaysinh, NhanVien.email, NhanVien.diachi, NhanVien.phone, PhongBan.phongban, ChucVu.chucvu FROM NhanVien JOIN PhongBan on PhongBan.phongban_id = NhanVien.phongban_id JOIN ChucVu on ChucVu.chucvu_id = NhanVien.chucvu_id";
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);

                sda.Fill(dt);
                dtgDSNS.DataSource = dt;
                dtgDSNS.Columns[0].Width = 35;
                dtgDSNS.Columns[0].HeaderText = "ID";
                dtgDSNS.Columns[1].Width = 130;
                dtgDSNS.Columns[1].HeaderText = "Họ tên";
                dtgDSNS.Columns[2].Width = 80;
                dtgDSNS.Columns[2].HeaderText = "Giới tính";
                dtgDSNS.Columns[3].Width = 80;
                dtgDSNS.Columns[3].HeaderText = "Ngày sinh";
                dtgDSNS.Columns[4].Width = 135;
                dtgDSNS.Columns[4].HeaderText = "Email";
                dtgDSNS.Columns[5].Width = 80;
                dtgDSNS.Columns[5].HeaderText = "Địa chỉ";
                dtgDSNS.Columns[6].Width = 70;
                dtgDSNS.Columns[6].HeaderText = "Phone";
                dtgDSNS.Columns[7].Width = 90;
                dtgDSNS.Columns[7].HeaderText = "Phòng ban";
                dtgDSNS.Columns[8].Width = 90;
                dtgDSNS.Columns[8].HeaderText = "Chức vụ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool KiemTraThongTin()
        {
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng điền họ và tên nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHoTen.Focus();
                return false;
            }
            if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Vui lòng điền địa chỉ của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return false;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Vui lòng điền Email của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return false;
            }
            if (rdoNam.Checked == false && rdoNu.Checked == false)
            {
                MessageBox.Show("Vui lòng chọn giới tính cho nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Vui lòng điền số điện thoại của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return false;
            }
            if (cbPhongBan.Text == "")
            {
                MessageBox.Show("Vui lòng điền phòng ban của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbPhongBan.Focus();
                return false;
            }
            if (cbChucVu.Text == "")
            {
                MessageBox.Show("Vui lòng điền số chức vụ của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbChucVu.Focus();
                return false;
            }
            return true;
        }

        private void Reset()
        {
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtHoTen.Text = "";
            txtID.Text = "";
            txtPhone.Text = "";
            rdoNam.Checked = false;
            rdoNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            cbChucVu.Text = "";
            cbPhongBan.Text = "";
            cbID_PB.Text = "";
            cbID_CV.Text = "";
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

                    // Tạo câu lệnh SQL để chèn dữ liệu vào bảng NhanVien
                    string sqlInsert = "INSERT INTO NhanVien (id, hoten, gioitinh, ngaysinh, email, diachi, phone, phongban_id, chucvu_id) VALUES (@id,@hoten, @gioitinh, @ngaysinh, @email, @diachi, @phone, @phongban_id, @chucvu_id)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, conn))
                    {
                        // Đặt các giá trị tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@id", txtID.Text);
                        command.Parameters.AddWithValue("@hoten", txtHoTen.Text);
                        command.Parameters.AddWithValue("@gioitinh", rdoNam.Checked ? "Nam" : "Nữ");
                        command.Parameters.AddWithValue("@ngaysinh", DateTime.Parse(dtpNgaySinh.Value.ToString("yyyy-MM-dd")));
                        command.Parameters.AddWithValue("@email", txtEmail.Text);
                        command.Parameters.AddWithValue("@diachi", txtDiaChi.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@phongban_id", cbID_PB.Text);
                        command.Parameters.AddWithValue("@chucvu_id", cbID_CV.Text);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi thêm thành công, làm mới danh sách nhân viên
                    LayDSNS();

                    MessageBox.Show("Thêm nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung các điều khiển để người dùng nhập lại thông tin
                    Reset();

                    // Đặt focus vào điều khiển đầu tiên để người dùng nhập thông tin mới
                    txtID.Focus();
                }
                catch (SqlException ex)
                {
                    // Kiểm tra lỗi SQL để xác định lỗi cụ thể (ví dụ: lỗi khóa ngoại)
                    if (ex.Number == 547) // Lỗi khóa ngoại (foreign key violation)
                    {
                        MessageBox.Show("Phòng ban hoặc chức vụ không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    // Lấy ID của người được chọn trong DataGridView
                    int selectedRowIndex = dtgDSNS.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dtgDSNS.Rows[selectedRowIndex];
                    int id = Convert.ToInt32(selectedRow.Cells[0].Value);

                    // Tạo câu lệnh SQL để cập nhật thông tin trong bảng NhanVien
                    string sqlUpdate = "UPDATE NhanVien SET hoten = @hoten, gioitinh = @gioitinh, ngaysinh = @ngaysinh, email = @email, " +
                        "diachi = @diachi, phone = @phone, phongban_id = @phongban_id, chucvu_id = @chucvu_id WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(sqlUpdate, conn))
                    {
                        // Đặt các giá trị tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@hoten", txtHoTen.Text);
                        command.Parameters.AddWithValue("@gioitinh", rdoNam.Checked ? "Nam" : "Nữ");
                        command.Parameters.AddWithValue("@ngaysinh", DateTime.Parse(dtpNgaySinh.Value.ToString("yyyy-MM-dd")));
                        command.Parameters.AddWithValue("@email", txtEmail.Text);
                        command.Parameters.AddWithValue("@diachi", txtDiaChi.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@phongban_id", cbID_PB.Text);
                        command.Parameters.AddWithValue("@chucvu_id", cbID_CV.Text);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                    }

                    conn.Close();

                    // Sau khi cập nhật thành công, làm mới danh sách nhân viên
                    LayDSNS();

                    MessageBox.Show("Cập nhật thông tin nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xóa nội dung các điều khiển để người dùng nhập lại thông tin
                    Reset();

                    // Đặt focus vào điều khiển đầu tiên để người dùng nhập thông tin mới
                    txtHoTen.Focus();
                }
                catch (SqlException ex)
                {
                    // Kiểm tra lỗi SQL để xác định lỗi cụ thể (ví dụ: lỗi khóa ngoại)
                    if (ex.Number == 547) // Lỗi khóa ngoại (foreign key violation)
                    {
                        MessageBox.Show("Phòng ban hoặc chức vụ không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dtgDSNS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dtgDSNS.Rows[e.RowIndex];

                // Lấy thông tin từ hàng được chọn
                string id = row.Cells[0].Value.ToString();
                string hoTen = row.Cells[1].Value.ToString();
                string gioiTinh = row.Cells[2].Value.ToString();
                DateTime ngaySinh = Convert.ToDateTime(row.Cells[3].Value);
                string email = row.Cells[4].Value.ToString();
                string diaChi = row.Cells[5].Value.ToString();
                string phone = row.Cells[6].Value.ToString();
                string phongBan = row.Cells[7].Value.ToString();
                string chucVu = row.Cells[8].Value.ToString();

                // Hiển thị thông tin trong các điều khiển trên Form
                txtID.Text = id;
                txtHoTen.Text = hoTen;
                dtpNgaySinh.Value = ngaySinh;
                txtEmail.Text = email;
                txtDiaChi.Text = diaChi;
                txtPhone.Text = phone;
                cbPhongBan.Text = phongBan;
                cbChucVu.Text = chucVu;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn một hàng trong DataGridView chưa
            if (dtgDSNS.SelectedRows.Count > 0)
            {
                // Lấy giá trị của cột ID từ hàng được chọn
                int id = Convert.ToInt32(dtgDSNS.SelectedRows[0].Cells[0].Value);

                // Xác nhận xóa bằng hộp thoại xác nhận
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Mở kết nối CSDL
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        // Tạo câu lệnh SQL DELETE để xóa nhân viên dựa trên ID
                        string sqlDelete = "DELETE FROM NhanVien WHERE id = @id";
                        using (SqlCommand command = new SqlCommand(sqlDelete, conn))
                        {
                            // Đặt giá trị tham số cho câu lệnh SQL
                            command.Parameters.AddWithValue("@id", id);

                            // Thực thi câu lệnh SQL DELETE
                            command.ExecuteNonQuery();
                        }

                        // Đóng kết nối CSDL
                        conn.Close();

                        // Cập nhật lại danh sách nhân viên trong DataGridView
                        LayDSNS();

                        MessageBox.Show("Xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SqlException ex)
                    {
                        // Xử lý ngoại lệ và thông báo lỗi
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbID_CV_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy chucvu_id từ ComboBox cb_id_cv
            int selectedChucVuId = int.Parse(cbID_CV.SelectedItem.ToString().Split('-')[0].Trim());

            // Lấy tên phòng ban tương ứng với chucvu_id
            string queryChucVu = "SELECT chucvu FROM ChucVu WHERE chucvu_id = @chucvu_id";

            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandChucVu = new SqlCommand(queryChucVu, conn);
                commandChucVu.Parameters.AddWithValue("@chucvu_id", selectedChucVuId);

                // Lấy tên phòng ban từ cơ sở dữ liệu
                string chucVu = commandChucVu.ExecuteScalar().ToString();

                // Hiển thị tên phòng ban trong ComboBox cbChucVu
                cbChucVu.Text = chucVu;
            }
        }

        private void cbID_PB_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy phongban_id từ ComboBox cb_id_pb
            int selectedPhongBanId = int.Parse(cbID_PB.SelectedItem.ToString().Split('-')[0].Trim());

            // Lấy tên phòng ban tương ứng với phongban_id
            string queryPhongBan = "SELECT phongban FROM PhongBan WHERE phongban_id = @phongban_id";

            using (SqlConnection conn = new SqlConnection(@"Data Source=ALLIGATOR;Initial Catalog=QLNS;Integrated Security=True"))
            {
                conn.Open();
                SqlCommand commandPhongBan = new SqlCommand(queryPhongBan, conn);
                commandPhongBan.Parameters.AddWithValue("@phongban_id", selectedPhongBanId);

                // Lấy tên phòng ban từ cơ sở dữ liệu
                string phongBan = commandPhongBan.ExecuteScalar().ToString();

                // Hiển thị tên phòng ban trong ComboBox cbPhongBan
                cbPhongBan.Text = phongBan;
            }
        }
        private DataTable originalDataTable; // Biến lưu trữ dữ liệu gốc

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa toàn bộ dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    DataTable dt = (DataTable)dtgDSNS.DataSource;
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
                        DataTable dt = (DataTable)dtgDSNS.DataSource;
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

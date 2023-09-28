create database QLNS;
use QLNS;

CREATE TABLE Accounts
(
	username varchar(255),
	password varchar(255)
);

CREATE TABLE PhongBan
(
	phongban_id INT PRIMARY KEY,
	phongban NVARCHAR(100)
);

CREATE TABLE ChucVu
(
	chucvu_id INT PRIMARY KEY,
	chucvu NVARCHAR(100),
	phongban_id INT,
	FOREIGN KEY (phongban_id) REFERENCES PhongBan(phongban_id)
);

CREATE TABLE NhanVien
(
	id VARCHAR(100) PRIMARY KEY,
	hoten NVARCHAR(50),
	gioitinh NCHAR(10),
	ngaysinh DATE,
	email VARCHAR(100),
	diachi NVARCHAR(100),
	phone VARCHAR(20),
	phongban_id INT,
	chucvu_id INT,
	FOREIGN KEY (phongban_id) REFERENCES PhongBan(phongban_id),
	FOREIGN KEY (chucvu_id) REFERENCES ChucVu(chucvu_id)
);

insert into Accounts values('admin','admin');

insert into PhongBan values(1, N'Công nghệ');

insert into ChucVu values(1,N'Giám đốc',1);
insert into ChucVu values(2,N'Quản lý',1);

insert into NhanVien values('1',N'Nguyễn Bảo Quốc','Nam','2002/06/15','quoc150620@gmail.com',N'Hà Nội','0845069340', 1, 1);
insert into NhanVien values('2',N'Nguyễn Bảo Quốc',N'Nữ','2002/06/15','quoc150620@gmail.com',N'Hà Nội','0845069340', 1, 2);

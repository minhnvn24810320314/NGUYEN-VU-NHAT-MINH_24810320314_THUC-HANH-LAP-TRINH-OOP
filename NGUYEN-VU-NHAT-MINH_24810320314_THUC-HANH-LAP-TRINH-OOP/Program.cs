using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGUYEN_VU_NHAT_MINH_24810320314_THUC_HANH_LAP_TRINH_OOP
{
    // ==========================================
    // A. ABSTRACT CLASS PhuongTien (Lớp cha trừu tượng)
    // ==========================================
    public abstract class PhuongTien
    {
        // Private Fields
        private string _maPT = "PT000";
        private string _tenHang = string.Empty;
        private int _namSanXuat;
        private decimal _giaGoc;

        // Properties (Validation Encapsulation)
        public string MaPT
        {
            get => _maPT;
            set => _maPT = string.IsNullOrWhiteSpace(value) ? "PT000" : value.Trim();
        }

        public string TenHang
        {
            get => _tenHang;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hãng không được để trống!");
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get => _namSanXuat;
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value < 1900 || value > currentYear)
                    throw new ArgumentException("Năm sản xuất không hợp lệ!");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get => _giaGoc;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá gốc phải lớn hơn 0!");
                _giaGoc = value;
            }
        }

        // Constructor
        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        // Abstract Method
        public abstract decimal TinhGiaLanBanh();

        // Virtual Method
        public virtual string GetInfo()
        {
            return $"[Mã: {MaPT}] Hãng: {TenHang} | Năm SX: {NamSanXuat} | Giá gốc: {GiaGoc:N0} VNĐ";
        }
    }

    // ==========================================
    // B. CLASS OTo (Kế thừa từ PhuongTien)
    // ==========================================
    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get => _soChoNgoi;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải lớn hơn 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get => _dungTichDongCo;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải lớn hơn 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        // Override TinhGiaLanBanh()
        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {
                // Giá lăn bánh = Giá gốc + Lệ phí trước bạ (12%) + Thuế tiêu thụ đặc biệt (30%)
                return GiaGoc + (GiaGoc * 0.12m) + (GiaGoc * 0.30m);
            }
            else
            {
                // Giá lăn bánh = Giá gốc + Lệ phí trước bạ (10%)
                return GiaGoc + (GiaGoc * 0.10m);
            }
        }

        // Override GetInfo()
        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Ô tô ({SoChoNgoi} chỗ) | Động cơ: {DungTichDongCo}L | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }

    // ==========================================
    // C. CLASS XeMay (Kế thừa từ PhuongTien)
    // ==========================================
    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get => _dungTichXylanh;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xi-lanh phải lớn hơn 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        // Override TinhGiaLanBanh()
        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {
                // Thuế Trước bạ (2%)
                return GiaGoc + (GiaGoc * 0.02m);
            }
            else
            {
                // Thuế Trước bạ (5%)
                return GiaGoc + (GiaGoc * 0.05m);
            }
        }

        // Override GetInfo()
        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Xe máy ({DungTichXylanh} cc) | Giá lăn bánh: {TinhGiaLanBanh():N0} VNĐ";
        }
    }

    // ==========================================
    // D. CLASS QuanLyPhuongTien (Quản lý Tập hợp)
    // ==========================================
    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach = new List<PhuongTien>();

        // 1. Thêm phương tiện mới
        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt != null)
                _danhSach.Add(pt);
        }

        // 2. In danh sách toàn bộ phương tiện kèm Giá lăn bánh
        public void DisplayAll()
        {
            Console.WriteLine("\n=== DANH SÁCH TOÀN BỘ PHƯƠNG TIỆN ===");
            if (_danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }

            foreach (var pt in _danhSach)
            {
                Console.WriteLine(pt.GetInfo());
            }
        }

        // 3. Tìm phương tiện có Giá lăn bánh cao nhất (Đa hình)
        public PhuongTien? FindMaxGiaLanBanh()
        {
            if (_danhSach.Count == 0) return null;
            return _danhSach.OrderByDescending(pt => pt.TinhGiaLanBanh()).FirstOrDefault();
        }

        // 4. Tìm danh sách phương tiện theo tên hãng (dùng LINQ)
        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<PhuongTien>();

            return _danhSach
                .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    // ==========================================
    // KỊCH BẢN KIỂM THỬ (TEST CASES)
    // ==========================================
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("==========================================================");
            Console.WriteLine("  BÀI THỰC HÀNH OOP: QUẢN LÝ PHƯƠNG TIỆN GIAO THÔNG");
            Console.WriteLine("  Họ tên: Nguyễn Vũ Nhật Minh - MSSV: 24810320314");
            Console.WriteLine("==========================================================\n");

            QuanLyPhuongTien ql = new QuanLyPhuongTien();

            // ----------------------------------------------------
            // TC01: Kiểm tra Validation Năm sản xuất (NamSanXuat = 1850)
            // ----------------------------------------------------
            Console.WriteLine("--- [TC01] Kiểm tra Validation Năm sản xuất = 1850 ---");
            try
            {
                OTo otoLoi = new OTo("OT001", "Toyota", 1850, 1000000000m, 5, 2.0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[KẾT QUẢ TC01 - BẮT LỖI MANG LẠI]: {ex.Message}");
            }

            // ----------------------------------------------------
            // TC02: Kiểm tra Tính Giá Lăn Bánh Ô tô (5 chỗ, 1,000,000,000 VNĐ)
            // ----------------------------------------------------
            Console.WriteLine("\n--- [TC02] Ô tô 5 chỗ, Giá gốc = 1,000,000,000 VNĐ ---");
            OTo oto5Cho = new OTo("OT002", "Honda CR-V", 2023, 1000000000m, 5, 1.5);
            decimal giaOTo = oto5Cho.TinhGiaLanBanh();
            Console.WriteLine($"Giá lăn bánh tính được: {giaOTo:N0} VNĐ");
            Console.WriteLine($"Kỳ vọng: 1,420,000,000 VNĐ => {(giaOTo == 1420000000m ? "ĐÚNG OK" : "SAI")}");

            // ----------------------------------------------------
            // TC03: Kiểm tra Tính Giá Lăn Bánh Xe máy (150cc, 50,000,000 VNĐ)
            // ----------------------------------------------------
            Console.WriteLine("\n--- [TC03] Xe máy 150cc, Giá gốc = 50,000,000 VNĐ ---");
            XeMay xeMay150 = new XeMay("XM001", "Honda SH", 2022, 50000000m, 150);
            decimal giaXeMay = xeMay150.TinhGiaLanBanh();
            Console.WriteLine($"Giá lăn bánh tính được: {giaXeMay:N0} VNĐ");
            Console.WriteLine($"Kỳ vọng: 51,000,000 VNĐ => {(giaXeMay == 51000000m ? "ĐÚNG OK" : "SAI")}");

            // ----------------------------------------------------
            // TC04: Kiểm tra Đa hình List<PhuongTien>
            // ----------------------------------------------------
            Console.WriteLine("\n--- [TC04] Kiểm tra Đa hình List<PhuongTien> ---");
            ql.AddPhuongTien(oto5Cho);
            ql.AddPhuongTien(xeMay150);
            ql.DisplayAll();

            // ----------------------------------------------------
            // TC05: Kiểm tra Tìm Giá Lăn Bánh Max
            // ----------------------------------------------------
            Console.WriteLine("\n--- [TC05] Kiểm tra Tìm Giá Lăn Bánh Max ---");
            PhuongTien? maxPT = ql.FindMaxGiaLanBanh();
            if (maxPT != null)
            {
                Console.WriteLine($"Phương tiện có giá lăn bánh cao nhất: {maxPT.TenHang} ({maxPT.TinhGiaLanBanh():N0} VNĐ)");
            }

            Console.WriteLine("\n==========================================================");
            Console.WriteLine("  ĐÃ HOÀN THÀNH TOÀN BỘ CÁC TEST CASES THỰC HÀNH OOP!");
            Console.WriteLine("==========================================================");
            Console.ReadLine();
        }
    }
}
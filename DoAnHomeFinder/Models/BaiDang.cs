using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnHomeFinder.Models
{
    public class BaiDang
    {
        public string id { get; set; }

        [Display(Name = "Bếp nấu")]
        public bool bep_nau { get; set; }

        [Display(Name = "Địa chỉ")]
        public string dia_chi { get; set; }

        [Display(Name = "Diện tích")]
        public string dien_tich { get; set; }

        [Display(Name = "Điều hòa")]
        public bool dieu_hoa { get; set; }

        [Display(Name = "Giá")]
        public string gia { get; set; }

        [Display(Name = "Giữ xe")]
        public bool giu_xe { get; set; }

        [Display(Name = "Mã bài đăng")]
        public string id_bai_dang { get; set; }

        [Display(Name = "Mã loại bài đăng")]
        public string id_loai_bai_dang { get; set; }

        [Display(Name = "Mã người gửi")]
        public string id_nguoi_dang { get; set; }

        [Display(Name = "Danh sách ảnh")]
        public List<string> list_image { get; set; }

        [Display(Name = "Máy giặt")]
        public bool may_giat { get; set; }

        [Display(Name = "Mô tả")]
        public string mo_ta { get; set; }

        [Display(Name = "Họ tên")]
        public string name { get; set; }

        [Display(Name = "Nhà vệ sinh")]
        public bool nha_ve_sinh { get; set; }

        [Display(Name = "SĐT")]
        public string sdt { get; set; }

        [Display(Name = "Thời gian đăng")]
        public string thoi_gian { get; set; }

        [Display(Name = "Tiêu đề")]
        public string tieu_de { get; set; }

        [Display(Name = "Trạng thái bài đăng")]
        public bool trang_thai_bai_dang { get; set; }

        [Display(Name = "Trạng thái duyệt")]
        public bool trang_thai_duyet { get; set; }

        [Display(Name = "Tự do")]
        public bool tu_do { get; set; }

        [Display(Name = "Tủ lạnh")]
        public bool tu_lanh { get; set; }

        [Display(Name = "Wifi")]
        public bool wifi { get; set; }
    }
}
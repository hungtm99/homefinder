using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnHomeFinder.Models
{
    public class NguoiDung
    {
        public string id { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Mã người dùng")]
        public string id_nguoi_dung { get; set; }

        [Display(Name = "Mật khẩu")]
        public string mat_khau { get; set; }

        [Display(Name = "Quyền")]
        public int quyen { get; set; }

        [Display(Name = "Số điện thoại")]
        public string sdt { get; set; }

        [Display(Name = "Họ và Tên")]
        public string ten { get; set; }
    }
}
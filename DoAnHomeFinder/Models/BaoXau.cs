using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnHomeFinder.Models
{
    public class BaoXau
    {
        [Display(Name = "Mã bài đăng")]
        public string id_bai_dang { get; set; }

        [Display(Name = "Mã báo cáo")]
        public string id_baocao { get; set; }

        [Display(Name = "Mã người gửi")]
        public string id_nguoi_dung { get; set; }

        [Display(Name = "Nội dung")]
        public string noi_dung { get; set; }

        [Display(Name = "Thời gian")]
        public string thoi_gian { get; set; }

        [Display(Name = "Tiêu đề")]
        public string tieu_de { get; set; }
    }
}
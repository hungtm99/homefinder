using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnHomeFinder.Models
{
    public class BaiViet
    {
        [Display(Name = "Mã bài viết")]
        public string id_bai_viet { get; set; }

        [Display(Name = "Tiêu đề")]
        public string tieu_de { get; set; }

        [Display(Name = "Nội dung")]
        public string noi_dung { get; set; }

        [Display(Name = "Danh sách ảnh")]
        public string hinh_anh { get; set; }
    }
}
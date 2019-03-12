using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web.Models
{
    public class User
    {
        [Display(Name = "编号", AutoGenerateFilter = true, Description = "编号是自动产生的")]
        [RegularExpression(@"^\d?$", ErrorMessage = "编号必须是数字")]
        public int Id
        {
            get;
            set;
        }
        [Display(Name = "姓名")]
        [Required]
        public string Name
        {
            get;
            set;
        }
        [Display(Name = "密码")]
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
        [Display(Name = "备注")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Remark
        {
            get;
            set;
        }
        [Display(Name = "生日")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday
        {
            get;
            set;
        }
        [Display(Name = "照片", Description = "上传自己的照片")]
        [DataType(DataType.ImageUrl)]
        public String Photo
        {
            get;
            set;
        }
        public override string ToString()
        {
            return string.Format("编号：{0}，姓名：{1}，密码：{2}，生日：{3}", this.Id, this.Name, this.Password, this.Birthday);
        }
    }
}
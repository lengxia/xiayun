using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace xiayun.Model
{
    public class User
    {
        [Key]
        public int Id { set; get; }

        [MaxLength(64, ErrorMessage = "昵称不应该超过32个字符")]
        [Required(ErrorMessage = "请填写昵称")]
        [Display(Name = "昵称")]
        public string Username { set; get; }

        [StringLength(32, ErrorMessage = "密码不能超过32位")]
        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace xiayun.Model
{
    public class File
    {
        [Key]
        public int Id { set; get; }

        [Display(Name = "文件路径")]
        [StringLength(1024)]
        public string FilePath { set; get; }

        [Display(Name = "文件名")]
        [StringLength(1024)]
        public string FileName { set; get; }

        [Display(Name = "文件名扩展名")]
        [StringLength(8)]
        public string FileExt { set; get; }

        [Display(Name = "文件拥有者")]
        [StringLength(20)]
        public string Belong { set; get; }

        [Display(Name = "修改时间")]
        [StringLength(100)]
        public string UpTime { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using xiayun.Model;
namespace xiayun.DAL
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("name=MyContext")
        {
           
        }
        

         public DbSet<File> Files { set; get; }
         public DbSet<User> Users { set; get; } 
    }
}

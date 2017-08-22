using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace xiayun.DAL
{
    public class ContextFactory
    {
        public static MyContext GetDbContext()
        {

            // Database.SetInitializer(new DropCreateDatabaseAlways<MyContext>());
            MyContext _netDiskContext = new MyContext();

            if (_netDiskContext.Database.Exists())
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MyContext>());
            else
                Database.SetInitializer(new DropCreateDatabaseAlways<MyContext>());

            return _netDiskContext;
        }
    }
}

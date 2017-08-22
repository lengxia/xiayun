using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using xiayun.DAL;
using PagedList;
using System.Collections;
using System.Text.RegularExpressions;
using xiayun.web.Models;
namespace xiayun.web.Controllers
{
    [CustomAuth("", "")]
    public class HomeController : Controller
    {
        // GET: Home
        FileRepository fileinfo = new FileRepository();
        
        public ActionResult Index(int? page,int? id,string filename,string nowdo,string path,string search)
        {
            int pag = page ?? 1;
            ViewData["page"] = pag;
            IPagedList<xiayun.Model.File> files;
            if (path == null)
            {
                path = "Home";
                ViewData["path"] = "Home";
            }
            else
            {
                ViewData["path"] = path;
            }
           

            if (id != null)
            {   xiayun.Model.File file = fileinfo.Find(x => x.Id == id);
                string npath=file.FilePath+'/'+file.FileName;
                
                IEnumerable<xiayun.Model.File> filess=fileinfo.FindAll(x=>x.FilePath.StartsWith(npath));
            if (nowdo == "delete")
            {
                
                foreach (var item in filess)
                {
                    fileinfo.Delete(item);

                }
                fileinfo.Delete(file);
            }
            else if (nowdo=="update")
            {
                foreach (var item in filess)
                {
                    //item.FilePath.Replace("we", "nihao");
                    item.FilePath=item.FilePath.Replace(file.FileName, filename);
                    item.UpTime = DateTime.Now.ToString();
                    fileinfo.Update(item);
                    /*
                    item.FilePath.Replace("we", "he");
                    item.UpTime = DateTime.Now.ToString();
                    fileinfo.Update(item);
                    */
                }

                file.FileName = filename;
                file.UpTime = DateTime.Now.ToString();
                fileinfo.Update(file);
            }
                fileinfo.SaveChanges();

            }
            if (nowdo == "add")
            {
                string foldername="新建文件夹";
                int i=0;
                while(true)
                {
                   bool p=fileinfo.Exist(x => x.FileName == foldername + i);
                   if (p == false) break;
                        i++;
                }
                 fileinfo.Add(new xiayun.Model.File
                {
                    FileName = foldername + i,
                    Belong = Session["user"].ToString(),
                      FileExt="folder",
                       UpTime=DateTime.Now.ToString(),
                        FilePath=path,
                        

                });
                fileinfo.SaveChanges();

            }
            if (search != null)
            {
                files = fileinfo.GetPaged(x => x.Belong == Session["user"].ToString() & x.FileName.Contains(search), x => x.UpTime, pag, 10, false);
                ViewData["search"] = search;
                return View(files);
            }
            files = fileinfo.GetPaged(x => x.Belong==Session["user"].ToString()&x.FilePath==path, x => x.UpTime, pag, 10, false);
            return View(files);
        }
        public ActionResult add()
        {
           
            for(int i=0;i<100;i++){
                DateTime now=DateTime.Now;
                string time =now.ToString();

                fileinfo.Add(new xiayun.Model.File
                {
                    FileName = "nowfolder" + i,
                     Belong=Session["user"].ToString(),
                      FileExt="folder",
                       UpTime=time,
                        FilePath="Home",
                        

                });
                
            }
            fileinfo.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
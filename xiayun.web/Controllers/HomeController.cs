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
using System.IO;
namespace xiayun.web.Controllers
{
    [CustomAuth("", "")]
    public class HomeController : Controller
    {
        // GET: Home
        FileRepository fileinfo = new FileRepository();
        
        public ActionResult Index(int? page,int? id,string filename,string nowdo,string path,string search,string attr)
        {
            int pag = page ?? 1;
            ViewData["page"] = pag;
            ViewData["attr"] = attr;
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
            if (attr!=null){
                string images = ".bmp,.jpg,.png,.tiff,.gif,.pcx,.tga,.exif,.fpx,.svg,.psd,.cdr,.pcd,.dxf,.ufo,.eps,.ai,.raw,.WMF";
                string musics = ",.mp3,.wma,.wav,.asf,.aac,.vqf,.falc,.ape,.mid,.ogg";
                string videos = ".rm,.rmvb,.mp4,.mov,.mtv,.dat,.wmv,.avi,.3gp,.amv,.dmv";
                switch (attr)
                {
                    case "img": files = fileinfo.GetPaged(x => x.Belong ==Session["user"].ToString() & images.Contains(x.FileExt.ToLower()), x =>x.UpTime , pag, 10, false); break;
                    case "music": files = fileinfo.GetPaged(x => x.Belong == Session["user"].ToString() & musics.Contains(x.FileExt.ToLower()), x => x.UpTime, pag, 10, false); break;
                    case "video": files = fileinfo.GetPaged(x => x.Belong == Session["user"].ToString() & videos.Contains(x.FileExt.ToLower()), x => x.UpTime, pag, 10, false); break;
                    default: files = fileinfo.GetPaged(x => x.Belong == Session["user"].ToString() , x => x.UpTime, pag, 10, false); break;


                }
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

        [HttpPost]
        public JsonResult PostedFile()
        {

            var file = HttpContext.Request.Files[0];
            if (file == null)
            {
                return new JsonResult() { Data = "{\"code\":503,\"msg\":\"没有选择文件\"}" };
            }
            string fileName = Path.GetFileName(file.FileName);
            string fileExt = Path.GetExtension(file.FileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
            string userName = Session["user"].ToString();
            string fileRelativePath =Request.Form["path"];
            try
            {
                string fileSaveDir = Server.MapPath("~\\upload\\" + Session["user"] +"\\"+ fileRelativePath);
                if (!Directory.Exists(fileSaveDir))
                {
                    Directory.CreateDirectory(fileSaveDir);
                }
                string fileSavePath = fileSaveDir +"\\"+ fileName;
                if (!System.IO.File.Exists(fileSavePath))
                {
                    file.SaveAs(fileSavePath);
                }

                var savefile = new xiayun.Model.File
                {
                    FileName = fileName,
                    FileExt = fileExt,
                    FilePath = fileRelativePath,
                    Belong = Session["user"].ToString(),
                     UpTime=DateTime.Now.ToString(),
                };

                fileinfo.Add(savefile);
                fileinfo.SaveChanges();
                return new JsonResult() { Data = "{\"code\":200,\"msg\":\"上传成功\"}" };

            }
            catch (Exception e)
            {
                return new JsonResult() { Data = "{\"code\":500,\"msg\":\"上传失败\"}" };

            }

        }
    }
}
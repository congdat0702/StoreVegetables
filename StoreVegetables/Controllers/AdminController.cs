using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreVegetables.Models;
using System.IO;
using PagedList;

namespace StoreVegetables.Controllers
{
    public class AdminController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var tendn = f["txtuser"];
            var matkhau = f["txtpass"];
            if (String.IsNullOrEmpty(tendn))
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            else if (String.IsNullOrEmpty(matkhau))
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            else
            {
                var ad = data.Admins.SingleOrDefault(n => n.useradmin == tendn && n.password == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
            }

            return View();
        }
        public ActionResult SanPham(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 7;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.SanPhams.ToList().OrderByDescending(n => n.spID).ToPagedList(pagenum, pagesize));
            }
        }
        public ActionResult Chitietsanpham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sp = from s in data.SanPhams where s.spID == id select s;
                return View(sp.SingleOrDefault());
            }
        }
        public ActionResult DeleteSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sp = from s in data.SanPhams where s.spID == id select s;
                return View(sp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("DeleteSanPham")]
        public ActionResult CofirmDeleteSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sp = data.SanPhams.SingleOrDefault(n => n.spID == id);
                data.SanPhams.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }

        [HttpGet]
        public ActionResult CreateSanPham()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.theloaiID = new SelectList(data.TheLoais.ToList().OrderBy(n => n.tenTheLoai), "theloaiID", "tenTheLoai");
                ViewBag.nsxID = new SelectList(data.NhaSanXuats.ToList().OrderBy(n => n.tenNSX), "nsxID", "tenNSX");
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CofirmCreateSanPham(SanPham sp, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        sp.hinhSanPham = fileName;
                        //Luu vao CSDL
                        data.SanPhams.InsertOnSubmit(sp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("SanPham", "Admin");
                }
            }
        }
        public ActionResult EditSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sp = data.SanPhams.SingleOrDefault(n => n.spID == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn theloai tương tưng 
                ViewBag.theloaiID = new SelectList(data.TheLoais.ToList().OrderBy(n => n.tenTheLoai), "theloaiID", "tenTheLoai", sp.theloaiID);
                ViewBag.nsxID = new SelectList(data.NhaSanXuats.ToList().OrderBy(n => n.tenNSX), "nsxID", "tenNSX", sp.nsxID);
                return View(sp);
            }
        }
        [HttpPost, ActionName("EditSanPham")]
        public ActionResult CofirmEditSanPham(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sp = data.SanPhams.SingleOrDefault(n => n.spID == id);
                UpdateModel(sp);
                data.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }
       
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Hinhsanpham/" + file.FileName));
            return "/Hinhsanpham/" + file.FileName;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreVegetables.Models;

namespace Webbansach.Controllers
{
    public class NhasanxuatController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        //1. Hiện thị danh sách các nhà xuất bản
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(data.NhaSanXuats.ToList());
        }
        //2. Hiện thi chi tiết 1 nhà xuất bản
        public ActionResult Details(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhaxuatban = from nxb in data.NhaSanXuats where nxb.nsxID == id select nxb;
                return View(nhaxuatban.SingleOrDefault());
            }
        }
        //3. Thêm mới Nhà xuất bản
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Create(NhaSanXuat nhasanxuat)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                data.NhaSanXuats.InsertOnSubmit(nhasanxuat);
                data.SubmitChanges();
                
                return RedirectToAction("Index", "Nhasanxuat");
            }
        }
        //4. Xóa 1 Nhà xuất bản gồm 2 trang: xác nhận xóa và xử lý xóa
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhasanxuat = from nsx in data.NhaSanXuats where nsx.nsxID == id select nsx;
                return View(nhasanxuat.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NhaSanXuat nhasanxuat = data.NhaSanXuats.SingleOrDefault(n => n.nsxID==id);
                data.NhaSanXuats.DeleteOnSubmit(nhasanxuat);
                data.SubmitChanges();
                
                return RedirectToAction("Index", "Nhasanxuat");
            }
        }
        //5. Điều chỉnh thông tin 1  Nhà xuất bản gồm 2 trang: Xem và điều chỉnh và cập nhật lưu lại
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhasanxuat = from nsx in data.NhaSanXuats where nsx.nsxID == id select nsx;
                return View(nhasanxuat.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Capnhat(int id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                NhaSanXuat nhasanxuat = data.NhaSanXuats.SingleOrDefault(n => n.nsxID == id);

                UpdateModel(nhasanxuat);
                data.SubmitChanges();
                return RedirectToAction("Index", "Nhasanxuat");
            }
        }
    }
}
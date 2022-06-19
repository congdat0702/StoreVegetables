using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreVegetables.Models;
using PagedList;
using PagedList.Mvc;
namespace StoreVegetables.Controllers
{
    public class StoreVegetablesController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        // GET: StoreVegetables
        private List<SanPham> Laysanphammoi(int count)
        {
            //Sắp xếp sách theo id, sau đó lấy top @count 
            return data.SanPhams.OrderByDescending(a => a.spID).Take(count).ToList();
        }
        //Phuong thức Index: Không có tham số (null) hoặc có tham số là số nguyên (biến page)
        public ActionResult Index(int? page)
        {
            //kích thước trang = số mẫu tin cho 1 trang
            int pagesize = 5;
            //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
            int pagenum = (page ?? 1);
            //Lấy top 5 Album bán chạy nhất
            var spnew = Laysanphammoi(15);
            return View(spnew.ToPagedList(pagenum, pagesize));
        }
        public ActionResult Theloai()
        {
            var theloai = from tl in data.TheLoais select tl;
            return PartialView(theloai);
        }
        public ActionResult Nhasanxuat()
        {
            var Nhasanxuat = from nxb in data.NhaSanXuats select nxb;
            return PartialView(Nhasanxuat);
        }
        public ActionResult Sptheotheloai(int id)
        {
            var sp = from s in data.SanPhams where s.theloaiID == id select s;
            return View(sp);

        }
        public ActionResult SptheoNSX(int id)
        {
            var sp = from s in data.SanPhams where s.nsxID == id select s;
            return View(sp);

        }
        public ActionResult Chitietsanpham(int id)
        {
            var sp = from s in data.SanPhams where s.spID == id select s;
            return View(sp.Single());
        }
    }
}
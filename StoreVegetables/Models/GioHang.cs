using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreVegetables.Models;


namespace StoreVegetables.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();

        public int ispID { set; get; }
        public string stenSanPham { set; get; }
        public string shinhSanPham { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        public GioHang(int spID)
        {
            ispID = spID;
            SanPham sp = data.SanPhams.Single(n => n.spID == ispID);
            stenSanPham = sp.tenSanPham;
            shinhSanPham = sp.hinhSanPham;
            dDongia = double.Parse(sp.giaSanPham.ToString());
            iSoluong = 1;
        }
    }
}
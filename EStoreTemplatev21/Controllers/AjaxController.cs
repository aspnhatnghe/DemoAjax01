using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EStoreTemplatev21.Models;
using Microsoft.AspNetCore.Mvc;

namespace EStoreTemplatev21.Controllers
{
    public class AjaxController : Controller
    {
        MyeStoreContext ctx;
        public AjaxController(MyeStoreContext db)
        {
            ctx = db;
        }

        public IActionResult SearchJson(string name, double from, double to)
        {
            var data = ctx.HangHoa.AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                data = data.Where(p => p.TenHh.Contains(name));
            }
            if(from > 0)
            {
                data = data.Where(p => p.DonGia >= from);
            }
            if (to > from)
            {
                data = data.Where(p => p.DonGia <= to);
            }
            return Json(data.Select(p => new {
                MaHH = p.MaHh,
                TenHH = p.TenHh,
                Hinh = fullPath("HangHoa", p.Hinh),
                DonGia = p.DonGia.Value,
                Loai = p.MaLoaiNavigation.TenLoai
            }));
        }

        public string fullPath(string thumuc, string tenfile)
        {            
            string http = HttpContext.Request.IsHttps ? "https" : "http";
            return $"{http}://{HttpContext.Request.Host}/Hinh/{thumuc}/{tenfile}";
        }
        public IActionResult TimKiem()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var data = ctx.HangHoa.Where(p => p.TenHh.Contains(keyword))
                .Select(p => new HangHoaViewModel {
                    MaHh = p.MaHh, TenHh = p.TenHh,
                    DonGia = p.DonGia.Value, Hinh = p.Hinh
                });
            return PartialView("SearchResult", data);
        }

        public string ServerTime()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public string Random(int N = 1)
        {
            //return new Random().Next(100, 10000);
            Random rd = new Random();
            List<int> arr = new List<int>();
            for(int i = 0; i < N; i++)
            {
                arr.Add(rd.Next(100, 1000));
            }
            return string.Join("; ", arr);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
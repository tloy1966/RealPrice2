using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
namespace RealPrice.Controllers
{
    public class HomeController : Controller
    {
        private Models.RealPriceContext _context;
        static IQueryable<Models.MainData> _dataAll;
        private IMemoryCache _cache;
        string TaipeiKey = "Taipei";
        DataTable _dtCacheTaipei;
        DataTable _dtCacheTaiChung;
        DataTable _dtCacheKaoHsiung;
        public static string googleMapKey = "AIzaSyA4pcY_w63SDnIUwlLf7kdmUCAdbiwN2EQ";
        public HomeController(Models.RealPriceContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }
        #region
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Hi there!";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult test()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        #region
        public IActionResult Treemap()
        {
            return View();
        }
        #endregion

#endregion
        #region Service
        public IActionResult testGetData()
        {
            return Json(_context.MainData.Take(5));
        }

        public DataTable testGetData2(int City)
        {
            DataTable dt = new DataTable();
            var tempData = _context.MainData.AsQueryable();
            switch (City)
            {
                case (int)Models.Paras.City.Taipei:
                    tempData = tempData.Where(w => (w.City == "A" || w.City == "F"));
                    break;
                case (int)Models.Paras.City.TaiChung:
                    tempData = _context.MainData.Where(w => (w.City == "B"));
                    break;
                case (int)Models.Paras.City.KaoHsiung:
                    tempData = _context.MainData.Where(w => (w.City == "E"));
                    break;
                default:
                    break;
            }
                
            tempData = tempData.Where(w => w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year >= 2016 && w.Sdate.Value.Year<=DateTime.Now.Year) && w.IsActive == true && w.Fdate.Value.Year >= 1960
            && w.Buitype != "店面(店鋪)" && w.Buitype != "其他" && w.Buitype != "辦工商業大樓"
            && w.Buitype != "辦公商業大樓" && w.Buitype != "透天厝"
            && w.Tprice <= 36000000 &&w.Uprice != null);
            var r = tempData
                .Select(s => new {
                    //id = s.Id,
                    City = s.City,
                    District = s.District,
                    Pbuild = s.Pbuild,
                    Buitype = s.Buitype,
                    Location = s.Location,
                    Sdate = s.Sdate,
                    Fdate = s.Fdate,
                    Uprice = s.Uprice,
                    Landa = s.Landa,
                    Tprice = s.Tprice,
                    //houseage = DateTime.Now.Year - s.Fdate.Value.Year
                }).GroupBy(g => new {
                    City = g.City,
                    District = g.District,
                    Pbuild = g.Pbuild,
                    Buitype = g.Buitype,
                    Location = g.Location
                }).Select(s => new {
                    City = s.Key.City,
                    District = s.Key.District,
                    Pbuild = s.Key.Pbuild,
                    Buitype = s.Key.Buitype,
                    Location = s.Key.Location,
                    Uprice = s.Average(g => g.Uprice),
                    Landa = s.Average(g => g.Landa),
                    Tprice = s.Average(g => g.Tprice),
                    houseage = DateTime.Now.Year - s.Average(g => g.Fdate.Value.Year),
                    countNum = s.Count()
                });
            dt.Columns.Add("City",typeof(string));
            dt.Columns.Add("District");
            dt.Columns.Add("Pbuild");
            dt.Columns.Add("Buitype");
            dt.Columns.Add("Location");
            dt.Columns.Add("Uprice", typeof(float));
            dt.Columns.Add("Landa", typeof(float));
            dt.Columns.Add("Tprice", typeof(int));
            dt.Columns.Add("houseage",typeof(int));
            dt.Columns.Add("countNum", typeof(int));
            foreach (var t in r)
            {
                var dr = dt.NewRow();
                dr[0] = t.City;
                dr[1] = t.District;
                dr[2] = t.Pbuild;
                dr[3] = t.Buitype;
                dr[4] = t.Location;
                dr[5] = t.Uprice;
                dr[6] = t.Landa;
                dr[7] = t.Tprice;
                dr[8] = t.houseage;
                dr[9] = t.countNum;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        
        public IActionResult GetData()
        {
            _dataAll = _context.MainData.Where(w => (w.City == "A" || w.City == "F")
            && w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year == 2016) && w.IsActive == true && w.Fdate.Value.Year >= 1960
            && w.Buitype != "店面(店鋪)" && w.Buitype != "其他" && w.Buitype != "辦工商業大樓"
            && w.Buitype != "辦公商業大樓" && w.Buitype != "透天厝"
            && w.Tprice <= 36000000);
            var r = _dataAll
                .Select(s => new {
                    //id = s.Id,
                    City = s.City,
                    District = s.District,
                    Pbuild = s.Pbuild,
                    Buitype = s.Buitype,
                    Location = s.Location,
                    //year = s.Sdate.Value.Year,
                    //Date = s.Sdate.Value.Date,
                    Sdate = s.Sdate,
                    Fdate = s.Fdate,
                    Uprice = s.Uprice,
                    Landa = s.Landa,
                    Tprice = s.Tprice,
                    //houseage = DateTime.Now.Year - s.Fdate.Value.Year
                }).GroupBy(g => new {
                    City = g.City,
                    District = g.District,
                    Pbuild = g.Pbuild,
                    Buitype = g.Buitype,
                    Location = g.Location
                }).Select(s => new {
                    City = s.Key.City,
                    District = s.Key.District,
                    Pbuild = s.Key.Pbuild,
                    Buitype = s.Key.Buitype,
                    Location = s.Key.Location,
                    Uprice = s.Average(g => g.Uprice),
                    Landa = s.Average(g => g.Landa),
                    Tprice = s.Average(g => g.Tprice),
                    houseage = DateTime.Now.Year - s.Average(g => g.Fdate.Value.Year),
                    countNum = s.Count()
                });//.OrderByDescending(o=>o.Tprice);
                   //pricing   (farea) * uprice + pprice * parea calTotal2,
                   /*var r2 = _dataAll
                       .Select(s => new
                       {
                           //id = s.Id,
                           City = s.City,
                           District = s.District,
                           Pbuild = s.Pbuild,
                           Buitype = s.Buitype,
                           Location = s.Location,
                           //year = s.Sdate.Value.Year,
                           //Date = s.Sdate.Value.Date,
                           Uprice = s.Uprice,
                           Landa = s.Landa,
                           Tprice = s.Tprice,
                           houseage = DateTime.Now.Year - s.Fdate.Value.Year
                       });*/

            return Json(r);
        }

        public IActionResult GetData2(string location)
        {
            var rt = _context.MainData.Where(w =>w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year >= 2015 && w.Sdate.Value.Year <= DateTime.Now.Year)
            && w.Location == location
            && w.IsActive == true && w.Fdate.Value.Year >= 1960
            && w.Buitype != "店面(店鋪)" && w.Buitype != "其他" && w.Buitype != "辦工商業大樓"
            && w.Buitype != "辦公商業大樓" && w.Buitype != "透天厝"
            && w.Tprice <= 36000000
            ).OrderBy(o => o.Sdate);
            var r = rt
                .Select(s => new {
                    id = s.Id,
                    City = s.City,
                    District = s.District,
                    Pbuild = s.Pbuild,
                    Buitype = s.Buitype,
                    Location = s.Location,
                    year = s.Sdate.Value.Year,
                    Date = s.Sdate.Value.Date,
                    Sdate = s.Sdate,
                    Uprice = s.Uprice,
                    Landa = s.Landa,
                    Tprice = s.Tprice,
                    Sbuild = s.Sbuild,
                    houseage = DateTime.Now.Year - s.Fdate.Value.Year,
                    BuildR = s.BuildR,
                    BuildL = s.BuildB,
                    BuildB = s.BuildB,
                    Rule = s.Rule,
                    Parktype = s.Parktype,
                    Rmnote = s.Rmnote
                });
            //pricing   (farea) * uprice + pprice * parea calTotal2,
            //var r3 = _dataAll.Where(w=>w.Location == location && w.Sdate !=null).OrderBy(o=>o.Sdate);
            return Json(r);

        }
        #endregion

        private JsonResult GetJsonData(string location)
        {
            var rt = _context.MainData.Where(w => (w.City == "A" || w.City == "F")
            && w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year >= 2015)
            && w.Location == location
            && w.IsActive  == true && w.Fdate.Value.Year >= 1960
            && w.Buitype != "店面(店鋪)" && w.Buitype != "其他" && w.Buitype != "辦工商業大樓"
            && w.Buitype != "辦公商業大樓" && w.Buitype != "透天厝"
            && w.Tprice <= 36000000
            ).OrderBy(o => o.Sdate);
            var r = rt
                .Select(s => new {
                    id = s.Id,
                    City = s.City,
                    District = s.District,
                    Pbuild = s.Pbuild,
                    Buitype = s.Buitype,
                    Location = s.Location,
                    year = s.Sdate.Value.Year,
                    Date = s.Sdate.Value.Date,
                    Sdate = s.Sdate,
                    Uprice = s.Uprice,
                    Landa = s.Landa,
                    Tprice = s.Tprice,
                    Sbuild = s.Sbuild,
                    houseage = DateTime.Now.Year - s.Fdate.Value.Year,
                    BuildR = s.BuildR,
                    BuildL = s.BuildB,
                    BuildB = s.BuildB,
                    Rule = s.Rule,
                    Parktype = s.Parktype,
                    Rmnote = s.Rmnote
                });
            //pricing   (farea) * uprice + pprice * parea calTotal2,
            var rr = Json(r);
            //var r3 = _dataAll.Where(w=>w.Location == location && w.Sdate !=null).OrderBy(o=>o.Sdate);
            return Json(r);
        }
        public JsonResult CachedData(int City)
        {
            try
            {
                switch (City)
                {
                    case (int)Models.Paras.City.Taipei:
                        if (!_cache.TryGetValue(Models.Paras.City.Taipei, out _dtCacheTaipei))
                        {

                            _dtCacheTaipei = testGetData2(City);
                            _cache.Set(TaipeiKey, _dtCacheTaipei, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }

                        return Json(_dtCacheTaipei);
                    case (int)Models.Paras.City.TaiChung:
                        if (!_cache.TryGetValue(Models.Paras.City.TaiChung, out _dtCacheTaiChung))
                        {
                            _dtCacheTaiChung = testGetData2(City);
                            _cache.Set(Models.Paras.City.TaiChung, _dtCacheTaiChung, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }

                        return Json(_dtCacheTaiChung);
                    case (int)Models.Paras.City.KaoHsiung:
                        if (!_cache.TryGetValue(Models.Paras.City.KaoHsiung, out _dtCacheKaoHsiung))
                        {
                            _dtCacheKaoHsiung = testGetData2(City);
                            _cache.Set(Models.Paras.City.KaoHsiung, _dtCacheKaoHsiung, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }

                        return Json(_dtCacheKaoHsiung);
                    default:
                        return Json(_dtCacheTaipei);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return Json(new DataTable());
            }

        }
    }
}

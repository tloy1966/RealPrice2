using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Newtonsoft.Json;
namespace RealPrice.Controllers
{
    public class HomeController : Controller
    {
        private Models.RealPriceContext _context;
        static IQueryable<Models.MainData> _dataAll;
        private IMemoryCache _cache;
        static private readonly string apiKey = "AIzaSyA4pcY_w63SDnIUwlLf7kdmUCAdbiwN2EQ";
        static private readonly string mrtKey = "AIzaSyD7QfARKeZFYosWmrMOSDueBK-ffqrCj-M";
        static private readonly int sdate = 2010;
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

        public DataTable GetAllData(int City)
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
                case (int)Models.Paras.City.Hsinchu:
                    tempData = _context.MainData.Where(w => (w.City == "O" || w.City=="J"));
                    break;
                case (int)Models.Paras.City.Taoyuan:
                    tempData = _context.MainData.Where(w => (w.City == "H"));
                    break;
                default:
                    return null;
            }
                
            tempData = tempData.Where(w => w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year >= sdate && w.Sdate.Value.Year<=DateTime.Now.Year) && w.IsActive == true && w.Fdate.Value.Year >= 1960
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
        
        public IActionResult GetData2(string location,string buitype)
        {
            var rt = _context.MainData.Where(w =>w.SellType == "A" && w.Pbuild == "住家用"
            && (w.Sdate.Value.Year >= sdate && w.Sdate.Value.Year <= DateTime.Now.Year)
            && w.Location == location
            && w.Buitype == buitype
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
        
        public JsonResult CachedData(int City)
        {
            if(!Enum.IsDefined(typeof(Models.Paras.City), City))
            {
                return null;
            }
            
            try
            {
                switch (City)
                {
                    case (int)Models.Paras.City.Taipei:
                        if (!_cache.TryGetValue(Models.Paras.City.Taipei, out Models.Paras._dtCacheTaipei))
                        {
                            Models.Paras._dtCacheTaipei = GetAllData(City);
                            _cache.Set(Models.Paras.City.Taipei, Models.Paras._dtCacheTaipei, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }
                        return Json(Models.Paras._dtCacheTaipei);

                    case (int)Models.Paras.City.TaiChung:
                        if (!_cache.TryGetValue(Models.Paras.City.TaiChung, out Models.Paras._dtCacheTaiChung))
                        {
                            Models.Paras._dtCacheTaiChung = GetAllData(City);
                            _cache.Set(Models.Paras.City.TaiChung, Models.Paras._dtCacheTaiChung, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }
                        return Json(Models.Paras._dtCacheTaiChung);

                    case (int)Models.Paras.City.KaoHsiung:
                        if (!_cache.TryGetValue(Models.Paras.City.KaoHsiung, out Models.Paras._dtCacheKaoHsiung))
                        {
                            Models.Paras._dtCacheKaoHsiung = GetAllData(City);
                            _cache.Set(Models.Paras.City.KaoHsiung, Models.Paras._dtCacheKaoHsiung, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }
                        return Json(Models.Paras._dtCacheKaoHsiung);

                    case (int)Models.Paras.City.Hsinchu:
                        if (!_cache.TryGetValue(Models.Paras.City.Hsinchu, out Models.Paras._dtCacheHsinchu))
                        {
                            Models.Paras._dtCacheHsinchu = GetAllData(City);
                            _cache.Set(Models.Paras.City.Hsinchu, Models.Paras._dtCacheHsinchu, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }
                        return Json(Models.Paras._dtCacheHsinchu);

                    case (int)Models.Paras.City.Taoyuan:
                        if (!_cache.TryGetValue(Models.Paras.City.Taoyuan, out Models.Paras._dtCacheTaoyuan))
                        {
                            Models.Paras._dtCacheTaoyuan = GetAllData(City);
                            _cache.Set(Models.Paras.City.Taoyuan, Models.Paras._dtCacheTaoyuan, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7)));
                        }
                        return Json(Models.Paras._dtCacheTaoyuan);
                    default:
                        return Json(Models.Paras._dtCacheTaipei);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return Json(new DataTable());
            }

        }

        #region Geo part
        private Models.Geo getBaseGeo(string location)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={location}&key=AIzaSyBlEnOEgknWMReRy_XAKq2ars1I0zhEuc8";
            if (string.IsNullOrEmpty(location))
            {
                return new Models.Geo(); 
            }
            string jsonResult = "";
            using (WebClient wc = new WebClient())
            {
                jsonResult = wc.DownloadString(url);
            }
            dynamic j = JsonConvert.DeserializeObject(jsonResult);
            Models.Geo _geo = new Models.Geo();
            _geo.lat = j.results[0].geometry.location.lat.Value;
            _geo.lng = j.results[0].geometry.location.lng.Value;
            return _geo;//nearby mrt place id
        }
        private string getNearByPlaceID(Models.Geo geo, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = "subway_station";
            }
            
            string jsonResult = "";
            using (WebClient wc = new WebClient())
            {
                string _url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={geo.lat},{geo.lng}&radius=1000&type={type}&key={mrtKey}";
                jsonResult = wc.DownloadString(_url);
            }
            dynamic j = JsonConvert.DeserializeObject(jsonResult);
            return j.results[0].place_id;//nearby mrt place id
        }
        

        public int getDistance(string location)
        {
            var geo = getBaseGeo(location);
            var place_id = getNearByPlaceID(geo, "");
            string jsonResult;
            using (WebClient wc = new WebClient())
            {
                string _url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={location}&destinations=place_id:{place_id}&key={mrtKey}";

                jsonResult = wc.DownloadString(_url);
            }
            dynamic j = JsonConvert.DeserializeObject(jsonResult);
            return j.rows[0].elements[0].distance.value;//return json format of duration and distance
        }

        private Models.Geo parseGeo(string jStr)
        {
            dynamic jsonResult = JsonConvert.DeserializeObject(jStr);
            Models.Geo _geo = new Models.Geo();
            try
            {
                _geo.statName = jsonResult.results[0].address_components[0].long_name;
                _geo.formattedAddress = jsonResult.results[0].formatted_address;
                _geo.lat = jsonResult.results[0].geometry.location.lat.Value;
                _geo.lng = jsonResult.results[0].geometry.location.lng.Value;
                _geo.place_id = jsonResult.results[0].place_id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return _geo;
        }
        #endregion
    }
}

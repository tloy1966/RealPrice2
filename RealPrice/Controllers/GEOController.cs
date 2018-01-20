using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using Newtonsoft.Json;
namespace RealPrice.Controllers
{
    public class GEOController : Controller
    {
        #region Geo part, no use for now
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
                
                string _url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={geo.lat},{geo.lng}&radius=1000&type={type}&key={Authority.KeyUtility.GMapMrtKey}";
                jsonResult = wc.DownloadString(_url);
            }
            dynamic j = JsonConvert.DeserializeObject(jsonResult);
            return j.results[0].place_id;//nearby mrt place id
        }
        
        public int getDistanceFromMRT(string location)
        {
            var geo = getBaseGeo(location);
            var place_id = getNearByPlaceID(geo, "");
            string jsonResult;
            string _url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={location}&destinations=place_id:{place_id}&key={Authority.KeyUtility.GMapMrtKey}";
            try
            {
                using (WebClient wc = new WebClient())
                {
                    jsonResult = wc.DownloadString(_url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
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
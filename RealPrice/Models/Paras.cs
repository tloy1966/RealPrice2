using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
namespace RealPrice.Models
{
    public class Paras
    {
        public static DataTable _dtCacheTaipei;
        public static DataTable _dtCacheTaiChung;
        public static DataTable _dtCacheKaoHsiung;
        public static DataTable _dtCacheHsinchu;
        public static DataTable _dtCacheTaoyuan;
        public readonly static string googleMapKey = "AIzaSyA4pcY_w63SDnIUwlLf7kdmUCAdbiwN2EQ";

        public enum City
        {
            Taipei = 0,
            TaiChung = 1,
            KaoHsiung = 2,
            Hsinchu = 3,
            Taoyuan = 4 
        }
        static public DataTable getCityData(int _city)
        {
            switch ((City)_city)
            {
                case City.Taipei:
                    return _dtCacheTaipei;
                case City.TaiChung:
                    return _dtCacheTaiChung;
                case City.KaoHsiung:
                    return _dtCacheKaoHsiung;
                case City.Hsinchu:
                    return _dtCacheHsinchu;
                case City.Taoyuan:
                    return _dtCacheTaoyuan;
                         
                default:
                    return null; 
            }
        }
        /*
         C,基隆市
A,臺北市
F,新北市
H,桃園縣
O,新竹市
J,新竹縣
K,苗栗縣
B,臺中市
M,南投縣
N,彰化縣
P,雲林縣
I,嘉義市
Q,嘉義縣
D,臺南市
E,高雄市
T,屏東縣
G,宜蘭縣
U,花蓮縣
V,臺東縣
X,澎湖縣
W,金門縣
Z,連江縣
         */
    }
}

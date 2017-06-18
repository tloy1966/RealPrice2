using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using RealPrice.Models;
namespace RealPrice.Controllers
{
    [Produces("application/json")]
    [Route("api/Heatmap")]
    public class HeatmapController : Controller
    {
        private readonly RealPriceContext _context;

        public HeatmapController(RealPriceContext context)
        {
            _context = context;
        }
        // GET: api/Heatmap
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_context.Location.Take(10));
        }

        // GET: api/Heatmap/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Heatmap
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Heatmap/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

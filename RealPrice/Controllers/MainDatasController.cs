using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealPrice.Models;

namespace RealPrice.Controllers
{
    public class MainDatasController : Controller
    {
        private readonly RealPriceContext _context;

        public MainDatasController(RealPriceContext context)
        {
            _context = context;    
        }

        // GET: MainDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.MainData.ToListAsync());

        }

        // GET: MainDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainData = await _context.MainData
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mainData == null)
            {
                return NotFound();
            }

            return View(mainData);
        }

        // GET: MainDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,SellType,District,CaseT,Location,Landa,CaseF,LandaX,LandaY,Sdate,Scnt,Sbuild,Tbuild,Buitype,Pbuild,Mbuild,Fdate,Farea,BuildR,BuildL,BuildB,BuildP,Rule,Furniture,Tprice,Uprice,Parktype,Parea,Pprice,Rmnote,Id2,IsActive,Lat,Lon")] MainData mainData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainData);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mainData);
        }

        // GET: MainDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainData = await _context.MainData.SingleOrDefaultAsync(m => m.Id == id);
            if (mainData == null)
            {
                return NotFound();
            }
            return View(mainData);
        }

        // POST: MainDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,SellType,District,CaseT,Location,Landa,CaseF,LandaX,LandaY,Sdate,Scnt,Sbuild,Tbuild,Buitype,Pbuild,Mbuild,Fdate,Farea,BuildR,BuildL,BuildB,BuildP,Rule,Furniture,Tprice,Uprice,Parktype,Parea,Pprice,Rmnote,Id2,IsActive,Lat,Lon")] MainData mainData)
        {
            if (id != mainData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainDataExists(mainData.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(mainData);
        }

        // GET: MainDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainData = await _context.MainData
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mainData == null)
            {
                return NotFound();
            }

            return View(mainData);
        }

        // POST: MainDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mainData = await _context.MainData.SingleOrDefaultAsync(m => m.Id == id);
            _context.MainData.Remove(mainData);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MainDataExists(int id)
        {
            return _context.MainData.Any(e => e.Id == id);
        }
    }
}

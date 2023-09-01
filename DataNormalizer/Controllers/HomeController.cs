using DataNormalizer.Data;
using DataNormalizer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using DataNormalizer.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using DataNormalizer.Services.Helpers;

namespace DataNormalizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //For now just for Sql servers
        [HttpPost]

        public ActionResult Submit(string ConnectionName, string ConnectionString)
        {
            var tables = _context.Model.GetEntityTypes()
             .Select(entityType => entityType.GetTableName())
             .ToList();
            var tableMappings = new Dictionary<string, dynamic>();
            foreach (var table in tables)
            {
                var dbSet = DbSetInstance.GetDbSetByTableName(_context, table);
                tableMappings.Add(table, dbSet);
            }
            new DatabaseETLService(_context).UploadDataToDb(ConnectionString, tableMappings);
            return View("Index");
        }
        [HttpPost]
        public ActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                new ChoETLService(_context).UploadDataToDb(file);
                ViewBag.Message = $"File uploaded successfully. Content length: {file.Length} bytes.";
            }
            else
            {
                ViewBag.Message = "No file uploaded.";
            }
            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
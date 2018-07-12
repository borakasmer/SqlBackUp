using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SqlBackUp.Models;
using BusinessContext;
using SqlFilter.Filters;
using System;

namespace SqlBackUp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (DataContext context = new DataContext())
            {
                var data = context.GetVehicleData();
                return View(data);
            }
        }
        public IActionResult Detail(int? ID)
        {
            if (ID == null)
            {
                Vehicle data = new Vehicle();
                data.MFDate=DateTime.Now;
                return View(data);
            }
            else
            {
                using (DataContext context = new DataContext())
                {
                    var data = context.GetVehicleDataByID((int)ID);
                    return View(data);
                }
            }
        }
        [ServiceFilter(typeof(BackUpFilter))]
        public IActionResult Update(Vehicle model)
        {
            using (DataContext context = new DataContext())
            {
                bool data;
                if (model.ID == 0)
                {
                    model.OperationType=(int)OperationTypes.Insert;
                    data = context.InsertVehicle(model);
                }
                else
                {
                    model.OperationType=(int)OperationTypes.Update;
                    data = context.UpdateVehicle(model);
                }
                return RedirectToAction("Index");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

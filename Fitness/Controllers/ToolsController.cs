using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fitness.Controllers
{
    public class ToolsController : Controller
    {

        public ActionResult PlateCalc(int? weight, string type = "lb")
        {
            List<object> objectList = new List<object>();
            objectList.Add(new
            {
                id = "lb",
                value = "lbs"
            });
            objectList.Add(new
            {
                id = "kg",
                value = "kgs"
            });

            ViewBag.type = new SelectList(objectList, "id", "value");
            if (weight == null)
            {
                return View();
            }
            if (weight == 0)
            {
                ViewBag.error = "No weight entered!";
                return View();
            }
            if (weight % 5 != 0)
            {
                ViewBag.error = "Weight is not an increment of 5!";
                return View();
            }
            if (weight <= 45)
            {
                ViewBag.error = "Weight entered is less than or equal to the bar itself, no extra weight is needed!";
                return View();
            }
            PlateModel model = new PlateModel();
            //take the weight of the bar off
            weight -= 45;
            
            while (weight >= 90)
            {
                model.fourtyFive++;
                weight -= 90;
            }

            while (weight >= 50)
            {
                model.twentyFive++;
                weight -= 50;
            }

            while (weight >= 20)
            {
                model.ten++;
                weight -= 20;
            }

            while (weight >= 10)
            {
                model.five++;
                weight -= 10;
            }

            while (weight >= 5)
            {
                model.twoFive++;
                weight -= 5;
            }

            return View(model);
        }
    }
}
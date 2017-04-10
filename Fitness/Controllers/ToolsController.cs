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

        public ActionResult PlateCalc(double? weight, string type = "lb")
        {
            List<object> objectList = new List<object>();
            objectList.Add(new
            {
                id = "lb",
                value = "Pounds"
            });
            objectList.Add(new
            {
                id = "kg",
                value = "Kilograms"
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

            PlateModel model = new PlateModel();
            model.type = type;

            if (type == "lb")
            {
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
            }
            else if (type == "kg")
            {

                if (weight % 2.5 != 0)
                {
                    ViewBag.error = "Weight is not an increment of 2.5kg!";
                    return View();
                }
                if (weight <= 20)
                {
                    ViewBag.error = "Weight entered is less than or equal to the bar itself, no extra weight is needed!";
                    return View();
                }

                //take bar off calculation
                weight -= 20;
                double dWeight = (double)weight;

                while (dWeight >= 40)
                {
                    model.fourtyFive++;
                    dWeight -= 40;
                }

                while (dWeight >= 20)
                {
                    model.twentyFive++;
                    dWeight -= 20;
                }

                while (dWeight >= 10)
                {
                    model.ten++;
                    dWeight -= 10;
                }

                while (dWeight >= 5)
                {
                    model.five++;
                    dWeight -= 5;
                }

                while (dWeight >= 2.5)
                {
                    model.twoFive++;
                    dWeight -= 2.5;
                }

            }
            else
            {
                ViewBag.error = "Hey! Don't mess with the URL";
            }

            

            return View(model);
        }
    }
}
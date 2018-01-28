using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Fitness.Models;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace Fitness.Controllers
{
    public class ExerciseListsController : Controller
    {
        private fitnessEntities db = new fitnessEntities();

        // GET: ExerciseLists
        public ActionResult Index()
        {
            StrengthAerobicList list = new StrengthAerobicList();
            list.strengthExercise = db.ExerciseLists.Where(e => e.UserID == db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Strength").ToList();
            list.aerobicExercise = db.ExerciseLists.Where(e => e.UserID == db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID).Where(x => x.Type == "Aerobic").ToList();
            return View(list);
        }

        // GET: ExerciseLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList.User.Email != User.Identity.Name)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "ExerciseLists/Details/" + id  });
            }
            List<object> focusList = new List<object>();
            focusList.Add(new SelectListItem { Text = "Duration", Value = "Duration" });
            focusList.Add(new SelectListItem { Text = "Distance", Value = "Distance" });
            ViewBag.focus = new SelectList(focusList, "Value", "Text");
            ViewBag.NoStrength = false;
            ViewBag.NoAerobic = false;
            ViewBag.URL = "CreateAerobicChart/?ChartType=Duration";
            if (exerciseList.StrengthExercises.Count == 0)
            {
                ViewBag.NoStrength = true;
            }
            if (exerciseList.AerobicExercises.Count == 0)
            {
                ViewBag.NoAerobic = true;
            }
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }

        public ActionResult DetailsTest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList.User.Email != User.Identity.Name)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "ExerciseLists/Details/" + id });
            }
            List<object> focusList = new List<object>();
            focusList.Add(new SelectListItem { Text = "Duration", Value = "Duration" });
            focusList.Add(new SelectListItem { Text = "Distance", Value = "Distance" });
            ViewBag.focus = new SelectList(focusList, "Value", "Text");
            ViewBag.NoStrength = false;
            ViewBag.NoAerobic = false;
            ViewBag.URL = "CreateAerobicChart/?ChartType=Duration";
            if (exerciseList.StrengthExercises.Count == 0)
            {
                ViewBag.NoStrength = true;
            }
            if (exerciseList.AerobicExercises.Count == 0)
            {
                ViewBag.NoAerobic = true;
            }
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }



        [HttpPost]
        public JsonResult CreateStrengthChart(int id)
        {

            var exercise = db.ExerciseLists.Find(id);

            //strength exercise chart
            if (exercise.StrengthExercises.Count == 0)
            {
                return null;
            }
            List<string> xData = new List<string>();
            List<decimal> yData = new List<decimal>();
            decimal maxWeight = 0;
            DateTime currentDay = exercise.StrengthExercises.OrderBy(x => x.DateRecorded).FirstOrDefault().DateRecorded;
            foreach (var item in exercise.StrengthExercises.OrderBy(x => x.DateRecorded))
            {
                if (currentDay < item.DateRecorded)
                {
                    xData.Add(currentDay.ToShortDateString().ToString());
                    yData.Add(maxWeight);
                    currentDay = item.DateRecorded;
                    maxWeight = 0;
                }
                if (maxWeight < item.Weight)
                {
                    maxWeight = item.Weight;
                }


            }
            xData.Add(currentDay.ToShortDateString().ToString());
            yData.Add(maxWeight);

            var data = new ChartData();
            data.labels = xData;
            data.data = yData;

            return Json(data);


        }


        [HttpPost]
        public JsonResult CreateAerobicDurationChart(int id)
        {
            var exercise = db.ExerciseLists.Find(id);
            if (exercise.AerobicExercises.Count == 0)
            {
                return null;
            }
            List<string> chartTypeList = new List<string>();
            chartTypeList.Add("Duration");
            chartTypeList.Add("Distance");
            ViewBag.chartTypeList = new SelectList(chartTypeList);
            //aerobic exercise chart
            List<string> dayData = new List<string>();
            List<double> durationData = new List<double>();
            TimeSpan maxDuration = TimeSpan.Zero;
            DateTime currentDay = exercise.AerobicExercises.OrderBy(x => x.DateRecorded).FirstOrDefault().DateRecorded;
            foreach (var item in exercise.AerobicExercises.OrderBy(x => x.DateRecorded))
            {
                if (currentDay < item.DateRecorded)
                {
                    dayData.Add(currentDay.ToShortDateString().ToString());
                    durationData.Add(Math.Round(maxDuration.TotalHours, 2));
                    currentDay = item.DateRecorded;
                    maxDuration = TimeSpan.Zero;
                }
                if (maxDuration < item.Duration)
                {
                    maxDuration = item.Duration;
                }


            }
            dayData.Add(currentDay.ToShortDateString().ToString());
            durationData.Add(Math.Round(maxDuration.TotalHours, 2));

            var data = new ChartData();
            data.labels = dayData;
            data.times = durationData;

            return Json(data);

        }

        public ActionResult CreateAerobicLengthChart(ExerciseList exercise)
        {
            exercise = db.ExerciseLists.Find(exercise.ExerciseListID);
            if (exercise.AerobicExercises.Count == 0)
            {
                return null;
            }
            List<string> chartTypeList = new List<string>();
            chartTypeList.Add("Duration");
            chartTypeList.Add("Distance");
            ViewBag.chartTypeList = new SelectList(chartTypeList);
            //aerobic exercise chart
            List<string> dayData = new List<string>();
            List<string> durationData = new List<string>();
            List<decimal> lengthData = new List<decimal>();
            TimeSpan maxDuration = TimeSpan.Zero;
            decimal maxLength = 0;
            DateTime currentDay = exercise.AerobicExercises.OrderBy(x => x.DateRecorded).FirstOrDefault().DateRecorded;
            foreach (var item in exercise.AerobicExercises.OrderBy(x => x.DateRecorded))
            {
                if (currentDay < item.DateRecorded)
                {
                    dayData.Add(currentDay.ToShortDateString().ToString());
                    durationData.Add(maxDuration.ToString());
                    lengthData.Add(maxLength);
                    currentDay = item.DateRecorded;
                    maxLength = 0;
                    maxDuration = TimeSpan.Zero;
                }
                if (maxLength < item.Length)
                {
                    maxLength = item.Length;
                }
                if (maxDuration < item.Duration)
                {
                    maxDuration = item.Duration;
                }


            }
            dayData.Add(currentDay.ToShortDateString().ToString());
            durationData.Add(maxDuration.ToString());
            lengthData.Add(maxLength);

            ViewBag.NoImage = false;
            Chart chart = new Chart();
            chart.BackColor = System.Drawing.Color.Transparent;
            chart.AntiAliasing = AntiAliasingStyles.Graphics;
            chart.Height = 600;
            chart.Width = 800;
            ChartArea chartArea = new ChartArea("ChartArea");
            chartArea.AxisY.TitleFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Regular);
            chartArea.AxisX.TitleFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Regular);
            chartArea.AxisX.Title = "Date Recorded";
            chartArea.AxisY.Title = "Miles Traveled";
            chart.ChartAreas.Add(chartArea);
            Series series = new Series("Distance");
            Series durationSeries = new Series("Duration");
            chart.Series.Add(series);
            series.ChartArea = "ChartArea";
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 4;
            for (int n = 0; n < dayData.Count; n++)
            {
                series.Points.AddXY(dayData.ElementAt(n), lengthData.ElementAt(n));
                series.Points.ElementAt(n).Label = Math.Round(lengthData.ElementAt(n), 2).ToString() + " Miles";
                series.Points.ElementAt(n).LabelBorderWidth = 5;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                chart.SaveImage(stream, ChartImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

            //System.Web.Helpers.Chart chart = new System.Web.Helpers.Chart(800, 600);

            //chart.AddLegend("Length", "Length");
            //chart.AddSeries(yValues: lengthData, legend: "Length", chartType: "Line");

            //chart.AddSeries(xValue: dayData, chartType: "Line");
            //chart.AddTitle(exercise.Name + " History");
            //chart.Write("png");


            //return null;
        }

        [HttpPost]
        public JsonResult CreateAerobicLengthChart(int id)
        {
            var exercise = db.ExerciseLists.Find(id);
            if (exercise.AerobicExercises.Count == 0)
            {
                return null;
            }
            List<string> chartTypeList = new List<string>();
            chartTypeList.Add("Duration");
            chartTypeList.Add("Distance");
            ViewBag.chartTypeList = new SelectList(chartTypeList);
            //aerobic exercise chart
            List<string> dayData = new List<string>();
            List<string> durationData = new List<string>();
            List<decimal> lengthData = new List<decimal>();
            TimeSpan maxDuration = TimeSpan.Zero;
            decimal maxLength = 0;
            DateTime currentDay = exercise.AerobicExercises.OrderBy(x => x.DateRecorded).FirstOrDefault().DateRecorded;
            foreach (var item in exercise.AerobicExercises.OrderBy(x => x.DateRecorded))
            {
                if (currentDay < item.DateRecorded)
                {
                    dayData.Add(currentDay.ToShortDateString().ToString());
                    durationData.Add(maxDuration.ToString());
                    lengthData.Add(maxLength);
                    currentDay = item.DateRecorded;
                    maxLength = 0;
                    maxDuration = TimeSpan.Zero;
                }
                if (maxLength < item.Length)
                {
                    maxLength = item.Length;
                }
                if (maxDuration < item.Duration)
                {
                    maxDuration = item.Duration;
                }


            }
            dayData.Add(currentDay.ToShortDateString().ToString());
            durationData.Add(maxDuration.ToString());
            lengthData.Add(maxLength);

            var data = new ChartData();
            data.labels = dayData;
            data.data = lengthData;

            return Json(data);

        }

        // GET: ExerciseLists/Create
        public ActionResult Create()
        {
            List<string> types = new List<string>();
            types.Add("Strength");
            types.Add("Aerobic");
            ViewBag.typelist = new SelectList(types);
            return View();
        }

        public PartialViewResult CreateModal()
        {
            List<string> types = new List<string>();
            types.Add("Strength");
            types.Add("Aerobic");
            ViewBag.typelist = new SelectList(types);
            return PartialView("CreateModal");
        }

        // POST: ExerciseLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Type")] ExerciseList exerciseList)
        {
            if (ModelState.IsValid)
            {   
                exerciseList.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.ExerciseLists.Add(exerciseList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exerciseList);
        }

        // GET: ExerciseLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList == null)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "ExerciseLists/Details/" + id });
            }
            if (exerciseList.User.Email != User.Identity.Name)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "ExerciseLists/Details/" + id });
            }
            List<string> types = new List<string>();
            types.Add("Strength");
            types.Add("Aerobic");
            ViewBag.typelist = new SelectList(types);
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            return View(exerciseList);
        }

        // POST: ExerciseLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExerciseListID,Name,UserID, Type")] ExerciseList exerciseList)
        {
            if (ModelState.IsValid)
            {
                exerciseList.UserID = db.Users.Where(x => x.Email == User.Identity.Name).FirstOrDefault().UserID;
                db.Entry(exerciseList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exerciseList);
        }

        // GET: ExerciseLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            if (exerciseList == null)
            {
                return HttpNotFound();
            }
            if (exerciseList.User.Email != User.Identity.Name)
            {
                return RedirectToAction("Unauthorized", "Users", new { returnURL = "ExerciseLists/Details/" + id });
            }
            return View(exerciseList);
        }

        // POST: ExerciseLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExerciseList exerciseList = db.ExerciseLists.Find(id);
            db.ExerciseLists.Remove(exerciseList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

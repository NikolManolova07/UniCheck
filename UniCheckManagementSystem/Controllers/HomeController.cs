using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Infrastructure;

namespace UniCheckManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        [AuthenticationFilter]
        [AuthorizationFilter("Admin")]
        public ActionResult Reports(string selectedReportType, int? id)
        {
            if (selectedReportType == null)
            {
                return View(db.Specialties.ToList());
            }

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/StudentsReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "StudentDataSet";
            if (id != null)
            {
                reportDataSource.Value = db.Students.Where(s => s.SpecialtyId == id).ToList();
            }
            else
            {
                reportDataSource.Value = db.Students.ToList();
            }

            localReport.DataSources.Add(reportDataSource);
            string reportType = selectedReportType;
            string mimeType, encoding, fileNameExtension;

            if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
            if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }
            if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment;filename=students_report_" + DateTime.Now.ToString("yymmssfff") + "." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
        }
    }
}
using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Transportation.Api
{
    public class ExportService
    {
        public ExportService()
        {
        }

        [Route(Framework.HttpVerb.Post, "/exportWagonToExcel")]
        public RestApiResult ExportToExcel(JObject json)
        {
            if (json == null)
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };

            var report = Wagon.FromJson(json);

            string appPath = HttpContext.Current.Request.ApplicationPath;
            string templateFileName = "ToaHang_Report_Template.xlsx";
            string physicalPath = HttpContext.Current.Request.MapPath(appPath).TrimEnd('\\');
            var templateFilePath = physicalPath + "\\template\\" + templateFileName;

            string folderName = string.Format(@"{0}", Guid.NewGuid());
            string outputFolder = string.Format(@"{0}\\output\\{1}", physicalPath, folderName);

            string fileNameReport = "ToaHang_" + report.ID;
            string filePath = CopyFromTemplate(templateFilePath, outputFolder, "ToaHang_" + report.ID);

            if (string.IsNullOrEmpty(filePath))
            {
                return new RestApiResult { StatusCode = HttpStatusCode.InternalServerError };
            }
            else
            {
                string fileName = string.Format(@"{0}/{1}.xlsx", folderName, fileNameReport.Replace("\'", "\\\'"));
                return new RestApiResult { Json = JObject.Parse(string.Format("{{ fileName: '{0}'}}", fileName)) };
            }
        }

        public string CopyFromTemplate(string templateFilePath, string outputFolder, string reportFileName)
        {
            string fileExtension = Path.GetExtension(templateFilePath);
            string reportFilePath = string.Format("{0}\\{1}{2}", outputFolder.TrimEnd('\\'), reportFileName, fileExtension);

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            File.Copy(templateFilePath, reportFilePath, true);
            return reportFilePath;
        }

    }
}

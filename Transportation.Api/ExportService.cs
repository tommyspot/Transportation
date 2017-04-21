using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Transportation.Api
{
    public class ExportService
    {
        public HelperService helperService;

        public ExportService()
        {
            this.helperService = new HelperService();
        }

        [Route(Framework.HttpVerb.Post, "/exportToExcel")]
        public RestApiResult ExportToExcel(JObject json)
        {
            try
            {
                //Start Excel and get Application object.
                Application excelApp = new Application();
                //Get a new workbook.
                _Workbook workBook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                _Worksheet workSheet = workBook.ActiveSheet;

                string fileName = "";
                if (json.Value<int>("type") == (int)ExportType.Truck)
                {
                    BindDataTruckToWorkSheet(workSheet);
                    //fileName = "Baocao_Xe_" + DateTime.Now.ToString("dd/MM/yyyy");
					fileName = "Baocao_Xe";
				}
                else if (json.Value<int>("type") == (int)ExportType.Employee)
                {
                    BindDataEmployeeToWorkSheet(workSheet);
                    //fileName = "Baocao_NhanVien_" + DateTime.Now.ToString("dd/MM/yyyy");
					fileName = "Baocao_NhanVien";
				}
                else if (json.Value<int>("type") == (int)ExportType.Customer)
                {
                    BindDataCustomerToWorkSheet(workSheet);
                    //fileName = "Baocao_KhachHang_" + DateTime.Now.ToString("dd/MM/yyyy");
					fileName = "Baocao_KhachHang";
				}


                string fileNamePath = createExcelFile(workBook, fileName);
                return new RestApiResult { Json = JObject.Parse(string.Format("{{ fileName: '{0}'}}", fileNamePath)) };
            }
            catch (Exception ex)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        private void BindDataTruckToWorkSheet(_Worksheet workSheet) {
            //workSheet.Name = "Xe_DuyenHai_" + DateTime.Now.ToString("dd/MM/yyyy");
			workSheet.Name = "Xe_DuyenHai";
			List<Truck> trucks = ClarityDB.Instance.Trucks.ToList();
            const int numOfColumn = 14;
            //Add table headers going cell by cell.
            string[] headers = new string[numOfColumn] {
                    "STT", "Biển số", "Năm sản xuất", "Số khung",
                    "Số máy", "Nhãn hiệu", "Tải trọng (tấn)", "Ngày mua",
                    "Ngày lưu hành", "Tiền thanh toán hàng tháng", "Cổ phần (%)",
                    "Ngày đăng kiểm gần nhất", "Bảo Hiểm Đến", "Tài xế chịu trách nhiệm" };
            ApplyHeaderForWorkSheet(workSheet, headers, numOfColumn);

            //Format text columns
            int[] textColumns = new int[] { 2, 4, 5, 6, 8, 9, 12, 13 };
            FormatTextForEntireColumn(workSheet, textColumns);

            //Binding data
            for (int i = 0; i < trucks.Count; i++)
            {
                var truck = trucks[i];
                int rowIndex = i + 2;
                workSheet.Cells[rowIndex, 1] = i + 1;
                workSheet.Cells[rowIndex, 2] = truck.LicensePlate;
                workSheet.Cells[rowIndex, 3] = truck.YearOfManufacture;
                workSheet.Cells[rowIndex, 4] = truck.Vin;
                workSheet.Cells[rowIndex, 5] = truck.EngineNo;
                workSheet.Cells[rowIndex, 6] = truck.Brand;
                workSheet.Cells[rowIndex, 7] = truck.Weight;
                workSheet.Cells[rowIndex, 8] = truck.BuyingDate;
                workSheet.Cells[rowIndex, 9] = truck.StartUsingDate;
                workSheet.Cells[rowIndex, 10] = truck.MonthlyPayment;
                workSheet.Cells[rowIndex, 11] = truck.Stock;
                workSheet.Cells[rowIndex, 12] = truck.CheckDate;
                workSheet.Cells[rowIndex, 13] = truck.InsuranceDate;
                workSheet.Cells[rowIndex, 14] = (truck.EmployeeId != null && truck.EmployeeId != "") ? this.helperService.GetEmployeeName(Convert.ToInt32(truck.EmployeeId)) : "";
            }

            //AutoFit columns
            AutoFixColumnWidth(workSheet, numOfColumn);
        }

        private void BindDataEmployeeToWorkSheet(_Worksheet workSheet)
        {
            //workSheet.Name = "NhanVien_DuyenHai_" + DateTime.Now.ToString("dd/MM/yyyy");
			workSheet.Name = "NhanVien_DuyenHai";

			List<Employee> employees = ClarityDB.Instance.Employees.ToList();
            const int numOfColumn = 14;
            //Add table headers going cell by cell.
            string[] headers = new string[numOfColumn] {
                    "STT", "Họ tên", "Số điện thoại", "CMND",
                    "Địa chỉ", "Chức vụ", "Hạng GPLX", "Nơi cấp GPLX",
                    "Số GPLX", "Ngày câp GPLX", "Ngày hết hạn GPLX",
                    "Ngày bắt đầu làm việc", "Vi phạm", "Ghi chú" };
            ApplyHeaderForWorkSheet(workSheet, headers, numOfColumn);

            //Format text columns
            int[] textColumns = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            FormatTextForEntireColumn(workSheet, textColumns);

            //Binding data
            for (int i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                int rowIndex = i + 2;
                workSheet.Cells[rowIndex, 1] = i + 1;
                workSheet.Cells[rowIndex, 2] = employee.FullName;
                workSheet.Cells[rowIndex, 3] = employee.Mobile;
                workSheet.Cells[rowIndex, 4] = employee.CardID;
                workSheet.Cells[rowIndex, 5] = employee.Address;
                workSheet.Cells[rowIndex, 6] = employee.Title;
                workSheet.Cells[rowIndex, 7] = employee.DriverLicenseRank;
                workSheet.Cells[rowIndex, 8] = employee.DriverLicenseAddress;
                workSheet.Cells[rowIndex, 9] = employee.DriverLicenseID;
                workSheet.Cells[rowIndex, 10] = employee.DriverLicenseDate;
                workSheet.Cells[rowIndex, 11] = employee.DriverLicenseExpirationDate;
                workSheet.Cells[rowIndex, 12] = employee.StartDate;
                workSheet.Cells[rowIndex, 13] = employee.Violation;
                workSheet.Cells[rowIndex, 14] = employee.Notes;
            }

            //AutoFit columns
            AutoFixColumnWidth(workSheet, numOfColumn);
        }

        private void BindDataCustomerToWorkSheet(_Worksheet workSheet)
        {
            //workSheet.Name = "KhachHang_DuyenHai_" + DateTime.Now.ToString("dd/MM/yyyy");
			workSheet.Name = "KhachHang_DuyenHai";

			List<Customer> customers = ClarityDB.Instance.Customers.ToList();
            const int numOfColumn = 10;
            //Add table headers going cell by cell.
            string[] headers = new string[numOfColumn] {
                    "STT","Mã khách hàng", "Họ tên", "Khu vực", "Số điện thoại",
                    "Nhân viên chịu trách nhiệm", "Số phát sinh", "Tổng trả",
                    "Còn nợ lại", "Xếp loại" };
            ApplyHeaderForWorkSheet(workSheet, headers, numOfColumn);

            //Format text columns
            int[] textColumns = new int[] { 2, 3, 4, 5, 6, 10 };
            FormatTextForEntireColumn(workSheet, textColumns);

            //Binding data
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                int rowIndex = i + 2;
                workSheet.Cells[rowIndex, 1] = i + 1;
                workSheet.Cells[rowIndex, 2] = customer.Code;
                workSheet.Cells[rowIndex, 3] = customer.FullName;
                workSheet.Cells[rowIndex, 4] = customer.Area;
                workSheet.Cells[rowIndex, 5] = customer.PhoneNo;
                workSheet.Cells[rowIndex, 6] = this.helperService.GetEmployeeName(Convert.ToInt32(customer.EmployeeID));
                workSheet.Cells[rowIndex, 7] = customer.TotalOwned;
                workSheet.Cells[rowIndex, 8] = customer.TotalPay;
                workSheet.Cells[rowIndex, 9] = customer.TotalDebt;
                workSheet.Cells[rowIndex, 10] = customer.Type;
            }

            //AutoFit columns
            AutoFixColumnWidth(workSheet, numOfColumn);
        }

        [Route(HttpVerb.Delete, "/deleteExcelFile/{folderName}")]
        public RestApiResult DeleteExcelFile(string folderName)
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string physicalPath = HttpContext.Current.Request.MapPath(appPath).TrimEnd('\\');
            string outputFolder = string.Format("{0}\\output\\{1}", physicalPath, folderName);
            
            if (Directory.Exists(outputFolder))
                Directory.Delete(outputFolder, true);

            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        private void ApplyHeaderForWorkSheet(_Worksheet workSheet, string[] headers, int numOfColumn) {
            for (int columnIndex = 0; columnIndex < headers.Count(); columnIndex++)
            {
                workSheet.Cells[1, columnIndex + 1] = headers[columnIndex];
            }
            //Format headers
            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, numOfColumn]].Font.Bold = true;
            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, numOfColumn]].VerticalAlignment = XlVAlign.xlVAlignCenter;
            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, numOfColumn]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
        }

        private void FormatTextForEntireColumn(_Worksheet workSheet, int[] columns) {
            foreach (int columnIndex in columns) {
                workSheet.Cells[1, columnIndex].EntireColumn.NumberFormat = "@";
            }
        }

        private void AutoFixColumnWidth(_Worksheet workSheet, int numOfColumn) {
            Range oRng = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, numOfColumn]];
            oRng.EntireColumn.AutoFit();
        }

        private string createExcelFile(_Workbook workBook, string fileName) {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string physicalPath = HttpContext.Current.Request.MapPath(appPath).TrimEnd('\\');
            string folderName = string.Format("{0}", Guid.NewGuid());
            string outputFolder = string.Format("{0}\\output\\{1}", physicalPath, folderName);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            string fileNamePath = string.Format("{0}\\{1}.xlsx", outputFolder, fileName);

            workBook.SaveAs(fileNamePath, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false,
                XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workBook.Close();

            return string.Format("{0}/{1}.xlsx", folderName, fileName);
        }
    }
}

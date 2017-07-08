using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Data;
using ExportToExcel;
using System.Globalization;

namespace Transportation.Api
{
    public class ExportService
    {
        public HelperService helperService;
        const string formatDate = "dd/MM/yyyy";
        const string formatStringDate = "dd-MM-yyyy";


        public ExportService()
        {
            this.helperService = new HelperService();
        }

        [Route(Framework.HttpVerb.Post, "/exportToExcel")]
        public RestApiResult ExportToExcel(JObject json)
        {
            try
            {
                string fileName = "";
                DataTable dt = null;
                if (json.Value<int>("type") == (int)ExportType.Truck)
                {
                    dt = BuildDataTableForTruck();
                    fileName = "Baocao_Xe_" + DateTime.Now.ToString(formatStringDate);
                }
                else if (json.Value<int>("type") == (int)ExportType.Employee)
                {
                    dt = BuildDataTableForEmployee();
                    fileName = "Baocao_NhanVien_" + DateTime.Now.ToString(formatStringDate);
                }
                else if (json.Value<int>("type") == (int)ExportType.Customer)
                {
                    dt = BuildDataTableForCustomer();
                    fileName = "Baocao_KhachHang_" + DateTime.Now.ToString(formatStringDate);
                }
                else if (json.Value<int>("type") == (int)ExportType.OrderCustomer)
                {
                    DateTime fromDate = DateTime.ParseExact(json.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(json.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);
                    dt = BuildDataTableForOrderCustomer(fromDate, toDate);
                    fileName = "Baocao_DonHang_" + DateTime.Now.ToString(formatStringDate);
                }
                else if (json.Value<int>("type") == (int)ExportType.Wagon)
                {
                    DateTime fromDate = DateTime.ParseExact(json.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(json.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);
                    dt = BuildDataTableForWagon(fromDate, toDate);
                    fileName = "Baocao_ToaHang_" + DateTime.Now.ToString(formatStringDate);
                }
                else if (json.Value<int>("type") == (int)ExportType.WagonSettlement)
                {
                    DateTime fromDate = DateTime.ParseExact(json.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(json.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);
                    dt = BuildDataTableForWagonSettlement(fromDate, toDate);
                    fileName = "Baocao_QuyetToan_" + DateTime.Now.ToString(formatStringDate);
                }

                string fileNamePath = saveExcelFile(dt, fileName);
                return new RestApiResult { Json = JObject.Parse(string.Format("{{ fileName: '{0}'}}", fileNamePath)) };
            }
            catch (Exception ex)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.InternalServerError, Json = ex.ToString() };
            }
        }

        [Route(Framework.HttpVerb.Post, "/exportGarageToExcel")]
        public RestApiResult ExportGarageToExcel(JArray jsonList)
        {
            try
            {
                DataTable dt = BuildDataTableForGarage(jsonList);
                string fileName = "Baocao_Garage_" + DateTime.Now.ToString(formatStringDate);
                

                string fileNamePath = saveExcelFile(dt, fileName);
                return new RestApiResult { Json = JObject.Parse(string.Format("{{ fileName: '{0}'}}", fileNamePath)) };
            }
            catch (Exception ex)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.InternalServerError, Json = ex.ToString() };
            }
        }
        private DataTable BuildDataTableForTruck()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Xe_" + DateTime.Now.ToString(formatStringDate);
            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Biển số", typeof(string));
            dt.Columns.Add("Năm sản xuất", typeof(int));
            dt.Columns.Add("Số khung", typeof(string));
            dt.Columns.Add("Số máy", typeof(string));
            dt.Columns.Add("Nhãn hiệu", typeof(string));
            dt.Columns.Add("Tải trọng (tấn)", typeof(int));
            dt.Columns.Add("Ngày mua", typeof(string));
            dt.Columns.Add("Ngày lưu hành", typeof(string));
            dt.Columns.Add("Tiền thanh toán hàng tháng", typeof(int));
            dt.Columns.Add("Cổ phần (%)", typeof(int));
            dt.Columns.Add("Ngày đăng kiểm gần nhất", typeof(string));
            dt.Columns.Add("Bảo Hiểm Đến", typeof(string));
            dt.Columns.Add("Tài xế chịu trách nhiệm", typeof(string));

            //Binding data
            List<Truck> trucks = ClarityDB.Instance.Trucks.ToList();
            for (int i = 0; i < trucks.Count; i++)
            {
                var truck = trucks[i];
                dt.Rows.Add(new object[] { i + 1 , truck.LicensePlate, truck.YearOfManufacture , truck.Vin, truck.EngineNo,
                                           truck.Brand, truck.Weight, truck.BuyingDate, truck.StartUsingDate,
                                           truck.MonthlyPayment, truck.Stock, truck.CheckDate, truck.InsuranceDate,
                                           String.IsNullOrEmpty(truck.EmployeeId) ? "" : this.helperService.GetEmployeeName(Convert.ToInt32(truck.EmployeeId))});
            }

            return dt;
        }

        private DataTable BuildDataTableForEmployee()
        {
            DataTable dt = new DataTable();
            dt.TableName = "NhanVien_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("CMND", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));
            dt.Columns.Add("Chức vụ", typeof(string));
            dt.Columns.Add("Hạng GPLX", typeof(string));
            dt.Columns.Add("Nơi cấp GPLX", typeof(string));
            dt.Columns.Add("Số GPLX", typeof(string));
            dt.Columns.Add("Ngày câp GPLX", typeof(string));
            dt.Columns.Add("Ngày hết hạn GPLX", typeof(string));
            dt.Columns.Add("Ngày bắt đầu làm việc", typeof(string));
            dt.Columns.Add("Vi phạm", typeof(string));
            dt.Columns.Add("Ghi chú", typeof(string));

            //Binding data
            List<Employee> employees = ClarityDB.Instance.Employees.ToList();
            for (int i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                dt.Rows.Add(new object[] { i + 1 , employee.FullName, employee.Mobile , employee.CardID, employee.Address, employee.Title,
                                           employee.DriverLicenseRank, employee.DriverLicenseAddress, employee.DriverLicenseID, employee.DriverLicenseDate,
                                           employee.DriverLicenseExpirationDate, employee.StartDate, employee.Violation, employee.Notes });
            }

            return dt;
        }

        private DataTable BuildDataTableForCustomer()
        {
            DataTable dt = new DataTable();
            dt.TableName = "KhachHang_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã khách hàng", typeof(string));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Số phát sinh", typeof(double));
            dt.Columns.Add("Tổng trả", typeof(double));
            dt.Columns.Add("Còn nợ lại", typeof(double));
            dt.Columns.Add("Xếp loại", typeof(string));

            //Binding data
            List<Customer> customers = ClarityDB.Instance.Customers.ToList();
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                dt.Rows.Add(new object[] { i + 1 , customer.Code, customer.FullName , customer.PhoneNo,
                    customer.TotalOwned, customer.TotalPay, customer.TotalDebt, customer.Type });
            }

            return dt;
        }

        private DataTable BuildDataTableForOrderCustomer(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            dt.TableName = "DonHang_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã đơn hàng", typeof(string));
            dt.Columns.Add("Tên khách hàng", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Khu vực", typeof(string));
            dt.Columns.Add("Nơi đi", typeof(string));
            dt.Columns.Add("Nơi đến", typeof(string));
            dt.Columns.Add("Ngày đi", typeof(string));
            dt.Columns.Add("Ngày đến", typeof(string));
            dt.Columns.Add("Đơn vị tính", typeof(string));
            dt.Columns.Add("Số lượng", typeof(double));
            dt.Columns.Add("Đơn giá", typeof(double));
            dt.Columns.Add("Thành tiền", typeof(double));
            dt.Columns.Add("Số xe", typeof(string));
            dt.Columns.Add("Ghi chú", typeof(string));

            //Filter data by from/to date
            List<CustomerOrder> customerOrders = ClarityDB.Instance.CustomerOrders.ToList();
            List<CustomerOrder> filteredCustomerOrders = new List<CustomerOrder>();
            foreach (CustomerOrder customerOrder in customerOrders)
            {
                DateTime departDate = DateTime.ParseExact(customerOrder.DepartDate, formatDate, CultureInfo.InvariantCulture);
                if (DateTime.Compare(departDate, fromDate) >= 0 && DateTime.Compare(departDate, toDate) <= 0)
                {
                    filteredCustomerOrders.Add(customerOrder);
                }
            }

            //Binding data
            for (int i = 0; i < filteredCustomerOrders.Count; i++)
            {
                var customerOrder = filteredCustomerOrders[i];
                dt.Rows.Add(new object[] { i + 1 , customerOrder.Code, customerOrder.CustomerName , customerOrder.CustomerPhone, customerOrder.CustomerArea,
                                           customerOrder.Departure, customerOrder.Destination, customerOrder.DepartDate, customerOrder.ReturnDate,
                                           customerOrder.Unit, customerOrder.Quantity, customerOrder.UnitPrice, customerOrder.TotalPay,
                                           String.IsNullOrEmpty(customerOrder.TruckID) ? "" : this.helperService.GetLicensePlate(Convert.ToInt32(customerOrder.TruckID)), customerOrder.Notes});
            }
            return dt;
        }

        private DataTable BuildDataTableForWagon(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            dt.TableName = "ToaHang_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã toa hàng", typeof(string));
            dt.Columns.Add("Ngày thanh toán", typeof(string));
            dt.Columns.Add("Nơi thanh toán", typeof(string));
            dt.Columns.Add("Ngày đi", typeof(string));
            dt.Columns.Add("Ngày đến", typeof(string));
            dt.Columns.Add("Số xe", typeof(string));
            dt.Columns.Add("Tài xế", typeof(string));
            dt.Columns.Add("Ghi chú", typeof(string));
            dt.Columns.Add("Phí xe", typeof(double));
            dt.Columns.Add("Điện thoại + Dịch vụ", typeof(double));
            dt.Columns.Add("Phí phát sinh", typeof(double));
            dt.Columns.Add("Nguyên nhân phát sinh", typeof(string));
            dt.Columns.Add("Biên bản phạt", typeof(double));
            dt.Columns.Add("Diễn giải phụ", typeof(double));
            dt.Columns.Add("Tiền xe", typeof(double));
            dt.Columns.Add("Sửa xe", typeof(double));
            dt.Columns.Add("Tiền dâu", typeof(double));
            dt.Columns.Add("Lượng", typeof(double));
            dt.Columns.Add("Dịch vụ", typeof(double));
            dt.Columns.Add("Hàng về", typeof(double));
            dt.Columns.Add("Trích 10%", typeof(double));
            dt.Columns.Add("Khác", typeof(double));

            //Filter data by from/to date
            List<Wagon> wagons = ClarityDB.Instance.Wagons.ToList();
            List<Wagon> filteredWagons = new List<Wagon>();
            foreach (Wagon wagon in wagons)
            {
                DateTime paymentDate = DateTime.ParseExact(wagon.PaymentDate, formatDate, CultureInfo.InvariantCulture);
                if (DateTime.Compare(paymentDate, fromDate) >= 0 && DateTime.Compare(paymentDate, toDate) <= 0)
                {
                    filteredWagons.Add(wagon);
                }
            }
            
            //Binding data
            for (int i = 0; i < filteredWagons.Count; i++)
            {
                var wagon = filteredWagons[i];
                dt.Rows.Add(new object[] { i + 1 , wagon.Code, wagon.PaymentDate , wagon.PaymentPlace, wagon.DepartDate,
                                           wagon.ReturnDate, wagon.Truck.LicensePlate, wagon.Employee.FullName,
                                           wagon.Notes, wagon.CostOfTruck, wagon.CostOfService, wagon.CostOfTangBoXe,
                                           wagon.ReasonForPhiPhatXinh, wagon.CostOfPenalty, wagon.CostOfExtra,
                                           wagon.PaymentOfTruck, wagon.PaymentOfRepairing, wagon.PaymentOfOil, wagon.PaymentOfLuong,
                                           wagon.PaymentOfService, wagon.PaymentOfHangVe, wagon.PaymentOf10Percent, wagon.PaymentOfOthers});
            }

            return dt;
        }

        private DataTable BuildDataTableForWagonSettlement(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            dt.TableName = "QuyetToan_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã toa hàng", typeof(string));
            dt.Columns.Add("Mã quyết toán", typeof(string));
            dt.Columns.Add("Khách hàng", typeof(string));
            dt.Columns.Add("Ngày phát sinh", typeof(string));
            dt.Columns.Add("Nơi thanh toán", typeof(string));
            dt.Columns.Add("Ngày giao dịch", typeof(string));
            dt.Columns.Add("Nơi đi", typeof(string));
            dt.Columns.Add("Nơi đến", typeof(string));
            dt.Columns.Add("Đơn vị tính", typeof(string));
            dt.Columns.Add("Số lượng", typeof(double));
            dt.Columns.Add("Đơn giá", typeof(double));
            dt.Columns.Add("Tổng phí", typeof(double));
            dt.Columns.Add("Đã thanh toán", typeof(double));
            dt.Columns.Add("Chiết khấu", typeof(double));
            dt.Columns.Add("Còn lại", typeof(double));

            //Filter data by from/to date
            List<WagonSettlement> wagonSettlements = ClarityDB.Instance.WagonSettlements.ToList();
            List<WagonSettlement> filteredWagonSettlements = new List<WagonSettlement>();
            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                if (wagonSettlement.PaymentDate != null) {
                    DateTime paymentDate = DateTime.ParseExact(wagonSettlement.PaymentDate, formatDate, CultureInfo.InvariantCulture);
                    if (DateTime.Compare(paymentDate, fromDate) >= 0 && DateTime.Compare(paymentDate, toDate) <= 0)
                    {
                        filteredWagonSettlements.Add(wagonSettlement);
                    }
                }
            }

            //Binding data
            for (int i = 0; i < filteredWagonSettlements.Count; i++)
            {
                var wagonSettlement = filteredWagonSettlements[i];
                dt.Rows.Add(new object[] { i + 1 , wagonSettlement.WagonCode, wagonSettlement.Code , wagonSettlement.Customer.FullName, wagonSettlement.Date,
                                           wagonSettlement.PaymentPlace, wagonSettlement.PaymentDate, wagonSettlement.Departure,
                                           wagonSettlement.Destination, wagonSettlement.Unit, wagonSettlement.Quantity, wagonSettlement.UnitPrice,
                                           wagonSettlement.TotalAmount, wagonSettlement.Payment, wagonSettlement.Discount, wagonSettlement.PaymentRemain});
            }

            return dt;
        }

        private DataTable BuildDataTableForGarage(JArray jsonList)
        {
            DataTable dt = new DataTable();
            dt.TableName = "Garage_" + DateTime.Now.ToString(formatStringDate);
            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("SL nhập", typeof(int));
            dt.Columns.Add("Tổng giá trị nhâp", typeof(int));
            dt.Columns.Add("SL bán", typeof(int));
            dt.Columns.Add("Tổng giá trị bán", typeof(int));
            dt.Columns.Add("SL hiện tại", typeof(int));
            dt.Columns.Add("Lợi nhuận ước tính", typeof(int));

            //Binding data
            int index = 1;
            foreach (JObject json in jsonList)
            {
                ProductInfo productInfo = ProductInfo.FromJson(json);
                dt.Rows.Add(new object[] { index , productInfo.Name, productInfo.SumOfInput, productInfo.SumOfInputTotalAmount,
                                            productInfo.SumOfSale, productInfo.SumOfSaleTotalAmount, productInfo.NumOfRemain, productInfo.Profit});
                index++;
            }

            return dt;
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

        private string saveExcelFile(DataTable dt, string fileName)
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string physicalPath = HttpContext.Current.Request.MapPath(appPath).TrimEnd('\\');
            string folderName = string.Format("{0}", Guid.NewGuid());
            string outputFolder = string.Format("{0}\\output\\{1}", physicalPath, folderName);
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            string fileNamePath = string.Format("{0}\\{1}.xlsx", outputFolder, fileName);

            CreateExcelFile.CreateExcelDocument(dt, fileNamePath);

            return string.Format("{0}/{1}.xlsx", folderName, fileName);
        }

    }
}

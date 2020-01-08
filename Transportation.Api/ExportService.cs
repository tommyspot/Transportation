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
        const string underline = "_";

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
                else if (json.Value<int>("type") == (int)ExportType.GarageOrder)
                {
                    DateTime fromDate = DateTime.ParseExact(json.Value<string>("fromDate"), formatDate, CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(json.Value<string>("toDate"), formatDate, CultureInfo.InvariantCulture);
                    dt = BuildDataTableForGarageOrder(fromDate, toDate);
                    fileName = "Baocao_Garage_DonHang_" + DateTime.Now.ToString(formatStringDate);
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
            dt.Columns.Add("Còn lưu hành", typeof(string));

            //Binding data
            List<Truck> trucks = ClarityDB.Instance.Trucks.OrderByDescending(x => x.ID).ToList();
            for (int i = 0; i < trucks.Count; i++)
            {
                var truck = trucks[i];
                dt.Rows.Add(new object[] { i + 1 , truck.LicensePlate, truck.YearOfManufacture , truck.Vin, truck.EngineNo,
                                           truck.Brand, truck.Weight, truck.BuyingDate, truck.StartUsingDate,
                                           truck.MonthlyPayment, truck.Stock, truck.CheckDate, truck.InsuranceDate,
                                           String.IsNullOrEmpty(truck.EmployeeId) ? "" : this.helperService.GetEmployeeName(Convert.ToInt32(truck.EmployeeId)),
                                           truck.IsDeleted ? "Không" : "Có"
                });
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
            dt.Columns.Add("Tình trạng", typeof(string));

            //Binding data
            List<Employee> employees = ClarityDB.Instance.Employees.OrderByDescending(x => x.ID).ToList();
            for (int i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                dt.Rows.Add(new object[] { i + 1 , employee.FullName, employee.Mobile , employee.CardID, employee.Address, employee.Title,
                                           employee.DriverLicenseRank, employee.DriverLicenseAddress, employee.DriverLicenseID, employee.DriverLicenseDate,
                                           employee.DriverLicenseExpirationDate, employee.StartDate, employee.Violation, employee.Notes,
                                           employee.IsDeleted ? "Không còn làm việc" : "Còn làm việc"
                });
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
            dt.Columns.Add("Nhân viên thu nợ", typeof(string));

            // Build meta data for PhiPhatSinh/Payment for each month
            var periods = GetPeriods();
            foreach (string period in periods)
            {
                dt.Columns.Add("Phát sinh - Tháng " + period, typeof(double));
                dt.Columns.Add("Thanh toán - Tháng " + period, typeof(double));
            }

            //Binding data
            List<Customer> customers = ClarityDB.Instance.Customers.OrderByDescending(x => x.ID).ToList();
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                customer.TotalOwned = GetTotalOwnedByCustomerID(customer.ID);
                customer.TotalPay = GetTotalPaymentByCustomerID(customer.ID);
                customer.TotalDebt = customer.TotalOwned - customer.TotalPay;
                // add to row
                DataRow dataRow = dt.NewRow();
                dataRow[0] = i + 1;
                dataRow[1] = customer.Code;
                dataRow[2] = customer.FullName;
                dataRow[3] = customer.PhoneNo;
                dataRow[4] = customer.TotalOwned;
                dataRow[5] = customer.TotalPay;
                dataRow[6] = customer.TotalDebt;
                dataRow[7] = customer.Type;
                dataRow[8] = String.IsNullOrEmpty(customer.EmployeeId)
                    ? ""
                    : this.helperService.GetEmployeeName(Convert.ToInt32(customer.EmployeeId));
                // Build data for PhiPhatSinh/Payment for each month
                int step = 0;
                foreach (string period in periods)
                {
                    dataRow[9 + step] = GetPhiPhatSinhByCustomerIDInPeriod(customer.ID, period);    // Calculate Phi Phat Sinh
                    dataRow[10 + step] = GetPaymentByCustomerIDInPeriod(customer.ID, period);        // Calculate Thanh Toan
                    step += 2;
                }
                dt.Rows.Add(dataRow);
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
            dt.Columns.Add("Nơi đi", typeof(string));
            dt.Columns.Add("Nơi đến", typeof(string));
            dt.Columns.Add("Số xe", typeof(string));
            dt.Columns.Add("Tài xế", typeof(string));
            dt.Columns.Add("Phí xe", typeof(double));
            dt.Columns.Add("Điện thoại + Dịch vụ", typeof(double));
            dt.Columns.Add("Tăng bo xe", typeof(double));
            dt.Columns.Add("Biên bản phạt", typeof(double));
            dt.Columns.Add("Diễn giải phụ", typeof(string));
            dt.Columns.Add("Phí diễn giải phụ", typeof(double));
            dt.Columns.Add("Tiền xe", typeof(double));
            dt.Columns.Add("Sửa xe", typeof(double));
            dt.Columns.Add("Tiền dầu", typeof(double));
            dt.Columns.Add("Lượng", typeof(double));
            dt.Columns.Add("Dịch vụ", typeof(double));
            dt.Columns.Add("Hàng về", typeof(double));
            dt.Columns.Add("Chênh lệch", typeof(double));
            dt.Columns.Add("Tổng tiền hàng", typeof(double));

            //Filter data by from/to date
            List<Wagon> wagons = ClarityDB.Instance.Wagons.OrderByDescending(x => x.ID).ToList();
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
                double totalAmount = 0;
                foreach (WagonSettlement wagonSettlement in wagon.WagonSetlements) {
                    totalAmount += wagonSettlement.Quantity * wagonSettlement.UnitPrice;
                }

                dt.Rows.Add(new object[] { i + 1 , wagon.Code, wagon.PaymentDate , wagon.PaymentPlace, wagon.DepartDate,
                                           wagon.ReturnDate, wagon.Departure, wagon.Destination, wagon.Truck.LicensePlate, wagon.Employee.FullName,
                                           wagon.CostOfTruck, wagon.CostOfService, wagon.CostOfTangBoXe,
                                           wagon.CostOfPenalty, wagon.TextOfExtra, wagon.CostOfExtra, 
                                           wagon.PaymentOfTruck, wagon.PaymentOfRepairing, wagon.PaymentOfOil, wagon.PaymentOfLuong,
                                           wagon.PaymentOfService, wagon.PaymentOfHangVe, wagon.PaymentOf10Percent, totalAmount });
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
            dt.Columns.Add("Ngày phát sinh", typeof(string));   // ToaHang's NgayThanhToan
            dt.Columns.Add("Ngày đi", typeof(string));
            dt.Columns.Add("Số lượng", typeof(double));
            dt.Columns.Add("Nơi thanh toán", typeof(string));
            dt.Columns.Add("Ngày thanh toán", typeof(string));
            dt.Columns.Add("Nơi đến", typeof(string));
            dt.Columns.Add("Lý do phát sinh", typeof(string));
            dt.Columns.Add("Phí phát sinh", typeof(double));
            dt.Columns.Add("Đơn vị tính", typeof(string));
            dt.Columns.Add("Đơn giá", typeof(double));
            dt.Columns.Add("Thành tiền", typeof(double));
            dt.Columns.Add("Đã thanh toán", typeof(double));
            dt.Columns.Add("Còn lại", typeof(double));
            dt.Columns.Add("Nợ", typeof(string));

            //Filter data by from/to date
            List<WagonSettlement> wagonSettlements = ClarityDB.Instance.WagonSettlements.OrderByDescending(x => x.ID).ToList();
            List<WagonSettlement> filteredWagonSettlements = new List<WagonSettlement>();
            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                if (!String.IsNullOrEmpty(wagonSettlement.PaymentDate)) {
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

                dt.Rows.Add(new object[] { i + 1 , wagonSettlement.Wagon.Code, wagonSettlement.Code , wagonSettlement.Customer.FullName, wagonSettlement.Wagon.PaymentDate,
                                           wagonSettlement.Wagon.DepartDate, wagonSettlement.Quantity,wagonSettlement.PaymentPlace, wagonSettlement.PaymentDate,
                                           wagonSettlement.Destination, wagonSettlement.LyDoPhatSinh, wagonSettlement.PhiPhatSinh, wagonSettlement.Unit,
                                           wagonSettlement.UnitPrice, wagonSettlement.Quantity * wagonSettlement.UnitPrice, wagonSettlement.Payment,
                                           wagonSettlement.PaymentRemain, wagonSettlement.PaymentStatus});
            }

            return dt;
        }

        private DataTable BuildDataTableForGarageOrder(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            dt.TableName = "QuyetToan_" + DateTime.Now.ToString(formatStringDate);

            //Add table headers going cell by cell.
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Số xe", typeof(string));
            dt.Columns.Add("Tên garage", typeof(string));
            dt.Columns.Add("Ghi chú", typeof(string));
            dt.Columns.Add("Ngày bán", typeof(string));
            dt.Columns.Add("Thành tiền", typeof(double));
            dt.Columns.Add("Giảm giá", typeof(string));
            dt.Columns.Add("Mô tả", typeof(string));
            dt.Columns.Add("Đơn vị tính", typeof(string));

            //Filter data by from/to date
            List<Order> orders = ClarityDB.Instance.Orders.OrderByDescending(x => x.ID).ToList();
            List<Order> filteredOrders = new List<Order>();
            foreach (Order order in orders)
            {
                if (!String.IsNullOrEmpty(order.Date))
                {
                    DateTime date = DateTime.ParseExact(order.Date, formatDate, CultureInfo.InvariantCulture);
                    if (DateTime.Compare(date, fromDate) >= 0 && DateTime.Compare(date, toDate) <= 0)
                    {
                        filteredOrders.Add(order);
                    }
                }
            }

            //Binding data
            for (int i = 0; i < filteredOrders.Count; i++)
            {
                var order = filteredOrders[i];
                dt.Rows.Add(new object[] { i + 1 , order.LicensePlate , order.CustomerName, order.Address,
                                           order.Date, order.TotalAmount, order.SaleOff.ToString() + '%', order.Note, order.Unit});
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
            dt.Columns.Add("Tổng giá trị nhâp", typeof(double));
            dt.Columns.Add("SL bán", typeof(int));
            dt.Columns.Add("Tổng giá trị bán", typeof(double));
            dt.Columns.Add("SL hiện tại", typeof(int));
            dt.Columns.Add("Lợi nhuận ước tính", typeof(double));

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

        // Ultil function
        private long GetTotalOwnedByCustomerID(long id)
        {
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == id);
            return wagonSettlements.Count() > 0 ? wagonSettlements.Sum(x => x.Quantity * x.UnitPrice + x.PhiPhatSinh) : 0;
        }
        private long GetTotalPaymentByCustomerID(long id)
        {
            // Get from WagonSettlement
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == id);
            long totalPaymentFromWagonSettlement = wagonSettlements.Count() > 0 ? wagonSettlements.Sum(x => x.Payment) : 0;
            // Get from Payment
            var payments = ClarityDB.Instance.Payments.Where(x => x.CustomerID == id);
            long totalPaymentFromPayment = payments.Count() > 0 ? payments.Sum(x => x.PaymentAmount) : 0;
            return (totalPaymentFromWagonSettlement + totalPaymentFromPayment);
        }
        private List<string> GetPeriods()
        {
            // From WagonSettlements
            var periodsFromWagonSettlements = new List<string>();
            var paymentDates = ClarityDB.Instance.WagonSettlements
                .Select(x => x.PaymentDate)
                .Distinct();
            foreach (string paymentDate in paymentDates) {
                DateTime date = ConvertToDate(paymentDate);
                periodsFromWagonSettlements.Add(date.Month + underline + date.Year);    // Ex: 12_2017
            }
            periodsFromWagonSettlements = periodsFromWagonSettlements.Distinct().ToList();

            // From WagonSettlements
            var periodsFromPayments = ClarityDB.Instance.Payments
                .Select(x => x.PaymentMonth + underline + x.PaymentYear)
                .Distinct().ToList();

            return periodsFromWagonSettlements.Union(periodsFromPayments)
                .OrderByDescending(x => x.Split(underline.ToCharArray()).Last())
                .ThenByDescending(x => x.Split(underline.ToCharArray()).First())
                .ToList();
        }

        private double GetPhiPhatSinhByCustomerIDInPeriod(long customerId, string period)
        {
            string month = period.Split(underline.ToCharArray()).FirstOrDefault();
            string year = period.Split(underline.ToCharArray()).LastOrDefault();
            if (String.IsNullOrEmpty(month) || String.IsNullOrEmpty(year))
            {
                return 0;
            }

            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == customerId);
            double result = 0;
            foreach(WagonSettlement wagonSettlement in wagonSettlements)
            {
                DateTime paymentDate = ConvertToDate(wagonSettlement.PaymentDate);
                if(paymentDate.Month == Convert.ToInt32(month.Trim()) && paymentDate.Year == Convert.ToInt32(year.Trim()))
                {
                    result += wagonSettlement.Quantity * wagonSettlement.UnitPrice + wagonSettlement.PhiPhatSinh;
                }
            }

            return result;
        }

        private double GetPaymentByCustomerIDInPeriod(long customerId, string period)
        {
            string month = period.Split(underline.ToCharArray()).FirstOrDefault();
            string year = period.Split(underline.ToCharArray()).LastOrDefault();
            if (String.IsNullOrEmpty(month) || String.IsNullOrEmpty(year))
            {
                return 0;
            }
            // For WagonSettlement
            var wagonSettlements = ClarityDB.Instance.WagonSettlements.Where(x => x.CustomerID == customerId);
            double result = 0;
            foreach (WagonSettlement wagonSettlement in wagonSettlements)
            {
                DateTime paymentDate = ConvertToDate(wagonSettlement.PaymentDate);
                if (paymentDate.Month == ConvertToValue(month) && paymentDate.Year == ConvertToValue(year))
                {
                    result += wagonSettlement.Payment;
                }
            }
            // For Payments
            var payments = ClarityDB.Instance.Payments.Where(x => x.CustomerID == customerId);
            foreach (Payment payment in payments)
            {
                if (payment.PaymentMonth == ConvertToValue(month) && payment.PaymentYear == ConvertToValue(year))
                {
                    result += payment.PaymentAmount;
                }
            }

            return result;
        }
        private DateTime ConvertToDate(string date)
        {
            return DateTime.ParseExact(date, formatDate, CultureInfo.InvariantCulture);
        }

        private int ConvertToValue(string data)
        {
            return Convert.ToInt32(data.Trim());
        }
    }
}

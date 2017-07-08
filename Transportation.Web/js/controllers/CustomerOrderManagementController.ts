/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class CustomerOrderManagementController {
		public currentCustomerOrder: Model.CustomerOrderModel;
    public customerOrderService: service.CustomerOrderService;
		public employeeService: service.EmployeeService;
		public truckService: service.TruckService;
		public customerList: Array<Model.CustomerModel>;
		public truckList: Array<Model.TruckModel>;
		public customerService: service.CustomerService;
		public employeeList: Array<Model.EmployeeModel>;
    public customerOrderList: Array<Model.CustomerOrderModel>;
		public customerOrderListTmp: Array<Model.CustomerOrderModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
		public areas: Array<String>;
		public unitPriceFormated: string;
    public mainHelper: helper.MainHelper;
    public isLoading: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
			private $routeParams: any, private $cookieStore: ng.ICookieStoreService) {

			this.customerOrderService = new service.CustomerOrderService($http);
			this.employeeService = new service.EmployeeService($http);
			this.customerService = new service.CustomerService($http);
			this.truckService = new service.TruckService($http);
      $scope.viewModel = this;
			this.pageSize = 10;
      this.initCustomerOrder();

			var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.customerOrderListTmp && self.customerOrderListTmp.length > 0) {
          self.customerOrderList = $filter('filter')(self.customerOrderListTmp, value);
          self.initPagination();
        }
      });
			this.mainHelper = new helper.MainHelper($http, $cookieStore);
			this.areas = ['An Giang', 'Bà Rịa - Vũng Tàu', 'Bắc Giang', 'Bắc Kạn', 'Bạc Liêu', 'Bắc Ninh', 'Bến Tre', 'Bình Định', 'Bình Dương', 'Bình Phước', 'Bình Thuận',
				'Cà Mau', 'Cần Thơ', 'Cao Bằng', 'Đà Nẵng', 'Đắk Lắk', 'Đắk Nông', 'Điện Biên', 'Đồng Nai', 'Đồng Tháp', 'Gia Lai', 'Hà Giang', 'Hà Nam', 'Hà Nội', 'Hà Tĩnh',
				'Hải Dương', 'Hải Phòng', 'Hậu Giang', 'Hòa Bình', 'Hưng Yên', 'Khánh Hòa', 'Kiên Giang', 'Kon Tum', 'Lai Châu', 'Lâm Đồng', 'Lạng Sơn', 'Lào Cai', 'Long An',
				'Nam Định', 'Nghệ An', 'Ninh Bình', 'Ninh Thuận', 'Phú Thọ', 'Phú Yên', 'Quảng Bình', 'Quảng Nam', 'Quảng Ngãi', 'Quảng Ninh', 'Quảng Trị', 'Sóc Trăng',
				'Sơn La', 'Tây Ninh', 'Thái Bình', 'Thái Nguyên', 'Thanh Hóa', 'Thừa Thiên Huế', 'Tiền Giang', 'TP HCM', 'Trà Vinh', 'Tuyên Quang', 'Vĩnh Long', 'Vĩnh Phúc',
				'Yên Bái'];
    }

		initEmployeeList() {
      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
      }, null);
    }

    initCustomerList() {
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
      }, null);
    }

		initTruckList() {
      this.truckService.getAll((results: Array<Model.TruckModel>) => {
        this.truckList = results;
      }, null);
    }

		initCustomerOrder() {
			this.initCustomerList();
			this.initTruckList();
      var customerOrderId = this.$routeParams.customerOrder_id;
			
      if (customerOrderId) {
        if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/' + customerOrderId) {
          this.customerOrderService.getById(customerOrderId, (data) => {
            this.currentCustomerOrder = data;
						this.unitPriceFormated = data.unitPrice != 0 ? data.unitPrice.toLocaleString() : '';
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/sua/' + customerOrderId) {
          if (this.currentCustomerOrder == null) {
            this.customerOrderService.getById(customerOrderId, (data) => {
              this.currentCustomerOrder = data;
							this.unitPriceFormated = data.unitPrice != 0 ? data.unitPrice.toLocaleString() : '';
							this.currentCustomerOrder.totalPayFormated = data.totalPay != 0 ? data.totalPay.toLocaleString() : '';
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/tao') {
          this.currentCustomerOrder = new Model.CustomerOrderModel();
        } else if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach') {
          this.isLoading = true;
          this.initCustomerOrderList();
        }
      }
    }

		initCustomerOrderList() {
      this.customerOrderService.getAll((results: Array<Model.CustomerOrderModel>) => {
        this.customerOrderList = results;
        this.customerOrderList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
        this.customerOrderListTmp = this.customerOrderList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.customerOrderList.length % this.pageSize === 0 ?
        this.customerOrderList.length / this.pageSize : Math.floor(this.customerOrderList.length / this.pageSize) + 1;
    }

		getCustomerOrderListOnPage() {
      if (this.customerOrderList && this.customerOrderList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.customerOrderList.slice(startIndex, endIndex);
      }
    }

    getNumberPage() {
      if (this.numOfPages > 0) {
        return new Array(this.numOfPages);
      }
      return new Array(0);
    }

    goToPage(pageIndex: number) {
      this.currentPage = pageIndex;
    }

    goToPreviousPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
        this.goToPage(this.currentPage);
      }
    }
    goToNextPage() {
      if (this.currentPage < this.numOfPages) {
        this.currentPage++;
        this.goToPage(this.currentPage);
      }
    }

    selectAllCustomerOrdersOnPage() {
      var customerOnPage = this.getCustomerOrderListOnPage();
      for (let index = 0; index < customerOnPage.length; index++) {
        var customerOrder = customerOnPage[index];
        customerOrder.isChecked = this.isCheckedAll;
      }
    }

    removeCustomerOrders() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng của khách hàng này?');
      if (confirmDialog) {
        for (let i = 0; i < this.customerOrderList.length; i++) {
          var customerOrder = this.customerOrderList[i];
          if (customerOrder.isChecked) {
            this.customerOrderService.deleteEntity(customerOrder, (data) => {
              this.initCustomerOrderList();
            }, () => { });
          }
        }
      }
    }

    removeCustomerOrderInDetail(customerOrder: Model.CustomerOrderModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng của khách hàng này?');
      if (confirmDialog) {
        this.customerOrderService.deleteEntity(customerOrder, (data) => {
          this.$location.path('/ql-toa-hang/don-hang-cua-khach');
        }, null);
      }
    }

    createCustomerOrder(customerOrder: Model.CustomerOrderModel) {

			if (customerOrder.customerId) {
				this.customerOrderService.create(customerOrder,
					(data) => {
						this.$location.path('/ql-toa-hang/don-hang-cua-khach');
					},
					() => { });

			} else {

				var newCustomer = new Model.CustomerModel();
				newCustomer.fullName = customerOrder.customerName;
				newCustomer.phoneNo = customerOrder.customerPhone;

				this.customerService.create(newCustomer, (data) => {
					customerOrder.customerId = data.id;
					customerOrder.customerCode = data.code;
					this.customerOrderService.create(customerOrder,
						(data) => {
							this.$location.path('/ql-toa-hang/don-hang-cua-khach');
						},
						() => { });
				}, null);
			}
    }

		updateCustomerOrder(customerOrder: Model.CustomerOrderModel) {
      this.customerOrderService.update(customerOrder, (data) => {
        this.$location.path('/ql-toa-hang/don-hang-cua-khach');
      }, null);
    }

    goToCustomerOrderForm() {
      this.$location.path('/ql-toa-hang/don-hang-cua-khach/tao');
    }

		applyCustomer(id) {
			if (this.currentCustomerOrder && this.currentCustomerOrder.customerId && this.customerList) {
				for (var i = 0; i < this.customerList.length; i++) {
					var customerOrderId = this.customerList[i].id;
					if (customerOrderId == id) {
						this.currentCustomerOrder.customerName = this.customerList[i].fullName;
						this.currentCustomerOrder.customerPhone = this.customerList[i].phoneNo;
						this.currentCustomerOrder.customerCode = this.customerList[i].code;
						break;
					}
				}

      }
      else {
				this.currentCustomerOrder.customerName = '';
				this.currentCustomerOrder.customerPhone = '';
				this.currentCustomerOrder.customerArea = '';
				this.currentCustomerOrder.customerCode = '';
			}
		}

		getEmployeeName(id) {
			if (this.employeeList){
				for (var i = 0; i < this.employeeList.length; i++) {
					var employee = this.employeeList[i];
					if (employee.id == id) {
						return employee.fullName;
					}
				}
			}
			return '';
		}

		changeFormatNumber(changeFormatNumber) {
			if (changeFormatNumber && changeFormatNumber != '') {
				this.currentCustomerOrder.unitPrice = parseInt(changeFormatNumber.replace(/,/g, ''));
				this.calculateTotalPay();
				this.unitPriceFormated = this.currentCustomerOrder.unitPrice.toLocaleString();
			}
		}

		getCustomCode(id) {
			if (this.customerList) {
				for (var i = 0; i < this.customerList.length; i++) {
					var customer = this.customerList[i];
					if (customer.id == id) {
						return customer.code;
					}
				}
			}
			return '';
		}

		calculateTotalPay() {
			if (this.currentCustomerOrder.unitPrice && this.currentCustomerOrder.quantity){
				this.currentCustomerOrder.totalPay = this.currentCustomerOrder.unitPrice * this.currentCustomerOrder.quantity;
				this.currentCustomerOrder.totalPayFormated = this.currentCustomerOrder.totalPay.toLocaleString();
			}
		}

		getCustomerOrderCode() {
			if (this.currentCustomerOrder.departDate && this.currentCustomerOrder.truckId) {
        this.currentCustomerOrder.truckLicensePlate = this.getTruckLicensePlate(this.currentCustomerOrder.truckId);
        this.currentCustomerOrder.code = this.currentCustomerOrder.departDate.replace(/\//g, '') + '_' + this.currentCustomerOrder.truckLicensePlate;
			}
		}

		getTruckLicensePlate(id) {
			if (this.truckList){
				for (var i = 0; i < this.truckList.length; i++) {
					var truck = this.truckList[i];
					if (truck.id == id) {
						return truck.licensePlate;
					}
				}
			}
			return '';
		}

	}
}
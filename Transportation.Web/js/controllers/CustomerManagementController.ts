/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
  import helper = Clarity.Helper;

  const formatSuffix = 'Formatted';

  export class CustomerManagementController {
    public customerService: service.CustomerService;
    public employeeService: service.EmployeeService;
    public mainHelper: helper.MainHelper;

    public currentCustomer: Model.CustomerModel;
    public customerList: Array<Model.CustomerModel>;
		public customerListTmp: Array<Model.CustomerModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
		public totalOwnedFormated: string;
		public totalPayFormated: string;
    public totalDebtFormated: string;
    public isLoading: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
      private $routeParams: any,
      private $cookieStore: ng.ICookieStoreService) {

			this.customerService = new service.CustomerService($http);
      this.employeeService = new service.EmployeeService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore); 
      $scope.viewModel = this;
			this.pageSize = 10;
      this.initCustomer();

			var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.customerListTmp && self.customerListTmp.length > 0) {
          self.customerList = $filter('filter')(self.customerListTmp, value);
          self.initPagination();
        }
      });
    }

		initCustomer() {
      var customerId = this.$routeParams.customer_id;
			this.initCustomerList();
      if (customerId) {
        if (this.$location.path() === '/ql-toa-hang/khach-hang/' + customerId) {
          this.customerService.getById(customerId, (data) => {
            this.currentCustomer = data;
            this.mainHelper.initFormattedProperty(this.currentCustomer, ['totalOwned', 'totalPay', 'totalDebt'], formatSuffix);
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/khach-hang/sua/' + customerId) {
          if (this.currentCustomer == null) {
            this.customerService.getById(customerId, (data) => {
              this.currentCustomer = data;
              this.mainHelper.initFormattedProperty(this.currentCustomer, ['totalOwned', 'totalPay', 'totalDebt'], formatSuffix);
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/khach-hang/tao') {
          this.currentCustomer = new Model.CustomerModel();
        }
      }
    }

    initCustomerList() {
      this.isLoading = true;
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
        this.customerList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.customerListTmp = this.customerList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.customerList.length % this.pageSize === 0 ?
        this.customerList.length / this.pageSize : Math.floor(this.customerList.length / this.pageSize) + 1;
    }

		getCustomerListOnPage() {
      if (this.customerList && this.customerList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.customerList.slice(startIndex, endIndex);
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

    selectAllCustomersOnPage() {
      var customerOnPage = this.getCustomerListOnPage();
      for (let index = 0; index < customerOnPage.length; index++) {
        var customer = customerOnPage[index];
        customer.isChecked = this.isCheckedAll;
      }
    }

    removeCustomers() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những khách hàng được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.customerList.length; i++) {
          var Customer = this.customerList[i];
          if (Customer.isChecked) {
            this.customerService.deleteEntity(Customer, (data) => {
              this.initCustomerList();
            }, () => { });
          }
        }
      }
    }

    removeCustomerInDetail(Customer: Model.CustomerModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa khách hàng này?');
      if (confirmDialog) {
        this.customerService.deleteEntity(Customer, (data) => {
          this.$location.path('/ql-toa-hang/khach-hang');
        }, null);
      }
    }

    createCustomer(customer: Model.CustomerModel) {
      this.customerService.create(customer,
        (data) => {
          this.$location.path('/ql-toa-hang/khach-hang');
        },
        () => { });
    }

		updateCustomer(customer: Model.CustomerModel) {
      this.customerService.update(customer, (data) => {
        this.$location.path('/ql-toa-hang/khach-hang');
      }, null);
    }

    goToCustomerForm() {
      this.$location.path('/ql-toa-hang/khach-hang/tao');
    }

    goToCustomerEditForm(event: Event, customerId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-toa-hang/khach-hang/sua/${customerId}`);
    }

    formatCurrency(propertyName) {
      this.mainHelper.formatCurrency(this.currentCustomer, propertyName, `${propertyName}${formatSuffix}`);
		}

	}
}
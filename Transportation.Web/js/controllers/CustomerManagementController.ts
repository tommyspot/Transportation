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
    public customerListView: Array<Model.CustomerViewModel>;
    public customerListViewTmp: Array<Model.CustomerViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;

    public isCheckedAll: boolean;
    public isLoading: boolean;
    public searchText: string;
    public errorMessage: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
      private $routeParams: any,
      private $cookieStore: ng.ICookieStoreService,
      private $timeout: ng.ITimeoutService) {

			this.customerService = new service.CustomerService($http);
      this.employeeService = new service.EmployeeService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter); 
      $scope.viewModel = this;

      this.pageSize = 10;
      this.searchText = '';
      this.errorMessage = '';
      this.initCustomer();

			var self = this;
      $scope.$watch('viewModel.searchText', function (value) {
        if (self.customerListViewTmp && self.customerListViewTmp.length > 0) {
          self.customerListView = $filter('filter')(self.customerListViewTmp, value);
          self.initPagination();
        }
      });
    }

		initCustomer() {
      var customerId = this.$routeParams.customer_id;
      if (customerId) {
        this.initCurrentCustomer(customerId);
      } else {
        if (this.$location.path() === '/ql-toa-hang/khach-hang/tao') {
          this.currentCustomer = new Model.CustomerModel();
        } else if (this.$location.path() === '/ql-toa-hang/khach-hang') {
          this.initCustomerList();
        }
      }
    }

    initCurrentCustomer(customerId: number) {
      if (this.currentCustomer == null) {
        this.$rootScope.showSpinner();
        this.customerService.getById(customerId, (data) => {
          this.currentCustomer = data;
          this.mainHelper.initCurrencyFormattedProperty(this.currentCustomer,
            ['totalOwned', 'totalPay', 'totalDebt'], formatSuffix);
          this.$rootScope.hideSpinner();
        }, null);
      }
    }

    initCustomerList() {
      this.isLoading = true;
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
        this.customerList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.mapToCustomerListView();
        this.customerListViewTmp = this.customerListView;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    mapToCustomerListView() {
      this.customerListView = this.customerList.map((customer: Model.CustomerModel) => {
        this.mainHelper.initCurrencyFormattedProperty(customer,
          ['totalOwned', 'totalPay', 'totalDebt'],formatSuffix);
        const customerView = new Model.CustomerViewModel();
        customerView.id = customer.id;
        customerView.code = customer.code;
        customerView.fullName = customer.fullName;
        customerView.phoneNo = customer.phoneNo;
        customerView.totalOwned = customer.totalOwnedFormatted;
        customerView.totalPay = customer.totalPayFormatted;
        customerView.totalDebt = customer.totalDebtFormatted;
        return customerView;
      });
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.customerListView.length % this.pageSize === 0 ?
        this.customerListView.length / this.pageSize : Math.floor(this.customerListView.length / this.pageSize) + 1;
    }

    getCustomerListOnPage(): Array<Model.CustomerViewModel> {
      if (this.customerListView && this.customerListView.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.customerListView.slice(startIndex, endIndex);
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
        for (let i = 0; i < this.customerListView.length; i++) {
          const customer = this.customerListView[i];
          if (customer.isChecked) {
            this.customerService.deleteEntity(customer, (data) => {
              this.initCustomerList();
            }, (error) => {
              this.errorMessage = 'Khách hàng được chọn đã có trong Quyết toán';
              this.$timeout(() => {
                this.errorMessage = '';
              }, 8000);
            });
          }
        }
      }
    }

    removeCustomerInDetail(customer: Model.CustomerModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa khách hàng này?');
      if (confirmDialog) {
        this.customerService.deleteEntity(customer, (data) => {
          this.$location.path('/ql-toa-hang/khach-hang');
        }, (error) => {
          this.errorMessage = 'Khách hàng đã có trong Quyết toán';
          this.$timeout(() => {
            this.errorMessage = '';
          }, 8000);
        });
      }
    }

    createCustomer(customer: Model.CustomerModel) {
      this.isSubmitting = true;
      this.customerService.create(customer, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-toa-hang/khach-hang');
      }, null);
    }

    updateCustomer(customer: Model.CustomerModel) {
      this.isSubmitting = true;
      this.customerService.update(customer, (data) => {
        this.isSubmitting = false;
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

    clearSearchText() {
      this.searchText = '';
    }

	}
}
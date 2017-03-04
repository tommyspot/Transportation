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
		//public employeeService: service.EmployeeService;
		//public employeeList: Array<Model.EmployeeModel>;
    public customerOrderList: Array<Model.CustomerOrderModel>;
		public customerOrderListTmp: Array<Model.CustomerOrderModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
			private $routeParams: any) {

			this.customerOrderService = new service.CustomerOrderService($http);
			//this.employeeService = new service.EmployeeService($http);
      $scope.viewModel = this;
			this.pageSize = 5;
      this.initCustomerOrder();

			var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.customerOrderListTmp && self.customerOrderListTmp.length > 0) {
          self.customerOrderList = $filter('filter')(self.customerOrderListTmp, value);
          self.initPagination();
        }
      });
    }

		initCustomerOrder() {
      var customerId = this.$routeParams.customer_id;
      if (customerId) {
        if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/' + customerId) {
          this.customerOrderService.getById(customerId, (data) => {
            this.currentCustomerOrder = data;
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/sua/' + customerId) {
          if (this.currentCustomerOrder == null) {
            this.customerOrderService.getById(customerId, (data) => {
              this.currentCustomerOrder = data;
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach/tao') {
          this.currentCustomerOrder = new Model.CustomerOrderModel();
					//this.initEmployeeList();
        } else if (this.$location.path() === '/ql-toa-hang/don-hang-cua-khach') {
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
      }, null);
    }

		//initEmployeeList() {
  //    this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
  //      this.employeeList = results;
  //    }, null);
  //  }

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
      var confirmDialog = this.$window.confirm('Do you want to delete the Customer Order?');
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
      var confirmDialog = this.$window.confirm('Do you want to delete the Customer Order?');
      if (confirmDialog) {
        this.customerOrderService.deleteEntity(customerOrder, (data) => {
          this.$location.path('/ql-toa-hang/don-hang-cua-khach');
        }, null);
      }
    }

    createCustomerOrder(customerOrder: Model.CustomerOrderModel) {
      this.customerOrderService.create(customerOrder,
        (data) => {
          this.$location.path('/ql-toa-hang/don-hang-cua-khach');
        },
        () => { });
    }

		updateCustomerOrder(customerOrder: Model.CustomerOrderModel) {
      this.customerOrderService.update(customerOrder, (data) => {
        this.$location.path('/ql-toa-hang/don-hang-cua-khach');
      }, null);
    }

    goToCustomerOrderForm() {
      this.$location.path('/ql-toa-hang/don-hang-cua-khach/tao');
    }

	}
}
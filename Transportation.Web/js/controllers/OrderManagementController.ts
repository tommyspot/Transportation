/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class OrderManagementController {
    public currentOrder: Model.OrderModel;
    public orderService: service.OrderService;

    public orderList: Array<Model.OrderModel>;
    public productInputListTmp: Array<Model.OrderModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.orderService = new service.OrderService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.initOrder();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.productInputListTmp && self.productInputListTmp.length > 0) {
          self.orderList = $filter('filter')(self.productInputListTmp, value);
          self.initPagination();
        }
      });
    }

    initOrder() {
      var orderId = this.$routeParams.order_id;
      if (orderId) {
        if (this.$location.path() === '/ql-garage/ban-hang/' + orderId) {
          this.orderService.getById(orderId, (data) => {
            this.currentOrder = data;
          }, null);
        } else if (this.$location.path() === '/ql-garage/ban-hang/sua/' + orderId) {
          if (this.currentOrder == null) {
            this.orderService.getById(orderId, (data) => {
              this.currentOrder = data;
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-garage/ban-hang/tao') {
          this.currentOrder = new Model.OrderModel();
        } else if (this.$location.path() === '/ql-garage/ban-hang') {
          this.isLoading = true;
          this.initOrderList();
        }
      }
    }

    initOrderList() {
      this.orderService.getAll((results: Array<Model.OrderModel>) => {
        this.orderList = results;
        this.orderList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
        this.productInputListTmp = this.orderList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.orderList.length % this.pageSize === 0 ?
        this.orderList.length / this.pageSize : Math.floor(this.orderList.length / this.pageSize) + 1;
    }

    getOrderListOnPage() {
      if (this.orderList && this.orderList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.orderList.slice(startIndex, endIndex);
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

    selectAllProductsOnPage() {
      var employeeOnPage = this.getOrderListOnPage();
      for (let index = 0; index < employeeOnPage.length; index++) {
        var employee = employeeOnPage[index];
        employee.isChecked = this.isCheckedAll;
      }
    }

    removeOrders() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng?');
      if (confirmDialog) {
        for (let i = 0; i < this.orderList.length; i++) {
          var product = this.orderList[i];
          if (product.isChecked) {
            this.orderService.deleteEntity(product, (data) => {
              this.initOrderList();
            }, () => { });
          }
        }
      }
    }

    removeProductInDetail(product: Model.ProductModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng?');
      if (confirmDialog) {
        this.orderService.deleteEntity(product, (data) => {
          this.$location.path('/ql-garage/san-pham');
        }, null);
      }
    }

    createProduct(product: Model.ProductModel) {
      this.orderService.create(product,
        (data) => {
          this.$location.path('/ql-garage/don-hang');
        }, null);
    }

    updateProduct(product: Model.ProductModel) {
      this.orderService.update(product, (data) => {
        this.$location.path('/ql-garage/don-hang');
      }, null);
    }

    goToOrderForm() {
      this.$location.path('/ql-garage/don-hang/tao');
    }
	}
}
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
    public inventoryService: service.InventoryService;
    public productService: service.ProductService;
    public mainHelper: helper.MainHelper;

    public orderList: Array<Model.OrderModel>;
    public inventoryViewList: Array<Model.InventoryViewModel>;
    public productList: Array<Model.ProductModel>;

    public productInputListTmp: Array<Model.OrderModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public todayFormat: string;
    public currentDay: Date;
    public originOrderDetails: Array<Model.OrderDetailModel>;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $cookieStore: ng.ICookieStoreService,
      private $routeParams: any) {

      this.mainHelper = new helper.MainHelper($http, $cookieStore);
      this.orderService = new service.OrderService($http);
      this.inventoryService = new service.InventoryService($http);
      this.productService = new service.ProductService($http);
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
        if (this.$location.path() === '/ql-garage/ban-hang/' + orderId ||
          this.$location.path() === '/ql-garage/ban-hang/in/' + orderId) {
          this.currentDay = new Date();
          this.orderService.getById(orderId, (data) => {
            this.currentOrder = data;
            this.initProductList();
          }, null);
        } else if (this.$location.path() === '/ql-garage/ban-hang/sua/' + orderId) {
          if (this.currentOrder == null) {
            this.orderService.getById(orderId, (data) => {
              this.currentOrder = data;
              this.originOrderDetails = angular.copy(this.currentOrder.orderDetails);
              this.initFormatPriceForOrderDetails(this.currentOrder);
              this.initInventoryViewList();
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-garage/ban-hang/tao') {
          this.todayFormat = new Date().toLocaleString();
          this.currentOrder = new Model.OrderModel();
          this.currentOrder.saleOff = 0;
          this.currentOrder.totalAmount = 0;
          this.initInventoryViewList();
        } else if (this.$location.path() === '/ql-garage/ban-hang') {
          this.isLoading = true;
          this.initOrderList();
        } else if (this.$location.path() === '/ql-garage/ban-hang/da-xoa') {
          this.isLoading = true;
          this.initDeletedOrderList();
        }
      }
    }

    initOrderList() {
      this.orderService.getAll((results: Array<Model.OrderModel>) => {
        this.orderList = results;
        this.orderList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.productInputListTmp = this.orderList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initDeletedOrderList() {
      this.orderService.getAllDeletedOrders((results: Array<Model.OrderModel>) => {
        this.orderList = results;
        this.orderList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.productInputListTmp = this.orderList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initInventoryViewList() {
      this.productService.getAll((results: Array<Model.ProductModel>) => {
        this.productList = results;

        this.inventoryService.getAll((results: Array<Model.InventoryModel>) => {
          this.inventoryViewList = [];
          results.map(inventory => {
            var inventoryView = new Model.InventoryViewModel();
            inventoryView.id = inventory.id;
            inventoryView.productId = inventory.productId;
            inventoryView.productName = this.getProductById(inventory.productId).name;
            inventoryView.latestPrice = inventory.latestPrice;
            inventoryView.quantity = inventory.quantity;
            this.inventoryViewList.push(inventoryView);
          });
        }, null);

      }, null);
    }

    initProductList() {
      this.productService.getAll((results: Array<Model.ProductModel>) => {
        this.productList = results;
      }, null);
    }

    initFormatPriceForOrderDetails(order: Model.OrderModel) {
      if (order && order.orderDetails && order.orderDetails.length > 0) {
        for (var orderDetail of order.orderDetails) {
          orderDetail.priceFormatted = orderDetail.price != 0 ? orderDetail.price.toLocaleString() : '';
        }
      }
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
          var order = this.orderList[i];
          if (order.isChecked) {
            order.status = false;
            this.orderService.changeOrderStatus(order, (data) => {
              this.initOrderList();
            }, () => { });
          }
        }
      }
    }

    removeOrderInDetail(order: Model.OrderModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng?');
      if (confirmDialog) {
        order.status = false;
        this.orderService.changeOrderStatus(order, (data) => {
          this.$location.path('/ql-garage/ban-hang');
        }, null);
      }
    }

    restoreOrder(order: Model.OrderModel) {
      order.status = true;
      this.orderService.changeOrderStatus(order, (data) => {
        this.initDeletedOrderList();
      }, null);
    }

    createOrder(order: Model.OrderModel) {
      this.orderService.create(order,
        (data) => {
          this.$location.path('/ql-garage/ban-hang');
        }, null);
    }

    updateOrder(product: Model.OrderModel) {
      this.orderService.update(product, (data) => {
        this.$location.path('/ql-garage/ban-hang');
      }, null);
    }

    goToOrderForm() {
      this.$location.path('/ql-garage/ban-hang/tao');
    }

    goToDeletedOrders() {
      this.$location.path('/ql-garage/ban-hang/da-xoa');
    }

    goToOrderPrintPage(event: Event, order: Model.OrderModel) {
      event.stopPropagation();
      this.$window.open('#/ql-garage/ban-hang/in/' + order.id);
    }

    addOrderDetail() {
      var orderDetail = new Model.OrderDetailModel();
      this.currentOrder.orderDetails.push(orderDetail);
    }

    deleteOrderDetail(index: number) {
      this.currentOrder.orderDetails.splice(index, 1);
      this.updateTotalAmount();
      this.updateNote();
    }

    getProductById(id: number) {
      if (this.productList && this.productList.length > 0) {
        for (let product of this.productList) {
          if (product.id == id) {
            return product;
          }
        }
      }
      return null;
    }

    getInventoryViewByProductId(id: number) {
      if (this.inventoryViewList && this.inventoryViewList.length > 0) {
        for (let inventory of this.inventoryViewList) {
          if (inventory.productId == id) {
            return inventory;
          }
        }
      }
      return null;
    }

    onInventoryViewChanged(orderDetail: Model.OrderDetailModel, index: number) {
      const originOrderDetail = this.originOrderDetails && this.originOrderDetails[index];
      if (this.originOrderDetails == null || (originOrderDetail && orderDetail.productId != originOrderDetail.productId)) {
        orderDetail.price = this.getInventoryViewByProductId(orderDetail.productId).latestPrice;
        orderDetail.priceFormatted = orderDetail.price != 0 ? orderDetail.price.toLocaleString() : '';
        orderDetail.quantity = 1;
        this.updateTotalAmount();
        this.updateNote();
      }
    }

    onQuantityOrderDetailChanged() {
      this.updateTotalAmount();
      this.updateNote();
    }

    updateTotalAmount() {
      this.currentOrder.totalAmount = 0;
      if (this.currentOrder && this.currentOrder.orderDetails && this.currentOrder.orderDetails.length) {
        for (var orderDetail of this.currentOrder.orderDetails) {
          this.currentOrder.totalAmount += orderDetail.price * orderDetail.quantity;
        }
      }
    }

    updateNote() {
      this.currentOrder.note = '';
      if (this.currentOrder && this.currentOrder.orderDetails && this.currentOrder.orderDetails.length) {
        for (var orderDetail of this.currentOrder.orderDetails) {
          this.currentOrder.note += this.getProductById(orderDetail.productId).name + ':' + orderDetail.quantity +
            ':' + orderDetail.price + ', ';
        }
      }
    }

    setFormatedCurencyForOrderDetail(orderDetail: Model.OrderDetailModel) {
      if (orderDetail.priceFormatted && orderDetail.priceFormatted != '') {
        orderDetail.price = parseInt(orderDetail.priceFormatted.replace(/,/g, ''));
        orderDetail.priceFormatted = orderDetail.price.toLocaleString();
      }
      this.updateTotalAmount();
      this.updateNote();
    }
	}
}
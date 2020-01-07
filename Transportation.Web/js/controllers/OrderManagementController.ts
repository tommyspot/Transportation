/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
  import service = Clarity.Service;
  import helper = Clarity.Helper;
  const formatSuffix = 'Formatted';

  export class OrderManagementController {
    public exportService: service.ExportService;
    public orderService: service.OrderService;
    public inventoryService: service.InventoryService;
    public productService: service.ProductService;
    public mainHelper: helper.MainHelper;

    public currentOrder: Model.OrderModel;
    public orderList: Array<Model.OrderModel>;
    public orderListView: Array<Model.OrderViewModel>;

    public inventoryViewList: Array<Model.InventoryViewModel>;
    public productList: Array<Model.ProductModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public todayFormat: string;
    public currentDay: Date;
    public originOrderDetails: Array<Model.OrderDetailModel>;
    public searchText: string;
    public units: Array<String>;

    public fromDate: string;
    public toDate: string;
    public isExportLoading: boolean;
    public isSubmitting: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $cookieStore: ng.ICookieStoreService,
      private $routeParams: any) {

      this.exportService = new service.ExportService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      this.orderService = new service.OrderService($http);
      this.inventoryService = new service.InventoryService($http);
      this.productService = new service.ProductService($http);
      $scope.viewModel = this;

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.units = ['Công', 'Bán'];
      this.initOrder();

      $scope.$watch('viewModel.searchText', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        if (this.$location.path() === '/ql-garage/ban-hang') {
          this.currentPage === 0 ? this.fetchOrderListPerPage() : (() => { this.currentPage = 0; })();
          this.fetchNumOfPages();
        } else if (this.$location.path() === '/ql-garage/ban-hang/da-xoa') {
          this.currentPage === 0 ? this.fetchDeletedOrderListPerPage() : (() => { this.currentPage = 0; })();
          this.fetchDeletedOrderNumOfPages();
        }
      });

      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        if (this.$location.path() === '/ql-garage/ban-hang') {
          this.fetchOrderListPerPage();
        } else if (this.$location.path() === '/ql-garage/ban-hang/da-xoa') {
          this.fetchDeletedOrderListPerPage();
        }
      });

      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        if (this.$location.path() === '/ql-garage/ban-hang') {
          this.initOrderList();
        } else if (this.$location.path() === '/ql-garage/ban-hang/da-xoa') {
          this.initDeletedOrderList();
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
          this.initOrderList();
        } else if (this.$location.path() === '/ql-garage/ban-hang/da-xoa') {
          this.initDeletedOrderList();
        }
      }
    }

    initOrderList() {
      this.fetchOrderListPerPage();
      this.fetchNumOfPages();
    }

    initDeletedOrderList() {
      this.fetchDeletedOrderListPerPage();
      this.fetchDeletedOrderNumOfPages();
    }

    fetchOrderListPerPage() {
      this.isLoading = true;
      this.orderService.getPerPage(this.currentPage, this.pageSize, this.searchText, (results: Array<Model.OrderModel>) => {
        this.initOrderListAfterCallAsync(results);
      });
    }

    fetchDeletedOrderListPerPage() {
      this.isLoading = true;
      this.orderService.getDeletedOrdersPerPage(this.currentPage, this.pageSize, this.searchText, (results: Array<Model.OrderModel>) => {
        this.initOrderListAfterCallAsync(results);
      }, null);
    }

    fetchNumOfPages() {
      this.orderService.getNumOfPages(this.pageSize, this.searchText, (results: number) => {
        this.currentPage = 0;
        this.numOfPages = parseInt(results['pages']);
      });
    }

    fetchDeletedOrderNumOfPages() {
      this.orderService.getDeletedOrdersNumOfPages(this.pageSize, this.searchText, (results: number) => {
        this.currentPage = 0;
        this.numOfPages = parseInt(results['pages']);
      }, null);
    }

    initOrderListAfterCallAsync(results: Array<Model.OrderModel>) {
      this.orderList = results;
      this.mapToOrderListView();
      this.isLoading = false;
    }

    mapToOrderListView() {
      this.orderListView = this.orderList.map((order: Model.OrderModel) => {
        const orderView = new Model.OrderViewModel();
        orderView.id = order.id;
        orderView.licensePlate = order.licensePlate;
        orderView.customerName = order.customerName;
        orderView.totalAmount = this.mainHelper.formatCurrency(order.totalAmount);
        orderView.saleOff = order.saleOff.toString() + '%';
        orderView.date = order.date;
        orderView.status = order.status;
        return orderView;
      });
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
          // init max quantity for order detail
          this.initMaxQuantityForOrderDetails(this.currentOrder);
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
          orderDetail.priceFormatted = this.mainHelper.formatCurrency(orderDetail.price);
        }
      }
    }

    initMaxQuantityForOrderDetails(order: Model.OrderModel) {
      if (order && order.orderDetails && order.orderDetails.length > 0) {
        for (var orderDetail of order.orderDetails) {
          orderDetail.maxQuantity = this.getInventoryViewByProductId(orderDetail.productId).quantity;
        }
      }
    }

    selectAllProductsOnPage() {
      this.orderListView.map(order => order.isChecked = this.isCheckedAll);
    }

    removeOrders() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những đơn hàng được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.orderListView.length; i++) {
          var order = this.orderListView[i];
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
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa đơn hàng này?');
      if (confirmDialog) {
        order.status = false;
        this.orderService.changeOrderStatus(order, (data) => {
          this.$location.path('/ql-garage/ban-hang');
        }, null);
      }
    }

    restoreOrder(orderViewModel: Model.OrderViewModel) {
      orderViewModel.status = true;
      this.orderService.changeOrderStatus(orderViewModel, (data) => {
        this.initDeletedOrderList();
      }, null);
    }

    createOrder(order: Model.OrderModel) {
      this.isSubmitting = true;
      this.orderService.create(order, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-garage/ban-hang');
      }, null);
    }

    updateOrder(product: Model.OrderModel) {
      this.isSubmitting = true;
      this.orderService.update(product, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-garage/ban-hang');
      }, null);
    }

    goToOrderForm() {
      this.$location.path('/ql-garage/ban-hang/tao');
    }

    goToOrderEditForm(event: Event, orderId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-garage/ban-hang/sua/${orderId}`);
    }

    goToDeletedOrders() {
      this.$location.path('/ql-garage/ban-hang/da-xoa');
    }

    goToOrderPrintPage(event: Event, orderViewModel: Model.OrderViewModel) {
      event.stopPropagation();
      this.$window.open('#/ql-garage/ban-hang/in/' + orderViewModel.id);
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
      if (this.shouldOrderDetailSetDefault(orderDetail, index)) {
        orderDetail.price = this.getInventoryViewByProductId(orderDetail.productId).latestPrice;
        orderDetail.priceFormatted = this.mainHelper.formatCurrency(orderDetail.price);
        orderDetail.quantity = 1;
        orderDetail.maxQuantity = this.getInventoryViewByProductId(orderDetail.productId).quantity;
        this.updateTotalAmount();
        this.updateNote();
      }
    }

    shouldOrderDetailSetDefault(orderDetail: Model.OrderDetailModel, index: number) {
      if (this.originOrderDetails == null)  // Add new
        return true;

      const originOrderDetail = this.originOrderDetails && this.originOrderDetails[index];
      if (originOrderDetail == null)  // Edit screen: add new a detail order
        return true;

      return false;
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
          this.currentOrder.note += this.getProductById(orderDetail.productId).name + ':' + orderDetail.quantity + ', ';
        }
      }
    }

    setFormatedCurencyForOrderDetail(orderDetail: Model.OrderDetailModel) {
      this.mainHelper.onCurrencyPropertyChanged(orderDetail, 'price', `price${formatSuffix}`);
      this.updateTotalAmount();
      this.updateNote();
    }

    clearSearchText() {
      this.searchText = '';
    }

    exportOrder() {
      this.isExportLoading = true;
      let jsonObject = {
        type: Model.ExportType.GarageOrder,
        fromDate: this.fromDate,
        toDate: this.toDate
      }

      this.exportService.exportToExcel(jsonObject, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
      }, () => {
        this.isExportLoading = false;
      });
    }

    hasSelectedOrder() {
      if (!this.orderListView) return false;
      return this.orderListView.some(order => order.isChecked);
    }

  }
}
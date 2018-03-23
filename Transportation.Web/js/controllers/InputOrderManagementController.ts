/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
  import helper = Clarity.Helper;

  const formatSuffix = 'Formatted';

  export class InputOrderManagementController {
    public currentInputOrder: Model.InputOrderModel;
    public inputOrderService: service.InputOrderService;
    public productService: service.ProductService;
    public mainHelper: helper.MainHelper;

    public inputOrderList: Array<Model.InputOrderModel>;
    public inputOrderListView: Array<Model.InputOrderViewModel>;
    public inputOrderListViewFilter: Array<Model.InputOrderViewModel>;

    public productList: Array<Model.ProductModel>;
    public productNameList: Array<string>;
    
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public todayFormat: string;
    public searchText: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $cookieStore: ng.ICookieStoreService,
      private $routeParams: any) {

      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      this.inputOrderService = new service.InputOrderService($http);
      this.productService = new service.ProductService($http);
      $scope.viewModel = this;

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.initInputOrder();

      $scope.$watch('viewModel.searchText', value => {
        if (this.inputOrderListViewFilter && this.inputOrderListViewFilter.length > 0) {
          this.inputOrderListView = $filter('filter')(this.inputOrderListViewFilter, value);
        }
      });

      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.fetchInputOrderListPerPage();
      });

      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.initInputOrderList();
      });
    }

    initInputOrder() {
      var inputOrderId = this.$routeParams.input_order_id;
      if (inputOrderId) {
        this.initCurrentInputOrder(inputOrderId);
      } else {
        if (this.$location.path() === '/ql-garage/nhap-kho/tao') {
          this.initProductList();
          this.todayFormat = new Date().toLocaleString();
          this.currentInputOrder = new Model.InputOrderModel();
          this.currentInputOrder.totalAmount = 0;
        } else if (this.$location.path() === '/ql-garage/nhap-kho') {
          this.isLoading = true;
          this.initInputOrderList();
        }
      }
    }

    initCurrentInputOrder(inputOrderId: number) {
      if (this.currentInputOrder == null) {
        this.inputOrderService.getById(inputOrderId, (data) => {
          this.currentInputOrder = data;
          this.initFormatPriceForProductInputs(this.currentInputOrder);
          this.initProductList();
        }, null);
      }
    }

    initInputOrderList() {
      this.fetchInputOrderListPerPage();
      this.fetchNumOfPages();
    }

    fetchInputOrderListPerPage() {
      this.isLoading = true;
      this.inputOrderService.getPerPage(this.currentPage, this.pageSize, (results: Array<Model.InputOrderModel>) => {
        this.inputOrderList = results;
        this.mapToInputOrderListView();
        this.inputOrderListViewFilter = this.inputOrderListView;
        this.isLoading = false;
      }, null);
    }

    fetchNumOfPages() {
      this.inputOrderService.getNumOfPages(this.pageSize, (results: number) => {
        this.currentPage = 0;
        this.numOfPages = parseInt(results['pages']);
      }, null);
    }

    mapToInputOrderListView() {
      this.inputOrderListView = this.inputOrderList.map((inputOrder: Model.InputOrderModel) => {
        const inputOrderView = new Model.InputOrderViewModel();
        inputOrderView.id = inputOrder.id;
        inputOrderView.vendor = inputOrder.vendor;
        inputOrderView.numOfProducts = inputOrder.productInputs ? inputOrder.productInputs.length : 0;
        inputOrderView.totalAmount = this.mainHelper.formatCurrency(inputOrder.totalAmount);
        inputOrderView.date = inputOrder.date;
        return inputOrderView;
      });
    }

    initProductList() {
      this.productService.getAll((results: Array<Model.ProductModel>) => {
        this.productList = results;
        this.productNameList = this.productList.map(product => {
          return product.name;
        });
      }, null);
    }

    initFormatPriceForProductInputs(inputOrder: Model.InputOrderModel) {
      if (inputOrder && inputOrder.productInputs && inputOrder.productInputs.length > 0) {
        for (var productInput of inputOrder.productInputs) {
          this.mainHelper.initCurrencyFormattedProperty(productInput,
            ['inputPrice', 'salePrice'], formatSuffix);
        }
      }
    }

    selectAllInputOrdersOnPage() {
      this.inputOrderListView.map(inputOrder => inputOrder.isChecked = this.isCheckedAll);
    }

    removeOrders() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những kho hàng được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.inputOrderListView.length; i++) {
          var product = this.inputOrderListView[i];
          if (product.isChecked) {
            this.inputOrderService.deleteEntity(product, (data) => {
              this.initInputOrderList();
            }, () => { });
          }
        }
      }
    }

    removeInputOrderInDetail(inputOrder: Model.InputOrderModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa kho hàng này?');
      if (confirmDialog) {
        this.inputOrderService.deleteEntity(inputOrder, (data) => {
          this.$location.path('/ql-garage/nhap-kho');
        }, null);
      }
    }

    createInputOrder(inputOrder: Model.InputOrderModel) {
      this.isSubmitting = true;
      this.inputOrderService.create(inputOrder,
        (data) => {
          this.isSubmitting = false;
          this.$location.path('/ql-garage/nhap-kho');
        }, null);
    }

    updateInputOrder(inputOrder: Model.InputOrderModel) {
      this.isSubmitting = true;
      this.inputOrderService.update(inputOrder, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-garage/nhap-kho');
      }, null);
    }

    goToInputOrderForm() {
      this.$location.path('/ql-garage/nhap-kho/tao');
    }

    goToInputOrderEditForm(event: Event, inputOrderId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-garage/nhap-kho/sua/${inputOrderId}`);
    }

    addProductInput() {
      var productInput = new Model.ProductInputModel();
      this.currentInputOrder.productInputs.push(productInput);
    }

    deleteProductInput(index: number) {
      this.currentInputOrder.productInputs.splice(index, 1);
      this.updateTotalAmount();
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

    updateTotalAmount() {
      this.currentInputOrder.totalAmount = 0;
      if (this.currentInputOrder && this.currentInputOrder.productInputs && this.currentInputOrder.productInputs.length) {
        for (var productInput of this.currentInputOrder.productInputs) {
          this.currentInputOrder.totalAmount += productInput.inputPrice * productInput.quantity;
        }
      }
    }

    formatCurrency(productInput: Model.ProductInputModel, propertyName: string) {
      this.mainHelper.onCurrencyPropertyChanged(productInput, propertyName, `${propertyName}${formatSuffix}`);
      if (propertyName === 'inputPrice') {
        this.updateTotalAmount();
      }
    }

    clearSearchText() {
      this.searchText = '';
    }

    hasSelectedInputOrder() {
      if (!this.inputOrderListView) return false;
      return this.inputOrderListView.some(wagon => wagon.isChecked);
    }

	}
}
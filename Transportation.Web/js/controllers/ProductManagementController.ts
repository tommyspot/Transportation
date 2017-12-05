/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class ProductManagementController {
    public currentProduct: Model.ProductModel;
    public productService: service.ProductService;

    public productList: Array<Model.ProductModel>;
    public productListTmp: Array<Model.ProductModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public errorMessage: string;
    public searchText: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $timeout: ng.ITimeoutService,
      private $routeParams: any) {

      this.productService = new service.ProductService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.errorMessage = '';
      this.searchText = '';
      this.initProduct();

      var self = this;
      $scope.$watch('viewModel.searchText', function (value) {
        if (self.productListTmp && self.productListTmp.length > 0) {
          self.productList = $filter('filter')(self.productListTmp, value);
          self.initPagination();
        }
      });
    }

    initProduct() {
      var productId = this.$routeParams.product_id;
      if (productId) {
        this.initCurrentProduct(productId);
      } else {
        if (this.$location.path() === '/ql-garage/san-pham/tao') {
          this.currentProduct = new Model.ProductModel();
        } else if (this.$location.path() === '/ql-garage/san-pham') {
          this.isLoading = true;
          this.initProductList();
        }
      }
    }

    initProductList() {
      this.productService.getAll((results: Array<Model.ProductModel>) => {
        this.productList = results;
        this.productList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.productListTmp = this.productList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initCurrentProduct(productId: number) {
      if (this.currentProduct == null) {
        this.productService.getById(productId, (data) => {
          this.currentProduct = data;
        }, null);
      }
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.productList.length % this.pageSize === 0 ?
        this.productList.length / this.pageSize : Math.floor(this.productList.length / this.pageSize) + 1;
    }

    getProductListOnPage() {
      if (this.productList && this.productList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.productList.slice(startIndex, endIndex);
      }
    }

    selectAllProductsOnPage() {
      var employeeOnPage = this.getProductListOnPage();
      for (let index = 0; index < employeeOnPage.length; index++) {
        var employee = employeeOnPage[index];
        employee.isChecked = this.isCheckedAll;
      }
    }

    removeProducts() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những sản phẩm được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.productList.length; i++) {
          var product = this.productList[i];
          if (product.isChecked) {
            this.productService.deleteEntity(product, (data) => {
              this.initProductList();
            }, () => { });
          }
        }
      }
    }

    removeProductInDetail(product: Model.ProductModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa sản phẩm này?');
      if (confirmDialog) {
        this.productService.deleteEntity(product, (data) => {
          this.$location.path('/ql-garage/san-pham');
        }, null);
      }
    }

    createProduct(product: Model.ProductModel) {
      this.isSubmitting = true;
      this.productService.create(product,
        (data) => {
          this.isSubmitting = false;
          this.$location.path('/ql-garage/san-pham');
        }, (error) => {
          this.isSubmitting = false;
          this.errorMessage = error.message;
          this.$timeout(() => {
            this.errorMessage = '';
          }, 8000);
        });
    }

    updateProduct(product: Model.ProductModel) {
      this.isSubmitting = true;
      this.productService.update(product, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-garage/san-pham');
      }, null);
    }

    goToProductForm() {
      this.$location.path('/ql-garage/san-pham/tao');
    }

    goToProductEditForm(event: Event, productId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-garage/san-pham/sua/${productId}`);
    }

    clearSearchText() {
      this.searchText = '';
    }
	}
}
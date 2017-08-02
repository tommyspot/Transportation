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

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.productService = new service.ProductService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.errorMessage = '';
      this.initProduct();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.productListTmp && self.productListTmp.length > 0) {
          self.productList = $filter('filter')(self.productListTmp, value);
          self.initPagination();
        }
      });
    }

    initProduct() {
      var productId = this.$routeParams.product_id;
      if (productId) {
        if (this.$location.path() === '/ql-garage/san-pham/' + productId) {
          this.productService.getById(productId, (data) => {
            this.currentProduct = data;
          }, null);
        } else if (this.$location.path() === '/ql-garage/san-pham/sua/' + productId) {
          if (this.currentProduct == null) {
            this.productService.getById(productId, (data) => {
              this.currentProduct = data;
            }, null);
          }
        }
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
      this.productService.create(product,
        (data) => {
          this.$location.path('/ql-garage/san-pham');
        }, (error) => {
          this.errorMessage = error.message;
        });
    }

    updateProduct(product: Model.ProductModel) {
      this.productService.update(product, (data) => {
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
	}
}
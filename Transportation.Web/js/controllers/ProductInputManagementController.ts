/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
  import service = Clarity.Service;
  import helper = Clarity.Helper;

  export class ProductInputManagementController {
    public currentProductInput: Model.ProductInputModel;
    public productInputService: service.ProductInputService;
    public productService: service.ProductService;
    public mainHelper: helper.MainHelper;

    public productInputList: Array<Model.ProductInputModel>;
    public productInputListForm: Array<Model.ProductInputModel>;
    public productInputListTmp: Array<Model.ProductInputModel>;
    public productList: Array<Model.ProductModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public todayFormat: string;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $cookieStore: ng.ICookieStoreService,
      private $routeParams: any) {

      this.mainHelper = new helper.MainHelper($http, $cookieStore);
      this.productInputService = new service.ProductInputService($http);
      this.productService = new service.ProductService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.initProductInput();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.productInputListTmp && self.productInputListTmp.length > 0) {
          self.productInputList = $filter('filter')(self.productInputListTmp, value);
          self.initPagination();
        }
      });
    }

    initProductInput() {
      this.initProductList();

      var productInputId = this.$routeParams.product_input_id;
      if (productInputId) {
        if (this.$location.path() === '/ql-garage/nhap-kho/' + productInputId) {
          this.productInputService.getById(productInputId, (data) => {
            this.currentProductInput = data;
          }, null);
        } else if (this.$location.path() === '/ql-garage/nhap-kho/sua/' + productInputId) {
          if (this.currentProductInput == null) {
            this.productInputService.getById(productInputId, (data) => {
              this.currentProductInput = data;
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-garage/nhap-kho/tao') {
          this.productInputListForm = [];
          this.todayFormat = this.mainHelper.formatDateTimeDDMMYYYY(new Date());
        } else if (this.$location.path() === '/ql-garage/nhap-kho') {
          this.isLoading = true;
          this.initProductInputList();
        }
      }
    }

    initProductInputList() {
      this.productInputService.getAll((results: Array<Model.ProductInputModel>) => {
        this.productInputList = results;
        this.productInputList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
        this.productInputListTmp = this.productInputList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initProductList() {
      this.productService.getAll((results: Array<Model.ProductModel>) => {
        this.productList = results;
        this.productList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.productInputList.length % this.pageSize === 0 ?
        this.productInputList.length / this.pageSize : Math.floor(this.productInputList.length / this.pageSize) + 1;
    }

    getProductInputListOnPage() {
      if (this.productInputList && this.productInputList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.productInputList.slice(startIndex, endIndex);
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
      var employeeOnPage = this.getProductInputListOnPage();
      for (let index = 0; index < employeeOnPage.length; index++) {
        var employee = employeeOnPage[index];
        employee.isChecked = this.isCheckedAll;
      }
    }

    removeProductInputs() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa kho hàng?');
      if (confirmDialog) {
        for (let i = 0; i < this.productInputList.length; i++) {
          var product = this.productInputList[i];
          if (product.isChecked) {
            this.productInputService.deleteEntity(product, (data) => {
              this.initProductInputList();
            }, () => { });
          }
        }
      }
    }

    removeProductInputInDetail(productInput: Model.ProductInputModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa kho hàng?');
      if (confirmDialog) {
        this.productInputService.deleteEntity(productInput, (data) => {
          this.$location.path('/ql-garage/nhap-kho');
        }, null);
      }
    }

    updateProductInput(productInput: Model.ProductInputModel) {
      this.productInputService.update(productInput, (data) => {
        this.$location.path('/ql-garage/nhap-kho');
      }, null);
    }

    goToProductInputForm() {
      this.$location.path('/ql-garage/nhap-kho/tao');
    }

    addProductInput() {
      let productInput = new Model.ProductInputModel();
      this.productInputListForm.push(productInput);
    }

    removeProductInput(index) {
      if (this.productInputListForm && this.productInputListForm.length > 0) {
        this.productInputListForm.splice(index, 1);
      }
    }

    createProductInputList(productInputs: Array<Model.ProductInputModel>) {
      this.productInputService.createList(productInputs, (data) => {
        this.$location.path('/ql-garage/nhap-kho');
      }, null);
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
	}
}
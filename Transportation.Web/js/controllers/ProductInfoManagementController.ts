/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class ProductInfoManagementController {
    public currentProductInfo: Model.ProductInfoModel;
    public productService: service.ProductService;
    public exportService: service.ExportService;
    public productInfoList: Array<Model.ProductInfoModel>;
    public productInfoListTmp: Array<Model.ProductInfoModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isLoading: boolean;
    public isExportLoading: boolean;

    public sortingCurrentPropertyName: string;
    public sortingIsReverse: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.productService = new service.ProductService($http);
      this.exportService = new service.ExportService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.initProduct();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.productInfoListTmp && self.productInfoListTmp.length > 0) {
          self.productInfoList = $filter('filter')(self.productInfoListTmp, value);
          self.initPagination();
        }
      });
    }

    initProduct() {
      if (this.$location.path() === '/ql-garage/quan-ly') {
        this.isLoading = true;
        this.initProductInfoList();
      }
    }

    initProductInfoList() {
      this.productService.getAllProductInfo((results: Array<Model.ProductInfoModel>) => {
        this.productInfoList = results;
        this.orderBy('name');
        this.productInfoListTmp = this.productInfoList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.productInfoList.length % this.pageSize === 0 ?
        this.productInfoList.length / this.pageSize : Math.floor(this.productInfoList.length / this.pageSize) + 1;
    }

    getProductInfoListOnPage() {
      if (this.productInfoList && this.productInfoList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.productInfoList.slice(startIndex, endIndex);
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

    orderBy(propertyName: string) {
      this.sortingIsReverse = propertyName && propertyName === this.sortingCurrentPropertyName ? !this.sortingIsReverse : false;
      this.sortingCurrentPropertyName = propertyName;
      this.productInfoList = this.$filter('orderBy')(this.productInfoList, this.sortingCurrentPropertyName, this.sortingIsReverse);
    }

    exportReport() {
      this.isExportLoading = true;
      this.exportService.exportGarageToExcel(this.productInfoList, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
        //this.exportService.deleteExcelFile(data['fileName'].split('/')[0], () => { }, null);
      }, () => {
        this.isExportLoading = false;
      });
    }
	}
}
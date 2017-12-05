/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class ProductInfoManagementController {
    public mainHelper: helper.MainHelper;
    public productService: service.ProductService;
    public exportService: service.ExportService;

    public currentProductInfo: Model.ProductInfoModel;
    public productInfoList: Array<Model.ProductInfoModel>;
    public productInfoListView: Array<Model.ProductInfoViewModel>;
    public productInfoListViewTmp: Array<Model.ProductInfoViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isLoading: boolean;
    public isExportLoading: boolean;
    public searchText: string;

    public sortingCurrentPropertyName: string;
    public sortingIsReverse: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $cookieStore: ng.ICookieStoreService,
      private $routeParams: any) {

      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      this.productService = new service.ProductService($http);
      this.exportService = new service.ExportService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.searchText = '';
      this.initProduct();

      var self = this;
      $scope.$watch('viewModel.searchText', function (value) {
        if (self.productInfoListViewTmp && self.productInfoListViewTmp.length > 0) {
          self.productInfoListView = $filter('filter')(self.productInfoListViewTmp, value);
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
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    mapToProductInfoListView() {
      this.productInfoListView = this.productInfoList.map((productInfo: Model.ProductInfoModel) => {
        const productInfoView = new Model.ProductInfoViewModel();
        productInfoView.id = productInfo.id;
        productInfoView.name = productInfo.name;
        productInfoView.sumOfInput = this.mainHelper.formatCurrency(productInfo.sumOfInput);
        productInfoView.sumOfSale = this.mainHelper.formatCurrency(productInfo.sumOfSale);
        productInfoView.numOfRemain = this.mainHelper.formatCurrency(productInfo.numOfRemain);
        productInfoView.sumOfInputTotalAmount = this.mainHelper.formatCurrency(productInfo.sumOfInputTotalAmount);
        productInfoView.sumOfSaleTotalAmount = this.mainHelper.formatCurrency(productInfo.sumOfSaleTotalAmount);
        productInfoView.profit = this.mainHelper.formatCurrency(productInfo.profit);
        return productInfoView;
      });
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.productInfoListView.length % this.pageSize === 0 ?
        this.productInfoListView.length / this.pageSize : Math.floor(this.productInfoListView.length / this.pageSize) + 1;
    }

    getProductInfoListOnPage() {
      if (this.productInfoListView && this.productInfoListView.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.productInfoListView.slice(startIndex, endIndex);
      }
    }

    orderBy(propertyName: string) {
      this.sortingIsReverse = propertyName && propertyName === this.sortingCurrentPropertyName ? !this.sortingIsReverse : false;
      this.sortingCurrentPropertyName = propertyName;
      this.productInfoList = this.$filter('orderBy')(this.productInfoList, this.sortingCurrentPropertyName, this.sortingIsReverse);

      this.clearSearchText();
      this.mapToProductInfoListView();
      this.productInfoListViewTmp = this.productInfoListView;
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

    clearSearchText() {
      if (this.searchText != '') {
        this.searchText = '';
      }
    }

	}
}
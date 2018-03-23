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
    public productInfoListViewFilter: Array<Model.ProductInfoViewModel>;

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

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.initProduct();

      $scope.$watch('viewModel.searchText', value => {
        if (this.productInfoListViewFilter && this.productInfoListViewFilter.length > 0) {
          this.productInfoListView = $filter('filter')(this.productInfoListViewFilter, value);
        }
      });

      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.fetchProductInfoListPerPage();
      });

      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.initProductInfoList();
      });
    }

    initProduct() {
      if (this.$location.path() === '/ql-garage/quan-ly') {
        this.isLoading = true;
        this.initProductInfoList();
      }
    }

    initProductInfoList() {
      this.fetchProductInfoListPerPage();
      this.fetchNumOfPages();
    }

    fetchProductInfoListPerPage() {
      this.isLoading = true;
      this.productService.getProductInfoPerPage(this.currentPage, this.pageSize, (results: Array<Model.ProductInfoModel>) => {
        this.productInfoList = results;
        this.orderBy('name', true);
        this.isLoading = false;
      }, null);
    }

    fetchNumOfPages() {
      this.productService.getNumOfPages(this.pageSize, (results: number) => {
        this.currentPage = 0;
        this.numOfPages = parseInt(results['pages']);
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

    orderBy(propertyName: string, isNoReverse?: boolean) {
      this.sortingIsReverse = isNoReverse
        ? false
        : propertyName && propertyName === this.sortingCurrentPropertyName ? !this.sortingIsReverse : false;
      this.sortingCurrentPropertyName = propertyName;
      this.productInfoList = this.$filter('orderBy')(this.productInfoList, this.sortingCurrentPropertyName, this.sortingIsReverse);

      this.clearSearchText();
      this.mapToProductInfoListView();
      this.productInfoListViewFilter = this.productInfoListView;
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
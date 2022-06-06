/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
  import service = Clarity.Service;
  import helper = Clarity.Helper;

  export class TransportationManagementController {
    public exportService: service.ExportService;
    public orderService: service.OrderService;
    public truckService: service.TruckService;
    public mainHelper: helper.MainHelper;

    public truckList: Array<Model.TruckModel>
    public currentOrder: Model.OrderModel;
    public orderList: Array<Model.OrderModel>;
    public orderListView: Array<Model.OrderViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public searchText: string;

    public truckLicensePlate: string;
    public fromDate: string;
    public toDate: string;
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
      this.truckService = new service.TruckService($http);
      $scope.viewModel = this;

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.initTransportation();

      $scope.$watch('viewModel.searchText', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.currentPage === 0 ? this.fetchOrderListPerPage() : (() => { this.currentPage = 0; })();
        this.fetchNumOfPages();
      });

      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.fetchOrderListPerPage();
      });

      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.initOrderList();
      });
    }

    initTransportation() {
      this.initTruckList();
    }
    
    initOrderList() {
      this.fetchOrderListPerPage();
      this.fetchNumOfPages();
    }

    initTruckList() {
      this.truckService.getAllCurtail((results: Array<Model.TruckModel>) => {
        this.truckList = results;
      }, null);
    }

    fetchOrderListPerPage() {
      if (!this.truckLicensePlate || !this.fromDate || !this.toDate)
        return;
      this.isLoading = true;
      this.orderService.getTruckInfoPerPage(
        this.currentPage,
        this.pageSize,
        this.searchText,
        this.truckLicensePlate,
        this.fromDate,
        this.toDate,
        (results: Array<Model.OrderModel>) => {
        this.initOrderListAfterCallAsync(results);
      });
    }

    fetchNumOfPages() {
      if (!this.truckLicensePlate || !this.fromDate || !this.toDate)
        return;
      this.orderService.getTruckInfoNumOfPages(
        this.pageSize,
        this.searchText,
        this.truckLicensePlate,
        this.fromDate,
        this.toDate,
        (results: number) => {
        this.currentPage = 0;
        this.numOfPages = parseInt(results['pages']);
      });
    }

    initOrderListAfterCallAsync(results: Array<Model.OrderModel>) {
      this.orderList = results;
      this.mapToOrderListView();
      this.isLoading = false;
      this.isSubmitting = false;
    }

    mapToOrderListView() {
      this.orderListView = this.orderList.map((order: Model.OrderModel) => {
        const orderView = new Model.OrderViewModel();
        orderView.id = order.id;
        orderView.customerName = order.customerName;
        orderView.date = order.date;
        orderView.notes = (order.note || '').split(',').slice(0, -1);
        return orderView;
      });
    }

    selectAllProductsOnPage() {
      this.orderListView.map(order => order.isChecked = this.isCheckedAll);
    }

    clearSearchText() {
      this.searchText = '';
    }

    fetchOrderTruckInfo() {
      this.isSubmitting = true;
      this.initOrderList();
    };
  }
}

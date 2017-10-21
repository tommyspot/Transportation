/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
  import helper = Clarity.Helper;

  const formatSuffix = 'Formatted';

  export class WagonSettlementManagementController {
    public mainHelper: helper.MainHelper;
    public truckService: service.TruckService;
    public customerService: service.CustomerService;
    public wagonService: service.WagonService;
    public paymentService: service.PaymentService;
    public exportService: service.ExportService;
    public wagonSettlementService: service.WagonSettlementService;

    public currentWagonSettlement: Model.WagonSettlementModel;
    public wagonSettlementList: Array<Model.WagonSettlementModel>;
    public wagonSettlementListView: Array<Model.WagonSettlementViewModel>;
    public wagonSettlementListViewTmp: Array<Model.WagonSettlementViewModel>;
    
    public truckList: Array<Model.TruckModel>
    public customerList: Array<Model.CustomerModel>
    public paymentList: Array<Model.PaymentModel>
    public wagonList: Array<Model.WagonModel>

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
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
      private $routeParams: any, private $cookieStore: ng.ICookieStoreService) {

      this.wagonSettlementService = new service.WagonSettlementService($http);
      this.truckService = new service.TruckService($http);
      this.customerService = new service.CustomerService($http);
      this.wagonService = new service.WagonService($http);
      this.exportService = new service.ExportService($http);
      this.paymentService = new service.PaymentService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.searchText = '';
      this.initWagonSettlement();

      var self = this;
      $scope.$watch('viewModel.searchText', function (value) {
        if (self.wagonSettlementListViewTmp && self.wagonSettlementListViewTmp.length > 0) {
          self.wagonSettlementListView = $filter('filter')(self.wagonSettlementListViewTmp, value);
          self.initPagination();
        }
      });
    }

    initWagonSettlement() {
      var wagonSettlementId = this.$routeParams.wagonSettlement_id;
      this.initCustomerList();
      this.initWagonList();

      if (wagonSettlementId) {
        if (this.$location.path() === '/ql-toa-hang/quyet-toan/' + wagonSettlementId) {
          this.$rootScope.showSpinner();
          this.wagonSettlementService.getById(wagonSettlementId, (data) => {
            this.currentWagonSettlement = data;
            this.initPaymentList(this.currentWagonSettlement.code);
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/quyet-toan/sua/' + wagonSettlementId) {
          if (this.currentWagonSettlement == null) {
            this.$rootScope.showSpinner();
            this.wagonSettlementService.getById(wagonSettlementId, (data) => {
              this.currentWagonSettlement = data;
              this.$rootScope.hideSpinner();
              // init data
              this.todayFormat = new Date().toLocaleString();
              this.mainHelper.initCurrencyFormattedProperty(this.currentWagonSettlement, ['unitPrice'], formatSuffix);
              this.currentWagonSettlement.paymentDate = '';
              this.currentWagonSettlement.paymentFormatted = '';
            }, null);
          }
				}
			} else {
        if (this.$location.path() === '/ql-toa-hang/quyet-toan') {
          this.initWagonSettlementList();
        } 
      }
    }

    initWagonSettlementList() {
      this.isLoading = true;
      this.wagonSettlementService.getAll((results: Array<Model.WagonSettlementModel>) => {
        this.wagonSettlementList = results;
        this.wagonSettlementList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.refreshWagonSettlementListView();
        this.isLoading = false;
      }, null);
    }

    mapToWagonSettlementListView() {
      this.wagonSettlementListView = this.wagonSettlementList.map((wagonSettlement: Model.WagonSettlementModel) => {
        const wagonSettlementView = new Model.WagonSettlementViewModel();
        wagonSettlementView.id = wagonSettlement.id;
        wagonSettlementView.wagonCode = this.getWagonCode(wagonSettlement.wagonId);
        wagonSettlementView.departDate = this.getDepartDate(wagonSettlement.wagonId);
        wagonSettlementView.customerName = this.getCustomerName(wagonSettlement.customerId);
        wagonSettlementView.totalAmount = this.mainHelper.formatCurrency(wagonSettlement.quantity * wagonSettlement.unitPrice + wagonSettlement.phiPhatSinh);
        wagonSettlementView.phiPhatSinh = this.mainHelper.formatCurrency(wagonSettlement.phiPhatSinh);
        wagonSettlementView.payment = this.mainHelper.formatCurrency(wagonSettlement.payment);
        wagonSettlementView.paymentRemain = this.mainHelper.formatCurrency(wagonSettlement.paymentRemain);
        wagonSettlementView.paymentStatus = wagonSettlement.paymentStatus;
        return wagonSettlementView;
      });
    }

    refreshWagonSettlementListView() {
      if (this.$location.path() === '/ql-toa-hang/quyet-toan') {
        if (this.wagonSettlementList) {
          this.mapToWagonSettlementListView();
          this.wagonSettlementListViewTmp = this.wagonSettlementListView;
          this.initPagination();
        }
      }
    }

		initCustomerList() {
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
        this.refreshWagonSettlementListView();
      }, null);
    }

    initWagonList() {
      this.wagonService.getAll((results: Array<Model.WagonModel>) => {
        this.wagonList = results;
        this.refreshWagonSettlementListView();
      }, null);
    }

		initPaymentList(code: string) {
			this.paymentService.getByWagonSettlementCode(code, (data) => {
        this.paymentList = data;
        this.$rootScope.hideSpinner();
			}, null);
		}

    updateWagonSettlement(customerOrder: Model.WagonSettlementModel) {
      this.isSubmitting = true;
      this.wagonSettlementService.update(customerOrder, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-toa-hang/quyet-toan');
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.wagonSettlementListView.length % this.pageSize === 0 ?
        this.wagonSettlementListView.length / this.pageSize : Math.floor(this.wagonSettlementListView.length / this.pageSize) + 1;
    }

    getWagonSettlementListOnPage() {
      if (this.wagonSettlementListView && this.wagonSettlementListView.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.wagonSettlementListView.slice(startIndex, endIndex);
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

		updateNewPayment() {
      this.mainHelper.onCurrencyPropertyChanged(this.currentWagonSettlement, 'newPayment', `newPayment${formatSuffix}`);
    }

    getCustomerName(customerId: number): string {
      if (customerId) {
        return this.mainHelper.getPropertyValue(this.customerList, 'id', customerId.toString(), 'fullName');
      }
    }

    getWagonCode(wagonId: number): string {
      if (wagonId) {
        return this.mainHelper.getPropertyValue(this.wagonList, 'id', wagonId.toString(), 'code');
      }
    }

    getDepartDate(wagonId: number): string {
      if (wagonId) {
        return this.mainHelper.getPropertyValue(this.wagonList, 'id', wagonId.toString(), 'departDate');
      }
    }

    goToWagonSettlementEditForm(event: Event, wagonSettlementId: number) {
      event.stopPropagation();
      this.$location.path('/ql-toa-hang/quyet-toan/sua/' + wagonSettlementId);
    }

    clearSearchText() {
      this.searchText = '';
    }

	}
}
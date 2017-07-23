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
    public truckList: Array<Model.TruckModel>
    public customerList: Array<Model.CustomerModel>
    public paymentList: Array<Model.PaymentModel>
    public wagonList: Array<Model.WagonModel>

    public wagonSettlementList: Array<Model.WagonSettlementModel>;
    public wagonSettlementListTmp: Array<Model.WagonSettlementModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isLoading: boolean;
    public todayFormat: string;

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
			this.mainHelper = new helper.MainHelper($http, $cookieStore);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.initWagonSettlement();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.wagonSettlementListTmp && self.wagonSettlementListTmp.length > 0) {
          self.wagonSettlementList = $filter('filter')(self.wagonSettlementListTmp, value);
          self.initPagination();
        }
      });
    }

    initWagonSettlement() {
      var wagonId = this.$routeParams.wagonSettlement_id;
      this.initCustomerList();
      this.initWagonList();
      if (wagonId) {
        if (this.$location.path() === '/ql-toa-hang/quyet-toan/' + wagonId) {
          this.wagonSettlementService.getById(wagonId, (data) => {
            this.currentWagonSettlement = data;
            this.initPaymentList(this.currentWagonSettlement.code);
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/quyet-toan/sua/' + wagonId) {
					if (this.currentWagonSettlement == null) {
            this.wagonSettlementService.getById(wagonId, (data) => {
              this.currentWagonSettlement = data;
              // init data
              this.todayFormat = new Date().toLocaleString();
              this.mainHelper.initFormattedProperty(this.currentWagonSettlement, ['unitPrice'], formatSuffix);
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
        this.wagonSettlementListTmp = this.wagonSettlementList;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

		initCustomerList() {
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
      }, null);
    }

    initWagonList() {
      this.wagonService.getAll((results: Array<Model.WagonModel>) => {
        this.wagonList = results;
      }, null);
    }

		initPaymentList(code: string) {
			this.paymentService.getByWagonSettlementCode(code, (data) => {
				this.paymentList = data;
			}, null);
		}

		updateWagonSettlement(customerOrder: Model.WagonSettlementModel) {
      this.wagonSettlementService.update(customerOrder, (data) => {
        this.$location.path('/ql-toa-hang/quyet-toan');
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.wagonSettlementList.length % this.pageSize === 0 ?
        this.wagonSettlementList.length / this.pageSize : Math.floor(this.wagonSettlementList.length / this.pageSize) + 1;
    }

    getWagonSettlementListOnPage() {
      if (this.wagonSettlementList && this.wagonSettlementList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.wagonSettlementList.slice(startIndex, endIndex);
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
      this.mainHelper.formatCurrency(this.currentWagonSettlement, 'newPayment', `newPayment${formatSuffix}`);
    }

    getCustomerName(customerId: string): string {
      return this.mainHelper.getPropertyValue(this.customerList, 'id', customerId, 'fullName');
    }

    getWagonCode(wagonId: string): string {
      return this.mainHelper.getPropertyValue(this.wagonList, 'id', wagonId, 'code');
    }

    goToWagonSettlementEditForm(event: Event, wagonSettlementId: number) {
      event.stopPropagation();
      this.$location.path('/ql-toa-hang/quyet-toan/sua/' + wagonSettlementId);
    }
	}
}
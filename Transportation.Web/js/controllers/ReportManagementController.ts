/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />
/// <reference path="../model/ExportTypeModel.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
  import helper = Clarity.Helper;
  import model = Clarity.Model;

  export class ReportManagementController {
    public exportService: service.ExportService;
    public orderCustomerService: service.CustomerOrderService;

    public isExportLoading: boolean;
    public isViewLoading: boolean;
    public fromDate: string;
    public toDate: string;
    public filteredCustomerOrders: Array<model.CustomerOrderModel>;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any, private $cookieStore: ng.ICookieStoreService) {

      this.exportService = new service.ExportService($http);
      this.orderCustomerService = new service.CustomerOrderService($http);
      $scope.viewModel = this;
    }

    exportTruck() {
      this.isExportLoading = true;
      this.exportService.exportToExcel({ type: model.ExportType.Truck }, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
        //this.exportService.deleteExcelFile(data['fileName'].split('/')[0], () => { }, null);
      }, () => {
        this.isExportLoading = false;
      });
    }

    exportEmployee() {
      this.isExportLoading = true;
      this.exportService.exportToExcel({ type: model.ExportType.Employee }, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
      }, () => {
        this.isExportLoading = false;
      });
    }

    exportCustomer() {
      this.isExportLoading = true;
      this.exportService.exportToExcel({ type: model.ExportType.Customer }, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
      }, () => {
        this.isExportLoading = false;
      });
    }

    viewCustomerOrder() {
      this.isViewLoading = true;
      this.orderCustomerService.getCustomerOrdersByDate({ fromDate: this.fromDate, toDate: this.toDate }, (data) => {
        this.isViewLoading = false;
        this.filteredCustomerOrders = data;
      }, null);
    }

    exportCustomerOrder() {
      this.isExportLoading = true;
      let jsonObject = {
        type: model.ExportType.OrderCustomer,
        fromDate: this.fromDate,
        toDate: this.toDate
      }

      this.exportService.exportToExcel(jsonObject, (data) => {
        this.isExportLoading = false;
        window.location.href = '/output/' + data['fileName'];
      }, () => {
        this.isExportLoading = false;
      });
    }
    
	}
}
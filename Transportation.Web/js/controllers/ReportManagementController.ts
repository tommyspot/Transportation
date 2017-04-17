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
    public isExportLoading: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any, private $cookieStore: ng.ICookieStoreService) {

      this.exportService = new service.ExportService($http);
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
    
	}
}
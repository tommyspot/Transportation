/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
    import service = Clarity.Service;
    import helper = Clarity.Helper;

    export class MainController {
        public mainHelper: helper.MainHelper;
        public authenticateService: service.AuthenticationService;

        constructor(private $scope,
            public $rootScope: IRootScope,
            private $http: ng.IHttpService,
            public $location: ng.ILocationService,
            public $window: ng.IWindowService,
            public $filter: ng.IFilterService,
            private $cookieStore: ng.ICookieStoreService) {

            $scope.viewModel = this;
            this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
            this.authenticateService = new service.AuthenticationService($http, $cookieStore);
        }

        goToWagonsManagement() {
            this.$location.path('/ql-toa-hang');
        }

        goToTruckSection() {
            this.$location.path('/ql-toa-hang/xe');
        }

        goToEmployeeSection() {
            this.$location.path('/ql-toa-hang/nhan-vien');
        }

        goToCustomerSection() {
            this.$location.path('/ql-toa-hang/khach-hang');
        }

        goToWagonsSection() {
            this.$location.path('/ql-toa-hang/toa-hang');
        }

        goToSettlementSection() {
            this.$location.path('/ql-toa-hang/quyet-toan');
        }

        goToUserManagement() {
            this.$location.path('/ql-dang-nhap');
        }

        goToReportManagement() {
            this.$location.path('/ql-bao-cao');
        }

        goToGarageManagement() {
            this.$location.path('/ql-garage');
        }

        goToProductManagement() {
            this.$location.path('/ql-garage/san-pham');
        }

        goToProductInputManagement() {
            this.$location.path('/ql-garage/nhap-kho');
        }

        goToSaleManagement() {
            this.$location.path('/ql-garage/ban-hang');
        }

        goToReportGarageManagement() {
            this.$location.path('/ql-garage/quan-ly');
        }

        goToTransportationManagement() {
          this.$location.path('/ql-phuong-tien');
        }

        isAdminRole() {
          return this.authenticateService.getUserRole() == Enum.Role.Admin;
        }

        hasWagonManagementPermission() {
          return this.isAdminRole() || this.authenticateService.getUserRole() == Enum.Role.WagonManagement;
        }

        hasGarageManagementPermission() {
          return this.isAdminRole() || this.authenticateService.getUserRole() == Enum.Role.GarageManagement;
        }

        hasReportManagementPermission() {
          return this.isAdminRole() || this.authenticateService.getUserRole() == Enum.Role.ReportManagement;
        }

        hasTransportationManagementPermission() {
          return this.isAdminRole() || this.authenticateService.getUserRole() == Enum.Role.TransportationManagement;
        }

        hasUserManagementPermission() {
          return this.authenticateService.getUserRole() != Enum.Role.TransportationManagement;
        }
    }
}
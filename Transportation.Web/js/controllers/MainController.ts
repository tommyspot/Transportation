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
		constructor(private $scope,
			public $rootScope: IRootScope,
			private $http: ng.IHttpService,
			public $location: ng.ILocationService,
			public $window: ng.IWindowService,
			public $filter: ng.IFilterService,
			private $cookieStore: ng.ICookieStoreService) {

      $scope.viewModel = this;
			this.mainHelper = new helper.MainHelper($http, $cookieStore);
    }

    goToWagonsManagement() {
      this.$location.path('/ql-toa-hang');
    }
    goToGarageManagement() {
      this.$location.path('/ql-garage');
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

    goToCustomerOrderSection() {
        this.$location.path('/ql-toa-hang/don-hang-cua-khach');
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

	}
}
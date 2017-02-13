/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class TruckManagementController {
		public currentTruck: Model.TruckModel;
    public truckService: service.TruckService;
    public truckList: Array<Model.TruckModel>;
    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService) {

			this.truckService = new service.TruckService($http);
      $scope.viewModel = this;
      this.initTruck();
    }

		initTruck() {
      this.currentTruck = new Model.TruckModel();

      this.truckService.getAll((results: Array<Model.TruckModel>) => {
        this.truckList = results;
      }, null);
    }

    createTruck(truck: Model.TruckModel) {
      this.truckService.create(truck,
        (data) => {
          this.$location.path('/ql-toa-hang/xe');
        },
        () => { });
    }

    goToTruckForm() {
      this.$location.path('/ql-toa-hang/xe/tao');
    }

	}
}
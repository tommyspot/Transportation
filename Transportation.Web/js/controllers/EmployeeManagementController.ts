/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class EmployeeManagementController {
    public currentEmployee: Model.EmployeeModel;
    public employeeService: service.EmployeeService;
    public employeeList: Array<Model.EmployeeModel>;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService) {

      this.employeeService = new service.EmployeeService($http);
      $scope.viewModel = this;
      this.initEmployee();
    }

    initEmployee() {
      this.currentEmployee = new Model.EmployeeModel();

      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
      }, null);
    }

    createEmployee(employee: Model.EmployeeModel) {
      this.employeeService.create(employee,
        (data) => {
          this.$location.path('/ql-toa-hang/nhan-vien');
        },
        () => { });
    }

    goToEmployeeForm() {
      this.$location.path('/ql-toa-hang/nhan-vien/tao');
    }

	}
}
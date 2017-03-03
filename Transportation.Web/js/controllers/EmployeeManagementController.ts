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
    public employeeListTmp: Array<Model.EmployeeModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.employeeService = new service.EmployeeService($http);
      $scope.viewModel = this;

      this.pageSize = 5;
      this.initEmployee();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.employeeListTmp && self.employeeListTmp.length > 0) {
          self.employeeList = $filter('filter')(self.employeeListTmp, value);
          self.initPagination();
        }
      });

    }

    initEmployee() {
      var employeeId = this.$routeParams.employee_id;
      if (employeeId) {
        if (this.$location.path() === '/ql-toa-hang/nhan-vien/' + employeeId) {
          this.employeeService.getById(employeeId, (data) => {
            this.currentEmployee = data;
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/nhan-vien/sua/' + employeeId) {
          if (this.currentEmployee == null) {
            this.employeeService.getById(employeeId, (data) => {
                this.currentEmployee = data;
                this.currentEmployee.driverLicenseDate = (data.driverLicenseDate != null && data.driverLicenseDate != null) ? new Date(data.driverLicenseDate) : null;
                this.currentEmployee.driverLicenseExpirationDate = (data.driverLicenseExpirationDate != null && data.driverLicenseExpirationDate != null) ? new Date(data.driverLicenseExpirationDate) : null;
                this.currentEmployee.startDate = (data.startDate != null && data.startDate != null) ? new Date(data.startDate) : null;
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/nhan-vien/tao') {
          this.currentEmployee = new Model.EmployeeModel();
        } else if (this.$location.path() === '/ql-toa-hang/nhan-vien') {
          this.initEmployeeList();
        }
      }
    }

    initEmployeeList() {
      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
        this.employeeList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
        this.employeeListTmp = this.employeeList;
        this.initPagination();
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.employeeList.length % this.pageSize === 0 ?
        this.employeeList.length / this.pageSize : Math.floor(this.employeeList.length / this.pageSize) + 1;
    }

    getEmployeeListOnPage() {
      if (this.employeeList && this.employeeList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.employeeList.slice(startIndex, endIndex);
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

    selectAllEmployeesOnPage() {
      var employeeOnPage = this.getEmployeeListOnPage();
      for (let index = 0; index < employeeOnPage.length; index++) {
        var employee = employeeOnPage[index];
        employee.isChecked = this.isCheckedAll;
      }
    }

    removeEmployees() {
      var confirmDialog = this.$window.confirm('Do you want to delete the employee?');
      if (confirmDialog) {
        for (let i = 0; i < this.employeeList.length; i++) {
          var employee = this.employeeList[i];
          if (employee.isChecked) {
            this.employeeService.deleteEntity(employee, (data) => {
              this.initEmployeeList();
            }, () => { });
          }
        }
      }
    }

    removeEmployeeInDetail(employee: Model.EmployeeModel) {
      var confirmDialog = this.$window.confirm('Do you want to delete the employee?');
      if (confirmDialog) {
        this.employeeService.deleteEntity(employee, (data) => {
          this.$location.path('/ql-toa-hang/nhan-vien');
        }, null);
      }
    }

    createEmployee(employee: Model.EmployeeModel) {
      this.employeeService.create(employee,
        (data) => {
          this.$location.path('/ql-toa-hang/nhan-vien');
        }, null);
    }

    updateEmployee(employee: Model.EmployeeModel) {
      this.employeeService.update(employee, (data) => {
        this.$location.path('/ql-toa-hang/nhan-vien');
      }, null);
    }

    goToEmployeeForm() {
      this.$location.path('/ql-toa-hang/nhan-vien/tao');
    }

    changeDateFormat(date) {
        var formatedDate = '';
        if (date) {
            var newDate = new Date(date);
            var dateNo = newDate.getDate().toString();
            dateNo = dateNo.toString().length == 2 ? dateNo : '0' + dateNo;
            var monthNo = (newDate.getMonth() + 1).toString();
            monthNo = monthNo.toString().length == 2 ? monthNo : '0' + monthNo;
            var yearNo = newDate.getFullYear();
            formatedDate = dateNo + '/' + monthNo + '/' + yearNo;
        }
        return formatedDate;
    }

	}
}
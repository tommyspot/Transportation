/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class EmployeeManagementController {
    public employeeService: service.EmployeeService;
    public currentEmployee: Model.EmployeeModel;

    public employeeList: Array<Model.EmployeeModel>;
    public employeeListTmp: Array<Model.EmployeeModel>;
    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public titles: Array<String>;
    public isLoading: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.employeeService = new service.EmployeeService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.initEmployee();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.employeeListTmp && self.employeeListTmp.length > 0) {
          self.employeeList = $filter('filter')(self.employeeListTmp, value);
          self.initPagination();
        }
      });
			this.titles = ['Giám Đốc', 'Trưởng Phòng', 'Phó Phòng', 'Trưởng Chi Nhánh', 'Phó Chi Nhánh', 'Tài Xế', 'Kế Toán'];
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
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/nhan-vien/tao') {
          this.currentEmployee = new Model.EmployeeModel();
        } else if (this.$location.path() === '/ql-toa-hang/nhan-vien') {
          this.isLoading = true;
          this.initEmployeeList();
        }
      }
    }

    initEmployeeList() {
      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
        this.employeeList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.employeeListTmp = this.employeeList;
        this.initPagination();
        this.isLoading = false;
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
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những nhân viên được chọn?');
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
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa nhân viên này?');
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

    goToEmployeeEditForm(event: Event, employeeId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-toa-hang/nhan-vien/sua/${employeeId}`);
    }

		checkStatusEmployee(employee) {
			return employee.isDeleted ? 'Không còn làm việc' : 'Còn làm việc';
		}

	}
}
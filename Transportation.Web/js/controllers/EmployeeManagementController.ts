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
    public employeeListView: Array<Model.EmployeeViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public titles: Array<String>;
    public isLoading: boolean;
    public searchText: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.employeeService = new service.EmployeeService($http);
      $scope.viewModel = this;

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.titles = ['Giám Đốc', 'Trưởng Phòng', 'Phó Phòng', 'Trưởng Chi Nhánh', 'Phó Chi Nhánh', 'Tài Xế', 'Kế Toán'];
      this.initEmployee();
			
      $scope.$watch('viewModel.searchText', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.currentPage === 0 ? this.fetchEmployeeListPerPage() : (() => { this.currentPage = 0; })();
        this.fetchNumOfPages();
      });

      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.fetchEmployeeListPerPage();
      });

      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.initEmployeeList();
      });
    }

    initEmployee() {
      var employeeId = this.$routeParams.employee_id;
      if (employeeId) {
        this.initCurrentEmployee(employeeId);
      } else {
        if (this.$location.path() === '/ql-toa-hang/nhan-vien/tao') {
          this.currentEmployee = new Model.EmployeeModel();
        } else if (this.$location.path() === '/ql-toa-hang/nhan-vien') {
          this.initEmployeeList();
        }
      }
    }

    initCurrentEmployee(employeeId: number) {
      if (this.currentEmployee == null) {
        this.$rootScope.showSpinner();

        this.employeeService.getById(employeeId, (data: Model.EmployeeModel) => {
          this.currentEmployee = data;
          this.$rootScope.hideSpinner();
        }, null);
      }
    }

    initEmployeeList() {
      this.currentPage = 0;
      this.fetchEmployeeListPerPage();
      this.fetchNumOfPages();
    }

    fetchEmployeeListPerPage() {
      this.isLoading = true;
      this.employeeService.getPerPage(this.currentPage, this.pageSize, this.searchText, (results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
        this.mapToEmployeeListView();
        this.isLoading = false;
      });
    }
    fetchNumOfPages() {
      this.employeeService.getNumOfPages(this.pageSize, this.searchText, (results: number) => {
        this.numOfPages = parseInt(results['pages']);
      });
    }

    mapToEmployeeListView() {
      this.employeeListView = this.employeeList.map((employee: Model.EmployeeModel) => {
        const employeeView = new Model.EmployeeViewModel();
        employeeView.id = employee.id;
        employeeView.fullName = employee.fullName;
        employeeView.mobile = employee.mobile;
        employeeView.title = employee.title;
        employeeView.startDate = employee.startDate;
        employeeView.status = employee.isDeleted ? 'Không còn làm việc' : 'Còn làm việc';
        return employeeView;
      });
    }

    selectAllEmployeesOnPage() {
      this.employeeListView.map(employee => employee.isChecked = this.isCheckedAll);
    }

    removeEmployees() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những nhân viên được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.employeeListView.length; i++) {
          var employee = this.employeeListView[i];
          if (employee.isChecked) {
            this.employeeService.deleteEntity(employee, (data) => {
              this.initEmployeeList();
            }, null);
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
      this.isSubmitting = true;
      this.employeeService.create(employee,
        (data) => {
          this.isSubmitting = false;
          this.$location.path('/ql-toa-hang/nhan-vien');
        }, null);
    }

    updateEmployee(employee: Model.EmployeeModel) {
      this.isSubmitting = true;
      this.employeeService.update(employee, (data) => {
        this.isSubmitting = false;
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

    clearSearchText() {
      this.searchText = '';
    }

    hasSelectedEmployee() {
      if (!this.employeeListView) return false;
      return this.employeeListView.some(employee => employee.isChecked);
    }

	}
}
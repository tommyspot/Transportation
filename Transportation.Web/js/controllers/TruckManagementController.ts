/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
  import service = Clarity.Service;
  import helper = Clarity.Helper;
  const formatSuffix = 'Formatted';

  export class TruckManagementController {
    public mainHelper: helper.MainHelper;
    public truckService: service.TruckService;
    public employeeService: service.EmployeeService;

    public currentTruck: Model.TruckModel;
    public employeeList: Array<Model.EmployeeModel>;
    public truckList: Array<Model.TruckModel>;
    public truckListView: Array<Model.TruckViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public searchText: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
      private $routeParams: any,
      private $cookieStore: ng.ICookieStoreService) {

      this.truckService = new service.TruckService($http);
      this.employeeService = new service.EmployeeService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      $scope.viewModel = this;

      this.currentPage = 0;
      this.pageSize = 10;
      this.searchText = '';
      this.initTruck();
      // Search
      $scope.$watch('viewModel.searchText', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        /** When currentPage change <=> Paging
            currentPage is 0, it should fetch truckList 
            currentPage is not 0, only update to 0 --> Paging will automatic proccess fetch truckList **/
        this.currentPage === 0 ? this.fetchTruckListPerPage() : (() => { this.currentPage = 0; })();
        this.fetchNumOfPages();
      });
      // Paging
      $scope.$watch('viewModel.currentPage', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.fetchTruckListPerPage();
      });
      // Display
      $scope.$watch('viewModel.pageSize', (newValue, oldValue) => {
        if (newValue === oldValue) return;
        this.clearSearchText();
        this.initTruckList();
      });
    }

    initTruck() {
      var truckId = this.$routeParams.truck_id;
      if (truckId) {  //Detail or Edit
        this.initCurrentTruck(truckId);
      } else {
        if (this.$location.path() === '/ql-toa-hang/xe/tao') {
          this.currentTruck = new Model.TruckModel();
          this.initEmployeeList();

        } else if (this.$location.path() === '/ql-toa-hang/xe') {
          this.initTruckList();
        }
      }
    }

    initTruckList() {
      this.currentPage = 0;
      this.fetchTruckListPerPage();
      this.fetchNumOfPages();
    }

    fetchTruckListPerPage() {
      this.isLoading = true;
      this.truckService.getPerPage(this.currentPage, this.pageSize, this.searchText, (results: Array<Model.TruckModel>) => {
        this.truckList = results;
        this.mapToTruckListView();
        this.isLoading = false;
      });
    }

    fetchNumOfPages() {
      this.truckService.getNumOfPages(this.pageSize, this.searchText, (results: number) => {
        this.numOfPages = parseInt(results['pages']);
      });
    }

    initCurrentTruck(truckId: number) {
      if (this.currentTruck == null) {
        this.$rootScope.showSpinner();

        this.truckService.getById(truckId, (data: Model.TruckModel) => {
          this.currentTruck = data;
          this.mainHelper.initCurrencyFormattedProperty(this.currentTruck, ['monthlyPayment'], formatSuffix);
          this.initEmployeeList();
        }, null);
      }
    }

    initEmployeeList() {
      this.employeeService.getAllCurtail((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
        this.$rootScope.hideSpinner();
      }, null);
    }

    mapToTruckListView() {
      this.truckListView = this.truckList.map((truck: Model.TruckModel) => {
        const truckView = new Model.TruckViewModel();
        truckView.id = truck.id;
        truckView.licensePlate = truck.licensePlate;
        truckView.yearOfManufacture = truck.yearOfManufacture > 0 ? truck.yearOfManufacture.toString() : '';
        truckView.brand = truck.brand;
        truckView.weight = truck.weight > 0 ? truck.weight + ' tấn' : '';
        truckView.hasUsing = truck.isDeleted ? 'Không' : 'Có';
        return truckView;
      });
    }

    selectAllTrucksOnPage() {
      this.truckListView.map(truck => truck.isChecked = this.isCheckedAll);
    }

    removeTrucks() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những xe được chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.truckListView.length; i++) {
          var truck = this.truckListView[i];
          if (truck.isChecked) {
            this.truckService.deleteEntity(truck, (data) => {
              this.initTruckList();
            }, null);
          }
        }
      }
    }

    removeTruckInDetail(truck: Model.TruckModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa xe này?');
      if (confirmDialog) {
        this.truckService.deleteEntity(truck, (data) => {
          this.$location.path('/ql-toa-hang/xe');
        }, null);
      }
    }

    createTruck(truck: Model.TruckModel) {
      this.isSubmitting = true;
      this.truckService.create(truck,
        (data) => {
          this.isSubmitting = false;
          this.$location.path('/ql-toa-hang/xe');
        }, null);
    }

    updateTruck(truck: Model.TruckModel) {
      this.isSubmitting = true;
      this.truckService.update(truck, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-toa-hang/xe');
      }, null);
    }

    goToTruckForm() {
      this.$location.path('/ql-toa-hang/xe/tao');
    }

    goToTruckEditForm(event: Event, truckId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-toa-hang/xe/sua/${truckId}`);
    }

    getEmployeeName(employeeId: string) {
      return this.mainHelper.getPropertyValue(this.employeeList, 'id', employeeId, 'fullName');
    }

    formatCurrency(propertyName: string) {
      this.mainHelper.onCurrencyPropertyChanged(this.currentTruck, propertyName, `${propertyName}${formatSuffix}`);
    }

    clearSearchText() {
      this.searchText = '';
    }

    hasSelectedTruck() {
      if (!this.truckListView) return false;
      return this.truckListView.some(truck => truck.isChecked);
    }
  }
}
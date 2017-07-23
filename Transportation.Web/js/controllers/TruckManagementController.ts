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
    public truckListTmp: Array<Model.TruckModel>;

		public numOfPages: number;
		public currentPage: number;
		public pageSize: number;
		public isCheckedAll: boolean;
    public isLoading: boolean;

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
			$scope.viewModel = this;
			this.pageSize = 10;
			this.initTruck();
      this.mainHelper = new helper.MainHelper($http, $cookieStore);

			var self = this;
			$scope.$watch('searchText', function (value) {
				if (self.truckListTmp && self.truckListTmp.length > 0) {
					self.truckList = $filter('filter')(self.truckListTmp, value);
					self.initPagination();
				}
			});
		}

		initTruck() {
			var truckId = this.$routeParams.employee_id;
			if (truckId) {
				if (this.$location.path() === '/ql-toa-hang/xe/' + truckId) {
					this.truckService.getById(truckId, (data) => {
            this.currentTruck = data;
            this.mainHelper.initFormattedProperty(this.currentTruck, ['monthlyPayment'], formatSuffix);
            this.initEmployeeList();
					}, null);
				} else if (this.$location.path() === '/ql-toa-hang/xe/sua/' + truckId) {
					if (this.currentTruck == null) {
						this.truckService.getById(truckId, (data) => {
							this.currentTruck = data;
              this.mainHelper.initFormattedProperty(this.currentTruck, ['monthlyPayment'], formatSuffix);
              this.initEmployeeList();
						}, null);
					}
				}
			} else {
				if (this.$location.path() === '/ql-toa-hang/xe/tao') {
          this.currentTruck = new Model.TruckModel();
          this.initEmployeeList();

        } else if (this.$location.path() === '/ql-toa-hang/xe') {
          this.isLoading = true;
					this.initTruckList();
				}
			}
		}

		initTruckList() {
			this.truckService.getAll((results: Array<Model.TruckModel>) => {
				this.truckList = results;
				this.truckList.sort(function (a: any, b: any) {
					return b.id - a.id;
				});
				this.truckListTmp = this.truckList;
        this.initPagination();
        this.isLoading = false;
			}, null);
		}

		initEmployeeList() {
			this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
			}, null);
		}

		initPagination() {
			this.currentPage = 1;
			this.numOfPages = this.truckList.length % this.pageSize === 0 ?
				this.truckList.length / this.pageSize : Math.floor(this.truckList.length / this.pageSize) + 1;
		}

		getTruckListOnPage() {
			if (this.truckList && this.truckList.length > 0) {
				var startIndex = this.pageSize * (this.currentPage - 1);
				var endIndex = startIndex + this.pageSize;
				return this.truckList.slice(startIndex, endIndex);
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

		selectAllTrucksOnPage() {
			var truckOnPage = this.getTruckListOnPage();
			for (let index = 0; index < truckOnPage.length; index++) {
				var truck = truckOnPage[index];
				truck.isChecked = this.isCheckedAll;
			}
		}

		removeTrucks() {
			var confirmDialog = this.$window.confirm('Bạn có muốn xóa những xe được chọn?');
			if (confirmDialog) {
				for (let i = 0; i < this.truckList.length; i++) {
					var truck = this.truckList[i];
					if (truck.isChecked) {
						this.truckService.deleteEntity(truck, (data) => {
							this.initTruckList();
						}, () => { });
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
			this.truckService.create(truck,
				(data) => {
					this.$location.path('/ql-toa-hang/xe');
				},
				() => { });
		}

		updateTruck(truck: Model.TruckModel) {
			this.truckService.update(truck, (data) => {
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

		checkStatusTruck(truck) {
			return truck.isDeleted ? 'Không' : 'Có';
		}

    getEmployeeName(employeeId: string) {
      return this.mainHelper.getPropertyValue(this.employeeList, 'id', employeeId, 'fullName');
		}

    formatCurrency(propertyName: string) {
      this.mainHelper.formatCurrency(this.currentTruck, propertyName, `${propertyName}${formatSuffix}`);
		}
	}
}
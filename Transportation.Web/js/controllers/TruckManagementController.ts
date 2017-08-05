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
    public truckListView: Array<Model.TruckViewModel>;  //show on view page for searching
    public truckListViewTmp: Array<Model.TruckViewModel>;

		public numOfPages: number;
		public currentPage: number;
		public pageSize: number;
		public isCheckedAll: boolean;
    public isLoading: boolean;
    public searchText: string;

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

      this.pageSize = 10;
      this.searchText = '';
			this.initTruck();

			var self = this;
			$scope.$watch('viewModel.searchText', function (value) {
				if (self.truckListViewTmp && self.truckListViewTmp.length > 0) {
					self.truckListView = $filter('filter')(self.truckListViewTmp, value);
					self.initPagination();
				}
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
      this.isLoading = true;
			this.truckService.getAll((results: Array<Model.TruckModel>) => {
				this.truckList = results;
				this.truckList.sort(function (a: any, b: any) {
					return b.id - a.id;
        });
        this.mapToTruckListView();
				this.truckListViewTmp = this.truckListView;
        this.initPagination();
        this.isLoading = false;
			}, null);
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
      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
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

		initPagination() {
			this.currentPage = 1;
			this.numOfPages = this.truckListView.length % this.pageSize === 0 ?
        this.truckListView.length / this.pageSize : Math.floor(this.truckListView.length / this.pageSize) + 1;
		}

		getTruckListOnPage() {
      if (this.truckListView && this.truckListView.length > 0) {
				var startIndex = this.pageSize * (this.currentPage - 1);
				var endIndex = startIndex + this.pageSize;
        return this.truckListView.slice(startIndex, endIndex);
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
			this.truckService.create(truck,
				(data) => {
					this.$location.path('/ql-toa-hang/xe');
				}, null);
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

    getEmployeeName(employeeId: string) {
      return this.mainHelper.getPropertyValue(this.employeeList, 'id', employeeId, 'fullName');
		}

    formatCurrency(propertyName: string) {
      this.mainHelper.onCurrencyPropertyChanged(this.currentTruck, propertyName, `${propertyName}${formatSuffix}`);
    }

    clearSearchText() {
      this.searchText = '';
    }
	}
}
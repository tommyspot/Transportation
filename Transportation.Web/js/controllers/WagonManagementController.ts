/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
  import helper = Clarity.Helper;

  const formatSufix = 'Formatted';

  export class WagonManagementController {
    public mainHelper: helper.MainHelper;
    public currentWagon: Model.WagonModel;

    public wagonService: service.WagonService;
    public employeeService: service.EmployeeService;
    public customerService: service.CustomerService;
    public exportService: service.ExportService;
    public truckService: service.TruckService;

    public truckList: Array<Model.TruckModel>
    public employeeList: Array<Model.EmployeeModel>
    public customerList: Array<Model.CustomerModel>
    public wagonList: Array<Model.WagonModel>;
    public wagonListView: Array<Model.WagonViewModel>;
    public wagonListViewTmp: Array<Model.WagonViewModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public isLoading: boolean;
    public totalAmountWagonSettlements: number;
    public searchText: string;
    public isSubmitting: boolean;

    constructor(private $scope,
      public $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any, private $cookieStore: ng.ICookieStoreService) {

      this.wagonService = new service.WagonService($http);
      this.truckService = new service.TruckService($http);
      this.employeeService = new service.EmployeeService($http);
      this.customerService = new service.CustomerService($http);
      this.exportService = new service.ExportService($http);
      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.searchText = '';
      this.initWagon();

      var self = this;
      $scope.$watch('viewModel.searchText', function (value) {
        if (self.wagonListViewTmp && self.wagonListViewTmp.length > 0) {
          self.wagonListView = $filter('filter')(self.wagonListViewTmp, value);
          self.initPagination();
        }
      });
    }

    initWagon() {
      var wagonId = this.$routeParams.wagon_id;
      if (wagonId) {
        this.initCurrentWagon(wagonId);
      } else {
        if (this.$location.path() === '/ql-toa-hang/toa-hang/tao') {
          this.currentWagon = new Model.WagonModel();
          this.initTruckList();
          this.initEmployeeList();
          this.initCustomerList();
        } else if (this.$location.path() === '/ql-toa-hang/toa-hang') {
          this.initWagonList();
        }
      }
    }

    initCurrentWagon(wagonId: number) {
      if (this.currentWagon == null) {
        this.$rootScope.showSpinner();
        this.wagonService.getById(wagonId, (data) => {
          this.currentWagon = data;
          this.initFormattedCurrencyForWagon();
          this.$rootScope.hideSpinner();

          this.initTruckList();
          this.initEmployeeList();
          this.initCustomerList();
        }, null);
      }
    }

    initWagonList() {
      this.isLoading = true;
      this.wagonService.getAll((results: Array<Model.WagonModel>) => {
        this.wagonList = results;
        this.wagonList.sort(function (a: any, b: any) {
          return b.id - a.id;
        });
        this.mapToWagonListView();
        this.wagonListViewTmp = this.wagonListView;
        this.initPagination();
        this.isLoading = false;
      }, null);
    }

    mapToWagonListView() {
      this.wagonListView = this.wagonList.map((wagon: Model.WagonModel) => {
        const wagonView = new Model.WagonViewModel();
        wagonView.id = wagon.id;
        wagonView.code = wagon.code;
        wagonView.paymentDate = wagon.paymentDate;
        wagonView.departDate = wagon.departDate;
        wagonView.returnDate = wagon.returnDate;
        wagonView.departure = wagon.departure;
        wagonView.destination = wagon.destination;
        const totalPayment = wagon.paymentOfTruck + wagon.paymentOfRepairing + wagon.paymentOfOil + wagon.paymentOfLuong +
                             wagon.paymentOfService + wagon.paymentOfHangVe - wagon.paymentOf10Percent;
        wagonView.totalPayment = this.$filter('currency')(totalPayment, '', 0).trim();
        return wagonView;
      });
    }

    initTruckList() {
			this.truckService.getAll((results: Array<Model.TruckModel>) => {
				this.$rootScope.truckList = results;
        this.truckList = this.$rootScope.truckList;
			}, null);
    }

    initEmployeeList() {
      this.employeeService.getAll((results: Array<Model.EmployeeModel>) => {
        this.employeeList = results;
      }, null);
    }

    initCustomerList() {
      this.customerService.getAll((results: Array<Model.CustomerModel>) => {
        this.customerList = results;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.wagonListView.length % this.pageSize === 0 ?
        this.wagonListView.length / this.pageSize : Math.floor(this.wagonListView.length / this.pageSize) + 1;
    }

    getWagonListOnPage() {
      if (this.wagonListView && this.wagonListView.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.wagonListView.slice(startIndex, endIndex);
      }
    }

    selectAllWagonsOnPage() {
      var wagonOnPage = this.getWagonListOnPage();
      for (let index = 0; index < wagonOnPage.length; index++) {
        var wagon = wagonOnPage[index];
        wagon.isChecked = this.isCheckedAll;
      }
    }

    removeWagons() {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa những toa hàng đã chọn?');
      if (confirmDialog) {
        for (let i = 0; i < this.wagonListView.length; i++) {
          var wagon = this.wagonListView[i];
          if (wagon.isChecked) {
            this.wagonService.deleteEntity(wagon, (data) => {
              this.initWagonList();
            }, null);
          }
        }
      }
    }

    removeWagonInDetail(wagon: Model.WagonModel) {
      var confirmDialog = this.$window.confirm('Bạn có muốn xóa toa hàng này?');
      if (confirmDialog) {
        this.wagonService.deleteEntity(wagon, (data) => {
          this.$location.path('/ql-toa-hang/toa-hang');
        }, null);
      }
    }

    createWagon(wagon: Model.WagonModel) {
      this.isSubmitting = true;
      this.wagonService.create(wagon, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-toa-hang/toa-hang');
      }, null);
    }

    updateWagon(wagon: Model.WagonModel) {
      this.isSubmitting = true;
      this.wagonService.update(wagon, (data) => {
        this.isSubmitting = false;
        this.$location.path('/ql-toa-hang/toa-hang');
      }, null);
    }

    createWagonSettlement(wagon: Model.WagonModel) {
      if (wagon.wagonSettlements == null) {
        wagon.wagonSettlements = [];
      }
      var wagonSettlement = new Model.WagonSettlementModel();
      wagon.wagonSettlements.push(wagonSettlement);
    }

    deleteWagonSettlement(index: number) {
      this.currentWagon.wagonSettlements.splice(index, 1);
      this.totalAmountWagonSettlements = this.calculateTotalAmountWagonSettlements(this.currentWagon);
    }

    goToWagonForm() {
      this.$location.path('/ql-toa-hang/toa-hang/tao');
    }

    goToWagonEditForm(event: Event, wagonId: number) {
      event.stopPropagation();
      this.$location.path('/ql-toa-hang/toa-hang/sua/' + wagonId);
    }

    updateWagonSettlementPayment(wagonSettlement: Model.WagonSettlementModel) {
      if (wagonSettlement && wagonSettlement.unitPrice && wagonSettlement.quantity) {
        const totalAmount = wagonSettlement.quantity * wagonSettlement.unitPrice;
        wagonSettlement.paymentRemain = (totalAmount + (wagonSettlement.phiPhatSinh ? wagonSettlement.phiPhatSinh : 0))
          - (wagonSettlement.payment ? wagonSettlement.payment : 0);
        wagonSettlement.paymentStatus = wagonSettlement.paymentRemain == 0 ? 'Không nợ' : 'Nợ';
      }
    }

		initFormattedCurrencyForWagon() {
      if (this.currentWagon) {
        const formattedProperties = [
          'costOfTruck', 'costOfService', 'costOfTangBoXe', 'costOfPenalty',
          'costOfExtra', 'paymentOfTruck', 'paymentOfRepairing', 'paymentOfOil',
          'paymentOfLuong', 'paymentOfService', 'paymentOfHangVe', 'paymentOf10Percent'
        ];
        this.mainHelper.initCurrencyFormattedProperty(this.currentWagon, formattedProperties, formatSufix);

        if (this.currentWagon.wagonSettlements && this.currentWagon.wagonSettlements.length > 0) {
          const wagonSettelments = this.currentWagon.wagonSettlements;
          const wagonSettlementFormattedProperties = ['unitPrice', 'phiPhatSinh', 'payment'];
          wagonSettelments.forEach(wagonSettelment => {
            this.mainHelper.initCurrencyFormattedProperty(wagonSettelment, wagonSettlementFormattedProperties, formatSufix);
          });
        }
        this.totalAmountWagonSettlements = this.calculateTotalAmountWagonSettlements(this.currentWagon);
			}
    }

    formatCurrency(object: Object, propertyName:string, formattedPropertyName: string) {
      this.mainHelper.onCurrencyPropertyChanged(object, propertyName, formattedPropertyName);
    }

    onChangeWagonSettlement(wagonSettlement: Model.WagonSettlementModel, propertyName: string, formattedPropertyName: string) {
      if (propertyName && formattedPropertyName) {
        this.mainHelper.onCurrencyPropertyChanged(wagonSettlement, propertyName, formattedPropertyName);
      }
      this.totalAmountWagonSettlements = this.calculateTotalAmountWagonSettlements(this.currentWagon);
      this.updateWagonSettlementPayment(wagonSettlement);
    }

    calculateTotalAmountWagonSettlements(wagon: Model.WagonModel) {
      let totalAmount = 0;
      if (wagon && wagon.wagonSettlements && wagon.wagonSettlements.length > 0) {
        wagon.wagonSettlements.forEach(wagonSettlement => {
          totalAmount += wagonSettlement.quantity * wagonSettlement.unitPrice;
        });
      }
      return totalAmount;
    }

    getLicensePlate(truckId: string): string {
      return this.mainHelper.getPropertyValue(this.truckList, 'id', truckId, 'licensePlate');
    }

    getEmployeeName(employeeId: string): string {
      return this.mainHelper.getPropertyValue(this.employeeList, 'id', employeeId, 'fullName');
    }

    getCustomerName(customerId: string): string {
      return this.mainHelper.getPropertyValue(this.customerList, 'id', customerId, 'fullName');
    }

    // wagonCode = NgayThanhToan_SoXe
    updateWagonCode() {
      if (this.currentWagon.paymentDate && this.currentWagon.truckId) {
        this.currentWagon.code = this.currentWagon.paymentDate.replace(/\//g, '') + '_' +
          this.mainHelper.getPropertyValue(this.truckList, 'id', this.currentWagon.truckId.toString(), 'licensePlate');
      }
    }

    clearSearchText() {
      this.searchText = '';
    }

	}
}
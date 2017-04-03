/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class WagonManagementController {
    public currentWagon: Model.WagonModel;

    public wagonService: service.WagonService;
    public customerOrderService: service.CustomerOrderService;
    public employeeService: service.EmployeeService;
    public customerService: service.CustomerService;
    public exportService: service.ExportService;
    public truckService: service.TruckService;

    public truckList: Array<Model.TruckModel>
    public employeeList: Array<Model.EmployeeModel>
    public customerList: Array<Model.CustomerModel>
    public customerOrderList: Array<Model.CustomerOrderModel>
    public wagonList: Array<Model.WagonModel>;
    public wagonListTmp: Array<Model.WagonModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public mainHelper: helper.MainHelper;

    constructor(private $scope,
      private $rootScope: IRootScope,
      private $http: ng.IHttpService,
      private $location: ng.ILocationService,
      private $window: ng.IWindowService,
      private $filter: ng.IFilterService,
      private $routeParams: any) {

      this.wagonService = new service.WagonService($http);
      this.customerOrderService = new service.CustomerOrderService($http);
      this.truckService = new service.TruckService($http);
      this.employeeService = new service.EmployeeService($http);
      this.customerService = new service.CustomerService($http);
      this.exportService = new service.ExportService($http);
      this.mainHelper = new helper.MainHelper();
      $scope.viewModel = this;

      this.pageSize = 5;
      this.initWagon();

      var self = this;
      $scope.$watch('searchText', function (value) {
        if (self.wagonListTmp && self.wagonListTmp.length > 0) {
          self.wagonList = $filter('filter')(self.wagonListTmp, value);
          self.initPagination();
        }
      });
    }

    initWagon() {
      var wagonId = this.$routeParams.wagon_id;
      if (wagonId) {
        if (this.$location.path() === '/ql-toa-hang/toa-hang/' + wagonId) {
          this.wagonService.getById(wagonId, (data) => {
            this.currentWagon = data;
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/toa-hang/sua/' + wagonId) {
          this.initTruckList();
          this.initEmployeeList();
          this.initCustomerList();
          this.initCustomerOrderList();

          if (this.currentWagon == null) {
            this.wagonService.getById(wagonId, (data) => {
              this.currentWagon = data;
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/toa-hang/tao') {
          this.initTruckList();
          this.initEmployeeList();
          this.initCustomerList();
          this.initCustomerOrderList();
          this.currentWagon = new Model.WagonModel();
        } else if (this.$location.path() === '/ql-toa-hang/toa-hang') {
          this.initWagonList();
        }
      }
    }

    initWagonList() {
      this.wagonService.getAll((results: Array<Model.WagonModel>) => {
        this.wagonList = results;
        this.wagonList.sort(function (a: any, b: any) {
          return a.id - b.id;
        });
        this.wagonListTmp = this.wagonList;
        this.initPagination();
      }, null);
    }

    initTruckList() {
      this.truckService.getAll((results: Array<Model.TruckModel>) => {
        this.truckList = results;
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

    initCustomerOrderList() {
      this.customerOrderService.getAll((results: Array<Model.CustomerOrderModel>) => {
        this.customerOrderList = results;
      }, null);
    }

    initPagination() {
      this.currentPage = 1;
      this.numOfPages = this.wagonList.length % this.pageSize === 0 ?
        this.wagonList.length / this.pageSize : Math.floor(this.wagonList.length / this.pageSize) + 1;
    }

    getWagonListOnPage() {
      if (this.wagonList && this.wagonList.length > 0) {
        var startIndex = this.pageSize * (this.currentPage - 1);
        var endIndex = startIndex + this.pageSize;
        return this.wagonList.slice(startIndex, endIndex);
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

    selectAllWagonsOnPage() {
      var wagonOnPage = this.getWagonListOnPage();
      for (let index = 0; index < wagonOnPage.length; index++) {
        var wagon = wagonOnPage[index];
        wagon.isChecked = this.isCheckedAll;
      }
    }

    removeWagons() {
      var confirmDialog = this.$window.confirm('Do you want to delete the wagon?');
      if (confirmDialog) {
        for (let i = 0; i < this.wagonList.length; i++) {
          var wagon = this.wagonList[i];
          if (wagon.isChecked) {
            this.wagonService.deleteEntity(wagon, (data) => {
              this.initWagonList();
            }, () => { });
          }
        }
      }
    }

    removeWagonInDetail(wagon: Model.WagonModel) {
      var confirmDialog = this.$window.confirm('Do you want to delete the wagon?');
      if (confirmDialog) {
        this.wagonService.deleteEntity(wagon, (data) => {
          this.$location.path('/ql-toa-hang/toa-hang');
        }, null);
      }
    }

    exportWagonToExcel(wagon: Model.WagonModel) {
      this.exportService.exportWagonToExcel(wagon, (data) => {
        window.location.href = '/output/' + data['fileName'];
      }, null);
    }

    createWagon(wagon: Model.WagonModel) {
      this.wagonService.create(wagon,
        (data) => {
          this.$location.path('/ql-toa-hang/toa-hang');
        }, null);
    }

    updateWagon(wagon: Model.WagonModel) {
      this.wagonService.update(wagon, (data) => {
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
    }

    goToWagonForm() {
      this.$location.path('/ql-toa-hang/toa-hang/tao');
    }

    bindDataCustomerOrderTowagonSettlement(wagonSettlement: Model.WagonSettlementModel, customerOrderId: number) {
      this.customerOrderList.map(customerOrder => {
        if (customerOrder.id == customerOrderId) {
          wagonSettlement.formatedCustomerOrder = customerOrder.quantity + '_' + this.mainHelper.formatDateTimeDDMMYYYY(customerOrder.createdDate) +
            '_' + customerOrder.customerArea + '_' + customerOrder.customerName + '_' + customerOrder.customerPhone;
          wagonSettlement.customerOrderId = customerOrder.id;
          wagonSettlement.customerId = customerOrder.customerId;
          wagonSettlement.date = customerOrder.createdDate;
          wagonSettlement.departure = customerOrder.departure;
          wagonSettlement.destination = customerOrder.destination;
          wagonSettlement.employeeId = customerOrder.employeeId;
          wagonSettlement.notes = customerOrder.notes;
          wagonSettlement.quantity = customerOrder.quantity;
          wagonSettlement.unit = customerOrder.unit;
          wagonSettlement.unitPrice = customerOrder.unitPrice;
          wagonSettlement.totalAmount = wagonSettlement.quantity * wagonSettlement.unitPrice;
        }
      });
    }

    showEditWagonSettlement(wagonSettlement: Model.WagonSettlementModel) {
      if (angular.isNumber(wagonSettlement.customerOrderId) && !isNaN(wagonSettlement.customerOrderId)) {
        return true;
      }
      return false;
    }

    onChangeWagonSettlementQuatity(wagonSettlement: Model.WagonSettlementModel) {
      wagonSettlement.totalAmount = wagonSettlement.quantity * wagonSettlement.unitPrice;
    }

    resetWagonSettlement(wagonSettlement: Model.WagonSettlementModel) {
      this.customerOrderList.map(customerOrder => {
        if (customerOrder.id == wagonSettlement.customerOrderId) {
          wagonSettlement.quantity = customerOrder.quantity;
          wagonSettlement.notes = customerOrder.notes;
          wagonSettlement.totalAmount = wagonSettlement.quantity * wagonSettlement.unitPrice;
        }
      });
    }

    getMaxQualityWagonSettlement(wagonSettlement: Model.WagonSettlementModel) {
      var max = wagonSettlement.quantity;
      return max;
    }

	}
}

//thaotest
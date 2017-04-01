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
		public customerOrderListTmp: Array<Model.CustomerOrderModel>
    public wagonList: Array<Model.WagonModel>;
    public wagonListTmp: Array<Model.WagonModel>;

    public numOfPages: number;
    public currentPage: number;
    public pageSize: number;
    public isCheckedAll: boolean;
    public mainHelper: helper.MainHelper;
		public truckListInCustomerOrder: Array<Model.TruckModel>;
		public searchDate: Date;
		public truckSearch: string;
		public paymentPlaceList: any;
		public search: any;

    constructor(private $scope,
      public $rootScope: IRootScope,
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
			this.search = { truckLicensePlate: '' };
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

			this.$scope.$watch('viewModel.searchDate', function (newVal, oldVal) {
        if ((oldVal == newVal) || oldVal == undefined || newVal == undefined)
          return;

				self.customerOrderListTmp = self.$filter('filter')(self.customerOrderList, { 'date': newVal });
      }, true);
    }

    initWagon() {
      var wagonId = this.$routeParams.wagon_id;
			this.initTruckList();
			this.initEmployeeList();
			this.initCustomerList();
			this.initCustomerOrderList();

      if (wagonId) {
        if (this.$location.path() === '/ql-toa-hang/toa-hang/' + wagonId) {
          this.wagonService.getById(wagonId, (data) => {
            this.currentWagon = data;
          }, null);
        } else if (this.$location.path() === '/ql-toa-hang/toa-hang/sua/' + wagonId) {
					
          if (this.currentWagon == null) {
            this.wagonService.getById(wagonId, (data) => {
              this.currentWagon = data;
							this.formatedCurencyForWagon();
            }, null);
          }
        }
      } else {
        if (this.$location.path() === '/ql-toa-hang/toa-hang/tao') {
          
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
			//if (this.$rootScope && this.$rootScope.truckList && this.$rootScope.truckList.length > 0) {
			//	this.truckList = this.$rootScope.truckList;
			//} else {
				this.truckService.getAll((results: Array<Model.TruckModel>) => {
					this.$rootScope.truckList = results;
					this.truckList = this.$rootScope.truckList;
				}, null);
			//}
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
				this.customerOrderListTmp = angular.copy(this.customerOrderList);
				this.getTruckListInCustomerOrder();
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
          wagonSettlement.formatedCustomerOrder = customerOrder.customerName + '_' + customerOrder.customerPhone + '_' + customerOrder.quantity;
          wagonSettlement.customerOrderId = customerOrder.id;
          wagonSettlement.customerId = customerOrder.customerId;
          wagonSettlement.date = customerOrder.createdDate;
          wagonSettlement.departure = customerOrder.departure;
          wagonSettlement.destination = customerOrder.destination;
          wagonSettlement.notes = customerOrder.notes;
          wagonSettlement.quantity = customerOrder.quantity;
          wagonSettlement.unit = customerOrder.unit;
          wagonSettlement.unitPrice = customerOrder.unitPrice;
					wagonSettlement.discount = 500000;
					wagonSettlement.payment = 100000;
          wagonSettlement.totalAmount = wagonSettlement.quantity * wagonSettlement.unitPrice - wagonSettlement.discount;
					wagonSettlement.paymentRemain = wagonSettlement.totalAmount - wagonSettlement.payment;
					wagonSettlement.paymentStatus = wagonSettlement.paymentRemain == 0 ? 'Không Nợ' : 'Nợ';
					wagonSettlement.paymentPlace = this.currentWagon.paymentPlace;
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

		getTruckListInCustomerOrder() {
			
			if (this.customerOrderList && this.truckList) {
				this.truckListInCustomerOrder = angular.copy(this.truckList);
				for (let i = this.truckListInCustomerOrder.length - 1; i >= 0; i--) {
					var truck = this.truckListInCustomerOrder[i];
					var foundTruck = false;
					for (let j = 0; j < this.customerOrderList.length; j++) {
						var customerOrder = this.customerOrderList[j];
						if (customerOrder.truckId == truck.id) {
							foundTruck = true;
							break;
						}
					}

					if (!foundTruck) {
						this.truckListInCustomerOrder.splice(i, 1);
					}
				}
				
			}
		}

		formatedCurencyForWagon() {
			if (this.currentWagon) {
				this.currentWagon.costOfTruckFormated = this.currentWagon.costOfTruck.toLocaleString();
				this.currentWagon.costOfServiceFormated = this.currentWagon.costOfService.toLocaleString();
				this.currentWagon.costOfTangBoXeFormated = this.currentWagon.costOfTangBoXe.toLocaleString();
				this.currentWagon.costOfPenaltyFormated = this.currentWagon.costOfPenalty.toLocaleString();
				this.currentWagon.costOfExtraFormated = this.currentWagon.costOfExtra.toLocaleString();
				this.currentWagon.paymentOfTruckFormated = this.currentWagon.paymentOfTruck.toLocaleString();
				this.currentWagon.paymentOfRepairingFormated = this.currentWagon.paymentOfRepairing.toLocaleString();
				this.currentWagon.paymentOfOilFormated = this.currentWagon.paymentOfOil.toLocaleString();
				this.currentWagon.paymentOfLuongFormated = this.currentWagon.paymentOfLuong.toLocaleString(); 
				this.currentWagon.paymentOfServiceFormated = this.currentWagon.paymentOfService.toLocaleString();
				this.currentWagon.paymentOfHangVeFormated = this.currentWagon.paymentOfHangVe.toLocaleString();
				this.currentWagon.paymentOfOthersFormated = this.currentWagon.paymentOfOthers.toLocaleString();
			}
		}

		setFormatedCurencyForWagon(changeFormatNumber, type) {
			if (changeFormatNumber && changeFormatNumber != '') {
				switch (type) {
					case '0':
						this.currentWagon.costOfTruck = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.costOfTruckFormated = this.currentWagon.costOfTruck.toLocaleString();
						break;
					case '1':
						this.currentWagon.costOfService = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.costOfServiceFormated = this.currentWagon.costOfService.toLocaleString();
						break;
					case '2':
						this.currentWagon.costOfTangBoXe = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.costOfTangBoXeFormated = this.currentWagon.costOfTangBoXe.toLocaleString();
						break;
					case '3':
						this.currentWagon.costOfPenalty= parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.costOfPenaltyFormated = this.currentWagon.costOfPenalty.toLocaleString();
						break;
					case '4':
						this.currentWagon.costOfExtra = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.costOfExtraFormated = this.currentWagon.costOfExtra.toLocaleString();
						break;
					case '5':
						this.currentWagon.paymentOfTruck = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfTruckFormated = this.currentWagon.paymentOfTruck.toLocaleString();
						break;
					case '6':
						this.currentWagon.paymentOfRepairing = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfRepairingFormated = this.currentWagon.paymentOfRepairing.toLocaleString();
						break;
					case '7':
						this.currentWagon.paymentOfOil = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfOilFormated = this.currentWagon.paymentOfOil.toLocaleString();
						break;
					case '8':
						this.currentWagon.paymentOfLuong = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfLuongFormated = this.currentWagon.paymentOfLuong.toLocaleString(); 
						break;
					case '9':
						this.currentWagon.paymentOfService = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfServiceFormated = this.currentWagon.paymentOfService.toLocaleString();
						break;
					case '10':
						this.currentWagon.paymentOfHangVe = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfHangVeFormated = this.currentWagon.paymentOfHangVe.toLocaleString();
						break;
					case '11':
						this.currentWagon.paymentOfOthers = parseInt(changeFormatNumber.replace(/,/g, ''));
						this.currentWagon.paymentOfOthersFormated = this.currentWagon.paymentOfOthers.toLocaleString();
						break;
				}

			}
		}
	}
}
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class OrderService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/orders';
    }

    getDeletedOrdersPerPage(pageIndex, pageSize, searchText: string, successCallback: Function, errorCallback: Function) {
        const search = searchText ? searchText.trim() : '';
        this.http.get(`/api/deletedOrders/page?pageIndex=${pageIndex}&pageSize=${pageSize}&search=${search}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getDeletedOrdersNumOfPages(pageSize, searchText: string, successCallback: Function, errorCallback: Function) {
        const search = searchText ? searchText.trim() : '';
        this.http.get(`/api/deletedOrders/numberOfPages?pageSize=${pageSize}&search=${search}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    changeOrderStatus(entity: Model.BaseModel, successCallback: Function, errorCallback: Function) {
      this.http.put(this.url + '/status/' + entity.id, entity)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getTruckInfoPerPage(
      pageIndex: number,
      pageSize: number,
      searchText: string,
      truckLicensePlate: string,
      fromDate: string,
      toDate: string,
      successCallback: Function,
      errorCallback?: Function) {
      const search = searchText ? searchText.trim() : '';
      this.http.get(`${this.url}/truckInfo/page?pageIndex=${pageIndex}&pageSize=${pageSize}&search=${search}&truckLicensePlate=${truckLicensePlate}&fromDate=${fromDate}&toDate=${toDate}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getTruckInfoNumOfPages(
      pageSize: number,
      searchText: string,
      truckLicensePlate: string,
      fromDate: string,
      toDate: string,
      successCallback: Function,
      errorCallback?: Function) {
      const search = searchText ? searchText.trim() : '';
      this.http.get(`${this.url}/truckInfo/numberOfPages?pageSize=${pageSize}&search=${search}&truckLicensePlate=${truckLicensePlate}&fromDate=${fromDate}&toDate=${toDate}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }
  }
}
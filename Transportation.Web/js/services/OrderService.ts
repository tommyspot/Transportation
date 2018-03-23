/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class OrderService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/orders';
    }

    getDeletedOrdersPerPage(pageIndex, pageSize, successCallback: Function, errorCallback: Function) {
      this.http.get(`/api/deletedOrders/page?pageIndex=${pageIndex}&pageSize=${pageSize}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getDeletedOrdersNumOfPages(pageSize, successCallback: Function, errorCallback: Function) {
      this.http.get(`/api/deletedOrders/pageSize/${pageSize}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    changeOrderStatus(entity: Model.BaseModel, successCallback: Function, errorCallback: Function) {
      this.http.put(this.url + '/status/' + entity.id, entity)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

  }
}
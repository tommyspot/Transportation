/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class CustomerOrderService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/customerOrders';
    }

    getById(id: number, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'GET', url: this.url + '/' + id })
        .success((data: Model.CustomerOrderModel) => {
          data.createdDate = new Date(data.createdDate.toString());
          data.returnDate = new Date(data.returnDate.toString());
          data.departDate = new Date(data.departDate.toString());
          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getAll(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url)
        .success((data: Array<Model.CustomerOrderModel>) => {
          for (let i = 0; i < data.length; i++) {
            var customerOrder = data[i];
            customerOrder.departDate = new Date(customerOrder.departDate.toString());
            customerOrder.returnDate = new Date(customerOrder.returnDate.toString());
            customerOrder.createdDate = new Date(customerOrder.createdDate.toString());
          }

          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

  }
}
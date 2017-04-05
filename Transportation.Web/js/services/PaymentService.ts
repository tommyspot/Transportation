/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class PaymentService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/payments';
    }

    getByWagonSettlementCode(code: string, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'GET', url: this.url + '/' + code })
        .success((data, status) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    //getAll(successCallback: Function, errorCallback: Function) {
    //  this.http.get(this.url)
    //    .success((data: Array<Model.WagonModel>) => {
    //      for (let i = 0; i < data.length; i++) {
    //        var wagon = data[i];
    //        wagon.departDate = new Date(wagon.departDate.toString());
    //        wagon.returnDate = new Date(wagon.returnDate.toString());
    //        wagon.paymentDate = new Date(wagon.paymentDate.toString());
    //      }

    //      this.doCallback(successCallback, data);
    //    })
    //    .error((data, status) => { this.doCallback(errorCallback, data, status); });
    //}

  }
}
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class WagonService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/wagons';
    }

    getById(id: number, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'GET', url: this.url + '/' + id })
        .success((data: Model.WagonModel) => {
          data.departDate = new Date(data.departDate.toString());
          data.returnDate = new Date(data.returnDate.toString());
          data.wagonSettlements = data.wagonSettlements.map(function (wagonSettlement, index) {
            wagonSettlement.date = new Date(wagonSettlement.date.toString());
            return wagonSettlement;
          });
          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getAll(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url)
        .success((data: Array<Model.WagonModel>) => {
          for (let i = 0; i < data.length; i++) {
            var wagon = data[i];
            wagon.departDate = new Date(wagon.departDate.toString());
            wagon.returnDate = new Date(wagon.returnDate.toString());
          }

          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

  }
}
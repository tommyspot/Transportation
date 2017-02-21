/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class WagonSettlementService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/wagonSettlements';
    }

    getById(id: number, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'GET', url: this.url + '/' + id })
        .success((data: Model.WagonSettlementModel) => {
          data.date = new Date(data.date.toString());
          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getAll(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url)
        .success((data: Array<Model.WagonSettlementModel>) => {
          for (let i = 0; i < data.length; i++) {
            var wagon = data[i];
            wagon.date = new Date(wagon.date.toString());
          }
          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

  }
}
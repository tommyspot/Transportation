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
          data.wagonSettlements = data.wagonSettlements.map((wagonSettlement, index) => {
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
          }

          this.doCallback(successCallback, data);
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getWagonReportByDate(dateObject: any, successCallback: Function, errorCallback: Function) {
      this.http.get('/api/wagonReportByDate', { params: { date: JSON.stringify(dateObject) } })
        .success((data: Array<Model.WagonReportModel>) => {
          this.doCallback(successCallback, data);
        })
        .error((data, status) => {
          this.doCallback(errorCallback, data, status);
        });
    }

    getAllDepartureAndDestination(successCallback: Function, errorCallback: Function) {
      this.http.get('/api/allDepartureAndDesination')
        .success((data) => { this.doCallback(successCallback, data) })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }
  }
}
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class ExportService {
    public http: ng.IHttpService;
    public url: string;

    constructor($http: ng.IHttpService) {
      this.http = $http;
    }

    doCallback(callback: Function, data: Object, status: Object) {
      if (callback) {
        callback(data, status);
      }
    }

    exportWagonToExcel(report: Model.WagonModel, successCallback: Function, errorCallback: Function) {
      this.url = '/api/exportWagonToExcel';
      this.http.post(this.url, report)
        .success((data, status) => { this.doCallback(successCallback, data, status) })
        .error((data, status) => { this.doCallback(errorCallback, data, status) });
    }


  }
}
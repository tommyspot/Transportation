/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class WagonService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/wagons';
    }
  }
}
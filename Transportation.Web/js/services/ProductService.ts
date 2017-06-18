/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class ProductService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/products';
    }

    getAllProductInfo(successCallback: Function, errorCallback: Function) {
      this.http.get('/api/productInfos')
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }
  }
}
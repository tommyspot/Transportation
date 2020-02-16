/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class ProductService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/products';
    }

    getProductInfoPerPage(pageIndex, pageSize, searchText: string, successCallback: Function, errorCallback: Function) {
        const search = searchText ? searchText.trim() : '';
        this.http.get(`/api/productInfos/page?pageIndex=${pageIndex}&pageSize=${pageSize}&search=${search}`)
            .success((data) => { this.doCallback(successCallback, data); })
            .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }
  }
}
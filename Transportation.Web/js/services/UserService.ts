/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />

module Clarity.Service {

  export class UserService extends BaseService {

    constructor($http: ng.IHttpService) {
      super($http);
      this.url = '/api/user';
    }

    createAssignee(entity: any, successCallback: Function, errorCallback: Function) {
      this.http.post(this.url + '/assignee', entity)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => {
          this.doCallback(errorCallback, data, status);
        });
    }

    getAllAssignee(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url + '/assignee')
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => {
          this.doCallback(errorCallback, data, status);
        });
    }
  }
}
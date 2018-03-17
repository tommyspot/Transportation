/// <reference path="IService.ts" />
/// <reference path="../model/BaseModel.ts" />
/// <reference path="../../lib/angular/angular.d.ts" />

module Clarity.Service {
  import baseModel = Clarity.Model.BaseModel;

  export class BaseService implements IService {

    public http: ng.IHttpService;
    public url: string;

    constructor($http: ng.IHttpService) {
      this.http = $http;
    }

    doCallback(callback: Function, data: Object, status?) {
      if (callback) {
        if (status != null) {
          callback(data, status);
        }
        else {
          callback(data);
        }
      }
    }

    getById(id: number, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'GET', url: this.url + '/' + id })
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getAll(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getPerPage(pageIndex, pageSize, successCallback: Function, errorCallback: Function) {
      this.http.get(`${this.url}/page?pageIndex=${pageIndex}&pageSize=${pageSize}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    getNumOfPages(pageSize, successCallback: Function, errorCallback: Function) {
      this.http.get(`${this.url}/pageSize/${pageSize}`)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    create(entity: baseModel, successCallback: Function, errorCallback: Function) {
      this.http.post(this.url, entity)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => {
          this.doCallback(errorCallback, data, status);
        });
    }

    update(entity: baseModel, successCallback: Function, errorCallback: Function) {
      this.http.put(this.url + '/' + entity.id, entity)
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    deleteEntity(entity: baseModel, successCallback: Function, errorCallback: Function) {
      this.http({ method: 'DELETE', url: this.url + '/' + entity.id })
        .success((data) => { this.doCallback(successCallback, data); })
        .error((data, status) => { this.doCallback(errorCallback, data, status); });
    }

    createOrUpdate(entity: baseModel, successCallback: Function, errorCallback: Function) {
      if (entity.id) {
        this.update(entity, successCallback, errorCallback);
      } else {
        this.create(entity, successCallback, errorCallback);
      }
    }
  }
}
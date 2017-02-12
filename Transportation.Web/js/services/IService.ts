module Clarity.Service {
  export interface IService {
    doCallback(callback: Function, data: Object);
    getById(id: number, successCallback: Function, errorCallback: Function);
    getAll(successCallback: Function, errorCallback: Function);
    create(data: Object, successCallback: Function, errorCallback: Function);
    update(data: Object, successCallback: Function, errorCallback: Function);
    deleteEntity(data: Object, successCallback: Function, errorCallback: Function);
    createOrUpdate(data: Object, successCallback: Function, errorCallback: Function);
  }
}
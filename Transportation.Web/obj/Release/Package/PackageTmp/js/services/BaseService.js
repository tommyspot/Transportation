/// <reference path="IService.ts" />
/// <reference path="../model/BaseModel.ts" />
/// <reference path="../../lib/angular/angular.d.ts" />
var Clarity;
(function (Clarity) {
    var Service;
    (function (Service) {
        var BaseService = (function () {
            function BaseService($http) {
                this.http = $http;
            }
            BaseService.prototype.doCallback = function (callback, data, status) {
                if (callback) {
                    if (status != null) {
                        callback(data, status);
                    }
                    else {
                        callback(data);
                    }
                }
            };
            BaseService.prototype.getById = function (id, successCallback, errorCallback) {
                var _this = this;
                this.http({ method: 'GET', url: this.url + '/' + id })
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            BaseService.prototype.getAll = function (successCallback, errorCallback) {
                var _this = this;
                this.http.get(this.url)
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            BaseService.prototype.create = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http.post(this.url, entity)
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) {
                    _this.doCallback(errorCallback, data, status);
                });
            };
            BaseService.prototype.update = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http.put(this.url + '/' + entity.id, entity)
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            BaseService.prototype.deleteEntity = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http({ method: 'DELETE', url: this.url + '/' + entity.id })
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            BaseService.prototype.createOrUpdate = function (entity, successCallback, errorCallback) {
                if (entity.id) {
                    this.update(entity, successCallback, errorCallback);
                }
                else {
                    this.create(entity, successCallback, errorCallback);
                }
            };
            return BaseService;
        }());
        Service.BaseService = BaseService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

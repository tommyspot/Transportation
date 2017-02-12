/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Clarity;
(function (Clarity) {
    var Service;
    (function (Service) {
        var TrashService = (function (_super) {
            __extends(TrashService, _super);
            function TrashService($http) {
                _super.call(this, $http);
                this.url = '/api/trashes';
            }
            TrashService.prototype.importCSV = function (fileUpload, successCallback, errorCallback) {
                var _this = this;
                this.http.post('/api/trash/importCSV', fileUpload, { headers: { 'Content-Type': undefined }, transformRequest: angular.identity })
                    .success(function (data) {
                    _this.doCallback(successCallback, data);
                })
                    .error(function (data) {
                    _this.doCallback(errorCallback, data);
                });
            };
            TrashService.prototype.importTrashRecord = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http.post('/api/trash/importTrashRecord', { 'trashes': entity })
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function () { _this.doCallback(errorCallback, null); });
            };
            return TrashService;
        }(Service.BaseService));
        Service.TrashService = TrashService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

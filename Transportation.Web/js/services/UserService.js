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
        var UserService = (function (_super) {
            __extends(UserService, _super);
            function UserService($http) {
                _super.call(this, $http);
                this.url = '/api/user';
            }
            UserService.prototype.createAssignee = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http.post(this.url + '/assignee', entity)
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) {
                    _this.doCallback(errorCallback, data, status);
                });
            };
            UserService.prototype.getAllAssignee = function (successCallback, errorCallback) {
                var _this = this;
                this.http.get(this.url + '/assignee')
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function (data, status) {
                    _this.doCallback(errorCallback, data, status);
                });
            };
            return UserService;
        }(Service.BaseService));
        Service.UserService = UserService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

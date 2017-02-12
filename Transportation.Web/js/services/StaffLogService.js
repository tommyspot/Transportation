var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
var Clarity;
(function (Clarity) {
    var Service;
    (function (Service) {
        var StaffLogService = (function (_super) {
            __extends(StaffLogService, _super);
            function StaffLogService($http) {
                _super.call(this, $http);
                this.url = '/api/stafflog';
            }
            return StaffLogService;
        }(Service.BaseService));
        Service.StaffLogService = StaffLogService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

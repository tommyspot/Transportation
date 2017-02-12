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
        var LocationService = (function (_super) {
            __extends(LocationService, _super);
            function LocationService($http) {
                _super.call(this, $http);
                this.url = '/api/location';
            }
            return LocationService;
        }(Service.BaseService));
        Service.LocationService = LocationService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

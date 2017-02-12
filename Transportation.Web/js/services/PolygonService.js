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
        var PolygonService = (function (_super) {
            __extends(PolygonService, _super);
            function PolygonService($http) {
                _super.call(this, $http);
                this.url = '/api/polygons';
            }
            PolygonService.prototype.importPolygons = function (entity, successCallback, errorCallback) {
                var _this = this;
                this.http.post('/api/importPolygons', { 'polygons': entity })
                    .success(function (data) { _this.doCallback(successCallback, data); })
                    .error(function () { _this.doCallback(errorCallback, null); });
            };
            return PolygonService;
        }(Service.BaseService));
        Service.PolygonService = PolygonService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

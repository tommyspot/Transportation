/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />
var Clarity;
(function (Clarity) {
    var Controller;
    (function (Controller) {
        var MainController = (function () {
            function MainController($scope, $rootScope, $http, $location, $window, $filter) {
                this.$scope = $scope;
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$location = $location;
                this.$window = $window;
                this.$filter = $filter;
                $scope.viewModel = this;
            }
            MainController.prototype.goToWagonsManagement = function () {
                this.$location.path('/ql-toa-hang');
            };
            MainController.prototype.goToGarageManagement = function () {
                this.$location.path('/ql-garage');
            };
            MainController.prototype.goToTruckSection = function () {
                this.$location.path('/ql-toa-hang/xe');
            };
            MainController.prototype.goToEmployeeSection = function () {
                this.$location.path('/ql-toa-hang/nhan-vien');
            };
            MainController.prototype.goToCustomerSection = function () {
                this.$location.path('/ql-toa-hang/khach-hang');
            };
            MainController.prototype.goToWagonsSection = function () {
                this.$location.path('/ql-toa-hang/toa-hang');
            };
            MainController.prototype.goToSettlementSection = function () {
                this.$location.path('/ql-toa-hang/quyet-toan');
            };
            return MainController;
        }());
        Controller.MainController = MainController;
    })(Controller = Clarity.Controller || (Clarity.Controller = {}));
})(Clarity || (Clarity = {}));

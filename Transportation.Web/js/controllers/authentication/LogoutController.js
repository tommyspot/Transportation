/// <reference path="../../services/AuthenticationService.ts" />
/// <reference path="../IController.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />
var Clarity;
(function (Clarity) {
    var Controller;
    (function (Controller) {
        var service = Clarity.Service;
        var LogoutController = (function () {
            function LogoutController($scope, $location, $window, $rootScope, $http, $cookieStore) {
                this.$scope = $scope;
                this.$location = $location;
                this.$window = $window;
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$cookieStore = $cookieStore;
                $scope.viewModel = this;
                this.authenticationService = new service.AuthenticationService($http, $cookieStore);
                this.mainHelper = new Clarity.Helper.MainHelper();
            }
            LogoutController.prototype.submit = function () {
                var _this = this;
                this.$rootScope.showSpinner();
                this.authenticationService.logout(function () { _this.onSubmitSuccess(); }, function () { _this.$rootScope.onError(); });
            };
            LogoutController.prototype.onSubmitSuccess = function () {
                this.$rootScope.clearCache();
                this.$rootScope.hideSpinner();
                this.$location.path('/login');
            };
            return LogoutController;
        }());
        Controller.LogoutController = LogoutController;
    })(Controller = Clarity.Controller || (Clarity.Controller = {}));
})(Clarity || (Clarity = {}));

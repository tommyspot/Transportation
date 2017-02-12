/// <reference path="../../services/AuthenticationService.ts" />
/// <reference path="../../model/UserModel.ts" />
/// <reference path="../IController.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />
/// <reference path="../../../lib/angular/angular-cookies.d.ts" />
var Clarity;
(function (Clarity) {
    var Controller;
    (function (Controller) {
        var service = Clarity.Service;
        var helper = Clarity.Helper;
        var LoginController = (function () {
            function LoginController($scope, $location, $window, $rootScope, $http, $cookieStore) {
                this.$scope = $scope;
                this.$location = $location;
                this.$window = $window;
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$cookieStore = $cookieStore;
                $scope.viewModel = this;
                this.authenticationService = new service.AuthenticationService($http, $cookieStore);
                this.mainHelper = new helper.MainHelper();
                this.errorMessage = '';
            }
            LoginController.prototype.submit = function () {
                var _this = this;
                this.$rootScope.showSpinner();
                this.authenticationService.login(this.user, function (data) { _this.onSubmitSuccess(data); }, function (data, status) { _this.onSubmitError(data, status); });
            };
            LoginController.prototype.onSubmitSuccess = function (user) {
                this.$rootScope.clearCache();
                this.$rootScope.user = user;
                if (this.$rootScope.returnUrl && this.$rootScope.returnUrl != '/login') {
                    this.$location.path(this.$rootScope.returnUrl);
                }
                else {
                    this.$location.path('/');
                }
                this.$rootScope.hideSpinner();
            };
            LoginController.prototype.onSubmitError = function (data, status) {
                switch (status) {
                    case 404:
                        this.errorMessage = 'User does not exist';
                        break;
                    case 409:
                        this.errorMessage = 'Wrong password';
                        break;
                    case 410:
                        this.errorMessage = 'User is disabled';
                        break;
                }
                this.$rootScope.hideSpinner();
            };
            return LoginController;
        }());
        Controller.LoginController = LoginController;
    })(Controller = Clarity.Controller || (Clarity.Controller = {}));
})(Clarity || (Clarity = {}));

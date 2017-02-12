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
            return MainController;
        }());
        Controller.MainController = MainController;
    })(Controller = Clarity.Controller || (Clarity.Controller = {}));
})(Clarity || (Clarity = {}));

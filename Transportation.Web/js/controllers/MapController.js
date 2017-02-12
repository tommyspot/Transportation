/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />
var Clarity;
(function (Clarity) {
    var Controller;
    (function (Controller) {
        var helper = Clarity.Helper;
        var MapController = (function () {
            function MapController($scope, $rootScope, $http, $location, $window, $mdDialog) {
                this.$scope = $scope;
                this.$rootScope = $rootScope;
                this.$http = $http;
                this.$location = $location;
                this.$window = $window;
                this.$mdDialog = $mdDialog;
                $scope.viewModel = this;
                this.trashService = new Clarity.Service.TrashService($http);
                this.userService = new Clarity.Service.UserService($http);
                this.mainHelper = new helper.MainHelper();
                this.selectedTrashInformationList = angular.fromJson(this.$window.sessionStorage.getItem('selectedTrashInfoList'));
                this.selectedTrashInfo = angular.fromJson(this.$window.sessionStorage.getItem('selectedTrashInfo'));
                this.initMarkers();
            }
            MapController.prototype.initMarkers = function () {
                this.markers = [];
                if (this.selectedTrashInformationList != null && this.selectedTrashInformationList.length > 0) {
                    for (var i = 0; i < this.selectedTrashInformationList.length; i++) {
                        var trashInfo = this.selectedTrashInformationList[i];
                        var marker = new Clarity.Model.Marker(trashInfo.id, trashInfo.size, trashInfo.longitude, trashInfo.latitude);
                        this.markers.push(marker);
                    }
                }
            };
            return MapController;
        }());
        Controller.MapController = MapController;
    })(Controller = Clarity.Controller || (Clarity.Controller = {}));
})(Clarity || (Clarity = {}));

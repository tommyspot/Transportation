/// <reference path="../../../lib/jasmine/jasmine.d.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />
/// <reference path="../../../lib/angular-spec/angular-mocks.d.ts" />
/// <reference path="../../../js/controllers/authentication/LogoutController.ts" />
var Clarity;
(function (Clarity) {
    var Spec;
    (function (Spec) {
        describe('LogoutController', function () {
            var logoutController;
            var httpBackend;
            var $scope, $window, $rootScope, $location;
            beforeEach(inject(function ($location, $cookieStore, $http, $rootScope, $window, $httpBackend) {
                $scope = $rootScope.$new();
                httpBackend = $httpBackend;
                $window = $window;
                logoutController = new Clarity.Controller.LogoutController($scope, $location, $window, $rootScope, $http, $cookieStore);
            }));
            describe('.constructor', function () {
                it('defines viewModel', function () {
                    expect($scope.viewModel).toBeDefined();
                });
                it('defines authentication service', function () {
                    expect(logoutController.authenticationService).toBeDefined();
                });
                //it('defines user log service', function () {
                //  expect(logoutController.userLogService).toBeDefined();
                //});
                it('defines mainHelper', function () {
                    expect(logoutController.mainHelper).toBeDefined();
                });
            });
            describe('.submit', function () {
                it('logs out', function () {
                    spyOn(logoutController.authenticationService, 'logout');
                    logoutController.submit();
                    expect(logoutController.authenticationService.logout).toHaveBeenCalled();
                });
            });
            describe('.onSubmitSuccess', function () {
                beforeEach(function () {
                    spyOn(logoutController, 'updateUserLog');
                    spyOn(logoutController.$rootScope, 'clearCache');
                });
                //it('update user log', function () {      
                //  logoutController.onSubmitSuccess();
                //  expect(logoutController.updateUserLog).toHaveBeenCalled();
                //});
                it('redirects to the login page', function () {
                    logoutController.onSubmitSuccess();
                    expect(logoutController.$location.path()).toEqual('/login');
                });
                it('clears cached', function () {
                    logoutController.onSubmitSuccess();
                    expect(logoutController.$rootScope.clearCache).toHaveBeenCalled();
                });
            });
            describe('.updateUserLog', function () {
                var user;
                beforeEach(function () {
                    user = new Clarity.Model.UserModel();
                    user.userLogId = 0;
                });
                //it('get user log needs to update', function () {
                //  spyOn(logoutController.userLogService, 'getById');
                //  logoutController.updateUserLog(user);
                //  expect(logoutController.userLogService.getById).toHaveBeenCalled();
                //});
                //it('get GPS position', function () {
                //  httpBackend
                //    .when('GET', '/api/userlog/0')
                //    .respond({ id: 0, interviewerId: '9999-Test' });
                //  spyOn(navigator.geolocation, 'getCurrentPosition');
                //  logoutController.updateUserLog(user);
                //  httpBackend.flush();
                //  expect(navigator.geolocation.getCurrentPosition).toHaveBeenCalled();
                //});
            });
        });
    })(Spec = Clarity.Spec || (Clarity.Spec = {}));
})(Clarity || (Clarity = {}));

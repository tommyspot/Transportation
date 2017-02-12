/// <reference path="../../../lib/jasmine/jasmine.d.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />
/// <reference path="../../../lib/angular-spec/angular-mocks.d.ts" />
/// <reference path="../../../js/controllers/authentication/LoginController.ts" />

module Clarity.Spec {

  //describe('LoginController', function () {
  //  var loginController: Controller.LoginController;
  //  var httpBackend: ng.IHttpBackendService;
  //  var $scope, $window, $rootScope, $location;

  //  beforeEach(inject(function ($location: ng.ILocationService, $cookieStore: ng.ICookieStoreService, $http: ng.IHttpService, $rootScope: Controller.IRootLoginControllerScope, $window: ng.IWindowService, $httpBackend: ng.IHttpBackendService) {
  //    $scope = $rootScope.$new();
  //    httpBackend = $httpBackend;
  //    $window = $window;
  //    $rootScope = $rootScope;
  //    $location = $location;

  //    loginController = new Controller.LoginController($scope, $location, $window, $rootScope, $http, $cookieStore);
  //  }));
    
  //  describe('.constructor', function () {

  //    it('defines viewModel', function () {
  //      expect($scope.viewModel).toBeDefined();
  //    });

  //    it('defines authentication service', function () {
  //      expect(loginController.authenticationService).toBeDefined();
  //    });

  //    it('defines terminalService service', function () {
  //      expect(loginController.terminalService).toBeDefined();
  //    });

  //    it('defines userService service', function () {
  //      expect(loginController.userService).toBeDefined();
  //    });

  //    it('defines userLogService service', function () {
  //      expect(loginController.userLogService).toBeDefined();
  //    });

  //    it('defines error message', function () {
  //      expect(loginController.errorMessage).toBeDefined();
  //    });
  //  });

  //  describe('.initPage', function () {

  //    it('initializes user from backend', function () {
  //      spyOn(loginController.userService, 'getAll');

  //      loginController.initPage();
  //      expect(loginController.userService.getAll).toHaveBeenCalled();
  //    });

  //    it('initializes termial from backend', function () {
  //      spyOn(loginController.terminalService, 'getAll');

  //      loginController.initPage();
  //      expect(loginController.terminalService.getAll).toHaveBeenCalled();
  //    });

  //    it('initializes noOfCounter list', function () {
  //      loginController.initPage();
  //      expect(loginController.numberOfCounterList.length).toBe(11);
  //    });

  //    it('initializes shift list', function () {
  //      loginController.initPage();
  //      expect(loginController.shiftList.length).toBe(24);
  //    });

  //  });

  //  describe('.onGetTerminalSuccess', function () {
  //    it('initializes terminal list', function () {
  //      loginController.terminalList = null;
  //      var terminals = [new Model.TerminalModel(), new Model.TerminalModel(), new Model.TerminalModel()];

  //      loginController.onGetTerminalSuccess(terminals);
  //      expect(loginController.terminalList.length).toBe(3);
  //    });
  //  });

  //  describe('.onGetUserSuccess', function () {
  //    it('initializes user list', function () {        
  //      var users = [new Model.UserModel(), new Model.UserModel()];
  //      loginController.userList = null;

  //      loginController.onGetUserSuccess(users);
  //      expect(loginController.userList.length).toBe(2);
  //    });
  //  });

  //  describe('.submit', function () {

  //    it('turns on showSpinner', function () {
  //      spyOn(loginController.$rootScope, 'showSpinner');

  //      loginController.submit();
  //      expect(loginController.$rootScope.showSpinner).toHaveBeenCalled();
  //    });

  //    it('gets current position', function () {
  //      spyOn(loginController.$window.navigator.geolocation, 'getCurrentPosition');

  //      loginController.submit();
  //      expect(loginController.$window.navigator.geolocation.getCurrentPosition).toHaveBeenCalled();
  //    });

  //    //it('logs in', function () {
  //    //  var user = new Model.UserModel();
  //    //  user.username = 'username';
  //    //  user.password = 'password';
  //    //  loginController.user = user;
  //    //  spyOn(loginController.authenticationService, 'login');

  //    //  loginController.submit();
  //    //  expect(loginController.authenticationService.login).toHaveBeenCalled();
  //    //});
  //  });

  //  describe('.onSubmitSuccess', function () {
  //    var user: Model.UserModel;

  //    beforeEach(function () {
  //      user = new Model.UserModel();
  //      user.username = 'username';
  //      spyOn(loginController.$rootScope, 'clearCache');
  //    });

  //    it('sets user to the scope', function () {
  //      loginController.onSubmitSuccess(user);
  //      expect($scope.user).toEqual(user);
  //    });

  //    it('creates user log', function () {
  //      spyOn(loginController, 'createUserLog');

  //      loginController.onSubmitSuccess(user);
  //      expect(loginController.createUserLog).toHaveBeenCalled();
  //    });

  //    it('redirects to the home page if there is no return url', function () {
  //      loginController.$rootScope.returnUrl = null;

  //      loginController.onSubmitSuccess(user);
  //      expect(loginController.$location.path()).toEqual('/');
  //    });

  //    it('redirects to the home page if return url is login', function () {
  //      loginController.$rootScope.returnUrl = null;

  //      loginController.onSubmitSuccess(user);
  //      expect(loginController.$location.path()).toEqual('/');
  //    });

  //    it('redirects to return url if exists', function () {
  //      loginController.$rootScope.returnUrl = '/user';

  //      loginController.onSubmitSuccess(user);
  //      expect(loginController.$location.path()).toEqual('/user');
  //    });

  //    it('clears cache', function () {
  //      loginController.onSubmitSuccess(user);
  //      expect(loginController.$rootScope.clearCache).toHaveBeenCalled();
  //    });
  //  });

  //  describe('.onSubmitError', function () {

  //    it('returns User does not exist if status is NotFound', function () {
  //      loginController.onSubmitError(null, 404);
  //      expect(loginController.errorMessage).toEqual('User does not exist');
  //    });

  //    it('returns message that Password is incorrect if status is Conflict but remainingAttemptCount is null', function () {
  //      loginController.onSubmitError(null, 409);
  //      expect(loginController.errorMessage).toEqual('Wrong password');
  //    });

  //    it('returns User is disabled if status is Gone', function () {
  //      loginController.onSubmitError(null, 410);
  //      expect(loginController.errorMessage).toEqual('User is disabled');
  //    });
  //  });

  //  describe('.createUserLog', function () {
  //    var userModel;

  //    beforeEach(function () {
  //      userModel = new Model.UserModel();
  //      userModel.username = 'SuperUser';
  //      userModel.terminalId = 1;
  //      userModel.locationId = 1;
  //      userModel.numberOfCounter = 10;
  //      userModel.shift = 1;

  //      spyOn(loginController.mainHelper, 'formatDateTimeToString').andReturn('2016/08/01 00:34:17');
  //      loginController.latitude = '10.7123206';
  //      loginController.longitude = '106.621156';
  //    });

  //    it('sets interviewerId for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.interviewerId).toBe('SuperUser');
  //    });


  //    it('sets terminalId for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.terminalId).toBe(1);
  //    });

  //    it('sets locationId for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.locationId).toBe(1);
  //    });

  //    it('sets numberOfCounter for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.numberOfCounter).toBe(10);
  //    });

  //    it('sets shift for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.shift).toBe(1);
  //    });

  //    it('sets loginTime for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.loginTime).toBe('2016/08/01 00:34:17');
  //    });

  //    it('sets loginLatitude for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.loginLatitude).toBe('10.7123206');
  //    });

  //    it('sets loginLongitude for new user log', function () {
  //      var userLog: Model.UserLogModel;
  //      spyOn(loginController.userLogService, 'create').andCallFake(
  //        function (userModel) {
  //          userLog = userModel;
  //        });

  //      loginController.createUserLog(userModel);
  //      expect(userLog.loginLongitude).toBe('106.621156');
  //    });

  //    it('creates new user log', function () {
  //      spyOn(loginController.userLogService, 'create');

  //      loginController.createUserLog(userModel);
  //      expect(loginController.userLogService.create).toHaveBeenCalled();
  //    });
  //  });

  //});
}
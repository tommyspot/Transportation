/// <reference path="../../lib/jasmine/jasmine.d.ts" />
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular-spec/angular-mocks.d.ts" />
/// <reference path="../../js/controllers/MainController.ts" />

module Clarity.Spec {

  //describe('MainController', function () {
  //  var controller: Controller.MainController;
  //  var httpBackend: ng.IHttpBackendService;
  //  var $scope;

  //  beforeEach(inject(function ($http: ng.IHttpService, $rootScope: Controller.IRootScope, $httpBackend: ng.IHttpBackendService, $mdDialog: any, $timeout: ng.ITimeoutService, $interval: ng.IIntervalService) {
  //    $scope = $rootScope.$new();
  //    httpBackend = $httpBackend;
  //    controller = new Controller.MainController($scope, $rootScope, $http, $mdDialog);
  //  }));

  //  describe('.constructor', function () {
  //    it('initializes kiosk log service', function () {
  //      expect(controller.kioskLogService).toBeDefined();
  //    });

  //    it('initializes location service', function () {
  //      expect(controller.locationService).toBeDefined();
  //    });

  //    it('initializes main helper', function () {
  //      expect(controller.mainHelper).toBeDefined();
  //    });
  //  });

  //  describe('.initPage', function () {
  //    it('initalizes Lat', function () {
  //      controller.initPage();
  //      expect(controller.latitude).toBe('');
  //    });

  //    it('initalizes Long', function () {
  //      controller.initPage();
  //      expect(controller.longitude).toBe('');
  //    });

  //    it('initalizes numberOfReceipts', function () {
  //      controller.initPage();
  //      expect(controller.numberOfReceipts.length).toBe(11);
  //    });

  //    it('initalizes numberOfPaxs', function () {
  //      controller.initPage();
  //      expect(controller.numberOfPaxs.length).toBe(21);
  //    });

  //    it('initalizes defaultColors', function () {
  //      controller.initPage();
  //      expect(controller.defaultColors.length).toBe(11);
  //    });
  //  });

  //  describe('.initKioskLogs', function () {
  //    it('adds 6 items', function () {
  //      controller.initKioskLogs();
  //      expect(controller.kioskLogs.length).toBe(6);
  //    });

  //    it('initalizes queue type is Counter(0) for each kiosk log', function () {
  //      controller.initKioskLogs();
  //      expect(controller.kioskLogs[0].queueType).toBe(0);
  //      expect(controller.kioskLogs[5].queueType).toBe(0);
  //    });

  //    it('initalizes gender is Female(0) for each kiosk log', function () {
  //      controller.initKioskLogs();
  //      expect(controller.kioskLogs[0].gender).toBe(0);
  //      expect(controller.kioskLogs[5].gender).toBe(0);
  //    });

  //    it('initalizes isAbandon is false for each kiosk log', function () {
  //      controller.initKioskLogs();
  //      expect(controller.kioskLogs[0].isAbandon).toBeFalsy();
  //      expect(controller.kioskLogs[5].isAbandon).toBeFalsy();
  //    });

  //    it('initalizes color is #FFFFFF for each kiosk log', function () {
  //      controller.initKioskLogs();
  //      expect(controller.kioskLogs[0].color).toBe('#FFFFFF');
  //      expect(controller.kioskLogs[5].color).toBe('#FFFFFF');
  //    });
  //  });

  //  describe('.initLocation', function () {
  //    it('call getAll function in location service', function () {
  //      spyOn(controller.locationService, 'getAll');

  //      controller.initLocation();
  //      expect(controller.locationService.getAll).toHaveBeenCalled();
  //    });

  //    it('initializes locationName variable', function () {
  //      httpBackend
  //        .when('GET', '/api/location')
  //        .respond([{ id: 0, name: 'Publish' }, { id: 1, name: 'Transit' }]);
  //      controller.$rootScope.user = new Model.UserModel();
  //      controller.$rootScope.user.locationId = 0;

  //      controller.initLocation();
  //      httpBackend.flush();
  //      expect(controller.locationName).toBe('Publish');
  //    });

  //  });

  //  describe('.submitKioskStartLog', function () {
  //    it('logs time when kiosk was started', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      var currentTime = controller.mainHelper.getCurrentDateTimeString();

  //      controller.submitKioskStartLog(kioskLog);
  //      expect(kioskLog.startTime).toEqual(currentTime);
  //    });
  //  });

  //  describe('.submitKioskEnterLog', function () {
  //    it('logs time when kiosk was enter', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      var currentTime = controller.mainHelper.getCurrentDateTimeString();

  //      controller.submitKioskEnterLog(kioskLog);
  //      expect(kioskLog.enterTime).toEqual(currentTime);
  //    });
  //  });

  //  describe('.submitKioskLeaveLog', function () {
  //    it('logs time when kiosk was left', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      var currentTime = controller.mainHelper.getCurrentDateTimeString();

  //      controller.submitKioskLeaveLog(kioskLog);
  //      expect(kioskLog.leaveTime).toEqual(currentTime);
  //    });
  //  });

  //  describe('.isKioskStartStarted', function () {
  //    it('returns false if start time is null', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.startTime = null;

  //      var result = controller.isKioskStartStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if start time is empty', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.startTime = '';

  //      var result = controller.isKioskStartStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns true if start time is valid', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.startTime = '12:30:00';

  //      var result = controller.isKioskStartStarted(kioskLog);
  //      expect(result).toBeTruthy();
  //    });
  //  });

  //  describe('.isKioskEnterStarted', function () {
  //    it('returns false if enter time is null', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.enterTime = null;

  //      var result = controller.isKioskEnterStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if enter time is empty', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.enterTime = '';

  //      var result = controller.isKioskEnterStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns true if enter time is valid', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.enterTime = '12:30:00';

  //      var result = controller.isKioskEnterStarted(kioskLog);
  //      expect(result).toBeTruthy();
  //    });
  //  });

  //  describe('.isKioskLeaveStarted', function () {
  //    it('returns false if leave time is null', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.leaveTime = null;

  //      var result = controller.isKioskLeaveStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if leave time is empty', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.leaveTime = '';

  //      var result = controller.isKioskLeaveStarted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns true if leave time is valid', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.leaveTime = '12:30:00';

  //      var result = controller.isKioskLeaveStarted(kioskLog);
  //      expect(result).toBeTruthy();
  //    });
  //  });

  //  describe('.enableSubmitKioskLog', function () {
  //    var kioskLog: Model.KioskLogModel;
  //    beforeEach(function () {
  //      kioskLog = new Model.KioskLogModel();
  //      kioskLog.id = 1;
  //      kioskLog.startTime = '12:30:00';
  //      kioskLog.enterTime = '12:40:00';
  //      kioskLog.leaveTime = '12:50:00';
  //      kioskLog.interviewerId = 'user1';
  //      kioskLog.terminalId = 1;
  //      kioskLog.numberOfReceipt = 4;
  //      kioskLog.numberOfPax = 11;
  //      kioskLog.queueType = 0;
  //      kioskLog.gender = 0;
  //      kioskLog.color = 'FF0000';
  //      kioskLog.remark1 = 'test1';
  //      kioskLog.remark2 = 'test2';
  //      kioskLog.abandonReason = 'test3';
  //      kioskLog.isAbandon = false;
  //    });

  //    it('returns true if kiosk log abandon and reason', function () {
  //      kioskLog.isAbandon = true;
  //      kioskLog.abandonReason = 'I do not like it.';

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeTruthy();
  //    });

  //    it('returns false if kiosk log abandon and does not have reason', function () {
  //      kioskLog.isAbandon = true;
  //      kioskLog.abandonReason = null;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have start time', function () {
  //      kioskLog.startTime = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have enter time', function () {
  //      kioskLog.enterTime = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have leave time', function () {
  //      kioskLog.leaveTime = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have number of receipt', function () {
  //      kioskLog.numberOfReceipt = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });
      
  //    it('returns false if kiosk log does not have number of pax', function () {
  //      kioskLog.numberOfPax = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have queue type', function () {
  //      kioskLog.queueType = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have gender', function () {
  //      kioskLog.gender = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if kiosk log does not have color', function () {
  //      kioskLog.color = undefined;

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns true if kiosk log has full information', function () {

  //      var result = controller.enableSubmitKioskLog(kioskLog);
  //      expect(result).toBeTruthy();
  //    });
  //  });

  //  describe('.submitKioskLog', function () {
  //    var kioskLog: Model.KioskLogModel;

  //    beforeEach(function () {
  //      kioskLog = new Model.KioskLogModel();
  //      var user = new Model.UserModel();
  //      user.terminalId = 1;
  //      user.locationId = 1;
  //      user.username = '9999-Test';
  //      user.numberOfCounter = 5;
  //      user.shift = 1;

  //      controller.$rootScope.user = user;
  //      controller.latitude = '10.76414515063937';
  //      controller.longitude = '106.70523849427879';
  //      controller.geoRequestedTime = '2016/07/27 10:55:00';
  //      spyOn(controller.mainHelper, 'getCurrentDateTimeString').andReturn('2016/07/27 10:55:09');
  //    });

  //    it('assigns finishedTime for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.finishedTime).toBe('2016/07/27 10:55:09');
  //    });

  //    it('assigns terminalId for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.terminalId).toBe(1);
  //    });

  //    it('assigns locationId for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.locationId).toBe(1);
  //    });

  //    it('assigns interviewerId for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.interviewerId).toBe('9999-Test');
  //    });

  //    it('assigns numberOfCounter for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.numberOfCounter).toBe(5);
  //    });

  //    it('assigns shift for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.shift).toBe(1);
  //    });

  //    it('assigns latitude for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.latitude).toBe('10.76414515063937');
  //    });

  //    it('assigns longitude for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.longitude).toBe('106.70523849427879');
  //    });

  //    it('assigns geoRequestedTime for kiosk log', function () {
  //      var log: Model.KioskLogModel;
  //      spyOn(controller.kioskLogService, 'create').andCallFake(function (kioskLog) {
  //        log = kioskLog;
  //      });

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(log.geoRequestedTime).toBe('2016/07/27 10:55:00');
  //    });

  //    it('creates a new kiosk log from kiosk log service', function () {
  //      spyOn(controller.kioskLogService, 'create');

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(controller.kioskLogService.create).toHaveBeenCalled();
  //    });

  //    it('deletes this kiosk log', function () {
  //      httpBackend
  //        .when('POST', '/api/kiosklog')
  //        .respond({});
  //      httpBackend
  //        .when('GET', '/api/location')
  //        .respond([{ id: 0, name: 'Publish' }, { id: 1, name: 'Transit' }]);
  //      spyOn(controller, 'deleteKioskLog');

  //      controller.submitKioskLog(kioskLog, 0);
  //      httpBackend.flush();
  //      expect(controller.deleteKioskLog).toHaveBeenCalled();
  //    });

  //    it('resets GPS location', function () {
  //      spyOn(controller, 'getGPSLocation');

  //      controller.submitKioskLog(kioskLog, 0);
  //      expect(controller.getGPSLocation).toHaveBeenCalled();
  //    });

  //  });

  //  describe('.deleteKioskLog', function () {
  //    var kioskLog1, kioskLog2, kioskLog3;

  //    beforeEach(function () {
  //      kioskLog1 = new Model.KioskLogModel();
  //      kioskLog1.Id = 1;
  //      kioskLog2 = new Model.KioskLogModel();
  //      kioskLog2.Id = 2;
  //      kioskLog3 = new Model.KioskLogModel();
  //      kioskLog3.Id = 3;

  //      controller.kioskLogs = [kioskLog1, kioskLog2, kioskLog3];
  //      spyOn(controller, 'createKioskLog');
  //    });

  //    it('deletes first element', function () {
  //      controller.deleteKioskLog(0);
  //      expect(controller.kioskLogs.length).toBe(2);
  //      expect(controller.kioskLogs[0]).toBe(kioskLog2);
  //      expect(controller.kioskLogs[1]).toBe(kioskLog3);
  //    });

  //    it('deletes any element', function () {
  //      controller.deleteKioskLog(1);
  //      expect(controller.kioskLogs.length).toBe(2);
  //      expect(controller.kioskLogs[0]).toBe(kioskLog1);
  //      expect(controller.kioskLogs[1]).toBe(kioskLog3);
  //    });

  //    it('deletes last element', function () {
  //      controller.deleteKioskLog(2);
  //      expect(controller.kioskLogs.length).toBe(2);
  //      expect(controller.kioskLogs[0]).toBe(kioskLog1);
  //      expect(controller.kioskLogs[1]).toBe(kioskLog2);
  //    });

  //    it('call create a new carlog function', function () {
  //      controller.deleteKioskLog(0);
  //      expect(controller.createKioskLog).toHaveBeenCalled();
  //    });
  //  });

  //  describe('.createKioskLog', function () {
  //    var kioskLog1, kioskLog2, kioskLog3;

  //    beforeEach(function () {
  //      kioskLog1 = new Model.KioskLogModel();
  //      kioskLog2 = new Model.KioskLogModel();
  //      kioskLog3 = new Model.KioskLogModel();
  //      controller.kioskLogs = [kioskLog1, kioskLog2, kioskLog3];
  //    });

  //    it('add a new kiosk log', function () {
  //      controller.createKioskLog();
  //      expect(controller.kioskLogs.length).toBe(4);
  //    });

  //    it('add a new car log if carLogs list is null', function () {
  //      controller.kioskLogs = null;

  //      controller.createKioskLog();
  //      expect(controller.kioskLogs.length).toBe(1);
  //    });

  //    it('initalizes queue type is Counter(0) for new kiosk log', function () {
  //      controller.createKioskLog();
  //      expect(controller.kioskLogs[3].queueType).toBe(0);
  //    });

  //    it('initalizes gender is Female(0) for new kiosk log', function () {
  //      controller.createKioskLog();
  //      expect(controller.kioskLogs[3].gender).toBe(0);
  //    });

  //    it('initalizes isAbandon is false for new kiosk log', function () {
  //      controller.createKioskLog();
  //      expect(controller.kioskLogs[3].isAbandon).toBeFalsy();
  //    });

  //    it('initalizes color is #ffffff for new kiosk log', function () {
  //      controller.createKioskLog();
  //      expect(controller.kioskLogs[3].color).toBe('#ffffff');
  //    });
  //  });

  //  describe('.getGPSLocation', function () {
  //    it('gets current position from navigator service', function () {
  //      spyOn(navigator.geolocation, 'getCurrentPosition');

  //      controller.getGPSLocation();
  //      expect(navigator.geolocation.getCurrentPosition).toHaveBeenCalled();
  //    });
  //  });

  //  describe('.onGetLocationSuccess', function () {
  //    var position;

  //    beforeEach(function () {
  //      position = { 'coords': { 'latitude': '10.76414515063937', 'longitude': '106.70523849427879'}};
  //      spyOn(controller.mainHelper, 'getCurrentDateTimeString').andReturn('2016/07/27 10:55:00');
  //    });

  //    it('sets latitude', function () {
  //      controller.onGetLocationSuccess(position);
  //      expect(controller.latitude).toBe('10.76414515063937');
  //    });

  //    it('sets longitude', function () {
  //      controller.onGetLocationSuccess(position);
  //      expect(controller.longitude).toBe('106.70523849427879');
  //    });

  //    it('sets geoRequestedTime', function () {
  //      controller.onGetLocationSuccess(position);
  //      expect(controller.geoRequestedTime).toBe('2016/07/27 10:55:00');
  //    });
  //  });

  //  describe('.onGetLocationFail', function () {

  //    beforeEach(function () {
  //      spyOn(controller.mainHelper, 'getCurrentDateTimeString').andReturn('2016/07/27 10:55:00');
  //    });

  //    it('sets latitude is empty', function () {
  //      controller.onGetLocationFail(null);
  //      expect(controller.latitude).toBe('');
  //    });

  //    it('sets longitude', function () {
  //      controller.onGetLocationFail(null);
  //      expect(controller.longitude).toBe('');
  //    });

  //    it('sets geoRequestedTime', function () {
  //      controller.onGetLocationFail(null);
  //      expect(controller.geoRequestedTime).toBe('2016/07/27 10:55:00');
  //    });
  //  });

  //  describe('.onIsAbandonChange', function () {
  //    var kioskLog: Model.KioskLogModel;

  //    beforeEach(function () {
  //      kioskLog = new Model.KioskLogModel();
  //      kioskLog.abandonReason = 'I do not like it';
  //      kioskLog.isAbandon = true;
  //    });

  //    it('sets abandon reason is empty when uncheck abandon', function () {
  //      kioskLog.isAbandon = false;

  //      controller.onIsAbandonChange(kioskLog);
  //      expect(kioskLog.abandonReason).toBe('');
  //    });
  //  });

  //  describe('.isKioskSubmitted', function () {
  //    it('returns false if finished time is null', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.finishedTime = null;

  //      var result = controller.isKioskSubmitted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns false if finished time is empty', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.finishedTime = '';

  //      var result = controller.isKioskSubmitted(kioskLog);
  //      expect(result).toBeFalsy();
  //    });

  //    it('returns true if finished time is valid', function () {
  //      var kioskLog = new Model.KioskLogModel();
  //      kioskLog.finishedTime = '12:30:00';

  //      var result = controller.isKioskSubmitted(kioskLog);
  //      expect(result).toBeTruthy();
  //    });
  //  });

  //});
}
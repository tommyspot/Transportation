/// <reference path="../../lib/jasmine/jasmine.d.ts" />
/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular-spec/angular-mocks.d.ts" />

describe('DataBatchService', function () {

  var httpBackend: ng.IHttpBackendService;
  var rootScope: Clarity.Controller.IRootScope;
  var http: ng.IHttpService;
  var service: Clarity.Service.DataBatchService;
  var callbacks;

  beforeEach(inject(function ($http: ng.IHttpService, $rootScope: Clarity.Controller.IRootScope, $httpBackend: ng.IHttpBackendService) {
    http = $http;
    rootScope = $rootScope;
    httpBackend = $httpBackend;

    service = new Clarity.Service.DataBatchService($http, $rootScope);

    callbacks = {
      success: function () { },
      error: function () { }
    };
  }));

  describe('.getByDataset', function () {
    it('executes callback on success', function () {
      spyOn(callbacks, "success");
      httpBackend
        .when('GET', '/api/datasets/1/databatches')
        .respond([{}]);
      service.getByDataset(1, callbacks.success, null);

      httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error");
      httpBackend
        .when('GET', '/api/datasets/1/databatches')
        .respond(500, '');

      service.getByDataset(1, null, callbacks.error);
      httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.deleteEntity', function () {
    var batch;

    beforeEach(function () {
      batch = { id: 1 };
    });

    it('deletes batch on the backend', function () {
      service.deleteEntity(batch, null, null);
      httpBackend
        .expectDELETE('/api/databatches/1')
        .respond({});
      httpBackend.flush();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, 'success');

      service.deleteEntity(batch, callbacks.success, null);
      httpBackend
        .when('DELETE', '/api/databatches/1')
        .respond({});
      httpBackend.flush();

      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, 'error');

      service.deleteEntity(batch, null, callbacks.error);
      httpBackend
        .when('DELETE', '/api/databatches/1')
        .respond(500, '');
      httpBackend.flush();

      expect(callbacks.error).toHaveBeenCalled();
    });
  });
});
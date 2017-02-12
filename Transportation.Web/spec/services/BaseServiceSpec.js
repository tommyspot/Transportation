describe('BaseService', function () {

  var $httpBackend, service, $scope, callbacks, $http;

  beforeEach(inject(function ($injector) {
    $httpBackend = $injector.get('$httpBackend');
    $http = $injector.get('$http');

    service = new Clarity.Service.BaseService($http);
    service.url = '/api/roles';
    $scope = {};

    callbacks = {
      success: function () { },
      error: function () { }
    };
  }));

  describe('.getAll', function () {
    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .when('GET', '/api/roles')
        .respond();
      service.getAll(callbacks.success, null);
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error").andReturn();
      $httpBackend
        .when('GET', '/api/roles')
        .respond(500, '');
      service.getAll(null, callbacks.error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.get', function () {
    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .when('GET', '/api/roles/1')
        .respond();
      service.getById(1, callbacks.success, null);
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error").andReturn();
      $httpBackend
        .when('GET', '/api/roles/1')
        .respond(500, '');
      service.getById(1, null, callbacks.error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.create', function () {
    beforeEach(function () {
      $scope.formData = { name: 'Create Dashboard' };
    });

    it('posts role data to the backend', function () {
      service.create($scope.formData);
      $httpBackend
        .expectPOST('/api/roles', $scope.formData)
        .respond();
      $httpBackend.flush();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();

      service.create($scope.formData, callbacks.success);
      $httpBackend
        .whenPOST('/api/roles', $scope.formData)
        .respond();
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      var error = { error: 'Role name exists' };
      spyOn(callbacks, "error").andReturn();

      service.create($scope.formData, null, callbacks.error);
      $httpBackend
        .whenPOST('/api/roles', $scope.formData)
        .respond(500, error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalledWith(error, 500);
    });
  });

  describe('.update', function () {
    beforeEach(function () {
      $scope.formData = { name: 'Edit Dashboard', id: 1 };
    });

    it('puts role data to the backend', function () {
      service.update($scope.formData);
      $httpBackend
        .expectPUT('/api/roles/' + $scope.formData.id, $scope.formData)
        .respond();
      $httpBackend.flush();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();

      service.update($scope.formData, callbacks.success);
      $httpBackend
        .whenPUT('/api/roles/' + $scope.formData.id, $scope.formData)
        .respond();
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      var error = { error: 'Role name exists' };
      spyOn(callbacks, "error").andReturn();

      service.update($scope.formData, null, callbacks.error);
      $httpBackend
        .whenPUT('/api/roles/' + $scope.formData.id, $scope.formData)
        .respond(500, error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalledWith(error, 500);
    });
  });

  describe('.deleteEntity', function () {
    var role;

    beforeEach(function () {
      role = { id: 1, name: 'Create dashboard' };
    });

    it('deletes role on the backend', function () {
      service.deleteEntity(role);
      $httpBackend
        .expectDELETE('/api/roles/1')
        .respond();
      $httpBackend.flush();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();

      service.deleteEntity(role, callbacks.success);
      $httpBackend
        .whenDELETE('/api/roles/1')
        .respond();
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error").andReturn();

      service.deleteEntity(role, null, callbacks.error);
      $httpBackend
        .whenDELETE('/api/roles/1')
        .respond(500, '');
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.createOrUpdate', function () {
    it('creates when data does not contain an id', function () {
      spyOn(service, 'create').andReturn();
      $scope.formData = {};
      service.createOrUpdate($scope.formData, callbacks.success, callbacks.error);
      expect(service.create).toHaveBeenCalledWith($scope.formData, callbacks.success, callbacks.error);
    });

    it('updates when data does contain an id', function () {
      spyOn(service, 'update').andReturn();
      $scope.formData = { id: 1 };
      service.createOrUpdate($scope.formData, callbacks.success, callbacks.error);
      expect(service.update).toHaveBeenCalledWith($scope.formData, callbacks.success, callbacks.error);
    });
  });

});
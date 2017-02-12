describe('AuthenticationService', function () {
  var $httpBackend, service, $scope, callbacks, $http, $cookieStore;

  beforeEach(inject(function ($injector) {
    $httpBackend = $injector.get('$httpBackend');
    $http = $injector.get('$http');
    $cookieStore = $injector.get('$cookieStore');

    service = new Clarity.Service.AuthenticationService($http, $cookieStore);
    $scope = {};

    callbacks = {
      success: function () { },
      error: function () { }
    };
  }));

  describe('.login', function () {
    var user = { username: 'myUsername', password: 'myPassword' };

    it('executes login from backend', function () {
      $httpBackend
        .expect('POST', '/api/auth/login', user)
        .respond(200, [{ username: 'username' }, 1809000]);

      service.login(user, callbacks.success, null);
      $httpBackend.flush();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .when('POST', '/api/auth/login', user)
        .respond(200, [{ username: 'username' }, 1809000]);

      service.login(user, callbacks.success, null);
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('updates current user on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .when('POST', '/api/auth/login', user)
        .respond(200, [{ username: 'username' }, 1809000]);

      service.login(user, null, null);
      $httpBackend.flush();
      expect(service.isAuthenticated()).toBeTruthy();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error").andReturn();
      $httpBackend
        .when('POST', '/api/auth/login', user)
        .respond(500, '');

      service.login(user, null, callbacks.error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.logout', function () {
    var user = { username: 'myUsername', password: 'myPassword' };

    it('executes logout from backend', function () {
      $httpBackend
        .expect('GET', '/api/auth/logout')
        .respond();
      service.logout(callbacks.success, null);
      $httpBackend.flush();
    });

    it('clears user login status on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .expect('GET', '/api/auth/logout')
        .respond();
      service.logout(callbacks.success, null);
      $httpBackend.flush();
      expect(service.isAuthenticated()).toBeFalsy();
    });

    it('executes callback on success', function () {
      spyOn(callbacks, "success").andReturn();
      $httpBackend
        .expect('GET', '/api/auth/logout')
        .respond();
      service.logout(callbacks.success, null);
      $httpBackend.flush();
      expect(callbacks.success).toHaveBeenCalled();
    });

    it('executes callback on error', function () {
      spyOn(callbacks, "error").andReturn();
      $httpBackend
        .expect('GET', '/api/auth/logout')
        .respond(500, '');
      service.logout(null, callbacks.error);
      $httpBackend.flush();
      expect(callbacks.error).toHaveBeenCalled();
    });
  });

  describe('.getUserName', function () {

    it('returns user name if current user is logged in', function () {
      var currentUser = { username: 'username', isAuthenticated: true };
      $cookieStore.put('user', currentUser);
      var username = service.getUserName();
      expect(username).toBe('username');
    });

    it('returns empty if current user is not logged in', function () {
      $cookieStore.remove('user');
      var username = service.getUserName();
      expect(username).toBe('');
    });

  });

  describe('.getUser', function () {

    it('returns user if current user is logged in', function () {
      var currentUser = { username: 'username', isAuthenticated: true };
      $cookieStore.put('user', currentUser);
      var user = service.getUser();
      expect(user).toEqual(currentUser);
    });

    it('returns null if current user is not logged in', function () {
      $cookieStore.remove('user');
      var user = service.getUser();
      expect(user).toEqual(null);
    });

  });

  describe('.isAuthenticated', function () {

    it('returns true if current user is logged in', function () {
      var currentUser = { username: 'username', isAuthenticated: true };
      $cookieStore.put('user', currentUser);
      var isAuthenticated = service.isAuthenticated();
      expect(isAuthenticated).toBeTruthy();
    });

    it('returns false if current user is not logged in', function () {
      var currentUser = { username: 'username', isAuthenticated: false };
      $cookieStore.put('user', currentUser);
      var isAuthenticated = service.isAuthenticated();
      expect(isAuthenticated).toBeFalsy();
    });

    it('returns false if current user is null', function () {
      $cookieStore.remove('user');
      var isAuthenticated = service.isAuthenticated();
      expect(isAuthenticated).toBeFalsy();
    });
  });
});
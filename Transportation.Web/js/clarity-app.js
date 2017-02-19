
angular.element(document).ready(function () {
  angular.bootstrap(document.documentElement, ['clarityApp']);
});

// using this to improve performance if angularJS 1.2
if (jQuery) {
  var originalFn = $.fn.data;
  $.fn.data = function () {
    if (arguments[0] !== '$binding')
      return originalFn.apply(this, arguments);
  }
}

var clarityApp = angular.module('clarityApp', ['ngCookies', 'ngRoute', 'ui.bootstrap', 'ui.select2', 'ui.sortable'], function ($routeProvider, $httpProvider) {
  // --- Routes ---
  $routeProvider
    .when('/', {
      templateUrl: '/html/index.html' + '?v=' + VERSION_NUMBER,
      controller: 'MainController',
      access: 'authorized'
    })
    .when('/login', {
      templateUrl: '/html/login.html' + '?v=' + VERSION_NUMBER,
      controller: 'LoginController',
      access: 'public'
    })
    .when('/ql-toa-hang', {
      templateUrl: '/html/toahang/ql-toa-hang.html' + '?v=' + VERSION_NUMBER,
      controller: 'MainController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/xe', {
      templateUrl: '/html/toahang/xe.html' + '?v=' + VERSION_NUMBER,
      controller: 'TruckManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/nhan-vien', {
      templateUrl: '/html/toahang/nhan-vien.html' + '?v=' + VERSION_NUMBER,
      controller: 'EmployeeManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/nhan-vien/tao', {
      templateUrl: '/html/toahang/nhan-vien-form.html' + '?v=' + VERSION_NUMBER,
      controller: 'EmployeeManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/nhan-vien/:employee_id', {
      templateUrl: '/html/toahang/nhan-vien-detail.html' + '?v=' + VERSION_NUMBER,
    	controller: 'EmployeeManagementController',
    	access: 'authorized'
    })
    .when('/ql-toa-hang/nhan-vien/sua/:employee_id', {
      templateUrl: '/html/toahang/nhan-vien-form.html' + '?v=' + VERSION_NUMBER,
      controller: 'EmployeeManagementController',
      access: 'authorized'
    })
		.when('/ql-toa-hang/xe/tao', {
			templateUrl: '/html/toahang/xe-form.html' + '?v=' + VERSION_NUMBER,
			controller: 'TruckManagementController',
			access: 'authorized'
		})
		.when('/ql-toa-hang/xe/:employee_id', {
			templateUrl: '/html/toahang/xe-detail.html' + '?v=' + VERSION_NUMBER,
			controller: 'TruckManagementController',
			access: 'authorized'
		})
		.when('/ql-toa-hang/xe/sua/:employee_id', {
			templateUrl: '/html/toahang/xe-form.html' + '?v=' + VERSION_NUMBER,
			controller: 'TruckManagementController',
			access: 'authorized'
		})
    .when('/ql-toa-hang/toa-hang', {
      templateUrl: '/html/toahang/toa-hang.html' + '?v=' + VERSION_NUMBER,
      controller: 'WagonManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/toa-hang/tao', {
      templateUrl: '/html/toahang/toa-hang-form.html' + '?v=' + VERSION_NUMBER,
      controller: 'WagonManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/toa-hang/sua/:wagon_id', {
      templateUrl: '/html/toahang/toa-hang-form.html' + '?v=' + VERSION_NUMBER,
      controller: 'WagonManagementController',
      access: 'authorized'
    })
    .when('/ql-toa-hang/toa-hang/:wagon_id', {
      templateUrl: '/html/toahang/toa-hang-detail.html' + '?v=' + VERSION_NUMBER,
      controller: 'WagonManagementController',
      access: 'authorized'
    })

    .when('/not_found', {
      templateUrl: '/html/not-found.html' + '?v=' + VERSION_NUMBER,
      controller: '',
      access: 'public'
    })
    .when('/error', {
      templateUrl: '/html/error.html' + '?v=' + VERSION_NUMBER,
      controller: '',
      access: 'public'
    })
    .when('/access_denied', {
      templateUrl: '/html/access-denied.html' + '?v=' + VERSION_NUMBER,
      controller: '',
      access: 'public'
    })
    .when('/not_authorized', {
      templateUrl: '/html/not-authorized.html' + '?v=' + VERSION_NUMBER,
      controller: '',
      access: 'share'
    })
    .otherwise({ redirectTo: '/' });
});


clarityApp.config(function ($controllerProvider, $provide, $compileProvider, $httpProvider) {
  clarityApp._controller = clarityApp.controller;

  clarityApp.controller = function (name, constructor) {
    $controllerProvider.register(name, constructor);
    return (this);
  };

  $httpProvider.interceptors.push('requestInterceptor');
}).factory('requestInterceptor', function ($q, $rootScope, $location) {
  $rootScope.pendingRequests = 0;

  return {
    'request': function (config) {
      $rootScope.disableElements();
      $rootScope.pendingRequests++;

      return config || $q.when(config);
    },

    'requestError': function (rejection) {
      $rootScope.pendingRequests--;
      $rootScope.enableElements();

      return $q.reject(rejection);
    },

    'response': function (response) {
      $rootScope.error = '';
      $rootScope.pendingRequests--;
      $rootScope.enableElements();

      return response || $q.when(response);
    },

    'responseError': function (rejection) {
      if (rejection.status === 401 || rejection.status === 403) {
        $rootScope.returnUrl = $location.path();
        $location.path('/login');
      }
      else if (rejection.status === 404) {
        if (rejection.config.url.indexOf('/api/') == 0) {
          $rootScope.error = 'not_found';
        }
      }
      else if (rejection.status === 500) {
        $rootScope.error = 'server_error';
      }

      return $q.reject(rejection);
    }
  }
});


clarityApp.directive('convertToNumber', function () {
  return {
    require: 'ngModel',
    link: function (scope, element, attrs, ngModel) {
      ngModel.$parsers.push(function (val) {
        return parseInt(val, 10);
      });
      ngModel.$formatters.push(function (val) {
        if (val === undefined)
          return '';
        return '' + val;
      });
    }
  };
});

clarityApp.run(function ($rootScope, $routeParams, $location, authenticationService, $http, $cookieStore, $window) {

    $rootScope.$on("$routeChangeStart", function (event, next, current) {
        $rootScope.error = null;

        if (next.access != 'public' && next.access != 'share' && !authenticationService.isAuthenticated()) {
            $rootScope.returnUrl = $location.path();
            $location.path('/login');
        }
        else {
            $rootScope.user = $cookieStore.get('user');
        }

        $rootScope.locationPath = $location.path();
        $rootScope.initBreadcrumbs();
    });

    $rootScope.onError = function (error) {
        if (error == null || error == '') {
            if ($rootScope.error === 'not_found') {
                $location.path('/not_found');
            } else if ($rootScope.error === 'server_error') {
            }
        }

        $rootScope.error = error;

        if ($location.path() == "/login" || $location.path() == "/not_found") {
            var e = jQuery.Event("keydown");
            e.which = 27;
            $("input").trigger(e);
        }

        $rootScope.hideSpinner();
    };

    $rootScope.getError = function () {
        return $rootScope.error;
    };

    $rootScope.showSpinner = function () {
        $("#bg-preload").show();
    };

    $rootScope.hideSpinner = function () {
        $("#bg-preload").hide();
    };

    $rootScope.clearCache = function ($window) {
        $rootScope.user = null;
    }

    $rootScope.enableElements = function () {
        if ($rootScope.pendingRequests < 1) {
            $('.wait-data-loading').removeAttr('disabled');
        }
    }

    $rootScope.disableElements = function () {
        $('.wait-data-loading').attr('disabled', 'disabled');
    }

    $rootScope.initBreadcrumbs = function () {
      $rootScope.breadCrumbs = [];
      $rootScope.breadCrumbs.push({ 'name': 'Trang chủ', 'path': '/' });

      var path = $rootScope.locationPath;
      if (path) {
        var paths = path.split('/');
        paths.splice(0, 1);

        var navigations = [];
        var pathTemp = '';

        //proccess path
        for (var index = 0; index < paths.length; index++) {
          pathTemp += index > 0 ? '/' + paths[index - 1] : '';
          var fullPath = pathTemp + '/' + paths[index];
          navigations.push(fullPath);
        }

        for (var index = 0; index < navigations.length; index++) {
          var nav = navigations[index];
          if (nav !== '/') {
            var breadCrumb = {};
            breadCrumb.path = nav;

            switch(nav) {
              case '/ql-toa-hang':
                breadCrumb.name = 'Quản lý toa hàng';
                break;
              case '/ql-toa-hang/nhan-vien':
                breadCrumb.name = 'Nhân viên';
                break;
              case '/ql-toa-hang/xe':
                breadCrumb.name = 'Xe';
                break;
              case '/ql-toa-hang/nhan-vien/tao':
                breadCrumb.name = 'Tạo mới';
                break;
              case '/ql-toa-hang/nhan-vien/sua':
                breadCrumb.name = 'Sửa';
                break;
              case '/ql-garage':
                breadCrumb.name = 'Quản lý garage';
                break;
              case '/ql-toa-hang/toa-hang':
                breadCrumb.name = 'Toa Hàng';
                break;
              case '/ql-toa-hang/toa-hang/tao':
                breadCrumb.name = 'Tạo mới';
                break;
              case '/ql-toa-hang/toa-hang/sua':
                breadCrumb.name = 'Sửa';
                break;
            }

            //Especial case: edit/detail
            var employeeDetailPattern = /\/ql-toa-hang\/nhan-vien\/(\d*)$/g;
            var wagonDetailPattern = /\/ql-toa-hang\/toa-hang\/(\d*)$/g;

            var employeeEditorPattern = /\/ql-toa-hang\/nhan-vien\/sua\/(\d*)$/g;
            var wagonEditorPattern = /\/ql-toa-hang\/toa-hang\/sua\/(\d*)$/g;

            if (employeeDetailPattern.test(nav) || employeeEditorPattern.test(nav)
              || wagonDetailPattern.test(nav) || wagonEditorPattern.test(nav)) {
              var pieces = nav.split('/');
              breadCrumb.name = pieces[pieces.length - 1];
            }

            $rootScope.breadCrumbs.push(breadCrumb);
          }
        }
      }
    }

    $rootScope.directToNav = function (path) {
      $location.path(path);
    }

});

clarityApp.service('authenticationService', Clarity.Service.AuthenticationService);
clarityApp.service('baseService', Clarity.Service.BaseService);
clarityApp.service('userService', Clarity.Service.UserService);
clarityApp.service('employeeService', Clarity.Service.EmployeeService);

clarityApp.controller('LoginController', Clarity.Controller.LoginController);
clarityApp.controller('LogoutController', Clarity.Controller.LogoutController);
clarityApp.controller('MainController', Clarity.Controller.MainController);
clarityApp.controller('EmployeeManagementController', Clarity.Controller.EmployeeManagementController);
clarityApp.controller('TruckManagementController', Clarity.Controller.TruckManagementController);
clarityApp.controller('WagonManagementController', Clarity.Controller.WagonManagementController);
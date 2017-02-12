define(function (require) {
  'use strict';
  return require('angular').module('myApp', [])
    .config(['$routeProvider', function ($routeProvider) {
      $routeProvider.when('/', {
        templateUrl: 'views/main.html',
        controller: require('controllers/main')
      });
    }]);
});
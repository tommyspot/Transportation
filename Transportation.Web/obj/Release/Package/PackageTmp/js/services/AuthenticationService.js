/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="../model/UserModel.ts" />
var Clarity;
(function (Clarity) {
    var Service;
    (function (Service) {
        var AuthenticationService = (function () {
            function AuthenticationService($http, $cookieStore) {
                this.http = $http;
                this.url = '/api/auth/';
                this.cookieStore = $cookieStore;
            }
            AuthenticationService.prototype.doCallback = function (callback, data, status) {
                if (callback) {
                    callback(data, status);
                }
            };
            AuthenticationService.prototype.login = function (user, successCallback, errorCallback) {
                var _this = this;
                this.http.post(this.url + 'login', user)
                    .success(function (data, status) {
                    var currentUser = data[0];
                    currentUser.terminalId = user.terminalId;
                    currentUser.locationId = user.locationId;
                    currentUser.numberOfCounter = user.numberOfCounter;
                    currentUser.shift = user.shift;
                    currentUser.loginTime = user.loginTime;
                    currentUser.isAuthenticated = true;
                    _this.cookieStore.put('user', currentUser);
                    _this.doCallback(successCallback, data[0], status);
                })
                    .error(function (data, status) { _this.doCallback(errorCallback, data[0], status); });
            };
            AuthenticationService.prototype.checkLogin = function (user, successCallback, errorCallback) {
                var _this = this;
                this.http.post(this.url + 'checkLogin', user)
                    .success(function (data, status) {
                    _this.doCallback(successCallback, data, status);
                })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            AuthenticationService.prototype.logout = function (successCallback, errorCallback) {
                var _this = this;
                this.http.get(this.url + 'logout')
                    .success(function (data, status) {
                    _this.cookieStore.remove('user');
                    _this.doCallback(successCallback, data, status);
                })
                    .error(function (data, status) { _this.doCallback(errorCallback, data, status); });
            };
            AuthenticationService.prototype.isAuthenticated = function () {
                var currentUser = this.cookieStore.get('user');
                return currentUser && currentUser.isAuthenticated;
            };
            AuthenticationService.prototype.getUserName = function () {
                var currentUser = this.cookieStore.get('user');
                return currentUser ? currentUser.username : '';
            };
            AuthenticationService.prototype.getUser = function () {
                var currentUser = this.cookieStore.get('user');
                return currentUser ? currentUser : null;
            };
            AuthenticationService.prototype.getUserRole = function () {
                var currentUser = this.cookieStore.get('user');
                return currentUser ? currentUser.role : '';
            };
            AuthenticationService.prototype.generatePointOfInterval = function (currentServerInterval) {
                var ret = [];
                var basicPointOfInterval = [0, 15 * 60 * 1000, 30 * 60 * 1000, 45 * 60 * 1000];
                var currDate = new Date();
                var currInterval = currDate.getMinutes() * 60 * 1000 + currDate.getSeconds() * 1000;
                var matchedInterval = Math.abs(currInterval - currentServerInterval);
                if (matchedInterval >= basicPointOfInterval[0] && matchedInterval < basicPointOfInterval[1]) {
                    ret.push(matchedInterval + (15 * 60 * 1000 * 0));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 1));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 2));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 3));
                }
                else if (matchedInterval >= basicPointOfInterval[1] && matchedInterval < basicPointOfInterval[2]) {
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-1)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 0));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 1));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 2));
                }
                else if (matchedInterval >= basicPointOfInterval[2] && matchedInterval < basicPointOfInterval[3]) {
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-2)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-1)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 0));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 1));
                }
                else {
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-3)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-2)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * (-1)));
                    ret.push(matchedInterval + (15 * 60 * 1000 * 0));
                }
                return ret;
            };
            return AuthenticationService;
        }());
        Service.AuthenticationService = AuthenticationService;
    })(Service = Clarity.Service || (Clarity.Service = {}));
})(Clarity || (Clarity = {}));

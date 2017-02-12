/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="../model/UserModel.ts" />

module Clarity.Service {
  import model = Clarity.Model;

  export class AuthenticationService {
    public http: ng.IHttpService;
    public url: string;
    public cookieStore: ng.ICookieStoreService;

    constructor($http: ng.IHttpService, $cookieStore) {
      this.http = $http;
      this.url = '/api/auth/';
      this.cookieStore = $cookieStore;
    }

    doCallback(callback: Function, data: Object, status: Object) {
      if (callback) {
        callback(data, status);
      }
    }

    login(user: model.UserModel, successCallback: Function, errorCallback: Function) {
      this.http.post(this.url + 'login', user)
        .success((data, status) => {
          var currentUser = data[0];
          currentUser.isAuthenticated = true;
          this.cookieStore.put('user', currentUser);
          this.doCallback(successCallback, data[0], status);
        })
        .error((data, status) => { this.doCallback(errorCallback, data[0], status) });
    }

    checkLogin(user: model.UserModel, successCallback: Function, errorCallback: Function) {
      this.http.post(this.url + 'checkLogin', user)
        .success((data, status) => {
          this.doCallback(successCallback, data, status)
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status) });
    }

    logout(successCallback: Function, errorCallback: Function) {
      this.http.get(this.url + 'logout')
        .success((data, status) => {
          this.cookieStore.remove('user');
          this.doCallback(successCallback, data, status)
        })
        .error((data, status) => { this.doCallback(errorCallback, data, status) });
    }

    isAuthenticated() {
      var currentUser = this.cookieStore.get('user');
      return currentUser && currentUser.isAuthenticated;
    }

    getUserName() {
      var currentUser = this.cookieStore.get('user');
      return currentUser ? currentUser.username : '';
    }

    getUser() {
      var currentUser = this.cookieStore.get('user');
      return currentUser ? currentUser : null;
    }

    getUserRole() {
      var currentUser = this.cookieStore.get('user');
      return currentUser ? currentUser.role : '';
    }

    generatePointOfInterval(currentServerInterval: number) {
      var ret = [];
      var basicPointOfInterval = [0, 15 * 60 * 1000, 30 * 60 * 1000, 45 * 60 * 1000]
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
    }
  }
}
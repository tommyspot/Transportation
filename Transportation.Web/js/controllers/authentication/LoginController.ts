/// <reference path="../../services/AuthenticationService.ts" />
/// <reference path="../../model/UserModel.ts" />
/// <reference path="../IController.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />
/// <reference path="../../../lib/angular/angular-cookies.d.ts" />

module Clarity.Controller {
  import service = Clarity.Service;
  import helper = Clarity.Helper;
  import userModel = Clarity.Model.UserModel;

  export interface ILoginControllerScope extends ng.IScope {
    viewModel: LoginController;
  }

  export interface IRootLoginControllerScope extends Clarity.Controller.IRootScope {
    user: userModel;
    returnUrl: string;
  }

  export class LoginController {

    public authenticationService: service.AuthenticationService;
    public mainHelper: helper.MainHelper;

    public user: userModel;
    public errorMessage: string;

    constructor(private $scope: ILoginControllerScope,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $rootScope: IRootLoginControllerScope,
      private $http: ng.IHttpService,
      private $cookieStore: ng.ICookieStoreService) {
      $scope.viewModel = this;
      this.authenticationService = new service.AuthenticationService($http, $cookieStore);
      this.mainHelper = new helper.MainHelper($http, $cookieStore);
      this.errorMessage = '';
    }

    public submit() {
      this.$rootScope.showSpinner();
      this.authenticationService.login(this.user,
        (data) => { this.onSubmitSuccess(data); },
        (data, status) => { this.onSubmitError(data, status); });
    }   

    public onSubmitSuccess(user: userModel) {
      this.$rootScope.clearCache();
      this.$rootScope.user = user;

      if (this.$rootScope.returnUrl && this.$rootScope.returnUrl != '/login') {
        this.$location.path(this.$rootScope.returnUrl);
      }
      else {
        this.$location.path('/');
      }

      this.$rootScope.hideSpinner();
    }

    public onSubmitError(data, status: number) {
      switch (status) {
        case 404:
          this.errorMessage = 'User does not exist';
          break;
        case 409:
          this.errorMessage = 'Wrong password';
          break;
        case 410:
          this.errorMessage = 'User is disabled';
          break;
      }
      this.$rootScope.hideSpinner();
    }

  }
}
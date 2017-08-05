/// <reference path="../../services/AuthenticationService.ts" />
/// <reference path="../IController.ts" />
/// <reference path="../../../lib/angular/angular.d.ts" />

module Clarity.Controller {
  import service = Clarity.Service;
  import model = Clarity.Model;

  export interface ILogoutControllerScope extends ng.IScope {
    viewModel: LogoutController;
  }

  export class LogoutController {
    public authenticationService: service.AuthenticationService;
    public mainHelper: Helper.MainHelper;

    constructor(private $scope: ILogoutControllerScope,
      public $location: ng.ILocationService,
      public $window: ng.IWindowService,
      public $rootScope: Controller.IRootScope,
      private $http: ng.IHttpService,
      private $cookieStore: ng.ICookieStoreService,
      private $filter: ng.IFilterService) {

      $scope.viewModel = this;
      this.authenticationService = new service.AuthenticationService($http, $cookieStore);
      this.mainHelper = new Helper.MainHelper($http, $cookieStore, $filter);   
    }

    public submit() {
      this.$rootScope.showSpinner();
      this.authenticationService.logout(() => { this.onSubmitSuccess(); }, () => { this.$rootScope.onError(); });
    }

		public getCurrentUserName() {
      return this.authenticationService.getUserName();
    }

    public onSubmitSuccess() {
      this.$rootScope.clearCache();
      this.$rootScope.hideSpinner();
      this.$location.path('/login');      
    }

  }
}
/// <reference path="../../lib/angular/angular.d.ts" />

module Clarity.Controller {
  export interface IRootScope extends ng.IRootScopeService {
    user: Model.UserModel;
		truckList: Array<Model.TruckModel>;

    error: string;
    onError(error?): any;
    
    showSpinner();
    hideSpinner();

    saveViewPortPosition();
    scrollToPreviousViewPortPosition();

    clearCache();
    enableElements();
    disableElements();
  }
}

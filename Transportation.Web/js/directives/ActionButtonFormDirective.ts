/// <reference path="../../lib/angular/angular.d.ts" />

module Clarity.Directive {

  export interface IActionButtonScope extends ng.IScope {
    form: Object;
    isSaveVisible: boolean;
    isEditVisible: boolean;
    isSaveOrEditDisabled: boolean;
    handleSave: Function;
    handleEdit: Function;
    handleCancel: Function;
    isSubmitting: boolean;
    viewModel: Object;
  }

  export class ActionButtonForm implements ng.IDirective {
    templateUrl = 'js/directives/templates/actionButtonForm.html' + '?v=' + VERSION_NUMBER;
    restrict = 'E';
    /***
    @ Used to pass a string value into the directive.
    = Used to create a two-way binding to an object that is passed into the directive.
    & Allows an external function to be passed into the directive and invoked.
    ***/
    public scope = {
      form: '=',
      isSaveVisible: '=',
      isEditVisible: '=',
      isSaveOrEditDisabled: '=',
      handleSave: '&',
      handleEdit: '&',
      handleCancel: '&',
      isSubmitting: '=',
    };

    public link(scope: IActionButtonScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes): void {
      scope.viewModel = scope;
    };

    public static Factory() {
      var directive = () => {
        return new ActionButtonForm();
      };
      // directive['$inject'] = [''];
      return directive;
    }
  }
}
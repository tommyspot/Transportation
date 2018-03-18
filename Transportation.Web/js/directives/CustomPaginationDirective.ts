/// <reference path="../../lib/angular/angular.d.ts" />

module Clarity.Directive {

  const MAX_VISIBLE_PAGE = 5;

  export interface ICustomPaginationScope extends ng.IScope {
    numOfPages: number;
    currentPage: number;
    goToPage: Function;
    goToPreviousPage: Function;
    goToNextPage: Function;
    getDynamicPage: Function,
    getIndexDynamicPage: Function,
    viewModel: Object;
  }

  export class CustomPagination implements ng.IDirective {
    templateUrl = 'js/directives/templates/customPagination.html' + '?v=' + VERSION_NUMBER;
    restrict = 'E';
    public scope = {
      numOfPages: '=',
      currentPage: '=',
    };

    public link(scope: ICustomPaginationScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes): void {
      // Internal methods into isolate scope
      scope.getDynamicPage = () => {
        return scope.numOfPages > 0
          ? scope.numOfPages < MAX_VISIBLE_PAGE ? new Array(scope.numOfPages) : new Array(MAX_VISIBLE_PAGE)
          : new Array(0);
      };

      scope.getIndexDynamicPage = (index) => {
        if (scope.currentPage <= 2)
          return index; // start 1-5
        else if (scope.currentPage <= scope.numOfPages - 3)
          return index + (scope.currentPage - 2);  // start (currentPage - 2) - (currentPage + 2)
        else
          return index + (scope.numOfPages - 5); // start (numOfPage - 4) - (numOfPage)
      };

      scope.goToPage = (pageIndex: number) => {
        scope.currentPage = pageIndex;
      };

      scope.goToPreviousPage = () => {
        if (scope.currentPage >= 1) {
          scope.currentPage--;
          scope.goToPage(scope.currentPage);
        }
      };

      scope.goToNextPage = () => {
        if (scope.currentPage < scope.numOfPages - 1) {
          scope.currentPage++;
          scope.goToPage(scope.currentPage);
        }
      }

      scope.viewModel = scope;
    };

    public static Factory() {
      var directive = () => {
        return new CustomPagination();
      };
      return directive;
    }
  }
}
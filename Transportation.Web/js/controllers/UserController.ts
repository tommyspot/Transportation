/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

  export class UserController {
    public mainHelper: helper.MainHelper;
    public userService: service.UserService;

		public currentUser: Model.UserModel;
    public userList: Array<Model.UserModel>;
    public userListView: Array<Model.UserViewModel>;
    public userListViewTmp: Array<Model.UserViewModel>;

		public numOfPages: number;
		public currentPage: number;
		public pageSize: number;
		public isCheckedAll: boolean;
    public isLoading: boolean;
    public searchText: string;
    public errorMessage: string;

		constructor(private $scope,
			public $rootScope: IRootScope,
			private $http: ng.IHttpService,
			public $location: ng.ILocationService,
			public $window: ng.IWindowService,
      public $filter: ng.IFilterService,
      public $timeout: ng.ITimeoutService,
			private $routeParams: any,
      private $cookieStore: ng.ICookieStoreService) {

      this.mainHelper = new helper.MainHelper($http, $cookieStore, $filter);
			this.userService = new service.UserService($http);
      $scope.viewModel = this;

      this.pageSize = 10;
      this.searchText = '';
      this.errorMessage = '';
			this.initUser();

			var self = this;
			$scope.$watch('viewModel.searchText', function (value) {
        if (self.userListViewTmp && self.userListViewTmp.length > 0) {
          self.userListView = $filter('filter')(self.userListViewTmp, value);
					self.initPagination();
				}
			});
		}

		initUser() {
			var userId = this.$routeParams.user_id;
			if (userId) {
        this.initCurrentUser(userId);
			} else {
				if (this.$location.path() === '/ql-dang-nhap/tao') {
					this.currentUser = new Model.UserModel();
				} else if (this.$location.path() === '/ql-dang-nhap') {
					this.initUserList();
				}
			}
		}

    initUserList() {
      this.isLoading = true;
			this.userService.getAll((results: Array<Model.UserModel>) => {
				this.userList = results;
				this.userList.sort((a: any, b: any) => {
					return b.id - a.id;
        });

        this.mapToUserListView();
				this.userListViewTmp = this.userListView;
        this.initPagination();
        this.isLoading = false;
			}, null);
    }

    initCurrentUser(userId: number) {
      if (this.currentUser == null) {
        this.userService.getById(userId, (data) => {
          this.currentUser = data;
          this.currentUser.repeatPassword = this.currentUser.password; 
        }, null);
      }
    }

    mapToUserListView() {
      this.userListView = this.userList.map((user: Model.UserModel) => {
        const userView = new Model.UserViewModel();
        userView.id = user.id;
        userView.firstName = user.firstName;
        userView.lastName = user.lastName;
        userView.username = user.username;
        userView.password = user.password;
        userView.repeatPassword = user.password;
        userView.role = this.getRoleName(user.role);
        return userView;
      });
    }

		initPagination() {
			this.currentPage = 1;
			this.numOfPages = this.userListView.length % this.pageSize === 0 ?
        this.userListView.length / this.pageSize : Math.floor(this.userListView.length / this.pageSize) + 1;
		}

		getUserListOnPage() {
      if (this.userListView && this.userListView.length > 0) {
				var startIndex = this.pageSize * (this.currentPage - 1);
				var endIndex = startIndex + this.pageSize;
        return this.userListView.slice(startIndex, endIndex);
			}
		}

		getNumberPage() {
			if (this.numOfPages > 0) {
				return new Array(this.numOfPages);
			}
			return new Array(0);
		}

		goToPage(pageIndex: number) {
			this.currentPage = pageIndex;
		}

		goToPreviousPage() {
			if (this.currentPage > 1) {
				this.currentPage--;
				this.goToPage(this.currentPage);
			}
		}
		goToNextPage() {
			if (this.currentPage < this.numOfPages) {
				this.currentPage++;
				this.goToPage(this.currentPage);
			}
		}

		selectAllUsersOnPage() {
			var userOnPage = this.getUserListOnPage();
			for (let index = 0; index < userOnPage.length; index++) {
				var user = userOnPage[index];
				user.isChecked = this.isCheckedAll;
			}
		}

		removeUsers() {
			var confirmDialog = this.$window.confirm('Bạn có muốn xóa những tài khoản được chọn?');
			if (confirmDialog) {
        for (let i = 0; i < this.userListView.length; i++) {
          const user = this.userListView[i];
					if (user.isChecked) {
						this.userService.deleteEntity(user, (data) => {
							this.initUserList();
						}, null);
					}
				}
			}
		}

		removeUserInDetail(user: Model.UserModel) {
			var confirmDialog = this.$window.confirm('Bạn có muốn xóa tài khoản này?');
			if (confirmDialog) {
				this.userService.deleteEntity(user, (data) => {
					this.$location.path('/ql-dang-nhap');
				}, null);
			}
		}

		createUser(user: Model.UserModel) {
			if (user.password == user.repeatPassword) {
				this.userService.create(user,
					(data) => {
						this.$location.path('/ql-dang-nhap');
          }, (error) => {
            this.errorMessage = 'Tên tài khoản đã tồn tại';
            this.$timeout(() => {
              this.errorMessage = '';
            }, 8000);
          });
			} else {
				var confirmDialog = this.$window.confirm('Mật khẩu và Nhập lại mật khẩu phải giống nhau');
			}
		}

		updateUser(user: Model.UserModel) {
			if (user.password == user.repeatPassword) {
				this.userService.update(user, (data) => {
					this.$location.path('/ql-dang-nhap');
				}, null);
			} else {
				var confirmDialog = this.$window.confirm('Mật khẩu và Nhập lại mật khẩu phải giống nhau');
			}
		}

		goToUserForm() {
			this.$location.path('/ql-dang-nhap/tao');
    }

    goToUserEditForm(event: Event, userId: number) {
      event.stopPropagation();
      this.$location.path(`/ql-dang-nhap/sua/${userId}`);
    }

    getRoleName(role: number) {
      const availableRoles = ['Admin', "Quản lý toa hàng", "Quản lý garage", "Quản lý báo cáo"];
      if (role >= availableRoles.length) return '';
      return availableRoles[role];
    }

    clearSearchText() {
      this.searchText = '';
    }

	}
}
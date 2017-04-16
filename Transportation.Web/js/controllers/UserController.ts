/// <reference path="../../lib/angular/angular.d.ts" />
/// <reference path="../../lib/angular/angular-cookies.d.ts" />
/// <reference path="IController.ts" />
/// <reference path="../services/AuthenticationService.ts" />

declare var VERSION_NUMBER;

module Clarity.Controller {
	import service = Clarity.Service;
	import helper = Clarity.Helper;

	export class UserController {
		public currentUser: Model.UserModel;
		public userService: service.UserService;
		public userList: Array<Model.UserModel>;
		public roleList: Array<Model.RoleModel>;
		public userListTmp: Array<Model.UserModel>;
		public roles: Array<string>;
		public Eroles: Array<string>;
		public numOfPages: number;
		public currentPage: number;
		public pageSize: number;
		public isCheckedAll: boolean;
		public mainHelper: helper.MainHelper;

		constructor(private $scope,
			public $rootScope: IRootScope,
			private $http: ng.IHttpService,
			public $location: ng.ILocationService,
			public $window: ng.IWindowService,
			public $filter: ng.IFilterService,
			private $routeParams: any,
			private $cookieStore: ng.ICookieStoreService) {

			this.userService = new service.UserService($http);
			$scope.viewModel = this;
			this.pageSize = 5;
			this.roles = ['Admin', 'Quyết toán', 'Toa hàng', 'Đơn hàng'];
			this.Eroles = ['Super', 'WagonSettlement', 'Wagon', 'CustomerOrder'];
			this.roleList = this.getRoleList();
			this.initUser();
			this.mainHelper = new helper.MainHelper($http, $cookieStore);

			var self = this;
			$scope.$watch('searchText', function (value) {
				if (self.userListTmp && self.userListTmp.length > 0) {
					self.userList = $filter('filter')(self.userListTmp, value);
					self.initPagination();
				}
			});
		}

		initUser() {
			var userId = this.$routeParams.user_id;
			if (userId) {
				if (this.$location.path() === '/ql-dang-nhap/' + userId) {
					this.userService.getById(userId, (data) => {
						this.currentUser = data;
					}, null);
				} else if (this.$location.path() === '/ql-dang-nhap/sua/' + userId) {
					if (this.currentUser == null) {
						this.userService.getById(userId, (data) => {
							this.currentUser = data;
							this.currentUser.isEdited = true;
						}, null);
					}
				}
			} else {
				if (this.$location.path() === '/ql-dang-nhap/tao') {
					this.currentUser = new Model.UserModel();

				} else if (this.$location.path() === '/ql-dang-nhap') {
					this.initUserList();
				}
			}
		}

		initUserList() {
			this.userService.getAll((results: Array<Model.UserModel>) => {
				this.userList = results;
				this.userList.sort(function (a: any, b: any) {
					return a.id - b.id;
				});
				this.userListTmp = this.userList;
				this.initPagination();
			}, null);
		}

		initPagination() {
			this.currentPage = 1;
			this.numOfPages = this.userList.length % this.pageSize === 0 ?
				this.userList.length / this.pageSize : Math.floor(this.userList.length / this.pageSize) + 1;
		}

		getUserListOnPage() {
			if (this.userList && this.userList.length > 0) {
				var startIndex = this.pageSize * (this.currentPage - 1);
				var endIndex = startIndex + this.pageSize;
				return this.userList.slice(startIndex, endIndex);
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
			var confirmDialog = this.$window.confirm('Bạn có muốn xóa tài khoản?');
			if (confirmDialog) {
				for (let i = 0; i < this.userList.length; i++) {
					var user = this.userList[i];
					if (user.isChecked) {
						this.userService.deleteEntity(user, (data) => {
							this.initUserList();
						}, () => { });
					}
				}
			}
		}

		removeUserInDetail(user: Model.UserModel) {
			var confirmDialog = this.$window.confirm('Bạn có muốn xóa tài khoản?');
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
					},
					() => { });
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

		checkStatusUser(user) {
			return user.isDeleted ? 'Không' : 'Có';
		}

		getRoleList() {
			var roleList = new Array<Model.RoleModel>();

			for (var i = 0; i < this.roles.length; i++){
				var role = new Model.RoleModel();
				role.id = i;
				role.Ename = this.Eroles[i];
				role.name = this.roles[i];
				roleList.push(role);
			}
			return roleList;
		}

		getVNRoleName(name) {
			for (var i = 0; i < this.Eroles.length; i++) {
				if (this.Eroles[i] == name) {
					return this.roles[i];
				}
			}
			return '';
		}

	}
}
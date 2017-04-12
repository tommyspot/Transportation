/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class UserModel extends Model.BaseModel {
    public firstName: string;
    public lastName: string;
    public username: string;
    public password: string;
		public repeatPassword: string;
    public terminalId: number;
    public locationId: number;
    public numberOfCounter: number;
    public shift: number;
    public userLogId: number;
    public loginTime: string;
    public role: string;
		public isChecked: boolean;
		public isEdited: boolean;
  }

  export class RoleModel extends Model.BaseModel {
    public name: string;
		public Ename: string;
  }
}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class UserModel extends Model.BaseModel {
    public firstName: string;
    public lastName: string;
    public username: string;
    public password: string;
		public repeatPassword: string;
    public role: number;
  }

  export class UserViewModel extends Model.BaseModel {
    public firstName: string;
    public lastName: string;
    public username: string;
    public password: string;
    public repeatPassword: string;
    public role: string;
    public isChecked: boolean;
  }
}
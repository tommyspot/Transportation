/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class UserModel extends Model.BaseModel {
    public firstName: string;
    public lastName: string;
    public username: string;
    public password: string;
    public terminalId: number;
    public locationId: number;
    public numberOfCounter: number;
    public shift: number;
    public userLogId: number;
    public loginTime: string;
    public role: string;
  }

  export class AssigneeModel extends Model.BaseModel {
    public username: string;
  }
}
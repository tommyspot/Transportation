/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class EmployeeModel extends Model.BaseModel {
    public code: string;
    public fullName: string;
    public cardId: string;
    public address: string;
    public mobile: string;
    public driverLicenseRank: string;
    public driverLicenseId: string;
    public driverLicenseAddress: string;
    public driverLicenseDate: string;
    public driverLicenseExpirationDate: string;
    public startDate: string;
    public violation: string;
    public notes: string;
    public title: string;
    public isChecked: boolean;
  }
}
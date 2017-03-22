/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class EmployeeModel extends Model.BaseModel {
        public fullName: string;
        public cardId: string;
        public address: string;
        public mobile: string;
        public driverLicenseRank: string;
        public driverLicenseId: string;
        public driverLicenseAddress: string;
        public driverLicenseDate: Date;
        public driverLicenseExpirationDate: Date;
        public startDate: Date;
        public violation: string;
        public notes: string;
        public title: string;
        public isChecked: boolean;
				public isDeleted: boolean;
    }
}
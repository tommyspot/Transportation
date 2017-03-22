/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerModel extends Model.BaseModel {
		public fullName: string;
    public area: string;
    public employeeId: number;
		public totalOwned: number;
		public totalPay: number;
		public totalDebt: number;
		public type: string;
		public phoneNo: string;
		public code: string;
		public isChecked: boolean;
	}

}
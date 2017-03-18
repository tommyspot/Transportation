/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerModel extends Model.BaseModel {
		public fullName: string;
    public area: string;
    public employeeId: number;
		public totalOwned: string;
		public totalPay: string;
		public totalDebt: string;
		public type: string;
		public phoneNo: string;
		public code: string;
		public isChecked: boolean;
	}

}
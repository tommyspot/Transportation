/// <reference path="BaseModel.ts" />

module Clarity.Model {
	export class CustomerModel extends Model.BaseModel {
		public fullName: string;
		public totalOwned: number;
		public totalPay: number;
		public totalDebt: number;

		public totalOwnedFormatted: string;
		public totalPayFormatted: string;
		public totalDebtFormatted: string;

		public type: string;
		public phoneNo: string;
		public code: string;
		public isChecked: boolean;
	}
}
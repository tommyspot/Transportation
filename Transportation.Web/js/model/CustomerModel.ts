/// <reference path="BaseModel.ts" />

module Clarity.Model {
	export class CustomerModel extends Model.BaseModel {
		public fullName: string;
		public totalOwned: number;
		public totalPay: number;
		public totalDebt: number;

		public type: string;
		public phoneNo: string;
    public code: string;

    public totalOwnedFormatted: string;
    public totalPayFormatted: string;
    public totalDebtFormatted: string;
  }

  export class CustomerViewModel extends Model.BaseModel {
    public code: string;
    public fullName: string;
    public phoneNo: string;
    public totalOwned: string;
    public totalPay: string;
    public totalDebt: string;
    public isChecked: boolean;
  }
}
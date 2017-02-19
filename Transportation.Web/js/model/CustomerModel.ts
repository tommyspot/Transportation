/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class CustomerModel extends Model.BaseModel {
		public fullName: string;
		public area: string; 
    public employeeId: string; 
    public totalOwned: string;
    public totalPay: string;
    public totalDebt: string;
    public type: string;
		public isChecked: boolean;
  }

}
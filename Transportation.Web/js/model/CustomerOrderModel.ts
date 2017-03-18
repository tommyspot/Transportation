/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerOrderModel extends Model.BaseModel {
		public customerName: string;
		public customerId: number;
		public customerPhone: string;
		public customerArea: string;
		public employeeId: number;
		public unit: string;
		public quantity: number;
		public departure: string;
		public destination: string;
		public unitPrice: number;
		public departDate: Date;
		public returnDate: Date;
    public notes: string;
    public createdDate: Date;
		public isChecked: boolean;
	}

}
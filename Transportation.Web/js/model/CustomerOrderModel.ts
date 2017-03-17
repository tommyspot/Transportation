/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerOrderModel extends Model.BaseModel {
		public customerName: string;
		public customerId: string;
		public customerPhone: number;
		public customerArea: string;
		public employeeId: string;
		public unit: string;
		public quantity: number;
		public departure: string;
		public destination: string;
		public unitPrice: number;
		public departDate: string;
		public returnDate: string;
		public notes: string;
		public isChecked: boolean;
	}

}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerOrderModel extends Model.BaseModel {
		public customerName: string;
		public customerId: string;
		public unit: string;
		public quantity: string;
		public departure: string;
		public destination: string;
		public unitPrice: string;
		public departDate: string;
		public returnDate: string;
		public notes: string;
		public isChecked: boolean;
	}

}
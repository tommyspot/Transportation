/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class CustomerOrderModel extends Model.BaseModel {
		public customerName: string;
		public customerId: number;
		public customerPhone: string;
		public customerArea: string;
		public customerCode: string;
		public unit: string;
		public quantity: number;
		public departure: string;
		public destination: string;
		public unitPrice: number;
		public totalPay: number;
		public totalPayFormated: string;
		public departDate: string;
		public returnDate: string;
    public notes: string;
		public code: string;
		public truckId: number;
		public truckLicensePlate: string;
		public createdUserId: number;
    public createdDate: Date;
		public isChecked: boolean;
	}

}
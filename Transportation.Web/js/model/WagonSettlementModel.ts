/// <reference path="BaseModel.ts" />
module Clarity.Model {
  export class WagonSettlementModel extends Model.BaseModel {
    
    public date: Date;
		
		public paymentDate: Date; 

		public customerOrderId: number;
    public customerId: number;
    public wagonId: number;
		public wagonCode: string;
		public employeeId: number;
		public payment: number;
		public paymentRemain: number;  //totalAmount - payment
    public quantity : number;
    public unitPrice: number;
		public discount: number;
    public totalAmount: number; //quantity*unitPrice-discount
		public newPayment: number;

		public paymentFormated: string;
		public paymentRemainFormated: string;  //totalAmount - payment
    public unitPriceFormated: string;
		public discountFormated: string;
    public totalAmountFormated: string;

		public code: string;
		public paymentPlace: string;
		public paymentStatus: string;  
		public unit: string;
		public departure: string;
    public destination: string;
    public notes: string;
    public formatedCustomerOrder: string;
		public dateFormated: string;
		public paymentDateFormated: string;
		public newPaymentFormated: string;
		public customerCode: string;
  }
}
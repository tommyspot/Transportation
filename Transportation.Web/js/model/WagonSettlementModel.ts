/// <reference path="BaseModel.ts" />
module Clarity.Model {
  export class WagonSettlementModel extends Model.BaseModel {
    public code: string;
    public paymentDate: string; 
    public customerId: number;
    public wagonId: number;
    public quantity : number;
    public unitPrice: number;
    public unit: string;
    public destination: string;

    public payment: number;
    public paymentRemain: number;  //totalAmount - payment
    public newPayment: number;

    public phiPhatSinh: number;
    public lyDoPhatSinh: number;

    public paymentPlace: string;
    public paymentStatus: string;

		public paymentFormatted: string;
		public paymentRemainFormatted: string;  //totalAmount - payment
    public unitPriceFormatted: string;
		public paymentDateFormatted: string;
    public newPaymentFormatted: string;
    public phiPhatSinhFormatted: string;
  }

  export class WagonSettlementViewModel extends Model.BaseModel {
    public wagonCode: string;
    public customerName: string;
    public departDate: string;
    public totalAmount: string; // quantity*unitPrice + phiPhatSinh = (payment + paymentRemain) + phiPhatSinh
    public phiPhatSinh: string;
    public payment: string;
    public paymentRemain: string;
    public paymentStatus: string;
  }
}
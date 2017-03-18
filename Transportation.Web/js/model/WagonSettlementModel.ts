/// <reference path="BaseModel.ts" />
module Clarity.Model {
  export class WagonSettlementModel extends Model.BaseModel {
    public code: string;
    public customerOrderId: number;
    public customerId: number;
    public wagonId: number;
    public date: Date;
    public employeeId: number;

    public payment: number;
    public paymentPlace: string;
    public paymentRemain: number;  //totalAmount - payment

    public unit: string;
    public quantity : number;
    public departure: string;
    public destination: string;
    public unitPrice: number;
    public totalAmount: number; //quantity*unitPrice
    public notes: string;

    public formatedCustomerOrder: string;
  }
}
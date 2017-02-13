/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonSetlementModel extends Model.BaseModel {
    public customerId: number;
    public wagonId: number;
    public date: string;
    public employeeId: number;
    //public sumPayment: number;
    public payment: number;

    public unit: string;
    public quantity : number;
    public departure: string;
    public destination: string;
    public unitPrice: number;
    public totalAmount: number;
    public notes: string;
  }
}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class OrderModel extends Model.BaseModel {
    public code: string;
    public customerName: string;
    public mobile: string;
    public licensePlate: string;
    public cardId: string;
    public address: string;
    public employeeName: string;
    public date: string;
    public orderDetails: Array<OrderDetailModel>;
    public saleOff: number;
    public totalAmount: number;
    public note: string;
    public isChecked: boolean;

    constructor() {
      super();
      this.orderDetails = new Array<OrderDetailModel>();
    }
  }

    export class OrderDetailModel extends Model.BaseModel {
      public orderId: number;
      public productId: number;
      public price: number;
      public quantity: number;
      public priceFormatted: string;
    }
}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class OrderModel extends Model.BaseModel {
    public licensePlate: string;
    public customerName: string;
    public address: string;
    public date: string;
    public orderDetails: Array<OrderDetailModel>;
    public saleOff: number;
    public totalAmount: number;
    public note: string;
    public status: boolean;

    constructor() {
      super();
      this.status = true;
      this.orderDetails = new Array<OrderDetailModel>();
    }
  }

  export class OrderDetailModel extends Model.BaseModel {
    public orderId: number;
    public productId: number;
    public price: number;
    public quantity: number;
    public maxQuantity: number;  // no map
    public priceFormatted: string;
  }

  export class OrderViewModel extends Model.BaseModel {
    public licensePlate: string;
    public customerName: string;
    public date: string;
    public saleOff: string;
    public totalAmount: string;
    public status: boolean;
    public unit: string;
    public isChecked: boolean;
  }
}
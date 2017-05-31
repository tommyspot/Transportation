/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class OrderModel extends Model.BaseModel {
        public code: string;
        public customerName: string;
        public employeeName: string;
        public date: string;
        public orderDetails: Array<OrderDetailModel>; 
        public totalAmount: number;
        public isChecked: boolean;
  }

    export class OrderDetailModel extends Model.BaseModel {
      public orderId: number;
      public productId: number;
      public price: number;
      public quantity: number;
      public date: string;
    }
}
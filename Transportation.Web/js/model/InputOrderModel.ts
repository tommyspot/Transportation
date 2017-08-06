/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class InputOrderModel extends Model.BaseModel {
    public code: string;
    public vendor: string;
    public date: string;
    public productInputs: Array<ProductInputModel>;
    public totalAmount: number;

    constructor() {
      super();
      this.productInputs = new Array<ProductInputModel>();
    }
  }

  export class InputOrderViewModel extends Model.BaseModel {
    public vendor: string;
    public date: string;
    public numOfProducts: number;
    public totalAmount: string;
    public isChecked: boolean;
  }
}
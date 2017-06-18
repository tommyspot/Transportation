/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class InputOrderModel extends Model.BaseModel {
    public code: string;
    public vendor: string;
    public date: string;
    public productInputs: Array<ProductInputModel>;
    public totalAmount: number;
    public isChecked: boolean;

    constructor() {
      super();
      this.productInputs = new Array<ProductInputModel>();
    }
  }
}
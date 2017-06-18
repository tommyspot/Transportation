/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class ProductModel extends Model.BaseModel {
    public name: string;
    public origin: string;
    public isChecked: boolean;
  }

  export class ProductInfoModel extends Model.BaseModel {
    public name: string;
    public sumOfInput: number;
    public sumOfInputTotalAmount: number;
    public sumOfSale: number;
    public sumOfSaleTotalAmount: number;
    public numOfRemain: number;
    public profit: number;
  }
}
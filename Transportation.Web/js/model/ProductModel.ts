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

  export class ProductInfoViewModel extends Model.BaseModel {
    public name: string;
    public sumOfInput: string;
    public sumOfInputTotalAmount: string;
    public sumOfSale: string;
    public sumOfSaleTotalAmount: string;
    public numOfRemain: string;
    public profit: string;
  }
}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class InventoryModel extends Model.BaseModel {
    public productId: number;
    public quantity: number;
  }

  export class InventoryViewModel extends Model.BaseModel {
    public productId: number;
    public productName: string;
    public productPrice: number;
    public quantity: number;
  }
}
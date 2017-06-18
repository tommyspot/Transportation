/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class InventoryModel extends Model.BaseModel {
    public productId: number;
    public quantity: number;
    public latestPrice: number;
  }

  export class InventoryViewModel extends Model.BaseModel {
    public productId: number;
    public productName: string;
    public latestPrice: number;
    public quantity: number;
  }
}
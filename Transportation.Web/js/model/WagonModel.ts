/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonModel extends Model.BaseModel {
    public departDate: string;
    public returnDate: string;
    public truckId: number;
    public employeeId: number;

    public costOfTruck: number;
    public costOfService: number;
    public costOfTangBoXe: number;
    public costOfPenalty: number;
    public costOfExtra: number;

    public paymentOfTruck: number;
    public paymentOfRepairing: number;
    public paymentOfOil: number;
    public paymentOfLuong: number;
    public paymentOfService: number;
    public paymentOfHangVe: number;
    public paymentOf10Percent: number;
    public paymentOfOthers: number;
    public wagonSettlements: Array<Model.WagonSetlementModel>;
    public isChecked: boolean;
  }
}
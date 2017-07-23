/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonModel extends Model.BaseModel {
    public code: string;

    public paymentDate: string;
    public paymentPlace: string;
    public departDate: string;
    public returnDate: string;
    public departure: string;
    public destination: string;
    public truckId: number;
    public employeeId: number;

    public costOfTruck: number;
    public costOfService: number;
    public costOfTangBoXe: number;
    public costOfPenalty: number;
    public costOfExtra: number;
    public textOfExtra: string;

    public paymentOfTruck: number;
    public paymentOfRepairing: number;
    public paymentOfOil: number;
    public paymentOfLuong: number;
    public paymentOfService: number;
    public paymentOfHangVe: number;
    public paymentOf10Percent: number;

		public costOfTruckFormatted: string;
    public costOfServiceFormatted: string;
    public costOfTangBoXeFormatted: string;
    public costOfPenaltyFormatted: string;
    public costOfExtraFormatted: string;

    public paymentOfTruckFormatted: string;
    public paymentOfRepairingFormatted: string;
    public paymentOfOilFormatted: string;
    public paymentOfLuongFormatted: string;
    public paymentOfServiceFormatted: string;
    public paymentOfHangVeFormatted: string;
    public paymentOf10PercentFormatted: string;
		
    public wagonSettlements: Array<Model.WagonSettlementModel>;
    public isChecked: boolean;
  }
}
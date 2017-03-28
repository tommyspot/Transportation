/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonModel extends Model.BaseModel {
    public code: string;

    public paymentDate: Date;
    public paymentPlace: string;

    public departDate: Date;
    public returnDate: Date;
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
		//public pay: number;
		//public discount: number;

		public costOfTruckFormated: string;
    public costOfServiceFormated: string;
    public costOfTangBoXeFormated: string;
    public costOfPenaltyFormated: string;
    public costOfExtraFormated: string;

    public paymentOfTruckFormated: string;
    public paymentOfRepairingFormated: string;
    public paymentOfOilFormated: string;
    public paymentOfLuongFormated: string;
    public paymentOfServiceFormated: string;
    public paymentOfHangVeFormated: string;
    public paymentOf10PercentFormated: string;
    public paymentOfOthersFormated: string;
		//public payFormated: string;
		//public discountFormated: string;
		
		public reasonForPhiPhatXinh: string;
    public wagonSettlements: Array<Model.WagonSettlementModel>;
    public notes: string;
    public isChecked: boolean;
  }
}
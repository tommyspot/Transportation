/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class TruckModel extends Model.BaseModel {
		public code: string;
		public licensePlate: string; // bien so xe
    public vin: string; //so khung
    public engineNo: string;
    public yearOfManufacture: string;
    public brand: string;
    public weight: string;
    public startUsingDate: Date;
    public employeeId: string;
    public stock: string;
    public buyingDate: Date;
    public monthlyPayment: string;
    public checkDate: Date;
		public insuranceDate: Date;
		public isChecked: boolean;
  }

}
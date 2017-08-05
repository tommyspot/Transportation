/// <reference path="BaseModel.ts" />

module Clarity.Model {
  export class TruckModel extends Model.BaseModel {
    public licensePlate: string; // bien so xe
    public vin: string; //so khung
    public engineNo: string;
    public yearOfManufacture: number;
    public brand: string;
    public weight: number;
    public startUsingDate: string;
    public employeeId: string;
    public stock: number;
    public buyingDate: string;
    public monthlyPayment: number;
    public checkDate: string;
    public insuranceDate: string;
    public isDeleted: boolean;

    public monthlyPaymentFormatted: string;
  }

  export class TruckViewModel extends Model.BaseModel {
    public licensePlate: string;
    public yearOfManufacture: string;
    public brand: string;
    public weight: string;
    public hasUsing: string;
    public isChecked: boolean;
  }
}
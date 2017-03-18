/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class TruckModel extends Model.BaseModel {
        public licensePlate: string; // bien so xe
        public vin: string; //so khung
        public engineNo: string;
        public yearOfManufacture: number;
        public brand: string;
        public weight: number;
        public startUsingDate: Date;
        public employeeId: string;
        public stock: number;
        public buyingDate: Date;
        public monthlyPayment: number;
        public checkDate: Date;
        public insuranceDate: Date;
        public isDeleted: boolean;
        public isChecked: boolean;
    }

}
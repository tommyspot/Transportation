/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonReportModel extends Model.BaseModel {
    public fromDate: string;
    public toDate: string;
    public data: WagonReportDataModel;
  }

  export class WagonReportDataModel {
    public code: string;
    public totalAmount: number;
    public totalPayment: number;
    public profit: number;
  }
}
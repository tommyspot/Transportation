/// <reference path="BaseModel.ts" />

module Clarity.Model {

  export class WagonSettlementReportModel extends Model.BaseModel {
    public fromDate: string;
    public toDate: string;
    public data: WagonSettlementReportDataModel;
  }

  export class WagonSettlementReportDataModel {
    public code: string;
    public totalAmount: number;
    public totalPayment: number;
    public profit: number;
  }
}
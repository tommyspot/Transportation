/// <reference path="BaseModel.ts" />

module Clarity.Model {
  export class CustomerReportModel extends Model.BaseModel {
    public fromDate: string;
    public toDate: string;
    public isCheckedAll: boolean;
    public selectedIds: Array<string>;

    constructor() {
      super();
      this.isCheckedAll = true;
    }
  }
}

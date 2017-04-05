/// <reference path="BaseModel.ts" />

module Clarity.Model {

	export class PaymentModel extends Model.BaseModel {
		public paymentDate: string;
    public paymentAmount: number;
    public wagonId: number;
	}

}
/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class ProductInputModel extends Model.BaseModel {
        public productId: string;
        public quantity: number;
        public price: number;
        public dateInput: string;
        public isChecked: boolean;
    }
}
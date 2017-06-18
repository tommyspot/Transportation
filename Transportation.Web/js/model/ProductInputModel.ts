/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class ProductInputModel extends Model.BaseModel {
        public productId: string;
        public quantity: number;
        public inputPrice: number;
        public salePrice: number;
        public isChecked: boolean;
        //format value
        public inputPriceFormated: string;
        public salePriceFormated: string;
    }
}
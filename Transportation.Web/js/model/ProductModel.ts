/// <reference path="BaseModel.ts" />

module Clarity.Model {

    export class ProductModel extends Model.BaseModel {
        public name: string;
        public origin: string;
        public isChecked: boolean;
    }
}
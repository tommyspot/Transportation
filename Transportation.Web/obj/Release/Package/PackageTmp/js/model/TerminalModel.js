/// <reference path="BaseModel.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Clarity;
(function (Clarity) {
    var Model;
    (function (Model) {
        var TerminalModel = (function (_super) {
            __extends(TerminalModel, _super);
            function TerminalModel() {
                _super.apply(this, arguments);
            }
            return TerminalModel;
        }(Clarity.Model.BaseModel));
        Model.TerminalModel = TerminalModel;
        var LocationModel = (function (_super) {
            __extends(LocationModel, _super);
            function LocationModel() {
                _super.apply(this, arguments);
            }
            return LocationModel;
        }(Clarity.Model.BaseModel));
        Model.LocationModel = LocationModel;
    })(Model = Clarity.Model || (Clarity.Model = {}));
})(Clarity || (Clarity = {}));

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
        var KioskLogModel = (function (_super) {
            __extends(KioskLogModel, _super);
            function KioskLogModel() {
                _super.apply(this, arguments);
            }
            return KioskLogModel;
        }(Clarity.Model.BaseModel));
        Model.KioskLogModel = KioskLogModel;
    })(Model = Clarity.Model || (Clarity.Model = {}));
})(Clarity || (Clarity = {}));

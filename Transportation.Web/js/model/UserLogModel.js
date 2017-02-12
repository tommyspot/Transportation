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
        var UserLogModel = (function (_super) {
            __extends(UserLogModel, _super);
            function UserLogModel() {
                _super.apply(this, arguments);
            }
            return UserLogModel;
        }(Clarity.Model.BaseModel));
        Model.UserLogModel = UserLogModel;
    })(Model = Clarity.Model || (Clarity.Model = {}));
})(Clarity || (Clarity = {}));

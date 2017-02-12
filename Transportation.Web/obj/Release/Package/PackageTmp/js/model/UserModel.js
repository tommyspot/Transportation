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
        var UserModel = (function (_super) {
            __extends(UserModel, _super);
            function UserModel() {
                _super.apply(this, arguments);
            }
            return UserModel;
        }(Clarity.Model.BaseModel));
        Model.UserModel = UserModel;
    })(Model = Clarity.Model || (Clarity.Model = {}));
})(Clarity || (Clarity = {}));

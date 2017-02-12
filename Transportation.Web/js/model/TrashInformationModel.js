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
        var TrashInformationModel = (function (_super) {
            __extends(TrashInformationModel, _super);
            function TrashInformationModel() {
                _super.apply(this, arguments);
            }
            return TrashInformationModel;
        }(Clarity.Model.BaseModel));
        Model.TrashInformationModel = TrashInformationModel;
        var TrashInformationViewModel = (function (_super) {
            __extends(TrashInformationViewModel, _super);
            function TrashInformationViewModel() {
                _super.apply(this, arguments);
            }
            return TrashInformationViewModel;
        }(TrashInformationModel));
        Model.TrashInformationViewModel = TrashInformationViewModel;
        var Coordinate = (function () {
            function Coordinate(longitude, latitude) {
                this.lng = longitude;
                this.lat = latitude;
            }
            return Coordinate;
        }());
        Model.Coordinate = Coordinate;
        var Marker = (function () {
            function Marker(id, size, longitude, latitude) {
                this.id = id;
                this.size = size;
                this.longitude = longitude;
                this.latitude = latitude;
            }
            return Marker;
        }());
        Model.Marker = Marker;
        (function (Status) {
            Status[Status["Unconfirmed"] = 0] = "Unconfirmed";
            Status[Status["Confirmed"] = 1] = "Confirmed";
            Status[Status["Cleaned"] = 2] = "Cleaned";
        })(Model.Status || (Model.Status = {}));
        var Status = Model.Status;
        (function (Types) {
            Types[Types["Household"] = 0] = "Household";
            Types[Types["Automotive"] = 1] = "Automotive";
            Types[Types["Construction"] = 2] = "Construction";
            Types[Types["Plastic"] = 3] = "Plastic";
            Types[Types["Electronic"] = 4] = "Electronic";
            Types[Types["Glass"] = 5] = "Glass";
            Types[Types["Metal"] = 6] = "Metal";
            Types[Types["Liquid"] = 7] = "Liquid";
            Types[Types["Dangerous"] = 8] = "Dangerous";
        })(Model.Types || (Model.Types = {}));
        var Types = Model.Types;
    })(Model = Clarity.Model || (Clarity.Model = {}));
})(Clarity || (Clarity = {}));

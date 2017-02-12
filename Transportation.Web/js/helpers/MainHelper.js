var Clarity;
(function (Clarity) {
    var Helper;
    (function (Helper) {
        var MainHelper = (function () {
            function MainHelper() {
            }
            ;
            MainHelper.prototype.getCurrentDateTimeString = function () {
                var d = new Date();
                var day, month, hh, mm, ss = '';
                var year = d.getFullYear();
                if (d.getMonth() + 1 < 10) {
                    month = '0' + (d.getMonth() + 1);
                }
                else {
                    month = '' + (d.getMonth() + 1);
                }
                if (d.getDate() < 10) {
                    day = '0' + d.getDate();
                }
                else {
                    day = '' + d.getDate();
                }
                if (d.getHours() < 10) {
                    hh = '0' + d.getHours();
                }
                else {
                    hh = '' + d.getHours();
                }
                if (d.getMinutes() < 10) {
                    mm = '0' + d.getMinutes();
                }
                else {
                    mm = '' + d.getMinutes();
                }
                if (d.getSeconds() < 10) {
                    ss = '0' + d.getSeconds();
                }
                else {
                    ss = '' + d.getSeconds();
                }
                var date = year + '/' + month + '/' + day + ' ' + hh + ':' + mm + ':' + ss;
                return date;
            };
            MainHelper.prototype.formatDateTimeToString = function (dateStr) {
                var d = new Date(dateStr);
                var day, month, hh, mm, ss = '';
                var year = d.getFullYear();
                if (d.getMonth() + 1 < 10) {
                    month = '0' + (d.getMonth() + 1);
                }
                else {
                    month = '' + (d.getMonth() + 1);
                }
                if (d.getDate() < 10) {
                    day = '0' + d.getDate();
                }
                else {
                    day = '' + d.getDate();
                }
                if (d.getHours() < 10) {
                    hh = '0' + d.getHours();
                }
                else {
                    hh = '' + d.getHours();
                }
                if (d.getMinutes() < 10) {
                    mm = '0' + d.getMinutes();
                }
                else {
                    mm = '' + d.getMinutes();
                }
                if (d.getSeconds() < 10) {
                    ss = '0' + d.getSeconds();
                }
                else {
                    ss = '' + d.getSeconds();
                }
                var date = year + '/' + month + '/' + day + ' ' + hh + ':' + mm + ':' + ss;
                return date;
            };
            MainHelper.prototype.formatDateToString = function (date) {
                var year = date.getFullYear();
                var month = (1 + date.getMonth()).toString();
                month = month.length > 1 ? month : '0' + month;
                var day = date.getDate().toString();
                day = day.length > 1 ? day : '0' + day;
                var hour = date.getHours() < 10 ? '0' + date.getHours() : date.getHours();
                var minute = date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
                var second = date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds();
                return month + '/' + day + '/' + year + ' ' + hour + ':' + minute + ':' + second;
            };
            MainHelper.prototype.convertToESTTimeZone = function (date) {
                var offset = -8.0;
                var utc = date.getTime() + (date.getTimezoneOffset() * 60000);
                var ESTDate = new Date(utc + (3600000 * offset));
                return ESTDate;
            };
            return MainHelper;
        }());
        Helper.MainHelper = MainHelper;
    })(Helper = Clarity.Helper || (Clarity.Helper = {}));
})(Clarity || (Clarity = {}));

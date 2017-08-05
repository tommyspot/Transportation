
module Clarity.Helper {

  export class MainHelper {
		public authenticationService: Clarity.Service.AuthenticationService;

    constructor(private $http: ng.IHttpService,
      private $cookieStore: ng.ICookieStoreService,
      private $filter: ng.IFilterService) {
			this.authenticationService = new Clarity.Service.AuthenticationService($http, $cookieStore);
		};

    getCurrentDateTimeString() {
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
    }

    formatDateTimeToString(dateStr: string) {
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
    }

    formatDateToString(date: Date) {
      var year = date.getFullYear();
      var month = (1 + date.getMonth()).toString();
      month = month.length > 1 ? month : '0' + month;
      var day = date.getDate().toString();
      day = day.length > 1 ? day : '0' + day;

      var hour = date.getHours() < 10 ? '0' + date.getHours() : date.getHours();
      var minute = date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
      var second = date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds();

      return month + '/' + day + '/' + year + ' ' + hour + ':' + minute + ':' + second;
    }

    convertToESTTimeZone(date: Date) {
      var offset = -8.0;
      var utc = date.getTime() + (date.getTimezoneOffset() * 60000);
      var ESTDate = new Date(utc + (3600000 * offset));

      return ESTDate;
    }

    formatDateTimeDDMMYYYY(d: any) {
      if (!(d instanceof Date)) {
        d = new Date(date);
      }

      var year = d.getFullYear().toString();
      var monthNumber = d.getMonth() + 1;
      var month = monthNumber < 10 ? '0' + monthNumber.toString() : monthNumber.toString();
      var day = d.getDate() < 10 ? '0' + d.getDate().toString() : d.getDate().toString();

      var date = day + '/' + month + '/' + year;
      return date;
    }

		formatDateTimeDDMMYYYYNumber(d: any) {
      if (!(d instanceof Date)) {
        d = new Date(date);
      }

      var year = d.getFullYear().toString();
      var monthNumber = d.getMonth() + 1;
      var month = monthNumber < 10 ? '0' + monthNumber.toString() : monthNumber.toString();
      var day = d.getDate() < 10 ? '0' + d.getDate().toString() : d.getDate().toString();

      var date = day + month + year;
      return date;
    }

		isUserHasGreaterOrEqualPermission(checkUserRole) {
      var userRole = this.authenticationService.getUserRole();
			if (userRole == checkUserRole) {
				return true;
			}
      //if (userRole == checkUserRole) {
      //  return true;
      //} else if (userRole === this.userRolesModel.Super && (checkUserRole === this.userRolesModel.Normal)) {
      //  return true;
      //} else if (userRole == this.userRolesModel.AccountOwner && (checkUserRole != this.userRolesModel.PartnerAccount)) {
      //  return true;
      //} else if (userRole == this.userRolesModel.PartnerAccount) {
      //  return true;
      //}
      return false;
    }

    formatStringToDateTime(date: string) {
      var day = parseInt(date.split('/')[0]);
      var month = parseInt(date.split('/')[1]) - 1;
      var year = parseInt(date.split('/')[2]);
      return new Date(year, month, day);
    }

    formatCurrency(value: number) {
      return this.$filter('currency')(value, '', 0).trim();
    }

    onCurrencyPropertyChanged(object: Object, propertyName: string, formattedPropertyName: string) {
      if (object[formattedPropertyName] && object[formattedPropertyName] != '') {
        object[propertyName] = parseInt(object[formattedPropertyName].replace(/\./g, ''));
        object[formattedPropertyName] = this.formatCurrency(object[propertyName]);
      }
    }

    initCurrencyFormattedProperty(object: Object, propertyNames: Array<string>, formatSuffix: string) {
      propertyNames.forEach((property: string) => {
        if (object.hasOwnProperty(property)) {
          object[`${property}${formatSuffix}`] = this.formatCurrency(object[property]);
        } 
      });
    }

    getPropertyValue(objectList: Array<any>, searchPropertyName: string, searchByValue: string, propertyName: string): string {
      if (objectList && objectList.length > 0) {
        const filterObjects = objectList.filter((object: any) => {
          return object[searchPropertyName] == searchByValue;
        });
        return filterObjects && filterObjects.length > 0 ? filterObjects[0][propertyName] : '';
      }
      return '';
    }

    getObjectById(objectList: Array<any>, id: number) {
      if (objectList && objectList.length > 0) {
        objectList.forEach((obj: any) => {
          if (obj.id == id) return obj;
        });
      }
      return null;
    }

  }

}
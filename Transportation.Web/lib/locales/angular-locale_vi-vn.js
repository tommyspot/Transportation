angular.module("ngLocale", [], ["$provide", function($provide) {
var PLURAL_CATEGORY = {ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other"};
$provide.value("$locale", {
  "DATETIME_FORMATS": {
    "AMPMS": [
      "SA",
      "CH"
    ],
    "DAY": [
      "Ch\u1ee7 nh\u1eadt",
      "Th\u1ee9 hai",
      "Th\u1ee9 ba",
      "Th\u1ee9 t\u01b0",
      "Th\u1ee9 n\u0103m",
      "Th\u1ee9 s\u00e1u",
      "Th\u1ee9 b\u1ea3y"
    ],
    "MONTH": [
      "Th\u00e1ng 1",
      "Th\u00e1ng 2",
      "Th\u00e1ng 3",
      "Th\u00e1ng 4",
      "Th\u00e1ng 5",
      "Th\u00e1ng 6",
      "Th\u00e1ng 7",
      "Th\u00e1ng 8",
      "Th\u00e1ng 9",
      "Th\u00e1ng 10",
      "Th\u00e1ng 11",
      "Th\u00e1ng 12"
    ],
    "SHORTDAY": [
      "CN",
      "Th\u1ee9 2",
      "Th\u1ee9 3",
      "Th\u1ee9 4",
      "Th\u1ee9 5",
      "Th\u1ee9 6",
      "Th\u1ee9 7"
    ],
    "SHORTMONTH": [
      "Thg 1",
      "Thg 2",
      "Thg 3",
      "Thg 4",
      "Thg 5",
      "Thg 6",
      "Thg 7",
      "Thg 8",
      "Thg 9",
      "Thg 10",
      "Thg 11",
      "Thg 12"
    ],
    "fullDate": "EEEE, 'ng\u00e0y' dd MMMM 'n\u0103m' y",
    "longDate": "'Ng\u00e0y' dd 'th\u00e1ng' M 'n\u0103m' y",
    "medium": "dd-MM-yyyy HH:mm:ss",
    "mediumDate": "dd-MM-yyyy",
    "mediumTime": "HH:mm:ss",
    "short": "dd/MM/yyyy HH:mm",
    "shortDate": "dd/MM/yyyy",
    "shortTime": "HH:mm"
  },
  "NUMBER_FORMATS": {
    "CURRENCY_SYM": "\u20ab",
    "DECIMAL_SEP": ",",
    "GROUP_SEP": ".",
    "PATTERNS": [
      {
        "gSize": 3,
        "lgSize": 3,
        "macFrac": 0,
        "maxFrac": 3,
        "minFrac": 0,
        "minInt": 1,
        "negPre": "-",
        "negSuf": "",
        "posPre": "",
        "posSuf": ""
      },
      {
        "gSize": 3,
        "lgSize": 3,
        "macFrac": 0,
        "maxFrac": 2,
        "minFrac": 2,
        "minInt": 1,
        "negPre": "-",
        "negSuf": "\u00a0\u00a4",
        "posPre": "",
        "posSuf": "\u00a0\u00a4"
      }
    ]
  },
  "id": "vi-vn",
  "pluralCat": function (n) {  return PLURAL_CATEGORY.OTHER;}
});
}]);
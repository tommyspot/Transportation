// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var cleanCss = require('gulp-clean-css');
var sass = require('gulp-sass');

var config = {
    // include all css files
    cssSrc: [
      'lib/select2/select2.css'
      , 'css/main-style.css'
    ],
    // include all js files
    jsSrc: [
    , 'lib/jquery/jquery-1.9.1.min.js'
    , 'lib/jquery/jquery-ui.min.js'
    , 'lib/jquery/jquery.scrollUp.min.js'
    , 'lib/select2/select2.js'
    , 'lib/angular/angular.min.js'
    , 'lib/angular/angular-route.min.js'
    , 'lib/angular/angular-cookies.min.js'
    , 'lib/angular/ui-bootstrap.min.js'
    , 'lib/angular/angular-inview.js'

    , 'lib/angular-material/angular-aria.js'
    , 'lib/angular-material/angular-animate.js'
    , 'lib/angular-material/angular-material.js'

    , 'lib/select2/angular-ui/angular-ui.js'
    , 'lib/select2/angular-ui/select2.js'
    , 'lib/sortable/sortable.js'

    , 'js/helpers/MainHelper.js'

    , 'js/model/BaseModel.js'
    , 'js/model/UserModel.js'
    , 'js/model/EmployeeModel.js'
		, 'js/model/TruckModel.js'
		, 'js/model/CustomerModel.js'
    , 'js/model/WagonModel.js'
    , 'js/model/WagonSetlementModel.js'
    
    , 'js/services/IService.js'
    , 'js/services/BaseService.js'
    , 'js/services/AuthenticationService.js'
    , 'js/services/UserService.js'
    , 'js/services/EmployeeService.js'
		, 'js/services/TruckService.js'
		, 'js/services/CustomerService.js'
    , 'js/services/WagonService.js'
    , 'js/services/ExportService.js'

    , 'js/controllers/IController.js'
    , 'js/controllers/authentication/LoginController.js'
    , 'js/controllers/authentication/LogoutController.js'
    , 'js/controllers/MainController.js'
    , 'js/controllers/EmployeeManagementController.js'
    , 'js/controllers/TruckManagementController.js'
		, 'js/controllers/CustomerManagementController.js'
    , 'js/controllers/WagonManagementController.js'

    // controllers, services written with TypeScript must be placed before clarity-app.js
    , 'js/clarity-app.js'
    // controllers, services, directives written without TypeScript must be placed after clarity-app.js
    ]
};

// delete the css output file(s)
gulp.task('clean-css', function () {
    return del(['css/transportation-style.css']);
});

// delete the js output file(s)
gulp.task('clean-js', function () {
    return del(['js/all.min.js']);
});

// combine and minify all files from the js folder
gulp.task('combine-minify-js', ['clean-js'], function () {
    return gulp.src(config.jsSrc)
      .pipe(concat('all.min.js'))
      .pipe(uglify({ mangle: false }))
      .pipe(gulp.dest('js/'));
});

// watch js files
gulp.task('watch-js', function () {
    return gulp.watch(['js/**/*.js', '!js/all.min.js'], ['combine-js'])
      .on('change', function (file) { console.log('JS changed' + ' (' + file.path + ')'); });
});

// combine all files from the js folder
gulp.task('combine-js', ['clean-js'], function () {
    return gulp.src(config.jsSrc)
      .pipe(concat('all.min.js'))
      .pipe(gulp.dest('js/'));
});

//Compile SASS to CSS
var input = 'css/sass/*.scss';
var output = 'css';
gulp.task('compile-sass', function () {
  return gulp
    .src(input)
    .pipe(sass())
    .pipe(gulp.dest(output));
});
//// watch scss files
//gulp.task('watch-sass', function () {
//  return gulp.watch('css/sass/*.scss', ['compile-sass'])
//    .on('change', function (file) { console.log('CSS changed' + ' (' + file.path + ')'); });
//});

// combine all files from the css folder
gulp.task('combine-css', ['clean-css'], function () {
  return gulp.src(config.cssSrc)
.pipe(concat('transportation-style.css'))
.pipe(gulp.dest('css'));
});

// watch js, css files
gulp.task('watch', function () {

  gulp.watch('css/sass/*.scss', ['compile-sass'])
  .on('change', function (file) { console.log('CSS changed' + ' (' + file.path + ')'); });

  gulp.watch(['css/*.css', '!css/transportation-style.css'], ['combine-css'])
    .on('change', function (file) { console.log('CSS changed' + ' (' + file.path + ')'); });

  gulp.watch(['js/**/*.js', '!js/all.min.js'], ['combine-js'])
    .on('change', function (file) { console.log('JS changed' + ' (' + file.path + ')'); });
});


// combine js, css files only
gulp.task('combine-only', ['combine-js', 'combine-css'], function () { });

// set a default tasks
gulp.task('default', ['combine-minify-js', 'combine-css'], function () { });


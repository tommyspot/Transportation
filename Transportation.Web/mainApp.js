require.config({
  shim: {
    angular: {
      deps: ['jquery'],
      exports: 'angular'
    }
  },


});

define(function () {
  alert("xxx");
});
angular.module('AgileMadeSimple', ['ui.bootstrap', 'ngRoute'])
.config(['$routeProvider',
  function($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: 'app/index.html',
            controller: 'AgileMadeSimpleController',
        }).
        when('/Sample', {
            templateUrl: 'sample.html',
            controller: 'SampleController'
        }).
        when('/SampleTwo/:sampleTwoId', {
            templateUrl: 'sample-two.html',
            controller: 'SampleTwoController'
        }).
        otherwise({
            redirectTo: '/'
        });
  }])
.controller('AgileMadeSimpleController', ['$scope', '$routeParams', function ($scope, $routeParams) {

}]);
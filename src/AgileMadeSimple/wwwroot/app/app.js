﻿angular.module('AgileMadeSimple', ['ui.bootstrap', 'ngRoute',
    //My custom modules
    'User'
])
.config(['$routeProvider',
  function($routeProvider) {
      $routeProvider.
        when('/', {
            templateUrl: 'app/index.html',
            controller: 'AgileMadeSimpleController',
        }).
        otherwise({
            redirectTo: '/'
        });
  }])
.controller('AgileMadeSimpleController', ['$scope', '$routeParams', '$location', '$http', function ($scope, $routeParams, $location, $http) {
    $scope.User = null;

    $http.get('api/User/CurrentUser').then(function (response) {
        $scope.User = response.data;
    });

    $scope.logout = function () {
        $http.delete('api/User/Logout').then(function (response) {
            $scope.User = null;
            $location.url('/#/');
        });
    };
}]);
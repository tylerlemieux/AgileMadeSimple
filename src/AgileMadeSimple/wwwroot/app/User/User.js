﻿(function () {
    'use strict';

    angular.module('User', [
        // Angular modules 
        'ngRoute',

        // Custom modules 
        'ui.bootstrap'
        // 3rd Party Modules
        
    ])
    .config(['$routeProvider',

        function($routeProvider) {
            $routeProvider.
              when('/Registration', {
                  templateUrl: 'app/User/registration.html',
                  controller: 'RegistrationController',
              }).
              when('/Login', {
                  templateUrl: 'app/User/login.html',
                  controller: 'LoginController'
              });
    }])
    .controller('RegistrationController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.username = '';
        $scope.password = '';
        $scope.confirmPassword = '';
        $scope.email = '';
        $scope.name = '';

        $scope.register = function () {
            var registerVM = {
                Username: $scope.username,
                Password: $scope.password,
                ConfirmPassword: $scope.confirmPassword,
                Email: $scope.email,
                Name: $scope.name
            };

            $http.post('api/User/Register', registerVM)
                .then(function (response) {
                    $scope.$parent.User = response.data;
                    $location.url('/#/');
                });
        }
    }])
    .controller('LoginController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
        $scope.showLoginError = false;

        $scope.login = function () {
            var loginVM = {
                Username: $scope.username,
                Password: $scope.password
            };

            $http.post('api/User/Login', loginVM)
                .then(function (response) {
                    if (response.data != null && response.data !== "") {
                        $scope.$parent.User = response.data;
                        $location.url('/#/');
                    }
                }, function () {
                    $scope.showLoginError = true;
                });
        };
    }]);

})();
angular.module('AgileMadeSimple', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'pageslide-directive',
    //My custom modules
    'User', 'EnterprisePlanning', 'Backlog'
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
        $scope.User = response.data == "" ? null : response.data;
    });

    $scope.logout = function () {
        $http.delete('api/User/Logout').then(function (response) {
            $scope.User = null;
            $location.url('/#/');
        });
    };
}]);
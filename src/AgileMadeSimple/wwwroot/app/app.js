angular.module('AgileMadeSimple', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'pageslide-directive', 'xeditable', 'dndLists',
    //My custom modules
    'User', 'EnterprisePlanning', 'Backlog', 'SprintWall', 'TeamModule'
])
.run(function(editableOptions) {
    editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default'
})

/*BEGIN GLOBAL 403 ERROR HANDLING*/
//.config(function ($routeProvider, $httpProvider) {
//    $httpProvider.interceptors.push('responseObserver');
//})
//.factory('responseObserver', function responseObserver($q, $window) {
//    return {
//        'responseError': function(errorResponse) {
//            switch (errorResponse.status) {
//                case 403:
//                    $window.location = './403.html';
//                    break;
//                    //For more error handling, add more cases to this :)
//                //case 500:
//                //    $window.location = './500.html';
//                //    break;
//            }
//            return $q.reject(errorResponse);
//        }
//    };
//})
/*END GLOBAL 403 ERROR HANDLING*/

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

    $scope.$watch('$routeParams.projectId', function (newValue, oldValue) {
        if (newValue == null || newValue == undefined) {
            $scope.ProjectID = null;
            $scope.CurrentSprintID = null;
        } else {
            $scope.ProjectID = newValue;
            $http.get('api/Epic/CurrentSprint/' + $scope.ProjectID).then(function (response) {
                $scope.CurrentSprintID = response.data;
            });
        }

    });

    //$scope.getTeams = function () {
    //    $http.get('api/Team').then(function (response) {
    //        $scope.UserTeams = response.data;
    //    });
    //};
    //$scope.getTeams();

}]);
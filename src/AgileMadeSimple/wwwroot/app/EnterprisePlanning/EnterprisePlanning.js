angular.module('EnterprisePlanning', ['ngRoute', 'ui.bootstrap'])
.config(['$routeProvider',

        function ($routeProvider) {
            $routeProvider.
              when('/EnterprisePlanning', {
                  templateUrl: 'app/EnterprisePlanning/dashboard.html',
                  controller: 'EnterprisePlanningController',
              });
        }])
.controller('EnterprisePlanningController', ['$scope', '$http', '$location', function ($scope, $http, $location) {
    $scope.epics = [{
        EpicID: 1,
        Name: "Epic Sample 1"
    }, {
        EpicID: 2,
        Name: "Epic Sample 2"
    }];

    function getStates() {
        $http.get('api/Epic/States/1').then(function (result) {
            $scope.states = result.data;
            
        })
    }

    getStates();

    $scope.addState = function () {
        var state = {
            Name: $scope.newStateName,
            Order: $scope.states == null ? 0 : $scope.states.length,
            Type: "Epic",
            TeamID: 1
        };

        $http.post('api/Epic/States/1', state).then(function (result) {
            $scope.states = result.data;
        })
    };

    $scope.deleteState = function (stateId) {
        $http.delete('api/Epic/States/' + stateId).then(function (result) {
            $scope.states = result.data;
        })
    }

}]);

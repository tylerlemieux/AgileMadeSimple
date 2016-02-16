angular.module('EnterprisePlanning', ['ngRoute', 'ngAnimate', 'ui.bootstrap'])
.config(['$routeProvider',

        function ($routeProvider) {
            $routeProvider.
              when('/EnterprisePlanning', {
                  templateUrl: 'app/EnterprisePlanning/dashboard.html',
                  controller: 'EnterprisePlanningController',
              });
        }])
.controller('EnterprisePlanningController', ['$scope', '$http', '$location', '$uibModal', function ($scope, $http, $location, $uibModal) {


    $http.get('api/Epic').then(
        function (response) {
            $scope.epics = response.data;
        },
        function () { })

    function getStates() {
        $http.get('api/Epic/States/1').then(function (result) {
            $scope.states = result.data;

        });
    };

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
        });
    };

    $scope.openAddProjectModal = function () {
        var modal = $uibModal.open({
            templateUrl: 'addProjectModal.html',
            controller: 'AddProjectController'
        });

        modal.result.then(function (epic) {
            $scope.epics.push(epic);
        }, function () {

        });
    };

}])
.controller('AddProjectController', ['$scope', '$http', '$uibModalInstance', function ($scope, $http, $uibModalInstance) {
    $scope.create = function () {
        var epic = {
            Name: $scope.name,
            Description: $scope.description,
            TeamID: 1
        };

        $http.post('api/Epic', epic).then(function (response) {
                if (response.data != null && response.data !== "") {
                    $uibModalInstance.close(response.data);
                }
            },
            function () {

            });


    };

    $scope.close = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);

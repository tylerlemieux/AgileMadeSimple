angular.module('TeamModule', ['ngRoute', 'ui.bootstrap'])
    .config(['$routeProvider',
function($routeProvider) {
    $routeProvider.
    when('/TeamOptions', {
        templateUrl: 'app/Team/teamoptions.html',
        controller: 'TeamOptionsController'
    })
}])
.controller('TeamOptionsController', ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    
    $scope.getTeams = function () {
        $http.get('api/Team').then(function (response) {
            $scope.teams = response.data;
        }, function () {
            //handle errors
        });
    };

    $scope.getAllUsers = function () {
        $http.get('api/User').then(function (response) {
            $scope.allUsers = response.data;
        });
    };

    $scope.getAllUsers();
    $scope.getTeams();

    $scope.removeUser = function (userId, teamId) {
        $http.delete('api/Team/Member/' + teamId + '/' + userId).then(function (response) {
            $scope.teams = response.data;
        });
    };

    $scope.addUser = function (username, teamId) {
        $http.post('api/Team/Member/' + teamId + '/' + username).then(function (response) {
            $scope.teams = response.data;
        });
    };

    $scope.addTeam = function (name) {
        $http.post('api/Team/' + name).then(function (response) {
            $scope.teams =response.data;
        });
    }

    $scope.deleteTeam = function (teamId) {
        var confirmDelete = confirm("Are you sure you want to delete this team? This action cannot be undone");
        if (confirmDelete) {
            $http.delete('api/Team/' + teamId).then(function (response) {
                $scope.teams = response.data;
            });
        } else {
            return;
        }
    }

}]);
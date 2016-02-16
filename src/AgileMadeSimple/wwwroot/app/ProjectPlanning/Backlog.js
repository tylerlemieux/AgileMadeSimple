angular.module('Backlog', ['ui.bootstrap', 'ngRoute', 'ngAnimate', 'pageslide-directive'])
.config(['$routeProvider',

    function ($routeProvider) {
        $routeProvider.
            when('/Backlog/:projectId', {
                templateUrl: 'app/ProjectPlanning/backlog.html',
                controller: 'BacklogController',
            });
    }])
.controller('BacklogController', ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.projectId = $routeParams.projectId;

    $scope.getProjects = function () {

        $http.get('api/Story/Project/' + $scope.projectId).then(function (response) {
            $scope.stories = response.data;

            $scope.stories.push({ StoryID: 1, Name: "A user can view a backlog of stories", Points: 8 });
            $scope.stories.push({ StoryID: 1, Name: "A user can view a taskboard", Points: 20 });
            $scope.stories.push({ StoryID: 1, Name: "A user can add a project", Points: 5 });
            $scope.stories.push({ StoryID: 1, Name: "A user can create a new story", Points: 3 });
        }, function () {
            $scope.stories = [];
            $scope.stories.push({ StoryID: 1, Name: "A user can view a backlog of stories", Points: 8 });
            $scope.stories.push({ StoryID: 1, Name: "A user can view a taskboard", Points: 20 });
            $scope.stories.push({ StoryID: 1, Name: "A user can add a project", Points: 5 });
            $scope.stories.push({ StoryID: 1, Name: "A user can create a new story", Points: 3 });

        });
    };

    $scope.getProjects();
}]);
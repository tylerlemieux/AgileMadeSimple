angular.module('Backlog', ['ui.bootstrap', 'ngRoute', 'ngAnimate', 'pageslide-directive', 'xeditable'])
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
        }, function () {

        });
    };

    $scope.getProjects();


    $scope.blankStory = {
        Name: null,
        Points: null,
        Description: null,
        AcceptanceCriteria: null,
        BlockedText: null,
        Blocked: 'N',
        EpicID: parseInt($routeParams.projectId),
        OwnerID: null,
        StateID: 2
    };

    $scope.currentOpenStory = $scope.blankStory;
    $scope.sidePanelOpen = false;

    $scope.openStory = function (story) {
        if (!story)
            $scope.currentOpenStory = $scope.blankStory;
        else
            $scope.currentOpenStory = story;

        $scope.sidePanelOpen = true;
    };

    $scope.closePanel = function () {
        $scope.sidePanelOpen = false;
    }

    $scope.saveChanges = function () {
        if (isNaN($scope.currentOpenStory.StoryID)) {
            //Create a new story
            $http.post('api/Story', $scope.currentOpenStory).then(function (response) {
                $scope.stories.push(response.data);
                $scope.sidePanelOpen = false;
            });
        }

        else {
            //Edit the story
            $http.put('api/Story', $scope.currentOpenStory).then(function (response) {
                $scope.stories = response.data;
                $scope.sidePanelOpen = false;
            });
        }
    }

}]);
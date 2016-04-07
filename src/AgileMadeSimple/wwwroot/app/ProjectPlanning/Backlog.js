angular.module('Backlog', ['ui.bootstrap', 'ngRoute', 'ngAnimate', 'pageslide-directive', 'xeditable', 'dndLists'])
.config(['$routeProvider',

    function ($routeProvider) {
        $routeProvider.
            when('/Backlog/:projectId', {
                templateUrl: 'app/ProjectPlanning/backlog.html',
                controller: 'BacklogController',
            });
    }])
.controller('BacklogController', ['$scope', '$routeParams', '$http', '$uibModal', function ($scope, $routeParams, $http, $uibModal) {
    $scope.projectId = $routeParams.projectId;
    $scope.filterApplied = false;

    $scope.getProjects = function () {

        $http.get('api/Story/Project/' + $scope.projectId).then(function (response) {
            $scope.stories = response.data;
        }, function () {

        });
    };

    $scope.getSprints = function () {
        $http.get('api/Story/Sprints/' + $scope.projectId).then(function (response) {
            $scope.sprints = response.data;
        });
    };

    $scope.getSprints();
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
    $scope.sprintPanelOpen = false;


    $scope.openStory = function (story) {
        if (!story)
            $scope.currentOpenStory = {
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
        else
            $scope.currentOpenStory = story;

        $scope.sidePanelOpen = true;
    };

    $scope.closePanel = function () {
        $scope.sidePanelOpen = false;
    }

    $scope.closeSprintPanel = function () {
        $scope.sprintPanelOpen = false;
    }

    $scope.openSprintPanel = function () {
        $scope.sprintPanelOpen = true;
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

    $scope.storyMoved = function (storyIndex, list) {
        list.splice(storyIndex, 1);
      
        //Reorder the priority
        angular.forEach($scope.stories, function (story, index) {
            story.Order = index;
            story.SprintID = null;
        });

        var storiesVM = $scope.stories;

        angular.forEach($scope.sprints, function (sprint) {
            angular.forEach(sprint.Stories, function (story, index) {
                story.Order = index;
                story.SprintID = sprint.SprintID;
                storiesVM.push(story);
            });
        });

        $http.put("api/Story/BulkEdit", storiesVM).then(function (response) {
            //Call this to refresh the stories in the backlog... hacky fix for now to fix all stories getting ordered into the backlog section
            $scope.getProjects();
        });
    }

    $scope.openAddSprint = function () {

        var modal = $uibModal.open({
            templateUrl: 'addSprint.html',
            controller: 'AddSprintController'
        });

        modal.result.then(function (response) {
            $scope.getSprints();
        }, function () {

        });
    }
}])
.controller('AddSprintController', ['$scope', '$uibModalInstance', '$http', '$routeParams', function ($scope, $uibModalInstance, $http, $routeParams) {
    
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.addSprint = function () {
        //Add the story post and check response
        //If successful close the modal and return the new sprint
        var sprintVM = {
            SprintGoals: $scope.sprintGoals,
            DefinitionOfDone: $scope.definitionOfDone,
            StartDate: $scope.startDate,
            EndDate: $scope.endDate,
            ProjectID: $routeParams.projectId
        };

        $http.post('api/Story/Sprint', sprintVM).then(function (response) {
            $uibModalInstance.close(response.data);
        }, function () {
            alert("failure creating sprint.. todo edit this message");
        });

    }
}]);
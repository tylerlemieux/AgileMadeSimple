angular.module('SprintWall', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'dndLists', 'pageslide-directive', 'xeditable'])
.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
            when('/SprintWall/:sprintId', {
                templateUrl: 'app/SprintWall/sprintwall.html',
                controller: 'SprintWallController',
            });
}])
.controller('SprintWallController', ['$scope', '$routeParams', '$http', function ($scope, $routeParams, $http) {
    $scope.sidePanelOpen = false;
    $scope.sidePanelType = null;
    $scope.sidePanelScope = null;

    $scope.teamUsers = [];


    //$scope.taskTemplate = {
    //    Name: null,
    //    Description: null,
    //    TotalHours: null,
    //    ToDoHours: null,
    //    Blocked: null,
    //    BlockedMessage: null,
    //    StoryID: null,
    //    SprintID: $routeParams.sprintId
    //};

    $scope.getSprintData = function () {
        //console.log($routeParams.sprintId);
        $http.get('api/Task/Sprint/' + $routeParams.sprintId).then(function (response) {
            $scope.Sprint = response.data;
            //console.log($scope.Sprint)
            $scope.getTeam();
            
        }, function (error) {
            console.log(error);
        });
    };

    $scope.getSprintData();

    $scope.openSidePanel = function (model, type, storyId) {
        if (model == null) {
            if (type === 'task') {
                $scope.sidePanelScope = {
                    Name: null,
                    Description: null,
                    TotalHours: null,
                    ToDoHours: null,
                    Blocked: null,
                    BlockedMessage: null,
                    StoryID: storyId,
                    SprintID: $routeParams.sprintId
                };
            }
        } else {
            $scope.sidePanelScope = model;
        }

        $scope.sidePanelType = type;
        $scope.sidePanelOpen = true;
    };

    $scope.closeSidePanel = function () {
        $scope.sidePanelScope = null;
        $scope.sidePanelType = null;
        $scope.sidePanelOpen = false;
    };

    $scope.saveChanges = function () {
        if ($scope.sidePanelType == "task") {
            if ($scope.sidePanelScope.TaskID == null || $scope.sidePanelScope.TaskID == undefined) {
                //create as new task
                $http.post('api/Task', $scope.sidePanelScope).then(function (response) { getSprintData(); }, function () { /*error message*/ });
            } else {
                //update task
                $http.put('api/Task', $scope.sidePanelScope).then(function (response) { getSprintData(); }, function () { /*error message*/ });
            }

        } else if ($scope.sidePanelType == "story") {
            //update story
            $http.put('api/Story', $scope.sidePanelScope).then(function (response) { getSprintData(); }, function () { /*error message*/ });

        }

        

        $scope.closeSidePanel();
    };

    $scope.showOwner = function (userId) {
        if (userId === null) return null;
        console.log(userId);
        angular.forEach($scope.teamUsers, function (user, index) {
            if (user.UserID == userId) {
                return user.Name;
            }
        });
        return null;
    }


    $scope.getTeam = function () {
        $http.get('api/Team/Project/' + $scope.Sprint.ProjectID).then(function (response) {
            $scope.teamUsers = response.data;
        });
    };

    $scope.getAllTags = function () {
        $http.get('api/Tag').then(function (response) {
            $scope.allTags = response.data;
        });
    };
    //do this after everything else
    $scope.getAllTags();

    $scope.addTag = function (type, id, tagName) {
        if (type === 'task') {
            $http.put('api/Task/AddTag/' + id + '/' + tagName).then(function (response) {
                $scope.sidePanelScope.Tags.push(tagName);
            });
            return;
        } else if (type === 'story') {
            $http.put('api/Story/AddTag/' + id + '/' + tagName).then(function (response) {
                $scope.sidePanelScope.Tags.push(tagName);
            });
            return;
        }
        throw("Invalid tag type: " + type);
    }

    $scope.removeTag = function (type, id, tagName, index) {
        if (type === 'task') {
            $http.delete('api/Task/RemoveTag/' + id + '/' + tagName).then(function (response) {
                $scope.sidePanelScope.Tags.splice(index, 1);
            });
            return;
        } else if (type === 'story') {
            $http.delete('api/Story/RemoveTag/' + id + '/' + tagName).then(function (response) {
                $scope.sidePanelScope.Tags.splice(index, 1);
            });
            return;
        }
        throw ("Invalid tag type: " + type);
    }
    
}]);
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
        console.log($routeParams.sprintId);
        $http.get('api/Task/Sprint/' + $routeParams.sprintId).then(function (response) {
            $scope.Sprint = response.data;
            console.log($scope.Sprint);


            
        }, function () {
            $scope.Sprint = {
                Story: [
                    {
                        Name: "Story 1", Points: 8, Task: [
                            {
                                Name: "Task 1",
                                TotalHours: 10,
                                ToDoHours: 5
                            },
                            {
                                Name: "Task 2",
                                TotalHours: 1,
                                ToDoHours: .5
                            },
                            {
                                Name: "Task 3",
                                TotalHours: 13,
                                ToDoHours: 8
                            }
                        ]
                    },
                    {
                        Name: "Story 2", Points: 8, Task: [
                            {
                                Name: "Task 4",
                                TotalHours: 10,
                                ToDoHours: 5
                            },
                            {
                                Name: "Task 5",
                                TotalHours: 1,
                                ToDoHours: .5
                            },
                            {
                                Name: "Task 6",
                                TotalHours: 13,
                                ToDoHours: 8
                            }
                        ]
                    }
                ]
            }

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
}]);
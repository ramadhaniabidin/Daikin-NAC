var app = angular.module('app');
app.directive('button', function () {
    return {
        restrict: 'E',
        link: function (scope, elem, attrs) {
            if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                elem.on('click', function (e) {
                    e.preventDefault();
                });
            }
        }
    };
});
app.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    elm.show();
                } else {
                    elm.hide();
                }
            });
        }
    };

}]);

app.controller('ctrl', function ($scope) {
    $scope.Agree = false;
    $scope.urlParams = new URLSearchParams(location.search);
    $scope.Module = $scope.urlParams.get('module');

    $scope.SelectDisplay = function () {
        if (!!$scope.Module)
            location.href = 'Display.aspx?module=' + $scope.Module;
        else
            alert("There is no Module defined");
    }
});
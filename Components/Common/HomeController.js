
App.controller('HomeController', ["FraudsApi", "Utility", "api", "$scope", "$rootScope", "$window", "webStorage",
    function (FraudsApi, Utility, api, $scope, $rootScope, $window, webStorage) {

        $scope.ui = {};
        var is_logged = webStorage.local.get("current_loggedin_user");

        if (is_logged == null)
            $scope.ui.show_logout = false;
        else
            $scope.ui.show_logout = true;

        $scope.Logout = function () {
            webStorage.local.add('current_loggedin_user', null);
            webStorage.local.add('access_control', null);
            $window.location.href = "/login";
        }

    }]);

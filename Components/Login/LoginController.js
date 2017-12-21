
App.controller('LoginController', ["FraudsApi", "Utility", "api", "$scope", "$rootScope", "$window", "webStorage",
    function (FraudsApi, Utility, api, $scope, $rootScope, $window, webStorage) {
        $scope.username;
        $scope.password;
        $scope.authenticate=function()
        {
            if ($scope.username !== "" && $scope.password !== "") {
                var data = {};
                data["username"] = $scope.username;
                data["password"] = $scope.password;
                api.Post("/login/authUser", null, data)
                .success(function (Response) {
                    if (Response.is_logged == true) {
                        webStorage.local.add('current_loggedin_user', $scope.username);
                        webStorage.local.add('access_control', Response.access_control);
                        webStorage.local.add('user_id', Response.user_id);
                        console.debug(Response.user_id);
                        $window.location.href = "search";
                    }
                    else
                        alert("Username or password in incorrect!");
                })
            .error(function (error) {
                status = 'Unable to load data from api call to auth user: ' + error;
                console.log(status);
            })

            }
       };
    }]);

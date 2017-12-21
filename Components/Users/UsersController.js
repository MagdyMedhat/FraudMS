
App.controller('UsersController', ["UsersModel", "FraudsApi", "Utility", "api", "$scope", "$window", "webStorage",
    function (UsersModel, FraudsApi, Utility, api, $scope, $window ,webStorage) {

        FraudsApi.CheckUserAccess(3);
        setTimeout(function () {
            $('#container').css("display", "block");
        }, 500);

        //pagination
        $scope.PageSize = 10;
        $scope.NumberOfPages = 1;
        $scope.TableRecords = [];
        $scope.range = [];
        $scope.selectedIndex = 0;

        $scope.ui = {};
        $scope.ui.users = null;

        $scope.GetUsers = function () {
            api.Get("/Users/GetUsers")
        .success(function (Response) {
            $scope.ui.users = Response;
            for (var i = 0; i < $scope.ui.users.length; i++) {
                if ($scope.ui.users[i].user_create_date !=null)
                $scope.ui.users[i].user_create_date = Utility.FixJsonDate($scope.ui.users[i].user_create_date);
                if ($scope.ui.users[i].user_last_login != null)
                $scope.ui.users[i].user_last_login = Utility.FixJsonDate($scope.ui.users[i].user_last_login);
            }
            $scope.NumberOfPages = Math.ceil($scope.ui.users.length / $scope.PageSize);
            $scope.range = [];
            for (var i = 1; i <= $scope.NumberOfPages; i++)
                $scope.range.push(i);

            $scope.Paginate(1);
        })
    .error(function (error) {
        status = 'Unable to load data from api call to users/GetUsers: ' + error;
        console.log(status);
    })

        }
        $scope.Edit = function (index){
            webStorage.local.add('current_user', $scope.TableRecords[index]);
            $window.location.href = "/edituser";
        }
        $scope.Add = function () {
            webStorage.local.add('current_user', "");
            $window.location.href = "/edituser";
        }


        $scope.Paginate = function (page) {
            $scope.selectedIndex = page - 1;
            var start = ((page - 1) * $scope.PageSize + 1);
            var end = (page * $scope.PageSize);
            $scope.TableRecords = [];
            for (var i = start - 1; i < end; i++) {

                if (i == $scope.ui.users.length) {
                    break;
                }
                var Temp = $scope.ui.users[i];
                $scope.TableRecords.push(Temp);
            }
        }
       $scope.GetUsers();
    }]);

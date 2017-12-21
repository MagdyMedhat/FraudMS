
App.controller('EditUserController', ["FraudsApi", "Utility", "api", "$scope", "$rootScope", "$window", "webStorage",
    function (FraudsApi, Utility, api, $scope, $rootScope, $window, webStorage) {

        FraudsApi.CheckUserAccess(1);
        setTimeout(function () {
            $('#container').css("display", "block");
        }, 500);

        $scope.ui = {};
        $scope.ui.access;
        $scope.ui.user_access;
        $scope.ui.current_user = webStorage.get("current_user");
        $scope.ui.deletebtn = true;

        $scope.GetAccess = function () {
            api.Post("/edituser/GetAccess", null)
               .success(function (Response) {
                   $scope.ui.access = Response;
                   var number = $scope.ui.access.length;
                   $scope.ui.user_access = Array(number);
                   for (var i = 0; i < number; i++) {
                       $scope.ui.user_access[i] = 0;
                   }
                   $scope.fillAccess();
               })
           .error(function (error) {
               status = 'Unable to load data from api call to get access: ' + error;
               console.log(status);
           })
        }
        
        $scope.fillAccess = function () {
            if ($scope.ui.current_user.user_access) {
                for (var i = 0; i < $scope.ui.current_user.user_access.length; i++) {
                    for (var j = 0; j < $scope.ui.access.length; j++) {
                        if ($scope.ui.current_user.user_access[i] == $scope.ui.access[j].key)
                            $scope.ui.user_access[j] = $scope.ui.access[j].key;
                    }
                }
            }
        };

        $scope.deleteUser = function () {
                if ($scope.ui.current_user !== "") {
                    var txt;
                    var r = confirm("تأكيد الحذف ؟");
                    if (r == true) {
                        var obj = {};
                        obj['user_id'] = $scope.ui.current_user.user_id;
                        api.Post("/edituser/deleteuser", null, obj)
                       .success(function (Response) {
                           $window.location.href = "/users";
                       })
                   .error(function (error) {
                       status = 'Unable to load data from api call to Delete User: ' + error;
                       console.log(status);
                   })
                    }
                }
                else {
                    alert("قم بحفظ بيانات المستخدم اولا قبل حذفه");
                }
        }

        $scope.updateUser = function () {
            var data = {};
            data["user"] = $scope.ui.current_user;
            var access_data = [];
            for (var i = 0; i < $scope.ui.user_access.length; i++) {
                console.debug($scope.ui.user_access[i]);
                if ($scope.ui.user_access[i] == 1 || $scope.ui.user_access[i] == 'True')
                    access_data.push($scope.ui.access[i].key);
            }
            data["access"] = access_data;
                api.Post("/edituser/UpdateUser", null, data)
                .success(function (Response) {
                    alert("تم التعديل بنجاح");
                    $window.location.href = "/users";
                })
            .error(function (error) {
                status = 'Unable to load data from api call to update user: ' + error;
                console.log(status);
            })
               
        }

        $scope.addUser = function () { 
            var data={};
            data["user"] = $scope.ui.current_user;
            var access_data = [];
            for (var i = 0; i < $scope.ui.user_access.length; i++) {
                if ($scope.ui.user_access[i] == 1)
                    access_data.push($scope.ui.access[i].key);
            }
            console.debug(access_data);
            data["access"] = access_data;
            api.Post("/edituser/CreateUser", null, data)
              .success(function (Response) {
                  if (Response.exists == false) {
                      alert("تم اضافة المستخدم بنجاح");
                      $window.location.href = "/users";
                  }
                  else {
                      alert("اسم المستخدم الذي ادخلته موجود مسبقا. يرجي اختيار اسم جديد");
                  }
              })
            .error(function (error) {
                status = 'Unable to load data from api call to Create User: ' + error;
                console.log(status);
            })
        };

        $scope.submitUser =function()
        {    if ($scope.ui.current_user.user_id != null) {
                    $scope.updateUser();
                }
                else {
                    $scope.addUser();
                }
        }; 

        if ($scope.ui.current_user) {

            if ($scope.ui.current_user.user_active == 1) {
                $scope.ui.active = 1;
                $scope.ui.inactive = 0;
            }
            else {
                $scope.ui.inactive = 1;
                $scope.ui.active = 0;
            }
        }
        else {
            $scope.ui.current_user.user_access = [];
            $scope.ui.inactive = 1;
            $scope.ui.active = 0;
            $scope.ui.deletebtn = false; 
        }

        $scope.GetAccess();


    }]);

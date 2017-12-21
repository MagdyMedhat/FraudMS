
App.factory("UsersModel", ["$rootScope", function ($rootScope) {

    var data = {};

    data.user_id = null;
    data.user_name = null;
     
    return {
        data: data
    };
}]);

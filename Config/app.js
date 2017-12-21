
var App = angular.module('App', ['webStorageModule', 'ui.bootstrap']);

App.factory("api", ["$http", "$rootScope", "$q", function ($http, $rootScope, $q) {

    function send(method, url, params, data) {
        return $http({
            method: method,
            url: url,
            params: params,
            data: data
        });
    }

    function Get(url, params, data) { return send("GET", url, params, data); }

    function Post(url, params, data) { return send("POST", url, params, data); }

    return {
        Get: Get,
        Post: Post
    };
}]);

App.factory("Utility", ["$rootScope", function ($rootScope) {

    function FixJsonDate(json_date) {
        var date = new Date(parseInt(json_date.substr(6)));
        var properlyFormatted = ("0" + date.getDate()).slice(-2) + '/' + ("0" + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
        return properlyFormatted;
    }

    function ExportToExcel(TableName, FileName) {
        $("#" + TableName).btechco_excelexport({
            containerid: TableName,
            datatype: $datatype.Table,
            filename: FileName
        });
    }


    return {
        FixJsonDate: FixJsonDate,
        ExportToExcel: ExportToExcel
    };
}]);

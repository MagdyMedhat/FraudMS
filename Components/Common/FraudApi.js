
App.factory("FraudsApi", ["api", "$rootScope", "$window", "webStorage", function (api, $rootScope, $window, webStorage) {

    var ControllerUrl = "/FraudApi/";

    var model = {};

    GetConnectionDomains = function () {
        api.Get(ControllerUrl + "GetConnectionDomains").success(function (response) {
            model.connection_domains = response;
            model.connection_domains.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetConnectionDomains ' + error;
            console.log(status);
        })
    }
    GetRegions = function () {
        api.Get(ControllerUrl + "GetRegions").success(function (ret) {
            model.regions = ret;
            model.regions.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetRegions ' + error;
            console.log(status);
        })
    }
    GetCities = function () {
        api.Get(ControllerUrl + "GetCities").success(function (ret) {
            model.cities = ret;
            model.cities.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetCities ' + error;
            console.log(status);
        })
    }
    GetFraudStatus = function () {
        api.Get(ControllerUrl + "GetFraudStatus").success(function (ret) {
            model.fraudStatus = ret;
            model.fraudStatus.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetFraudStatus ' + error;
            console.log(status);
        })
    }
    GetFraudTypes = function () {
        api.Get(ControllerUrl + "GetFraudTypes").success(function (ret) {
            model.fraudTypes = ret;
            model.fraudTypes.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetFraudTypes ' + error;
            console.log(status);
        })
    }
    GetNationalities = function () {
        api.Get(ControllerUrl + "GetNationalities").success(function (ret) {
            model.nationalities = ret;
            model.nationalities.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetNationalities ' + error;
            console.log(status);
        })
    }
    GetGSMTypes = function () {
        api.Get(ControllerUrl + "GetGSMTypes").success(function (ret) {
            model.GSMTypes = ret;
            model.GSMTypes.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetGSMTypes ' + error;
            console.log(status);
        })
    }
    GetUsers = function () {
        api.Get(ControllerUrl + "GetUsers").success(function (ret) {
            model.users = ret;
            model.users.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetUsers ' + error;
            console.log(status);
        })
    }
    GetCaseStatus = function () {
        api.Get(ControllerUrl + "GetCaseStatus").success(function (ret) {
            model.caseStatus = ret;
            model.caseStatus.splice(0, 0, "");
        })
        .error(function (error) {
            status = 'Unable to load data at function FraudsApi.GetCaseStatus ' + error;
            console.log(status);
        })
    }

    var CheckAccess = function (page_id) {
        var access_control = webStorage.local.get("access_control");

        if (access_control == null)
            $window.location.href = "FraudApi/error";

        var access = false;
        for (i = 0; i < access_control.length; i++) {
            if (access_control[i] == page_id) {
                access = true;
                break;
            }
        }
        if (access == false) {
            $window.location.href = "FraudApi/error";
        }
    }

    GetConnectionDomains();
    GetRegions();
    GetCities();
    GetFraudStatus();
    GetFraudTypes();
    GetNationalities();
    GetGSMTypes();
    GetUsers();
    GetCaseStatus();

    return {
        model: model,
        CheckUserAccess: CheckAccess
    };
}]);
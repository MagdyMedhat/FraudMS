
App.controller('EditController', ["FraudsApi", "Utility", "api", "$scope", "$rootScope", "webStorage", "$window",
function (FraudsApi, Utility, api, $scope, $rootScope, webStorage, $window) {

    FraudsApi.CheckUserAccess(1);
    setTimeout(function () {
        $('#container').css("display", "block");
    }, 500);

    //fetch combo boxes values
    $scope.ui = FraudsApi.model;
    $scope.ui.case = {};
    $scope.ui.caselines = {};
   
    //$scope.getCaseLineDateId = function (index) {
    //    return '#case_line_date' + i;
    //}

    //$scope.getCaseLineHijriId = function (index) {
    //    return "#case_line_hijri" + i;
    //}

    convertDateWrapper = function (i) {
        return function () {
            convertDate(i);
        };

    }

    convertDate = function (i) {

        var m = moment($('#case_line_date' + i).val(), 'D/M/YYYY');
        alert(m.format('iYYYY/iM/iD'));
        $('#case_line_hijri' + i).val(m.format('iYYYY/iM/iD'));

    }

    setTimeout(function () {
        for (var i = 0; i < $scope.ui.caselines.length; i++) {
            $('#case_line_date' + i).change(convertDateWrapper(i));
        }
        
    }, 2000);
    
       
        
        
    
    $('#case_date').change(function () {
        
        var m = moment($('#case_date').val(), 'D/M/YYYY');
            $('#case_hijri').val(m.format('iYYYY/iM/iD'));
        
    });
       
        
    

    $scope.MatchSelection = function (option, value) {

        if (option == value)
            return true;
        else
            return false;
    }

    $scope.Delete = function () {
        if (confirm("تأكيد مسح القضية ؟"))
            api.Get("/edit/delete", { id: JSON.parse(sessionStorage.case_id) })
             .success(function (Response) {
                 $window.location.href = "/search";
             })
            .error(function (error) {
                status = 'Unable to load data from api call to edit/Update: ' + error;
                console.log(status);
            })
    }
    $scope.Update = function () {

        //IMPORTANT: ng-model does not work with jquery updates, handle it manually here    
        $scope.case_bill_date = document.getElementById("case_bill_date").value;
        $scope.case_Activate_date = document.getElementById("case_Activate_date").value;
        $scope.case_date = document.getElementById("case_date").value;

        var data = {};
        data.retcase = $scope.ui.case;
        data.case_lines = $scope.ui.caselines;

        api.Post("/edit/Update", null, data)
            .success(function (Response) {
                alert("تم الحفظ بنجاح");
            })
        .error(function (error) {
            status = 'Unable to load data from api call to edit/Update: ' + error;
            console.log(status);
        })
    }

    $scope.addRow = function () {
        $scope.ui.caselines.push({});
        setTimeout(function () {
            $('a[href="' + this.location.pathname + '"]').parent().addClass('active');
            $("[data-toggle=tooltip]").tooltip();

            $('#sandbox-container input').datepicker({
                autoclose: true,
                format: 'dd/mm/yyyy'
            });

            $('#sandbox-container input').on('show', function (e) {
                console.debug('show', e.date, $(this).data('stickyDate'));

                if (e.date) {
                    $(this).data('stickyDate', e.date);
                }
                else {
                    $(this).data('stickyDate', null);
                }
            });

            $('#sandbox-container input').on('hide', function (e) {
                console.debug('hide', e.date, $(this).data('stickyDate'));
                var stickyDate = $(this).data('stickyDate');

                if (!e.date && stickyDate) {
                    console.debug('restore stickyDate', stickyDate);
                    $(this).datepicker('setDate', stickyDate);
                    $(this).data('stickyDate', null);
                }
            });
        }, 1000);

        setTimeout(function () {
            for (var i = 0; i < $scope.ui.caselines.length; i++) {
                $('#case_line_date' + i).change(convertDateWrapper(i));
            }

        }, 2000);

    }

    $scope.deleteRow = function (index) {

        if (index > -1) {
            $scope.ui.caselines.splice(index, 1);
        }
    }

    GetCase = function (id) {
        params = { 'id': id };
        api.Get("/edit/GetCase", params)
            .success(function (Response) {
                if (Response[0].case_bill_date)
                    Response[0].case_bill_date = Utility.FixJsonDate(Response[0].case_bill_date);
                if (Response[0].case_Activate_date)
                    Response[0].case_Activate_date = Utility.FixJsonDate(Response[0].case_Activate_date);
                if (Response[0].case_date)
                    Response[0].case_date = Utility.FixJsonDate(Response[0].case_date);

                $scope.ui.case = Response[0];
                $scope.ui.caselines = Response[1];
                for (var i = 0; i < $scope.ui.caselines.length; i++) {
                    if($scope.ui.caselines[i].case_line_date)
                    $scope.ui.caselines[i].case_line_date = Utility.FixJsonDate($scope.ui.caselines[i].case_line_date);
                }

            })
        .error(function (error) {
            status = 'Unable to load data from api call to edit/Update: ' + error;
            console.log(status);
        })
    }

    GetCase(JSON.parse(sessionStorage.case_id));

}]);
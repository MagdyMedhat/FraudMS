
App.controller('SearchController', 
    function (SearchModel, FraudsApi, Utility, api, $scope, $rootScope, $window, webStorage) {
        FraudsApi.CheckUserAccess(1);
        setTimeout(function () {
            $('#container').css("display", "block");
        }, 500);
        //fraud case search data
        $scope.search_data = SearchModel.data;
       
        //order by variable
        $scope.orderByType = 'case_date';
        $scope.sortReverse = false;

        //Fetch DropDown Menus
        $scope.ui = FraudsApi.model;
        $scope.ui.currentIndex = null;
        $scope.ui.cases = null;
        $scope.ui.case_lines = null;

        //pagination
        $scope.ui.tablePageSize = 50;
        $scope.ui.tableTotalCount = 0;
        $scope.TableRecords = [];
        $scope.ui.tableCurrentPage = 1;

        //search results details
        $scope.cases_count;
        $scope.acutal_loss;
        $scope.estimated_loss;
        $scope.disconnected_numbers;

        //export
        $scope.all_records;

        $scope.firstTimeSearch = true;


        DrawCharts = function () {
            $('#column_chart1').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ""
                },
                yAxis: { title: { text: '<span style="font-size:15px">العدد</span>' }, maxPadding: 0.2, endOnTick: false },
                xAxis: {
                    type: 'category'
                },

                legend: {
                    enabled: false
                },

                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false,
                            x: 10
                        }
                    }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                series: [{
                    name: 'عدد',
                    colorByPoint: true,
                    data: [{
                        name: "المحاضر",
                        y: $scope.cases_count,
                    }, {
                        name: "الأرقام المفصولة",
                        y: $scope.disconnected_numbers,
                    }]
                }]
            });

            $('#column_chart2').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ""
                },
                yAxis: { title: { text: "مجموع المبالغ" }, maxPadding: 0.2, endOnTick: false },
                xAxis: {
                    type: 'category'
                },

                legend: {
                    enabled: false
                },

                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false,
                            x: 20
                        }
                    }
                }
                ,
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                series: [{
                    name: 'مجموع',
                    colorByPoint: true,
                    data: [{
                        name: "المبالغ المفقودة",
                        y: $scope.acutal_loss,
                    }, {
                        name: "المبالغ المقدرة",
                        y: $scope.estimated_loss,
                    }]
                }]
            });

        }

        $scope.fixJsonDate = Utility.FixJsonDate;


        $scope.Search = function () {

            //IMPORTANT: ng-model does not work with jquery datepicker, handle it manually here    
            $scope.search_data.case_bill_date = document.getElementById("case_bill_date").value;
            $scope.search_data.case_Activate_date = document.getElementById("case_Activate_date").value;
            $scope.search_data.case_date = document.getElementById("case_date").value;
            $scope.search_data.case_date2 = document.getElementById("case_date2").value;
            $scope.search_data.case_line_date = document.getElementById("case_line_date").value;
            $scope.search_data.case_line_date2 = document.getElementById("case_line_date2").value;

            $scope.search_data.page_size = $scope.ui.tablePageSize;
            $scope.search_data.current_page = $scope.ui.tableCurrentPage;

            $scope.SearchSummary();
        }
        $scope.SearchSummary = function () {
            $("#loading").css("display", "block");
            api.Get("/search/Search", $scope.search_data)
                .success(function (Response) {
                    $("#loading").css("display", "none");

                    //fetch summary details
                    $scope.cases_count = Response.cases_count;
                    $scope.acutal_loss = Response.acutal_loss;
                    $scope.estimated_loss = Response.estimated_loss;
                    $scope.disconnected_numbers = Response.disconnected_numbers;

                    //pagination
                    $scope.ui.tableTotalCount = Response.cases_count;

                    DrawCharts();
                    $scope.Paginate();

                })
            .error(function (error) {
                status = 'Unable to load data from api call to search/search: ' + error;
                console.log(status);
            })
        }

        $scope.EditCase = function (id) {
            sessionStorage.case_id = JSON.stringify(id);
            $window.location.href = "/edit/";//index/" + id;
        }

        $scope.OpenCase = function (currentId) {
            var i = 0;
            for (i; i < $scope.ui.tablePageSize; i++) {
                if ($scope.ui.cases[i].case_id == currentId)
                    break;
            }
            $scope.ui.currentIndex = i;
            //used in viewModal.cshtml


            //convert gregorian date to hijri
            if ($scope.ui.cases[$scope.ui.currentIndex].case_date) {
                var m = moment($scope.ui.cases[$scope.ui.currentIndex].case_date, 'YYYY/M/D');
                $scope.ui.cases[$scope.ui.currentIndex].case_hijri = m.format('iYYYY/iM/iD');
            }
            
            //m = moment('1410/8/28', 'iYYYY/iM/iD'); // Parse a Hijri date. 
            //console.debug(m.format('iYYYY/iM/iD [is] YYYY/M/D'));

        }

        $scope.Paginate = function () {
            $scope.search_data.current_page = $scope.ui.tableCurrentPage;
            $("#loading").css("display", "block");
            api.Get("/search/GetPage", $scope.search_data)
                 .success(function (Response) {
                     $("#loading").css("display", "none");

                     //fix dates format
                     for (var i = 0; i < Response.cases.length; i++) {
                         if (Response.cases[i].case_bill_date)
                             Response.cases[i].case_bill_date = Utility.FixJsonDate(Response.cases[i].case_bill_date);
                         if (Response.cases[i].case_Activate_date)
                             Response.cases[i].case_Activate_date = Utility.FixJsonDate(Response.cases[i].case_Activate_date);
                         if (Response.cases[i].case_date)
                             Response.cases[i].case_date = Utility.FixJsonDate(Response.cases[i].case_date);

                     }
                     //fetch cases and case lines
                     $scope.ui.cases = Response.cases;
                     $scope.ui.case_lines = Response.case_lines;

                     $scope.TableRecords = $scope.ui.cases;

                     

                 })
            .error(function (error) {
                status = 'Unable to load data from api call to search/search: ' + error;
                console.log(status);
            })
        }

        $scope.setSearchParameters = function () {
            if (webStorage.local.has('statsToken')) {
                webStorage.local.remove('statsToken');
                var statsParameters = webStorage.local.get('statSearchParameters');
                $scope.search_data.case_user_id = statsParameters.case_user_id;
                $scope.search_data.case_connection_domain = statsParameters.case_connection_domain;
                document.getElementById('case_date').value = statsParameters.case_date;
                document.getElementById('case_date2').value = statsParameters.case_date2;
            }
            else if ($scope.firstTimeSearch) {
                $scope.firstTimeSearch = false;
                $scope.search_data.case_user_id = "";
            }

        }

        $scope.exportCasetable = function () {
            //Utility.ExportToExcel("mytable01", "Search Results");
            api.Get("/search/ExportAll", $scope.search_data)
            .success(function (response) {
                var data = [];
                var numFields = 22;


                //remove foreign key columns
                for (var i = 0; i < response.length; i++) {
                    if(response[i][0].case_Activate_date)
                        response[i][0].case_Activate_date = Utility.FixJsonDate(response[i][0].case_Activate_date);
                    if (response[i][0].case_date)
                        response[i][0].case_date = Utility.FixJsonDate(response[i][0].case_date);
                    if (response[i][0].case_bill_date)
                        response[i][0].case_bill_date = Utility.FixJsonDate(response[i][0].case_bill_date);
                    if (response[i][1].case_line_date)
                        response[i][1].case_line_date = Utility.FixJsonDate(response[i][1].case_line_date);
                    if (response[i][1].case_line_activate_date)
                        response[i][1].case_line_activate_date = Utility.FixJsonDate(response[i][1].case_line_activate_date);
                    //if (response[i][0].case_comment)
                        delete response[i][0].case_comment;
                    //if (response[i][0].case_status)
                        delete response[i][0].case_status;
                    //if (response[i][0].case_nationality_id)
                        delete response[i][0].case_nationality_id;
                    //if (response[i][0].case_fraudStatus_id)
                        delete response[i][0].case_fraudStatus_id;
                    //if (response[i][0].case_region)
                        delete response[i][0].case_region;
                    //if (response[i][1].case_fraudType_id)
                        delete response[i][1].case_fraudType_id;
                    //if (response[i][1].case_gsm_id)
                        delete response[i][1].case_gsm_id;
                    //if (response[i][1].case_id)
                        delete response[i][1].case_id;                 
                    //if (response[i][1].case_line_billing_user)
                        delete response[i][1].case_line_billing_user;
                    //if (response[i][1].case_line_connection_domain)
                        delete response[i][1].case_line_connection_domain;
                    //if (response[i][1].case_user_id)
                        delete response[i][1].case_user_id;

                }

                //case headers
                //var caseHeaders = Object.getOwnPropertyNames(response[0]);
                var caseHeadersObj = {
                    field1: 'bulk insertion', field2: 'تاريخ التأسيس', field3: 'المبلغ المفقود', field4: 'دورة الفوترة', field5: 'تاريخ الفوترة', field6: 'الفوترة هجري',
                    field7: 'المستخدم المخالف', field8: 'المدينة',  field9: 'الحد الإئتماني', field10: 'اسم المشترك', field11: 'رقم المشترك', field12: 'التاريخ الميلادي',
                    field13: 'عدد الأرقام المفصولة', field14: 'الفقد المتوقع', field15: 'التاريخ الهجري', field16: 'رقم القضية', field17: 'رقم الهوية', field18: 'النظام المصدر',
                    field19: 'الحالة', field20: 'حالة الإختلاس', field21: 'الجنسية', field22: 'المنطقة'
                };
                //for (var i = 0; i < caseHeaders.length; i++)
                //    caseHeadersObj[caseHeaders[i]] = caseHeaders[i];

                //caseline headers
                //var caseLineHeaders = Object.getOwnPropertyNames(response[1]);
                var caseLineHeadersObj = {
                    field1: 'رقم الحساب', field2: 'المبلغ', field3: 'تاريخ التأسيس',  field4: 'التاريخ الميلادي', field5: 'وصف القضية',
                    field6: 'التاريخ الهجري', field7: 'ملاحظة', field8: 'المستخدم المخالف', field9: 'نوع الإختلاس', field10: 'نوع الفاتورة',
                    field11: 'نوع الخدمة', field12: 'رقم الخدمة', field13: 'مدخل البيانات', field14: '', field16: '', field17: '', field18: '', field19: '', field20: '', field21: '', field22: ''
                };
                var headers = {
                    0: "'المبلغ المفقود",
                    1: "دورة الفوترة",
                    2: "تاريخ التأسيس",
                    3: "المستخدم المخالف",
                    4: "رقم المشترك",
                    5: "الحالة",               
                    6: "التاريخ الهجري",
                    7: "النظام المصدر",                 
                8: "المدينة",
                9: "تاريخ الفوترة",
                10: "الجنسية",
                11: "رقم الهوية",
                12: "التاريخ الميلادي",
                13: "حالة الإختلاس",
                14: "المنطقة",
                15: "عدد الأرقام المفصولة",
                16: "الفقد المتوقع",
                17: "الفوترة هجري",
                18: "الحد الإئتماني",
                19: "bulk insertion",
                20: "رقم القضية",
                21: "رقم الهوية",
                22: "المستخدم المخالف",
                23: "نوع الإختلاس",
                24: "رقم الخدمة",
                25  : "نوع الخدمة",
                26: "ملاحظة",
                27: "رقم الحساب",
                28: "تاريخ التأسيس",
                29: "مدخل البيانات",
                30: "التاريخ الهجري",
                31: "المبلغ",
                32: "نوع الاشتراك",
                33: "التاريخ الميلادي",
                34: "وصف القضية"
                };
                //for (var i = 0; i < caseLineHeaders.length; i++)
                //    caseLineHeadersObj[caseLineHeaders[i]] = caseLineHeaders[i];

                //response.splice(0, 0, caseHeadersObj);

                ////copy case and caseline rows into an object with same field names
                //var data = [];
                //for (var i = 0; i < response.length; i++) {
                //    var obj = {}, propName = 'field';
                //    for (var j = 0; j < numFields; j++) {
                //        var index = propName + (j + 1);
                //        var propertyNames = Object.getOwnPropertyNames(response[i]);
                //        if (response[i][propertyNames[j]])
                //            obj[index] = response[i][propertyNames[j]];
                //        else
                //            obj[index] = '';
                //    }
                //    data.push(obj);
                //}


                //insert case and caseline headers based on an exculsive property
                //for (var i = 0; i < response.length; i++) {
                //    if (response[i].case_ID_number) {
                //        response.splice(i, 0, caseHeadersObj);
                //        i++;
                //    }
                //    if (response[i].amount) {
                //        response.splice(i, 0, caseLineHeadersObj);
                //        i++;
                //    }

                //}
                var data = [];
                var caseProps = Object.getOwnPropertyNames(response[0][0]);
                var caselineProps = Object.getOwnPropertyNames(response[0][1]);
                for (var i = 0; i < response.length; i++) {
                    var arr = [];
                    for (var j = 0; j < caseProps.length; j++)
                        arr.push(response[i][0][caseProps[j]]);
                    for (var j = 0; j < caselineProps.length; j++)
                        arr.push(response[i][1][caselineProps[j]]);
                    data.push(arr);
                }
                data.unshift(headers);
                alasql('SELECT * INTO XLSX("report.xlsx",{headers:false}) FROM ?', [data]); // response[1] is the actual records from database
            })
            .error(function (error) {
                status = 'Unable to load data from api call to search/ExportAll: ' + error;
                console.log(status);
            })
            //var headers = ["header1", "header2", "header3"];
            //var data = [["row1", "row1", "row1"],
            //            ["row2", "row2", "row2"],
            //            ["row3", "row3", "row3"]];
            //data.splice(0, 0, headers);
            //alasql('SELECT * INTO XLSX("report.xlsx",{headers:false}) FROM ?', [data]); // response[1] is the actual records from database
        }

        $scope.search_data.case_user_id = webStorage.local.get('user_id');

        $scope.setSearchParameters();
        $scope.Search();


    });

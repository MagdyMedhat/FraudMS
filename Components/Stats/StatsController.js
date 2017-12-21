
App.controller('StatsController', ["FraudsApi", "Utility", "api", "$scope", "$rootScope", "$window", "webStorage",
    function (FraudsApi, Utility, api, $scope, $rootScope, $window, webStorage) {
        FraudsApi.CheckUserAccess(2);
        setTimeout(function () {
            $('#container').css("display", "block");
        }, 500);
        //Fetch DropDown Menus
        $scope.ui = FraudsApi.model;

        //Summary Details
        $scope.ui.disconnected_numbers;
        $scope.ui.opened_cases;
        $scope.ui.closed_cases;
        $scope.ui.cases_count;
        $scope.ui.actual_loss;
        $scope.ui.estimated_loss;
        $scope.ui.disconnected_numbers;

        //Statistics count
        $scope.ui.fraud_status_stats;
        $scope.ui.fraud_type_stats;

        //Search Form
        $scope.ui.search_form = {};
        $scope.ui.search_form.case_date;
        $scope.ui.search_form.case_date2;
        $scope.ui.search_form.case_connection_domain;
        $scope.ui.search_form.case_user_id;


        $scope.Delete = function () {
            $scope.ui.search_form = {};
            $scope.ui.search_form.case_date;
            $scope.ui.search_form.case_date2;
            $scope.ui.search_form.case_connection_domain;
            $scope.ui.search_form.case_user_id;
            $scope.Search();
        }
        DrawCharts = function () {
            $('#column_chart1').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: ""
                },
                yAxis: { title: { text: "العدد" }, maxPadding: 0.2, endOnTick: false },
                xAxis: {
                    type: 'category'
                },

                legend: {
                    enabled: false
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:10">{series.name}: </td>' +
                        '<td style="padding:0"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false,
                            x: 30
                        }
                    },
                    events: {
                        click: function (event) {
                            $scope.sendSearchParameters();
                            $window.location.href = '/Search/Index';
                        }
                    }
                },
                series: [{
                    name: 'عدد',
                    colorByPoint: true,
                    data: [{
                        name: "المحاضر",
                        y: $scope.ui.cases_count,
                    }, {
                        name: "المحاضر المفتوحة",
                        y: $scope.ui.opened_cases,
                    }, {
                        name: "المحاضر المغلقة",
                        y: $scope.ui.closed_cases,
                    }, {
                        name: "الأرقام المفصولة",
                        y: $scope.ui.disconnected_numbers,
                    }],
                    point: {
                        events: {
                            click: function (event) {
                                $scope.sendSearchParameters();
                                $window.location.href = '/Search/Index';
                            }
                        }
                    }
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
                }, tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:30">{series.name}: </td>' +
                        '<td style="padding:20"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },

                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false,
                            x: 30
                        }
                    },
                    events: {
                        click: function (event) {
                            $scope.sendSearchParameters();
                            $window.location.href = '/Search/Index' ;
                        }
                    }
                },
                series: {
                    name: 'مجموع',
                    colorByPoint: true,
                    data: [{
                        name: "المبالغ المفقودة",
                        y: $scope.ui.actual_loss,
                    }, {
                        name: "المبالغ المقدرة",
                        y: $scope.ui.estimated_loss,
                    }],
                    point: {
                        events: {
                            click: function (event) {
                                $scope.sendSearchParameters();
                                $window.location.href = '/Search/Index';
                            }
                        }
                    }
                }
            });
            var intPadding = 30;
            var data = [];
            for (i = 0; i < $scope.ui.fraud_type_stats.length; i++) {
                var arr = [];
                arr[0] = $scope.ui.fraud_type_stats[i].fraud_type;
                arr[1] = $scope.ui.fraud_type_stats[i].count;
                data.push(arr);
            }

            $('#pie_chart1').highcharts({
                chart: {
                    type: 'pie'
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    x: 200
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{point.percentage:.1f}%',
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:30">{series.name}: </td>' +
                        '<td style="padding:20"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    pie: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    },
                    events: {
                        click: function (event) {
                            $scope.sendSearchParameters();
                            $window.location.href = '/Search/Index' ;
                        }
                    }
                },
                series: [{
                    name: 'نوع الاختلاس',
                    colorByPoint: true,
                    data: data,
                    point: {
                        events: {
                            click: function (event) {
                                $scope.sendSearchParameters();
                                $window.location.href = '/Search/Index';
                            }
                        }
                    }
                }]
            });

            var data2 = [];
            for (i = 0; i < $scope.ui.fraud_status_stats.length; i++) {
                data2[i] = [$scope.ui.fraud_status_stats[i].fraud_status, $scope.ui.fraud_status_stats[i].count]
            }

            $('#pie_chart2').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{point.percentage:.1f}%',
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:30">{series.name}: </td>' +
                        '<td style="padding:20"><b>{point.y}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    pie: {
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'حالة الاختلاس',
                    colorByPoint: true,
                    data: data2,
                    point: {
                        events: {
                            click: function (event) {
                                $scope.sendSearchParameters();
                                $window.location.href = '/Search/Index';
                            }
                        }
                    }
                }]
            });

            setTimeout(function () {
                var legend = $('.highcharts-legend-item>text')[0];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[1];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[2];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[3];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[4];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[5];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[6];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[7];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[8];
                legend.setAttribute('x', -10);
                legend = $('.highcharts-legend-item>text')[9];
                legend.setAttribute('x', -10);

                console.log(legend);
            }, 500);
        }

        $scope.Search = function () {
            //IMPORTANT: ng-model does not work with jquery datepicker, handle it manually here    
            $scope.ui.search_form.case_date = document.getElementById("case_date").value;
            $scope.ui.search_form.case_date2 = document.getElementById("case_date2").value;
            $("#loading").css("display", "block");
            api.Get("/stats/Search", $scope.ui.search_form)
                .success(function (Response) {
                    $("#loading").css("display", "none");
                    //fetch response objects
                    $scope.ui.opened_cases = Response.opened_cases;
                    $scope.ui.closed_cases = Response.closed_cases;
                    $scope.ui.cases_count = Response.cases_count;
                    $scope.ui.actual_loss = Response.actual_loss;
                    $scope.ui.estimated_loss = Response.estimated_loss;
                    $scope.ui.disconnected_numbers = Response.disconnected_numbers;
                    $scope.ui.fraud_status_stats = Response.fraud_status_stats;
                    $scope.ui.fraud_type_stats = Response.fraud_type_stats;

                    DrawCharts();

                })
            .error(function (error) {
                status = 'Unable to load data from api call to stats/search: ' + error;
                console.log(status);
            })
        }

        $scope.sendSearchParameters = function () {
            webStorage.local.add('statsToken', true);
            $scope.SetDates();
            console.debug($scope.ui.search_form);
            webStorage.local.add('statSearchParameters', $scope.ui.search_form);
        }
        $scope.SetDates = function () {
            $scope.ui.search_form.case_date = document.getElementById('case_date').value;
            $scope.ui.search_form.case_date2 = document.getElementById('case_date2').value;
        }

        $scope.Search();
    }]);

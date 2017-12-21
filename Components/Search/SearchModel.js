
App.factory("SearchModel", ["$rootScope", function ($rootScope) {

    var data = {};

    data.case_connection_domain = null;
    data.case_region = null;
    data.case_city = null;
    data.case_fraudStatus_id = null;
    data.case_fraudType_id = null;
    data.case_customer_name = null;
    data.case_customer_number = null;
    data.case_ID_number = null;
    data.case_nationality_id = null;
    data.case_gsm_id = null;
    data.case_bill_cycle = null;
    data.case_bill_date = null;
    data.case_bill_hdate = null;
    data.case_source = null;
    data.case_user_id = null;
    data.case_date = null;
    data.case_date2 = null;
    data.case_hijri = null;
    data.case_id = null;
    data.case_billing_user2 = null;
    data.case_billing_user = null;
    data.case_status = null;
    data.case_disconnected_numbers_count = null;
    data.case_Activate_date = null;
    data.service_no = null;
    data.account_no = null;
    data.current_page = null;
    data.page_size = null;

    return {
        data: data
    };
}]);

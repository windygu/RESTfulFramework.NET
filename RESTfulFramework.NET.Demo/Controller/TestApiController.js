var agModele = angular.module("app", []);
agModele.controller("ApiInfoController",
    function ($scope) {
        $scope.fullApiUrl = "";
        $scope.api = "";
        $scope.id = "";
        //$scope.token = "1";
        $scope.tokenShow = "hidden";
        //取api信息列表
        var db = new DbSqliteHelper();
        //取所有API接口信息
        db.GetAllTestInfo(function (data, textStatus, jqXHR) {

            if (textStatus == "success") {
                if (data.Code > 0) {
                    apiInfos = data.Msg;
                    $scope.apiInfoList = apiInfos;
                    $scope.apiInfoRequestMethodList = [{ "name": "POST" }, { "name": "GET" }];
                    $scope.apiFormList = [];
                    $scope.apiInfoSelected = $scope.apiInfoList[0];
                    $scope.apiInfoRequestMethod = $scope.apiInfoRequestMethodList[0];
                    $scope.$apply();

                    try {
                        $('#apiNameList').selectmenu('refresh', true);
                    } catch (e) { }

                    try {
                        $('#apiRequestMode').selectmenu('refresh', true);
                    } catch (e) { }

                }
                else {
                    alert("获取接口信息列表失败！");
                }
            }
            else {
                alert("获取接口信息列表失败！");
            }

        });

        //单击测试按钮事件
        $scope.TestApiClick = function () {
            $("#responseView").JSONView({ request: "请求ing..." });
            localStorage.token = $scope.token;
            var apiInfo = $scope.apiInfoSelected;
            apiInfo.token = localStorage.token;
            if ($scope.fullApiUrl == undefined || $scope.fullApiUrl == null || $scope.fullApiUrl == "") {
                $scope.fullApiUrl = apiInfo.url;
            }
            var dataHttpHelper = new DataHttpHelper();
            try {
                if (apiInfo.requestMethod == "GET") {
                    if ($scope.fullApiUrl.indexOf("http", 0) >= 0) {
                        $.getJSON($scope.fullApiUrl, "", function (data, textStatus, jqHRX) {
                            if (textStatus == "success") {
                                $("#responseView").JSONView(data);
                                if ($scope.fullApiUrl.indexOf("/login?") > 0)
                                {
                                    localStorage.token = data.Msg.Token;
                                    $scope.token = localStorage.token;
                                }
                            }
                            else {
                                alert("调用接口失败，请重试！");
                            }
                        });
                    }
                    else {
                        dataHttpHelper.GetJson($("#bodyTextarea").val(), apiInfo.api, apiInfo.token, function (data, textStatus, jqXHR) {
                            if (textStatus == "success") {
                                $("#responseView").JSONView(data);
                            }
                            else {
                                alert("调用接口失败，请重试！");
                            }
                        });
                    }
                }
                else {
                    dataHttpHelper.PostJson($("#bodyTextarea").val(), apiInfo.api, apiInfo.token, function (data, textStatus, jqXHR) {
                        if (textStatus == "success") {
                            $("#responseView").JSONView(data);
                        }
                        else {
                            alert("调用接口失败，请重试！");
                        }
                    });
                }
            } catch (e) {
                alert(e.message);
            }

        };

        //Combobox Api切换
        $scope.SwithApi = function () {
            $scope.tokenShow = "hidden";
            $scope.fullApiUrl = "";
            var apiInfo = $scope.apiInfoSelected;
            $scope.api = apiInfo.api;
            $scope.id = apiInfo.id;
            $scope.token = localStorage.token;
            $scope.apiFormList = [];
            //URL地址参数
            if (apiInfo.url != "" && apiInfo.url != undefined && apiInfo.url != null && apiInfo.url.indexOf("http") >= 0) {
                var para = getUrlArgObject(apiInfo.url);
                $scope.apiFormList = [];
                var paraUrl = "";
                for (var i = 0; i < para.length; i++) {
                    $scope.apiFormList.push(para[i]);
                }
            }
            else {
                $scope.apiFormList = [];
                $scope.tokenShow = "visibility";
                //$scope.apiFormList.push({ key: "token", value: localStorage.token });
            }


            for (var i = 0; i < $scope.apiInfoRequestMethodList.length; i++) {
                if (apiInfo.requestMethod == $scope.apiInfoRequestMethodList[i].name) {
                    $scope.apiInfoRequestMethod = $scope.apiInfoRequestMethodList[i];
                    break;
                }
            }
            $scope.token = localStorage.token;
        };

        $scope.SwithRequestMethod = function () {

        };

        $scope.RefreshUrlInput = function () {


        };

        $scope.FormInputTextChange = function () {

            var partUrl = "";
            for (var i = 0; i < $scope.apiFormList.length; i++) {
                if (i == 0) {
                    partUrl = "?" + $scope.apiFormList[i].key + "=" + $scope.apiFormList[i].value;
                } else {
                    partUrl += "&" + $scope.apiFormList[i].key + "=" + $scope.apiFormList[i].value;
                }

            }
            $scope.fullApiUrl = getUrlWithOutArg($scope.apiInfoSelected.url) + partUrl;
        }

    }
);
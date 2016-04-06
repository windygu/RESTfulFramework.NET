var agModele = angular.module("app", []);
agModele.controller("ApiInfoController",
    function ($scope) {
    $scope.fullApiUrl="";
    //取api信息列表
    var db = new DbSqliteHelper();
    //取所有API接口信息
    db.GetAllTestInfo(function (data, textStatus, jqXHR) {

        if (textStatus == "success") {
            if (data.Code > 0) {
                apiInfos = data.Msg;
                $scope.apiInfoSelected = "";
                $scope.apiInfoList = apiInfos;
                $scope.apiInfoRequestMethod = "";
                $scope.apiInfoRequestMethodList = [{ "name": "POST" }, { "name": "GET" }];
                $scope.apiFormList = [];

                $scope.$apply();
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

        var apiInfo = $scope.apiInfoSelected;

        var dataHttpHelper = new DataHttpHelper();
        if (apiInfo.requestMethod == "GET") {
            if (apiInfo.url.indexOf("http", 0) >= 0) {
                $.getJSON(apiInfo.url, "", function (data, textStatus, jqHRX) {
                    $("#responseView").JSONView(data);
                });
            }
            else {
                dataHttpHelper.GetJson(apiInfo.body, apiInfo.api, apiInfo.token, function (data, textStatus, jqXHR) {
                    $("#responseView").JSONView(data);
                });
            }
        }
        else {
            dataHttpHelper.PostJson(apiInfo.body, apiInfo.api, apiInfo.token, function (data, textStatus, jqXHR) {
                $("#responseView").JSONView(data);
            });
        }
    };

    //Combobox Api切换
    $scope.SwithApi = function () {
        var apiInfo = $scope.apiInfoSelected;

        //URL地址参数
        var para = getUrlArgObject(apiInfo.url);
        $scope.apiFormList = [];
        var paraUrl="";
        for (var i = 0; i < para.length; i++) {
            $scope.apiFormList.push(para[i]);
        }
       
       
        for (var i = 0; i < $scope.apiInfoRequestMethodList.length; i++) {
            if (apiInfo.requestMethod == $scope.apiInfoRequestMethodList[i].name) {
                $scope.apiInfoRequestMethod = $scope.apiInfoRequestMethodList[i];
                break;
            }
        }

    };

    $scope.SwithRequestMethod = function () {

    };

    $scope.RefreshUrlInput = function () {


    };
   
    $scope.FormInputTextChange=function(){
 
        var partUrl="";
        for (var i = 0; i < $scope.apiFormList.length; i++) {
            if (i==0){
                partUrl="?"+    $scope.apiFormList[i].key + "="+  $scope.apiFormList[i].value;
            }else{
                partUrl+="&"+    $scope.apiFormList[i].key + "="+  $scope.apiFormList[i].value;
            }
            
        }
        $scope.fullApiUrl=getUrlWithOutArg( $scope.apiInfoSelected.url)+partUrl;
    }
   
}
);
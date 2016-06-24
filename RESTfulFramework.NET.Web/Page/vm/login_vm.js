/// <reference path="../scripts/library/jquery-1.8.0.js" />
/// <reference path="../scripts/library/jquery.mobile-1.4.5.js" />
/// <reference path="../scripts/library/vue.js" />
/// <reference path="extend_vue.js" />
/// <reference path="../core/DBHelper.ts" />

(function (selector) {

    var login_vm = {};
    var isLoadVM = false;

    //$(document).on("pagebeforeshow", selector, function (event, data) {
        login_vm = LoginVM(selector);
    //});

    //$(document).on("pagehide", selector, function (event, data) {
    //    login_vm.$destroy(true);
    //});
    function LoginVM(selector) {
        var vm = new Vue({
            el: selector,
            data: {
                user_name: "", password: "", is_login: true,token:"",user_id:""
            },
            methods: {
                //登陆
                LoginAction: function (userName, password) {
                    var context = this;
                    //$.mobile.loading("show", {
                    //    text: "",
                    //    textVisible: true,
                    //    theme: "z",
                    //    html: ""
                    //});
                    var userHttp = new UserHttpHelper();
                    userHttp.Login(userName, password, function (data, textStatus, jqXHR) {
                        if (data.Code > 0) {
                            //登陆成功
                            localStorage.token = data.Msg.Token;
                            localStorage.user_id = data.Msg.UserId;
                            localStorage.password = password;
                            context. token = data.Msg.Token;
                            context.    user_id = data.Msg.UserId;
                            //登陆成功
                            this.is_login = true;
                            //window.location.href = "index.html";
                           
                        }
                        else {
                            //登陆失败
                            //$("#loginTip").text("登陆失败，帐号密码不正确或用户不存在。");
                        }

                    });
                }
            },
            created: function () {
                //初始化
            }
        });
        return vm;
    }

})("#loginArea");
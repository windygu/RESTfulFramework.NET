/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
class UserHttpHelper extends HttpHelper {

    Login(userName: string, password: string, ResultFunction: any): void {
        var loginUrl = this.BulidUserServiceLoginUrl(userName, password);
        $.getJSON(loginUrl, function (data, textStatus, jqXHR) {   
                ResultFunction(data, textStatus);
        });
    }
}
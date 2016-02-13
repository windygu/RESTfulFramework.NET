
function GetPostUrl(token, api, type) {
    var baseUrl = "http://localhost:8735/RESTful/post?";
    var systemSecretKey = "88888888";
    var timestamp = Math.round(new Date().getTime() / 1000); 
    var sign = $.md5(token + api + type + timestamp + systemSecretKey).toUpperCase();
    return baseUrl + "token=" + token + "&api=" + api + "&type=" + type + "&timestamp=" + timestamp + "&sign=" + sign;
}

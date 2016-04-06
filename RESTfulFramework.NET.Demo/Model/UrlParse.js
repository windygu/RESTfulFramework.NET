


function getQueryString(url, name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = url.match(reg);

    if (r != null) return unescape(r[2]); return null;
}

//返回的是对象形式的参数
function getUrlArgObject(url) {
    var args = [];
    var query = url;//获取查询串
    var queryPara = query.split("?")[1];
    var pairs = queryPara.split("&");
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('=');//查找name=value
        if (pos == -1) {//如果没有找到就跳过
            continue;
        }
        var argname = pairs[i].substr(0, pairs[i].split("=")[0].length);//提取name
        var value = pairs[i].substr(pairs[i].split("=")[0].length + 1, pairs[i].split("=")[1].length);//提取value
        args.push({ key: argname, value: value });
    }
    return args;//返回对象
}
function getUrlWithOutArg(url) {
    return url.split("?")[0];
}
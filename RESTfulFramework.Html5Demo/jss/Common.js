/**
 * 验证手机号是否为正确号码
 * @param {string} phone 
 * @returns {bool} 
 */
function validate_phone(phone) {
    return /^1[3|4|5|7|8]\d{9}$/.test(phone);
}

/**
 * 获取URL参数值
 * @param {string} name 
 * @returns {string} 
 */
function getUrlParam(name) {
    //构造一个含有目标参数的正则表达式对象
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    //匹配目标参数
    var r = window.location.search.substr(1).match(reg);
    //返回参数值
    if (r != null) return unescape(r[2]);
    return null;
}

/**
 * 格式为时间
 * @param {datetime} datetime 
 * @param {string} format 
 * @returns {string} 
 */
function formatDate(datetime, format) {

    function formatNumber(number, fmt) {
        number = number + '';
        if (fmt.length > number.length) {
            return fmt.substring(number.length) + number;
        }
        return number;
    }

    var cfg = {
        MMM: ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二'],
        MMMM: ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二']
    },
    values = {
        y: datetime.getFullYear(),
        M: datetime.getMonth() + 1,
        d: datetime.getDate(),
        H: datetime.getHours(),
        m: datetime.getMinutes(),
        s: datetime.getSeconds(),
        S: datetime.getMilliseconds()
    };
    /*用正则表达式拆分日期格式各个元素*/
    var elems = format.match(/y+|M+|d+|H+|m+|s+|S+|[^yMdHmsS]/g);
    //将日期元素替换为实际的值
    for (var i = 0; i < elems.length; i++) {
        if (cfg[elems[i]]) {
            elems[i] = cfg[elems[i]][values[elems[i].charAt(0)]];
        } else if (values[elems[i].charAt(0)]) {
            elems[i] = formatNumber(values[elems[i].charAt(0)], elems[i].replace(/./g, '0'));
        }
    }
    return elems.join('');
}


/**
 * 倒计时
 * @param {int} ts 以秒为单位 
 * @param {} func 每隔一秒执行一次
 * @returns {} 
 */
function countdown_time(ts, func) {
    function showtime() {
        ts = ts - 1;
        if (ts >= 0) {
            func(ts);
            setTimeout(showtime, 1000);
        }
    }
    setTimeout(showtime, 1000);
}

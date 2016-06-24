var Tool = (function () {
    function Tool() {
    }
    /**
     * 生成伪guid
     */
    Tool.prototype.uuid = function () {
        var s = [];
        var hexDigits = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
            s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
        s[8] = s[13] = s[18] = s[23] = "-";
        var uuid = s.join("");
        return uuid;
    };
    /**
     * 验证手机号是否为正确号码
     * @param {string} phone
     * @returns {bool}
     */
    Tool.prototype.validate_phone = function (phone) {
        return /^1[3|4|5|7|8]\d{9}$/.test(phone);
    };
    /**
     * 格式为时间
     * @param {datetime} datetime 时间
     * @param {string} format 格式化时间的字符串，例如 yyyy-MM-dd HH:mm:ss
     * @returns {string} 返回已格式化的时间字符串
     */
    Tool.prototype.formatDate = function (datetime, format) {
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
        }, values = {
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
            }
            else if (values[elems[i].charAt(0)] >= 0) {
                elems[i] = formatNumber(values[elems[i].charAt(0)], elems[i].replace(/./g, '0'));
            }
        }
        return elems.join('');
    };
    /**
     * 判断是否是数组
     * @param o 对像
     */
    Tool.prototype.isArray = function (o) { return Object.prototype.toString.call(o) === '[object Array]'; };
    /**
     * 获取url指定参数的值
     * @param url url地址
     */
    Tool.prototype.GetRequest = function (url) {
        var strs = [];
        var theRequest = [];
        if (url.indexOf("?") != -1) {
            var str = url.substr(url.indexOf("?") + 1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest.push({ key: strs[i].split("=")[0], value: decodeURI(strs[i].split("=")[1]) });
            }
        }
        return theRequest;
    };
    return Tool;
}());
//# sourceMappingURL=Tool.js.map
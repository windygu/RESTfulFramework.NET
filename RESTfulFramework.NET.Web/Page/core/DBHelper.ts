/// <reference path="thenjs.d.ts" />
/// <reference path="sqlite.d.ts" />

/**
 * 对sqlite数据库的封装操作及联网
 */
class DBHelper {

    private DBName: string;

    constructor(dbName: string) {
        this.DBName = dbName;
    }

    public InitDB(): void {
        //创建日记表 
        var db = new lanxDB(this.DBName);
        db.init("note_details", [
            { name: 'id', type: 'char(36) NOT NULL' },
            { name: 'title', type: 'varchar(255) DEFAULT NULL' }, 
            { name: 'content', type: 'text' },
            { name: 'weather', type: 'varchar(50) DEFAULT NULL' },
            { name: 'user_id', type: 'char(36) NOT NULL' },
            { name: 'create_time', type: "DATETIME DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime'))" },
            { name: 'update_time', type: "DATETIME DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime'))" },
            { name: 'modified_time', type: "DATETIME DEFAULT (datetime(CURRENT_TIMESTAMP,'localtime'))" },
            { name: 'valid', type: "bit(1) NOT NULL DEFAULT  '1'" },
            { name: 'is_sync', type: "bit(1) NOT NULL DEFAULT  '0'" }
        ]);

    }

    /**
     * 推送数据(选推送到本地数据库，然后再到服务器)
     * @param entity 包含表名、字段值的实体
     */
    public PushData(entity: any, result: any, successResult: any, failResult: any, errorResult: any): void {

        var db = new lanxDB(this.DBName);

        //取表名
        var tableName = "";
        for (var key in entity) {
            tableName = key;
            break;
        }

        var data = entity[tableName];
        var id = data["id"];

        Thenjs(function (cont) {
            //取要操作的数据
            var regExp = new RegExp("^\\w\\w\\w\\w\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w$", "gi");
            if (!regExp.test(id)) {
                errorResult("id字段不是标准的uuid。");
                return;
            }

            //需要将is_sync设为0(判断data有没有is_sync字段，没有则手动添加is_sync字段)
            data.is_sync = 0;

            //查看数据是否存在本地数据库       
            db.switchTable(tableName).where({ id: id }).getData(
                function (dataResult: Array<Object>) {
                    Thenjs.nextTick(function () { cont(null, dataResult) });
                });
        })
            //取得该条记录在本地数据库的详细
            .then(function (cont, dataResult) {

                if (dataResult.length > 0 && dataResult[0] != "0") {
                    //如果存在，则更新记录
                    db.where({ id: id }).updateData(data,
                        function (dataResult2) {
                            Thenjs.nextTick(function () { cont(null, dataResult2) });
                        });
                }
                else {
                    //如果不存在，则创建记录
                    db.insertData([data],
                        function (dataResult2) {
                            Thenjs.nextTick(function () { cont(null, dataResult2) });
                        });
                }


            })
            //取得更新本地数据库后的结果(插入或更新)
            .then(function (cont, dataResult) {
                if (dataResult.length > 0 && dataResult[0] > 0) {     //如果更新记录成功

                    //过滤掉modified_time与valid字段
                    if (data.hasOwnProperty("modified_time")) {
                        delete data.modified_time;
                    }

                    if (data.hasOwnProperty("is_sync")) {
                        delete data.is_sync;
                    }

                    console.log(entity);
                    //推送到服务器,localStroge.token必需有效才能成功
                    try {
                        var dataHttpHelper = new DataHttpHelper();
                        dataHttpHelper.PushJson(JSON.stringify(entity),
                            function (_data) {
                                Thenjs.nextTick(function () { cont(null, _data); })
                            },
                            function (error) {
                                //此处是请求失败，则分情况处理
                                failResult();
                            }
                        );
                    } catch (exception) {
                        console.log(exception);
                        failResult();
                    }

                } else {    //如果更新记录失败
                    failResult();
                }
                console.log(result);
            })
            //取得请求的结果数据
            .then(function (cont, args) {
                //如返回成功，则需要将本地is_sync设为1                
                if (args.Code > 0) {
                    //将is_sync设为已同步
                    db.switchTable(tableName).where({ id: id }).updateData({ is_sync: 1 },
                        function (resultData: Array<Object>) {
                            Thenjs.nextTick(function () { cont(null, resultData); })
                        });
                }
                else {
                    //服务器返回失败，则分情况处理
                    failResult();
                }
            })
            .then(function (cont, args) {
                if (args.length > 0 && args[0] > 0) {
                    successResult("success");
                } else {
                    failResult();
                }
            });
    }

    /**
     * 推送数据(仅推送到本地数据库)
     * @param entity 包含表名、字段值的实体
     */
    public PushLocalData(entity: any, successResult: any, failResult: any, errorResult: any): void {

        var db = new lanxDB(this.DBName);
        //取表名
        var tableName = "";
        for (var key in entity) {
            tableName = key;
            break;
        }
        var data = entity[tableName];

        Thenjs(function (next) {
            //取要操作的数据   
            var id = data["id"];
            var regExp = new RegExp("^\\w\\w\\w\\w\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w$", "gi");
            if (!regExp.test(id)) {
                errorResult({ Code: -999, Msg: "id字段不是标准的uuid。" }, "local_fails", null);
                return;
            }
            //需要将is_sync设为0(判断data有没有is_sync字段，没有则手动添加is_sync字段)
            data.is_sync = 0;
            //查看数据是否存在本地数据库
            db.switchTable(tableName).where({ id: id }).getData(
                function (result: Array<Object>) {
                    console.log(result);
                    Thenjs.nextTick(function () {
                        next(null, result);
                    });
                });
        })
            .then(function (next, result) {
                if (result.length > 0 && result[0] != "0") {
                    //如果存在，则更新记录
                    db.where({ id: data["id"] }).updateData(data,
                        function (result) {
                            Thenjs.nextTick(function () { next(null, result); });
                        });
                }
                else {
                    //如果不存在，则创建记录
                    db.insertData([data],
                        function (result) {
                            Thenjs.nextTick(function () { next(null, result); });
                        });
                }
            })
            .then(function (next, result) {
                if (result.length > 0 && result[0] > 0) { //如果更新记录成功        
                    successResult();
                } else {
                    //如果更新记录失败
                    failResult();
                }
                console.log(result);
            });

    }
    /**
     * 删除数据,并同时删除服务器上的日志
     * @param entity
     * @param callback
     */
    public RemoveData(entity: any, successResult: any, failResult: any, errorResult: any): void {
        //取表名
        var tableName = "";
        for (var key in entity) {
            tableName = key;
            break;
        }

        //取要操作的数据
        var data = entity[tableName];
        var id = data["id"];
        var regExp = new RegExp("^\\w\\w\\w\\w\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w$", "gi");
        if (!regExp.test(id)) {
            failResult("id字段不是标准的uuid。");
            return;
        }
        //需要将is_sync设为0(判断data有没有is_sync字段，没有则手动添加is_sync字段)
        data.is_sync = 0;

        //查看数据是否存在本地数据库
        var db = new lanxDB(this.DBName);
        Thenjs(function (cont) {
            db.switchTable(tableName).where({ id: id }).getData(
                function (result: Array<Object>) {
                    console.log("获取即将被删除的那条数据信息。表名" + tableName);
                    console.log(result);
                    Thenjs.nextTick(function () { cont(null, result); });
                });
        })
            //取得本地数据库记录的详细信息
            .then(function (cont, args) {
                if (args.length > 0 && args[0] != "0") {
                    //如果存在，则更新记录
                    if (!data.hasOwnProperty("valid")) {
                        data.valid = 0;
                    }
                    db.where({ id: id }).updateData(data,
                        function (result) {
                            Thenjs.nextTick(function () { cont(null, result); });
                        });
                }
            })
            //取得更新本地数据库记录的结果
            .then(function (cont, args) {
                console.log(args);
                if (args.length > 0 && args[0] > 0) { //如果更新记录成功

                    //过滤掉modified_time与valid字段
                    if (data.hasOwnProperty("valid")) {
                        delete data.valid;
                    }

                    if (data.hasOwnProperty("modified_time")) {
                        delete data.modified_time;
                    }

                    if (data.hasOwnProperty("is_sync")) {
                        delete data.is_sync;
                    }

                    //推送到服务器,localStroge.token必需有效才能成功
                    var dataHttpHelper = new DataHttpHelper();

                    var json = JSON.stringify(entity);
                    dataHttpHelper.RemoveData(json,
                        function (dataResult) {
                            Thenjs.nextTick(function () { cont(null, dataResult); });
                        },
                        function (error) {
                            failResult("删除失败");
                        });

                } else {
                    //如果更新记录失败
                    failResult("删除(逻辑)本地记录失败。");
                }
            })
            //取得从服务器请求删记录的结果
            .then(function (cont, args) {
                //如返回成功，则需要将本地is_sync设为1
                if (args.Code > 0) {
                    //将is_sync设为已同步
                    db.switchTable(tableName).where({ id: id }).updateData({ is_sync: 1 },
                        function (result: Array<Object>) {
                            Thenjs.nextTick(function () { cont(null, result); });
                        });
                }
                else {
                    //服务器返回失败，则分情况处理
                    failResult("删除失败");
                }
            })
            //取得从本地数据库标记为同步的记录的结果
            .then(function (cont, args) {
                if (args.length > 0 && args[0] > 0) {
                    successResult("删除成功");
                }
                else {
                    failResult("删除失败");
                }
            })
    }

    /**
     * 仅删除本地的日记
     * @param entity
     * @param callback
     */
    public RemovieLocalData(entity: any, successResult: any, failResult: any, errorResult: any): void {
        try {
            //取表名
            var tableName = "";
            for (var key in entity) {
                tableName = key;
                break;
            }
            //取要操作的数据
            var data = entity[tableName];
            var id = data["id"];
            var regExp = new RegExp("^\\w\\w\\w\\w\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w$", "gi");
            if (!regExp.test(id)) {
                errorResult("id字段不是标准的uuid。");
                return;
            }
            //需要将is_sync设为0(判断data有没有is_sync字段，没有则手动添加is_sync字段)
            data.is_sync = 0;

            //查看数据是否存在本地数据库
            var db = new lanxDB(this.DBName);
            db.switchTable(tableName).where({ id: id }).getData(
                function (result: Array<Object>) {
                    console.log("获取即将被删除的那条数据信息。表名" + tableName + " 数据:" + result);
                    if (result.length > 0 && result[0] != "0") {
                        //如果存在，则更新记录
                        if (!data.hasOwnProperty("valid")) {
                            data.valid = 0;
                        }
                        db.where({ id: id }).updateData(data,
                            function (result) {
                                if (result.length > 0 && result[0] > 0) { //如果更新记录成功
                                    successResult();
                                } else {
                                    //如果更新记录失败
                                    failResult();
                                }
                            });
                    }
                    else {
                        failResult();
                    }
                });
        } catch (excetion) {
            errorResult();
        }
    }


    /**
     * 拉取数据
     * @param fetchEntity
     * @param callback
     */
    public PullData(fetchEntity: any, successResult: any, failResult: any, errorResult: any): void {
        try {
            var sum: number = 0;
            var dataList: Array<any>;
            var db = new lanxDB(this.DBName);
            //取表名
            var tableName = "";
            for (var key in fetchEntity.Entity.Main) {
                tableName = key;
                break;
            }

            var dataHttpHelper = new DataHttpHelper();
            dataHttpHelper.FetchJson(JSON.stringify(fetchEntity),
                function (data) {
                    if (data.Code > 0) {
                        var tool = new Tool();
                        if (tool.isArray(data.Msg)) {
                            if (data.Msg.length > 0) {
                                dataList = data.Msg;
                                UpdateToLocal(dataList[0]);
                            } else {
                                successResult();
                            }
                   
                        } else {
                            UpdateToLocal(data.Msg);
                        }
                    }
                    else {
                        failResult();
                    }
                },
                function () {
                    errorResult();
                });

            //更新到本地数据库
            function UpdateToLocal(data: any) {
                UpdateLocalSingleData(data);
            }

            //更新本地数据库表的单条数据
            function UpdateLocalSingleData(data: any) {
                db.switchTable(tableName).where({ id: data.id }).getData(
                    function (result: Array<any>) {
                        if (result.length > 0 && result[0] != "0") { //更新本地数据        
                            var tLocalUpdateTime = Date.parse(result[0].update_time);
                            var tServerUpdateTime = Date.parse(data.update_time);
                            if (tLocalUpdateTime > tServerUpdateTime) { //如果本地数据的更新时间大于服务器的时间
                                //忽略此条数据更新
                                //更新下一条
                                NextUpdateLocalSingleData();
                            } else { //如果本地数据的更新时间小于服务器的时间
                                if (!data.hasOwnProperty("is_sync")) {
                                    data.is_sync = 1;
                                }
                                db.where({ id: data.id }).updateData(data,
                                    function (result) {
                                        //更新下一条
                                        NextUpdateLocalSingleData();
                                    });
                            }
                        }
                        else { //插入本地数据
                            //将服务器返回的这条记录添加到本地数据库
                            if (!data.hasOwnProperty("is_sync")) {
                                data.is_sync = 1;
                            }
                            db.insertData([data],
                                function (result) {
                                    console.log(result);
                                    //更新下一条数据
                                    NextUpdateLocalSingleData();
                                });
                        }
                    });
            }

            //更新本地数据库表单条数据的回调,更新下一条
            function NextUpdateLocalSingleData() {
                sum++;
                if (dataList.length > sum) { //如果记录大于已同步的记录
                    UpdateToLocal(dataList[sum]);
                } else {
                    successResult();
                }
            }
        } catch (ex) {
            errorResult();
        }



    }

    /**
     * 推送本地未同步的数据到服务端
     * @param tableName 表名
     * @param successResult 成功回调
     * @param failResult 失败回调
     * @param errorResult 异常回调
     */
    public SyncPushData(tableName: string, successResult: any, failResult: any, errorResult: any) {
        try {
            var db = new lanxDB(this.DBName);
            var dataList: Array<any>;
            var sum = 0;
            //取本地库未同步至服务器的数据
            db.switchTable(tableName).where({ is_sync: 0 }).getData(
                function (result: Array<any>) {
                    if (result.length > 0 && result[0] != "0") {
                        dataList = result;
                        //将第一条数据推送到服务器          
                        CheckData(dataList[sum]);
                    }
                    else {
                        successResult();
                    }
                });

            //检测数据
            function CheckData(data: any) {
                try {
                    var _data = JSON.parse(JSON.stringify(data));
                    var regExp = new RegExp("^\\w\\w\\w\\w\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w-\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w\\w$", "gi");
                    if (!regExp.test(_data.user_id)) { //该条日记不属于任务用户，则重置为给当前用户。
                        _data.user_id = localStorage.getItem("user_id");
                        db.switchTable(tableName).where({ id: _data.id }).updateData({ user_id: localStorage.getItem("user_id") }, function () {
                            PushDataToServer(_data);
                        });
                    }
                    else {
                        PushDataToServer(_data);
                    }
                } catch (ex) {
                    //产生异常，此时已无法推送到服务器了，应该继续推送下一条
                    if (sum < dataList.length) {
                        CheckData(dataList[sum]);
                    }
                    else {
                        successResult();
                    }
                }
            }

            //推送数据
            function PushDataToServer(simpleData: any) {
                try {
                    //过滤掉modified_time字段
                    if (simpleData.hasOwnProperty("modified_time")) {
                        delete simpleData.modified_time;
                    }
                    //过滤掉is_sync字段
                    if (simpleData.hasOwnProperty("is_sync")) {
                        delete simpleData.is_sync;
                    }

                    var entityString = "{ \"" + tableName + "\":" + JSON.stringify(simpleData) + "}";
                    console.log("推送到服务端的数据。" + entityString);
                    //推送到服务器,localStroge.token必需有效才能成功
                    var dataHttpHelper = new DataHttpHelper();
                    dataHttpHelper.PushJson(entityString,
                        function (data) {
                            //如返回成功，则需要将本地is_sync设为1
                            console.log("服务端返回的结果。" + data);
                            if (data.Code > 0) {//推送成功
                                //将is_sync设为已同步
                                db.switchTable(tableName).where({ id: simpleData.id }).updateData({ is_sync: 1 },
                                    function (result: Array<Object>) {
                                        if (result.length > 0 && result[0] > 0) {
                                            PushToServerCallback();//推送下一条
                                        }
                                    });
                            }
                            else {
                                //服务器返回失败，则分情况处理
                                PushToServerCallback();//无论成功与否，推送下一条
                            }

                        },
                        function (error) {
                            PushToServerCallback();//无论异常与否，推送下一条
                        });
                } catch (ex) {
                    PushToServerCallback();//无论异常与否，推送下一条
                }
            }

            //推送完成后的回调，紧接着推送下一条
            function PushToServerCallback() {
                sum++;
                if (dataList.length > sum) {
                    CheckData(dataList[sum]);
                }
                else {
                    successResult();
                }
            }

        } catch (ex) {
            errorResult();
        } finally { }

    }

    /**
     * 取本地数据
     * @param tableName
     * @param _where
     * @param callback
     */
    public GetLocalValidData(tableName: string, _where: any, callback: any) {
        var db = new lanxDB(this.DBName);
        db.switchTable(tableName).where(_where).getData(
            function (result) {
                if (result.length > 0 && result[0] != "0") {
                    var dataList: Array<any> = [];
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].valid == 1 || result[i].valid == "1") {
                            dataList.push(result[i]);
                        }
                    }
                    callback(dataList);
                }
                else {
                    callback(result);
                }

            });
    }

}
class TestSqlite {
    Test(): void {
        var db = new lanxDB("testDB");
        db.init('channel_list', [{ name: 'id', type: 'integer primary key autoincrement' }, { name: 'name', type: 'text' }, { name: 'link', type: 'text' }, { name: 'cover', type: 'text' }, { name: 'updatetime', type: 'integer' }, { name: 'orders', type: 'integer' }]);
        db.init('feed_list', [{ name: 'parentid', type: 'integer' }, { name: 'feed', type: 'text' }]);

        db.switchTable('channel_list').insertData([{ name: 'aa', link: 'ss', updatetime: new Date().getTime() }, { name: 'bb', link: 'kk', updatetime: new Date().getTime() }],
            function () {

            });
        db.where({ name: 'aa' }).getData(function (result) {
            console.log(result);//result为Array
        });
        db.where({ name: 'aa' }).deleteData(function (result) {
            console.log(result[0]);//删除条数
        });
        db.where({ name: 'bb' }).saveData({ link: 'jj' }, function (result) {
            console.log(result);//影响条数
        })

    }


}
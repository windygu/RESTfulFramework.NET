interface IlanxDB {
    new (dbName: string): IlanxDB;
    getDBName(): string;
    init(tableName: string, colums: any): IlanxDB;
    createTable(colums: any): void;
    switchTable(tableName: any): IlanxDB;
    insertData(data: any, callback: any): IlanxDB;
    _where: string;
    where(where: any): IlanxDB;
    updateData(data: any, callback: any): void;
    saveData(data: any, callback: any): void;
    getData(callback: any): void;
    doQuery(sql: any, callback: any): void;
    deleteData(callback: any): void;
    dropTable(): void;
    _error: string;
    onfail(t: any, e: any): void;
    toArray(obj: any): any;
}
declare var lanxDB: IlanxDB; 
 
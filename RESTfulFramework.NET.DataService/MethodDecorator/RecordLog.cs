
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Reflection;

[module: RecordLog]

public class RecordLogAttribute : MethodDecoratorAttribute
{
    private dynamic _instance { get; set; }
    private MethodBase _method { get; set; }
    private object[] _args { get; set; }
    private string _logoType { get; set; }


    public override void Init(object instance, MethodBase method, object[] args)
    {
        _instance = instance;
        _method = method;
        _args = args;
        if (method.Name == "ApiHandler") _logoType = "接口通道ApiHandler";
        else if (method.Name == "InfoApiHandler") _logoType = "接口通道InfoApiHandler";
        else if (method.Name == "StreamApiHandler") _logoType = "接口通道StreamApiHandler";
    }

    public override void OnEntry()
    {
        var method = _method;
        var args = _args;
        var logManager = _instance.Context.LogManager as ILogManager;
        new Action(() =>
        {
            logManager.WriteLog($"描述：{_logoType}  方法名：{ method.Name}  参数：{Newtonsoft.Json.JsonConvert.SerializeObject(args)}");
        }).BeginInvoke(new AsyncCallback((iAsyncResult) => { }), null);
    }

    public override void OnException(Exception exception)
    {
        var method = _method;
        var args = _args;
        var logManager = _instance.Context.LogManager as ILogManager;
        new Action(() =>
        {
            logManager.WriteLog($"描述：{_logoType} 异常：{exception.ToString()}  方法名：{ method.Name}  参数：{Newtonsoft.Json.JsonConvert.SerializeObject(args)}");
        }).BeginInvoke(new AsyncCallback((iAsyncResult) => { }), null);
    }

    public override void OnExit()
    {

    }
}
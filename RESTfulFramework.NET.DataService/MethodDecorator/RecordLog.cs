using System;
using System.Reflection;

[module: RecordLog]

public class RecordLogAttribute : MethodDecoratorAttribute
{
    private dynamic _instance { get; set; }
    private MethodBase _method { get; set; }
    private object[] _args { get; set; }
    public override void Init(object instance, MethodBase method, object[] args)
    {
        _instance = instance;
        _method = method;
        _args = args;
    }

    public override void OnEntry()
    {
        throw new NotImplementedException();
    }

    public override void OnException(Exception exception)
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }
}
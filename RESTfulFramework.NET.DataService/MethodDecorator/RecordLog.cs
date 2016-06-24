using RESTfulFramework.NET.ComponentModel;
using System;
using System.Reflection;

[module: RecordLog]

public class RecordLogAttribute : MethodDecoratorAttribute
{
    private dynamic _instance { get; set; }
    private MethodBase _method { get; set; }
    private object[] _args { get; set; }
    private ILogManager _logManager { get; set; }
    private IJsonSerialzer _jsonSerialzer { get; set; }
    public override void Init(object instance, MethodBase method, object[] args)
    {
        _instance = instance;
        _method = method;
        _args = args;
        _logManager = _instance?.LogManager as ILogManager;
        _jsonSerialzer = _instance?.Serialzer as IJsonSerialzer;
    }

    public override void OnEntry()
    {
        try
        {
            if (_logManager != null)
            {
                _logManager.WriteLog(_jsonSerialzer.SerializeObject(_args));
            }
        }
        catch (Exception)
        {

            
        }
      
    }

    public override void OnExit()
    {
       
    }

    public override void OnException(Exception exception)
    {
        if (_logManager != null)
        {
            _logManager.WriteLog(exception.ToString());
        }
    }
}
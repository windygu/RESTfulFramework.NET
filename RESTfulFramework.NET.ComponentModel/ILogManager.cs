﻿namespace RESTfulFramework.NET.ComponentModel
{
    public interface ILogManager
    {
        void WriteLog(string msg);
        void Debug(string msg);
        void Info(string msg);
        void Warn(string msg);
        void Error(string msg);
        void Fatal(string msg);
        void Trace(string msg);
    }
}

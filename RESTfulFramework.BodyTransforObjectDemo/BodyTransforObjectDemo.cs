using System;
using System.ComponentModel.Composition;

namespace RESTfulFramework.BodyTransforObjectDemo
{
    [Export(typeof(IBodyTransforObjectPlugin.IBodyTransforObject))]
    public class BodyTransforObjectDemo : IBodyTransforObjectPlugin.IBodyTransforObject
    {
        public object TransforObject(string body)
        {
            return body.ToJsonObject();
        }
    }
}

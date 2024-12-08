using System.Collections.Generic;

namespace System.Web.Mvc
{
    public interface IJSInitializer
    {
        IJSInitializer CreateSerializer();
        string Initialize(string id, string name, IDictionary<string, object> options);
        string InitializeFor(string selector, string name, IDictionary<string, object> options);
        string Serialize(IDictionary<string, object> value);
    }
}
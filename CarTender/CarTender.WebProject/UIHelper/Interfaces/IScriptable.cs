using System.IO;

namespace System.Web.Mvc
{
    public interface IScriptable
    {
        void WriteInitializationScript(TextWriter writer);
    }
}
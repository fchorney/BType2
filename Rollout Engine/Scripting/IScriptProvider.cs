using System.Xml.Linq;

namespace Rollout.Scripting
{
    public interface IScriptProvider
    {
        void Load(string assetName);
        IAction ProcessTemplate(XElement template, string sourceId);
    }
}
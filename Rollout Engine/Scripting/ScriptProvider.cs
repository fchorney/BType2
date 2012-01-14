using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Rollout.Core;
using Rollout.Utility;

namespace Rollout.Scripting
{
    public class ScriptProvider
    {
        public XmlScriptingEngine Engine { get; private set; }

        public static Dictionary<string, XElement> Templates;
        private static Dictionary<string, ActionInfo> ActionTypes { get; set; } 

        public ScriptProvider(XmlScriptingEngine engine)
        {
            Engine = engine;
            Templates = new Dictionary<string, XElement>();
            ActionTypes = new Dictionary<string, ActionInfo>();

            RegisterActions();
        }

        public void Load(string assetName)
        {
            var doc = XElement.Load(G.Content.RootDirectory + @"\Scripts\" + assetName + ".xml");


            var templates = doc.Elements("template");
            foreach (var template in templates)
            {
                string id = template.Attribute("id").Value;
                Templates.Add(id, template);
            }

            var scripts = doc.Elements("script");
            foreach(var script in scripts)
            {
                ProcessScript(script);
            }

        }

        private void RegisterActions()
        {
            //find all Types with the ActionAttribute
            var actionTypes = AttributeHelper.GetTypesWith<ActionAttribute>();
            foreach (var actionType in actionTypes)
            {
                var actionAttribute = (ActionAttribute)actionType.GetCustomAttributes(typeof (ActionAttribute), false)[0];
                var actionInfo2 = new ActionInfo() {Name = actionAttribute.Name, Type = actionType};

                //get all ActionParam attributes on the Type
                foreach(ActionParamAttribute actionParam in actionType.GetCustomAttributes(typeof(ActionParamAttribute), false))
                {
                    var paramInfo = new ActionInfo.ParamInfo() {Name = actionParam.Name, Type= actionParam.Type, Order = actionParam.Order};
                    actionInfo2.Params.Add(paramInfo);
                }

                //sort the ActionParams since we can't guarantee order
                actionInfo2.Params = actionInfo2.Params.OrderBy(p => p.Order).ToList();

                ActionTypes.Add(actionInfo2.Name, actionInfo2);
            }
        }

        private void ProcessScript(XElement script)
        {
            string forName = script.Attribute("for") != null ? script.Attribute("for").Value : "_screen";

            foreach (var child in script.Elements())
            {
                //Engine.AddAction(forName, ProcessElement(child,forName));
                IAction action = ProcessAction(child, forName);
                if (action != null)
                    Engine.AddAction(forName, action);
            }
        }

        public static IAction ProcessAction(XElement node, string forName)
        {
            IAction action = null;
            string actionName = node.Name.ToString();

            if (ActionTypes.ContainsKey(actionName))
            {
                ActionInfo actionInfo = ActionTypes[actionName];

                List<object> args = new List<object>();

                args.Add(forName);

                foreach (var paramInfo in actionInfo.Params)
                {
                    object param = node.Attribute(paramInfo.Name) != null
                                       ? Convert.ChangeType(node.Attribute(paramInfo.Name).Value, paramInfo.Type)
                                       : (paramInfo.DefaultValue ?? Activator.CreateInstance(paramInfo.Type));

                    args.Add(param);
                }

                action = Activator.CreateInstance(actionInfo.Type, args.ToArray()) as IAction;

                if (action != null)
                {
                    action.Wait = IsWaitingAction(node);

                    foreach (var child in node.Elements())
                    {
                        var childAction = ProcessAction(child, forName);
                        if (childAction != null)
                        action.AddAction(childAction);
                    }
                }
            }

            return action;
        }

        private static bool IsWaitingAction(XElement node)
        {
            if (node.Attribute("wait") == null || node.Name == "wait")
                return true;
            return false;
        }

    }

}

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Rollout.Core;
using Rollout.Scripting.Actions;

namespace Rollout.Scripting
{
    public class ScriptProvider
    {
        public ScriptingEngine Engine { get; private set; }

        public static Dictionary<string, XElement> Templates;
        private static Dictionary<string, ActionInfo> ActionTypes { get; set; } 

        public ScriptProvider(ScriptingEngine engine)
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
            //move
            ActionInfo actionInfo = new ActionInfo() {Name = "move", Type = typeof (MoveAction)};
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name="x", Type=typeof(int) });
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name="y", Type=typeof(int) });
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name="speed", Type=typeof(double) });
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name="duration", Type=typeof(int) });

            ActionTypes.Add(actionInfo.Name, actionInfo);


            //repeat
            actionInfo = new ActionInfo() { Name = "repeat", Type = typeof(RepeatAction) };
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name = "count", Type = typeof(int), DefaultValue = -1 });

            ActionTypes.Add(actionInfo.Name, actionInfo);

            //wait
            actionInfo = new ActionInfo() { Name = "wait", Type = typeof(WaitAction) };
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name = "duration", Type = typeof(int)});

            ActionTypes.Add(actionInfo.Name, actionInfo);

            //action
            actionInfo = new ActionInfo() { Name = "action", Type = typeof(Action) };

            ActionTypes.Add(actionInfo.Name, actionInfo);


            //create
            actionInfo = new ActionInfo() { Name = "create", Type = typeof(CreateAction) };
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name = "id", Type = typeof(string) });
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name = "x", Type = typeof(int) });
            actionInfo.Params.Add(new ActionInfo.ParamInfo() { Name = "y", Type = typeof(int) });
            
            ActionTypes.Add(actionInfo.Name, actionInfo);

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

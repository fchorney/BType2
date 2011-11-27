using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Scripting.Actions;
using Rollout.Utility;

namespace Rollout.Scripting
{
    public class ScriptProvider
    {
        private static int Counter;
        public ScriptingEngine Engine { get; private set; }

        private Dictionary<string, XElement> Templates; 

        public ScriptProvider(ScriptingEngine engine)
        {
            Engine = engine;
            Templates = new Dictionary<string, XElement>();
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

        private void ProcessScript(XElement script)
        {
            string forName = script.Attribute("for") != null ? script.Attribute("for").Value : "_screen";

            foreach (var child in script.Elements())
            {
                Engine.AddAction(forName, ProcessElement(child,forName));
            }
        }

        private IAction ProcessElement(XElement node, string forName)
        {
            IAction action = null;
            if (node.Name == "move")
                action = CreateMoveAction(node, forName);
            else if (node.Name == "repeat")
                action = CreateRepeatAction(node, forName);
            else if (node.Name == "wait")
                action = CreateWaitAction(node, forName);
            else if (node.Name == "create")
                action = CreateCreateAction(node, forName);

            if (action != null) action.Wait = Waits(node);

            return action;
        }

        private IAction CreateCreateAction(XElement node, string forName)
        {
            IAction action;
            string id = node.Attribute<string>("id");
            int x = node.Attribute<int>("x");
            int y = node.Attribute<int>("y");


            List<IAction> actions = new List<IAction>();

            var targetname = id + "|" + Counter++;

            foreach (var child in Templates[id].Elements())
            {
                actions.Add(ProcessElement(child, targetname));
            }

            action = new CreateAction(forName, targetname, new Vector2(x, y), actions);

            return action;
        }

        private IAction CreateWaitAction(XElement node, string forName)
        {
            IAction action;
            int duration = node.Attribute<int>("duration");
            action = new WaitAction(Time.ms(duration));
            return action;
        }

        private IAction CreateRepeatAction(XElement node, string forName)
        {
            IAction action;
            
            int count = node.Attribute<int>("count", -1);

            action = new RepeatAction(count);

            foreach (var child in node.Elements())
            {
                var childAction = ProcessElement(child, forName);
                action.AddAction(childAction);
            }
            return action;
        }

        private IAction CreateMoveAction(XElement node, string forName)
        {
            IAction action;
            int x = node.Attribute<int>("x");
            int y = node.Attribute<int>("y");
            int speed = node.Attribute<int>("speed");
            int duration = node.Attribute<int>("duration");

            if (speed != 0)
            {
                action = new MoveAction(forName, new Vector2(x, y), speed);
            }
            else
            {
                action = new MoveAction(forName, new Vector2(x, y), Time.ms(duration));
            }

            return action;
        }

        private static bool Waits(XElement node)
        {
            if (node.Attribute("wait") == null || node.Name == "wait")
                return true;
            return false;
        }

    }

    static class XElementExtensions
    {
        public static T Attribute<T>(this XElement node, string attributeName, T defaultValue = default(T))
        {
            return node.Attribute(attributeName) != null ? (T)Convert.ChangeType(node.Attribute(attributeName).Value, typeof(T)) : defaultValue;
        }
    }

}

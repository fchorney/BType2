using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Scripting.Actions;
using Rollout.Utility;
using Rollout.Utility.Cloning;

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
            string id = node.Attribute("id").Value;
            int x = Convert.ToInt32(GetAttribute(node, "x", 0));
            int y = Convert.ToInt32(GetAttribute(node, "y", 0));


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
            int count = Convert.ToInt32(GetAttribute(node, "duration", 0));
            action = new WaitAction(Time.ms(count));
            return action;
        }

        private IAction CreateRepeatAction(XElement node, string forName)
        {
            IAction action;
            int count = Convert.ToInt32(GetAttribute(node, "count", -1));
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
            int x = Convert.ToInt32(GetAttribute(node, "x", 0));
            int y = Convert.ToInt32(GetAttribute(node, "y", 0));
            int speed = Convert.ToInt32(GetAttribute(node, "speed", 0));
            int duration = Convert.ToInt32(GetAttribute(node, "duration", 0));

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

        private static Object GetAttribute(XElement node, string name, Object defaultValue = null)
        {
            return node.Attribute(name) != null ? node.Attribute(name).Value : defaultValue;
        }
    }
}

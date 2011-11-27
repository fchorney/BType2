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
        public ScriptingEngine Engine { get; private set; }

        public ScriptProvider(ScriptingEngine engine)
        {
            Engine = engine;
        }

        public void Load(string assetName)
        {
            var doc = XElement.Load(G.Content.RootDirectory + @"\Scripts\" + assetName + ".xml");
            var scripts = doc.Elements("script");

            foreach(var script in scripts)
            {
                ProcessScript(script);
            }
        }

        private void ProcessScript(XElement script)
        {
            string forName = script.Attribute("for") != null ? script.Attribute("for").Value : "";

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

            if (action != null) action.Wait = Waits(node);

            IAction whatthefuck = ObjectCloner.Clone(action);

            return whatthefuck;
        }

        private IAction CreateWaitAction(XElement node, string forName)
        {
            IAction action;
            int count = Convert.ToInt32(GetAttribute(node, "count", 0));
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

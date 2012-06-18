using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{
    [Action("create")]
    [ActionParam("id")]
    [ActionParam("x")]
    [ActionParam("y")]
    public sealed class CreateAction : Action
    {
        private string templateid;
        private static int Counter;

        public CreateAction(Dictionary<string, Expression> args)
            : base(args)
        {
            templateid = Args["id"].AsString();
        }

        public override void Update(GameTime gameTime)
        {
            var template = ScriptProvider.Templates[templateid];

            var targetName = templateid + "|" + Counter++;

            var sprite = CreateSprite(targetName, template.Attribute("sprite").Value);

            var createTarget = Source as DrawableGameObject;
            if (createTarget != null) createTarget.Add(sprite);


            var targetActions = new ActionQueue();
            foreach (var action in template.Elements())
            {
                targetActions.Add(ScriptProvider.ProcessAction(action, targetName));
            }

            ScriptingEngine.Add(targetName, sprite, targetActions);
            CollisionEngine.Add(sprite);

            Finished = true;
        }

        public Sprite CreateSprite(string name, string spriteType)
        {

            var type = ScriptProvider.SpriteTypes.ContainsKey(spriteType) ? ScriptProvider.SpriteTypes[spriteType] : typeof (Sprite);
            var sprite = (Sprite)Activator.CreateInstance(type);

            sprite.Name = name;
            sprite.Position = new Vector2(Args["x"].AsInt(), Args["y"].AsInt());

            return sprite;
        }
    }
}
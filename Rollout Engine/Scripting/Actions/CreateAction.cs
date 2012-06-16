using System;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;
using Rollout.Utility.EquationHelper;

namespace Rollout.Scripting.Actions
{
    [Action("create")]
    [ActionParam(0, "id", typeof(string))]
    [ActionParam(1, "x", typeof(string))]
    [ActionParam(2, "y", typeof(string))]
    public sealed class CreateAction : Action
    {
        private string target;
        private string templateid;
        private Vector2 position;
        private static int Counter;
        private Equation x,y;

        public CreateAction(string target, string id, string x,  string y)
        {
            this.y = Equation.Parse(y);
            this.x = Equation.Parse(x);          
            position = new Vector2(this.x.SolveAsInt(), this.y.SolveAsInt());

            this.target = target;
            templateid = id;
        }

        public CreateAction(string target, string templateid, Vector2 position, ActionQueue actions)
        {
            this.target = target;
            this.templateid = templateid;
            this.position = position;
            this.actions = actions;
        }

        public override void Update(GameTime gameTime)
        {
            var template = ScriptProvider.Templates[templateid];

            var targetName = templateid + "|" + Counter++;

            var sprite = CreateSprite(targetName, template.Attribute("sprite").Value);

            var createTarget = ScriptingEngine.Item(target) as DrawableGameObject;
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
            sprite.Position = new Vector2(this.x.SolveAsInt(), this.y.SolveAsInt());

            return sprite;
        }
    }
}
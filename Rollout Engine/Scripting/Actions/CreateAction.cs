using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    [Action("create")]
    [ActionParam(0, "id", typeof(string))]
    [ActionParam(1, "x", typeof(int))]
    [ActionParam(2, "y", typeof(int))]
    public sealed class CreateAction : Action
    {
        private string target;
        private string templateid;
        private Vector2 position;
        private static int Counter;

        public CreateAction(string target, string id, int x,  int y)
        {
            position = new Vector2(x,y);

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
            var targetName = templateid + "|" + Counter++;

            var sprite = CreateSprite(targetName);

            var createTarget = ScriptingEngine.Item(target) as DrawableGameObject;
            if (createTarget != null) createTarget.Add(sprite);


            var childActions = new ActionQueue();
            foreach (var child in ScriptProvider.Templates[templateid].Elements())
            {
                childActions.Add(ScriptProvider.ProcessAction(child, targetName));
            }
            ScriptingEngine.Add(targetName, sprite, childActions);

            //actions = new ActionQueue();
            //foreach (var child in ScriptProvider.Templates[templateid].Elements())
            //{
            //    actions.Add(ScriptProvider.ProcessAction(child, targetName));
            //}

            //ScriptingEngine.Add(targetName, sprite, actions);
            //CollisionEngine.Add(sprite);

            Finished = true;
        }

        public Sprite CreateSprite(string name)
        {
            var sprite = new Sprite();

            sprite.Name = name;
            sprite.Position = position;
            sprite.AddAnimation("main", Animation.Load("player"));
            sprite.Shape = new Collision.Shapes.Rectangle(0, 0, 64, 64);

            return sprite;
        }

    }
}
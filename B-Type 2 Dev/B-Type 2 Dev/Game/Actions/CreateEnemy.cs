using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Core.GameObject;
using Rollout.Drawing.Sprites;
using Rollout.Game.Entities;
using Rollout.Scripting;
using Rollout.Scripting.Actions;

namespace Rollout.Game.Actions
{
    class CreateEnemy
    {
        [Action("ecreate")]
        [ActionParam(0, "id", typeof(string))]
        [ActionParam(1, "x", typeof(int))]
        [ActionParam(2, "y", typeof(int))]
        public sealed class CreateAction : Scripting.Actions.Action
        {
            private string target;
            private string templateid;
            private Vector2 position;
            private static int Counter;

            public CreateAction(string target, string id, int x, int y)
            {
                position = new Vector2(x, y);

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
                CollisionEngine.Add(sprite);

                Finished = true;
            }

            public Sprite CreateSprite(string name)
            {
                return new EnemyTest(name) {Position = position};
            }
        }
    }
}

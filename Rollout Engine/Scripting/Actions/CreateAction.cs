using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Drawing;
using Rollout.Utility;

namespace Rollout.Scripting.Actions
{
    //gonna find a way to make this work.
    //gonna find a way to make this work.
    //gonna find a way to make this work.
    //gonna find a way to make this work.
    public class ScriptAttribute : Attribute
    {
        public ScriptAttribute(string Name)
        {
            
        }
    }
    
    [Script("create")]
    public class CreateAction : Action, IAction
    {
        private string target;
        private string name;
        private Vector2 position;
        private List<IAction> actions;

        public CreateAction(string target, string name, Vector2 position, List<IAction> actions)
        {
            this.target = target;
            this.name = name;
            this.position = position;
            this.actions = actions;
        }

        public override void Update(GameTime gameTime)
        {
            var enemy = CreateEnemy(name);
            (Engine[target] as DrawableGameObject).Add(enemy);
            Engine.Add(name, enemy, actions);

            Finished = true;
        }


        public Sprite CreateEnemy(string name)
        {
            var enemy = new Sprite(position,
                    new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { 0.3f, 0.3f })) { Name = name };

            return enemy;

        }
    }
}
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rollout.Core.GameObject
{
    public interface IGameObject
    {
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }

    public class GameObject : IGameObject
    {
        public bool Enabled { get; set; }

        protected IGameObject Parent { get; set; }
        protected List<IGameObject> Children { get; set; }

        public GameObject()
        {
            Children = new List<IGameObject>();
            Enabled = true;
        }


        public void Add(GameObject obj)
        {
            obj.Parent = this;
            Children.Add(obj);
        }

        public void Remove(GameObject obj)
        {
            if (Children.Contains(obj))
                Children.Remove(obj);
        }

        public virtual void Initialize()
        {
            foreach (var gameObject in Children)
                gameObject.Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var gameObject in Children)
                gameObject.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (var gameObject in Children)
                gameObject.Draw(gameTime);
        }
    }
}
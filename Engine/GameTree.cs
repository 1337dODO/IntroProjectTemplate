using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class GameTree
{
    public List<Node> Nodes { get; } = [];

    public Node AddGameObject(GameObject gameObject)
    {
        var node = new Node(gameObject);
        Nodes.Add(node);
        gameObject.Initialize();
        return node;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var node in Nodes) node.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var node in Nodes) node.Draw(spriteBatch);
    }

    public class Node
    {
        public GameObject GameObject { get; }

        public List<Node> Children { get; } = [];

        internal Node(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public Node AddChild(GameObject gameObject)
        {
            var node = new Node(gameObject);
            Children.Add(node);
            gameObject.Parent = GameObject;
            gameObject.Initialize();
            return node;
        }

        internal void Update(GameTime gameTime)
        {
            GameObject.Update(gameTime);
            foreach (var child in Children) child.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            GameObject.Draw(spriteBatch);
            foreach (var child in Children) child.Draw(spriteBatch);
        }
    }
}

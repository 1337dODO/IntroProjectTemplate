using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// The data structure which holds all game objects. Each <see cref="Node"/> can hold additional children.
/// </summary>
public class GameTree
{
    private readonly List<Node> _nodes = [];

    /// <summary>
    /// The nodes at the top level of the tree.
    /// </summary>
    public ReadOnlyCollection<Node> Nodes => _nodes.AsReadOnly();

    /// <summary>
    /// Add a new <see cref="GameObject"/> to the tree.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to add.</param>
    /// <returns>The <see cref="Node"/> in which the added <see cref="GameObject"/> resides.</returns>
    public Node AddGameObject(GameObject gameObject)
    {
        var node = new Node(gameObject, this);
        _nodes.Add(node);
        gameObject.Initialize();
        return node;
    }

    /// <summary>
    /// Updates all <see cref="GameObject"/>s in the tree.
    /// </summary>
    /// <param name="gameTime">The time elapsed since the last update.</param>
    public void Update(GameTime gameTime)
    {
        foreach (var node in _nodes) node.Update(gameTime);
    }

    /// <summary>
    /// Draws all <see cref="GameObject"/>s in the tree.
    /// </summary>
    /// <param name="spriteBatch">The batch to draw the objects in.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var node in _nodes) node.Draw(spriteBatch);
    }

    /// <summary>
    /// Nodes store the object and its children.
    /// </summary>
    public class Node
    {
        private readonly Node? _parent;
        private readonly GameTree _gameTree;
        private readonly List<Node> _children = [];

        /// <summary>
        /// The object stored in this <see cref="Node"/>.
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// The children of this <see cref="Node"/>.
        /// </summary>
        public ReadOnlyCollection<Node> Children => _children.AsReadOnly();

        internal Node(GameObject gameObject, GameTree gameTree, Node? parent = null)
        {
            GameObject = gameObject;
            _gameTree = gameTree;
            _parent = parent;
        }

        /// <summary>
        /// Add a new child to this <see cref="Node"/>.
        /// </summary>
        /// <param name="gameObject">The object to add.</param>
        /// <returns>A new <see cref="Node"/>, which contains the <paramref name="gameObject"/>.</returns>
        public Node AddChild(GameObject gameObject)
        {
            var node = new Node(gameObject, _gameTree, this);
            _children.Add(node);
            gameObject.Parent = GameObject;
            gameObject.Initialize();
            return node;
        }

        /// <summary>
        /// Remove this node from the <see cref="GameTree"/>.
        /// </summary>
        public void Remove()
        {
            if (_parent != null) _parent._children.Remove(this);
            else _gameTree._nodes.Remove(this);

            GameObject.OnRemove();
        }

        internal void Update(GameTime gameTime)
        {
            GameObject.Update(gameTime);
            foreach (var child in _children) child.Update(gameTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            GameObject.Draw(spriteBatch);
            foreach (var child in _children) child.Draw(spriteBatch);
        }
    }
}

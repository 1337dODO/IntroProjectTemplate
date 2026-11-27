using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// This class represents all objects existing with a location in the game world. Extend the functionality of game
/// objects by adding components using the <see cref="AddComponent{T}"/> method.
/// </summary>
public class GameObject
{
    private float _baseDrawLayer = 0.0f;
    private float _fineDrawLayer = 0.0f;
    private bool _isVisible = true;

    private readonly List<Component> _components = [];

    /// <summary>
    /// The parent of this <see cref="GameObject"/> in the <see cref="GameTree"/>, if it exists.
    /// </summary>
    public GameObject? Parent { get; internal set; }

    /// <summary>
    /// The local position of this <see cref="GameObject"/>, relative to its <see cref="Parent"/>.
    /// </summary>
    public Vector2 LocalPosition { get; set; } = Vector2.Zero;

    public float BaseDrawLayer
    {
        get => _baseDrawLayer;
        set => _baseDrawLayer = Math.Clamp(value, 0.0f, 1.0f);
    }

    public float FineDrawLayer
    {
        get => _fineDrawLayer;
        set => _fineDrawLayer = Math.Clamp(value, 0.0f, 1.0f);
    }

    public float DrawLayer => _baseDrawLayer + _fineDrawLayer;

    /// <summary>
    /// Invoked when the visibility of the object has changed.
    /// </summary>
    public event EventHandler? VisibilityChanged;
    /// <summary>
    /// Invoked when the object is initialized.
    /// </summary>
    public event EventHandler? Initialized;
    /// <summary>
    /// Invoked when the object is removed from the <see cref="GameTree"/>.
    /// </summary>
    public event EventHandler? Removed;

    /// <summary>
    /// The global position of the <see cref="GameObject"/> in the game world.
    /// </summary>
    public Vector2 GlobalPosition
    {
        get
        {
            if (Parent == null) return LocalPosition;

            return LocalPosition + Parent.GlobalPosition;
        }
        set
        {
            if (Parent == null) LocalPosition = value;
            else LocalPosition += value - GlobalPosition;
        }
    }

    /// <summary>
    /// Whether the <see cref="GameObject"/> is visible.
    /// </summary>
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            var hasChanged = value != _isVisible;
            _isVisible = value;

            if (hasChanged) VisibilityChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Create and add a component to this <see cref="GameObject"/>.
    /// </summary>
    /// <typeparam name="T">The type of the component to create and add.</typeparam>
    /// <returns>The component that was created and added.</returns>
    public T AddComponent<T>() where T : Component, new()
    {
        var component = new T
        {
            Owner = this
        };

        _components.Add(component);
        return component;
    }

    /// <summary>
    /// Gets the first component with type <typeparamref name="T"/>, if it exists. Is intended to be used when it is
    /// necessary to both check for the existence of a certain component, and retrieve this component if it exists.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <returns>The first component of type <typeparamref name="T"/>, if it exists. Returns <see langword="null"/> if
    /// no component of type <typeparamref name="T"/> is found.</returns>
    /// <remarks>Usually, when something needs a component from a <see cref="GameObject"/>, it is looking for a unique
    /// instance of the type of component. Therefore, it is not possible to retrieve any instances other than the first
    /// one when retrieving components.</remarks>
    public T? GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T)) return component as T;
        }

        return null;
    }

    /// <summary>
    /// Remove the first component of type <typeparamref name="T"/> this <see cref="GameObject"/> holds.
    /// </summary>
    /// <typeparam name="T">The type of component to look for and remove.</typeparam>
    /// <returns><see langword="true"/> if a component of type <typeparamref name="T"/> was successfully found and
    /// removed, <see langword="false"/> otherwise.</returns>
    public bool RemoveComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T))
            {
                _components.Remove(component);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Called when the object is added to the <see cref="GameTree"/>.
    /// </summary>
    internal void Initialize()
    {
        foreach (var component in _components)
        {
            component.Initialize();
        }

        Initialized?.Invoke(this, EventArgs.Empty);
    }

    internal void Update(GameTime gameTime)
    {
        for (var i = _components.Count - 1; i >= 0; i--)
        {
            var component = _components[i];
            component.Update(gameTime);
        }
    }

    internal void Draw(SpriteBatch spriteBatch)
    {
        if (!_isVisible) return;

        foreach (var component in _components)
        {
            component.Draw(spriteBatch);
        }
    }

    internal void OnRemove()
    {
        Removed?.Invoke(this, EventArgs.Empty);
    }
}

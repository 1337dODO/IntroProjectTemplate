using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class GameObject
{
    private float _baseDrawLayer = 0.0f;
    private float _fineDrawLayer = 0.0f;
    private bool _isVisible = true;

    private readonly List<Component> _components = [];

    public GameObject? Parent { get; internal set; }

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

    public event EventHandler? VisibilityChanged;
    public event EventHandler? Initialized;
    public event EventHandler? Removed;

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

    public T AddComponent<T>() where T : Component, new()
    {
        var component = new T
        {
            Owner = this
        };

        _components.Add(component);
        return component;
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component.GetType() == typeof(T)) return component as T;
        }

        return null;
    }

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

    public void Initialize()
    {
        foreach (var component in _components)
        {
            component.Initialize();
        }

        Initialized?.Invoke(this, EventArgs.Empty);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var component in _components)
        {
            component.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isVisible) return;

        foreach (var component in _components)
        {
            component.Draw(spriteBatch);
        }
    }

    public void Remove()
    {
        Removed?.Invoke(this, EventArgs.Empty);
    }
}

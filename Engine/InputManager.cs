using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine;

/// <summary>
/// Handles keyboard and mouse input.
/// </summary>
public sealed class InputManager
{
    // Could differ per mouse, but this value seems to be a norm of sorts.
    private const int SingleScrollDetentAmount = 120;
    private ButtonState _leftClick = ButtonState.Released;
    private ButtonState _middleClick = ButtonState.Released;
    private ButtonState _rightClick = ButtonState.Released;
    private int _scrollWheelValue = 0;
    private Keys[] _previousPressedKeys = [];
    private Keys[] _pressedKeys = [];

    #region Mouse Movement
    public Vector2 MousePosition { get; private set; } = Vector2.Zero;
    public Vector2 MouseDisplacement { get; private set; } = Vector2.Zero;
    public bool HasMouseMoved { get; private set; } = false;
    public bool IsMouseInsideScreen { get; private set; } = false;
    #endregion

    #region Mouse Scrolling
    public bool HasMouseScrolledDown { get; private set; } = false;
    public bool HasMouseScrolledUp { get; private set; } = false;
    public bool HasMouseScrolled { get; private set; } = false;
    public int ScrollDisplacement { get; private set; } = 0;
    public int NormalizedScrollDisplacement { get; private set; } = 0;
    #endregion

    #region Mouse Clicking
    public bool HasMouseJustLeftClicked { get; private set; } = false;
    public bool HasMouseJustMiddleClicked { get; private set; } = false;
    public bool HasMouseJustRightClicked { get; private set; } = false;
    #endregion

    #region Combinations
    public bool HasMouseDragged { get; private set; } = false;
    #endregion

    #region Keyboard Pressing
    public bool IsAnyKeyPressed { get; private set; } = false;
    public bool IsAnyKeyJustReleased { get; private set; } = false;

    public bool CheckIsKeyPressed(Keys key)
    {
        return _pressedKeys.Contains(key);
    }

    public bool CheckIsKeyJustPressed(Keys key)
    {
        return !_previousPressedKeys.Contains(key)
            && CheckIsKeyPressed(key);
    }

    public bool CheckIsKeyReleased(Keys key)
    {
        return !_pressedKeys.Contains(key);
    }

    public bool CheckIsKeyJustReleased(Keys key)
    {
        return _previousPressedKeys.Contains(key)
            && CheckIsKeyReleased(key);
    }
    #endregion

    /// <summary>
    /// Calling this method at the start of every frame is necessary if you want input detection.
    /// </summary>
    public void Update()
    {
        ReadMouse();
        ReadKeyboard();
    }

    private void ReadMouse()
    {
        // Movement.
        var mouseState = Mouse.GetState();
        var previousMousePosition = MousePosition;
        MousePosition = mouseState.Position.ToVector2();
        MouseDisplacement = MousePosition - previousMousePosition;
        HasMouseMoved = !MathUtils.IsApproxEqual(previousMousePosition, MousePosition);

        // Scrolling.
        var previousScrollWheelValue = _scrollWheelValue;
        _scrollWheelValue = mouseState.ScrollWheelValue;
        HasMouseScrolledDown = previousScrollWheelValue > _scrollWheelValue;
        HasMouseScrolledUp = previousScrollWheelValue < _scrollWheelValue;
        HasMouseScrolled = HasMouseScrolledDown || HasMouseScrolledUp;
        ScrollDisplacement = _scrollWheelValue - previousScrollWheelValue;
        NormalizedScrollDisplacement = ScrollDisplacement / SingleScrollDetentAmount;

        // Clicking.
        var previousLeftClick = _leftClick;
        _leftClick = mouseState.LeftButton;
        var previousMiddleClick = _middleClick;
        _middleClick = mouseState.MiddleButton;
        var previousRightClick = _rightClick;
        _rightClick = mouseState.RightButton;
        var hasMouseLeftClicked = _leftClick == ButtonState.Pressed;
        HasMouseJustLeftClicked = (previousLeftClick == ButtonState.Released) && (hasMouseLeftClicked);
        HasMouseJustMiddleClicked = (previousMiddleClick == ButtonState.Released) && (_middleClick == ButtonState.Pressed);
        HasMouseJustRightClicked = (previousRightClick == ButtonState.Released) && (_rightClick == ButtonState.Pressed);

        // Combinations.
        HasMouseDragged = HasMouseMoved && hasMouseLeftClicked;
    }

    private void ReadKeyboard()
    {
        var keyboardState = Keyboard.GetState();
        _previousPressedKeys = _pressedKeys;
        _pressedKeys = keyboardState.GetPressedKeys();
        IsAnyKeyPressed = _pressedKeys.Length > 0;
        IsAnyKeyJustReleased = Array.Exists(
            array: _previousPressedKeys,
            match: CheckIsKeyJustReleased
        );
    }
}

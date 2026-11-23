using System;

namespace Engine;

/// <summary>
/// Implements a timer, which raises an event after a certain amount of time is elapsed.
/// </summary>
/// <param name="timeLimit">The amount of time in seconds after which the timer signals completion.</param>
public class Timer(float timeLimit)
{
    private float _timeLimit = timeLimit;

    /// <summary>
    /// The amount of time elapsed in seconds after the creation of the <see cref="Timer"/>, or after resuming the
    /// <see cref="Timer"/>.
    /// </summary>
    public float ElapsedTime { get; private set; } = 0.0f;

    /// <summary>
    /// Whether the timer updates its <see cref="ElapsedTime"/>.
    /// </summary>
    public bool IsPaused { get; private set; } = false;

    /// <summary>
    /// Whether to automatically restart the timer after elapsing.
    /// </summary>
    public bool AutoRestart { get; set; } = true;

    public float TimeLimit
    {
        get => _timeLimit;
        set
        {
            if (value <= 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "The time limit must be greater than 0.");
            }
            _timeLimit = value;
        }
    }

    /// <summary>
    /// Whether the <see cref="Timer"/> has elapsed. Use this or the <see cref="Elapsed"/> event to determine when the
    /// <see cref="Timer"/> has elapsed.
    /// </summary>
    public bool IsFinished => ElapsedTime >= _timeLimit;

    /// <summary>
    /// How much time is left, in seconds, before the <see cref="Timer"/> elapses.
    /// </summary>
    public float TimeLeft => _timeLimit - ElapsedTime;

    /// <summary>
    /// The relative amount of time left before the <see cref="Timer"/> elapses, expressed as a ratio between the time
    /// left and the time limit.
    /// </summary>
    public float TimeLeftRatio => TimeLeft / _timeLimit;

    /// <summary>
    /// The relative amount of time elapsed, expressed as a ratio between the elapsed time and the time limit.
    /// </summary>
    public float ElapsedTimeRatio => ElapsedTime / _timeLimit;

    /// <summary>
    /// Raised when the <see cref="Timer"/> elapses.
    /// </summary>
    public event EventHandler? Elapsed;

    /// <summary>
    /// Raised when the <see cref="Timer"/> is paused, either through the <see cref="Pause"/> method, or through
    /// elapsing with <see cref="AutoRestart"/> set to <see langword="false"/>.
    /// </summary>
    public event EventHandler? Paused;

    /// <summary>
    /// Raised when the <see cref="Timer"/> resumes from a paused state.
    /// </summary>
    public event EventHandler? Resumed;

    /// <summary>
    /// Update the <see cref="ElapsedTime"/> of the <see cref="Timer"/>. This is intended to be called every frame.
    /// </summary>
    /// <param name="deltaTime">How much time to add to <see cref="ElapsedTime"/>.</param>
    public void Update(float deltaTime)
    {
        // Don't update if the timer is paused
        if (IsPaused) return;

        ElapsedTime += deltaTime;
        if (!IsFinished) return;

        // If AutoRestart is enabled, subtract the time limit from the elapsed time instead of setting the elapsed time
        // to zero, to ensure that the time elapsed after the limit was reached is taken into account.
        if (AutoRestart) ElapsedTime -= _timeLimit;
        else Pause();

        Elapsed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Pause the <see cref="Timer"/> from updating.
    /// </summary>
    public void Pause()
    {
        if (IsPaused) return;

        IsPaused = true;
        Paused?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Resume updating the <see cref="Timer"/>. Does nothing if the <see cref="Timer"/> is not paused.
    /// </summary>
    public void Resume()
    {
        if (!IsPaused) return;

        IsPaused = false;
        Resumed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Restart the <see cref="Timer"/>, setting its <see cref="ElapsedTime"/> to zero.
    /// </summary>
    public void Restart()
    {
        ElapsedTime = 0.0f;
        Resume();
    }
}

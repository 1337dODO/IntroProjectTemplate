using System;
using Microsoft.Xna.Framework;

namespace Engine;

public class Scene
{
    public GameTree GameTree { get; }

    public Color DrawClearColor { get; set; } = Color.White;

    public event SceneAdvancedEventHandler Advanced;

    public void OnAdvance(Scene nextScene)
    {
        Advanced.Invoke(this, new SceneAdvancedEventArgs(nextScene));
    }
}

public delegate void SceneAdvancedEventHandler(object sender, SceneAdvancedEventArgs e);

public class SceneAdvancedEventArgs : EventArgs
{
    public SceneAdvancedEventArgs(Scene nextScene)
    {
        NextScene = nextScene;
    }

    public Scene NextScene { get; }
}

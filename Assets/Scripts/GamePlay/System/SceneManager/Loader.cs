using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum SceneType
    {
        MainMenu,
        GameMainScene,
        LoadingScene,
    }

    private static SceneType targetSceneType;

    public static void Load(SceneType targetSceneType)
    {
        Loader.targetSceneType = targetSceneType;
        SceneManager.LoadScene(SceneType.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSceneType.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Loader;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button QuitButton;

    // Start is called before the first frame update
    void Awake()
    {
        PlayButton.onClick.AddListener(() =>
        {
            GameEntry.Event.Fire(ChangeSceneEventArgs.EventId,
                ChangeSceneEventArgs.Create(GameConst.SceneIDS.Level1SceneID));
        });

        QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public void NextLevel()
    {
        SceneManager.LoadScene("_Scene_1");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}

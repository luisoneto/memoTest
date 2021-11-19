using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static int Dificultad;
    public void BtnFacil()
    {
        Dificultad = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void BtnNormal()
    {
        Dificultad = 2;
        SceneManager.LoadScene("GameScene");
    }

    public void BtnDificil()
    {
        Dificultad = 3;
        SceneManager.LoadScene("GameScene");
    }
}

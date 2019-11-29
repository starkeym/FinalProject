using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void Shooter() {
        SetupLocalPlayer.chID = 0;
        SceneManager.LoadScene(1);
    }
    public void Healer()
    {
        SetupLocalPlayer.chID = 1;
        SceneManager.LoadScene(1);
    }
    public void Tank()
    {
        SetupLocalPlayer.chID = 2;
        SceneManager.LoadScene(1);
    }
}

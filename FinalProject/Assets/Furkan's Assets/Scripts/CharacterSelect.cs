using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public GameObject NetworkManager;
    public GameObject[] buttons;
    public void Shooter() {
        foreach (Transform item in gameObject.transform)
        {

            item.gameObject.tag = "0";
        }
        NetworkManager.SetActive(true);
        foreach (var item in buttons)
        {
            item.SetActive(false);
        }
    }
    public void Healer()
    {
        foreach (Transform item in gameObject.transform)
        {

            item.gameObject.tag = "1";
        }
        NetworkManager.SetActive(true);
        foreach (var item in buttons)
        {
            item.SetActive(false);
        }
    }
    public void Tank()
    {
        foreach (Transform item in gameObject.transform)
        {

            item.gameObject.tag = "2";
        }

        NetworkManager.SetActive(true);
        foreach (var item in buttons)
        {
            item.SetActive(false);
        }
    }
}

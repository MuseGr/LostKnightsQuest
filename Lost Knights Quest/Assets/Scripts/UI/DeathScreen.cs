using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance;
    public Animator animator;

    private void Start()
    {
        instance = this;
    }

    public void Show()
    {
        animator.SetTrigger("Show");
        Invoke("GoToMenu", 2.5f);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

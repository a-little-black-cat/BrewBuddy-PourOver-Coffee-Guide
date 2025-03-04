using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class HomePage : MonoBehaviour
{
    public Button Notebook;
    public Button pourOver;
    public Button exitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Notebook.onClick.AddListener(openNotebook);
        pourOver.onClick.AddListener(switchToPourOver);
        exitButton.onClick.AddListener(exitApplication);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openNotebook()
    {
        SceneManager.LoadScene("Notebook");
    }

    public void switchToPourOver()
    {
        SceneManager.LoadScene("HoffmanMethod");
    }
    public void exitApplication() {
        Application.Quit();
    }
}

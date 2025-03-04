using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using System.Text;
using UnityEditor;
using SFB;

public class Notebook : MonoBehaviour
{
    public string formatToTxt;
    public bool savingToFile;
    public string directory;
    public Button toFile;
    public Button toFolder;
    public Button toMenu;
    [SerializeField] public TMP_Text errorText;
    [SerializeField] public TMP_InputField coffeeName;
    [SerializeField] public TMP_InputField roastLevel;
    [SerializeField] public TMP_InputField TastingNotes;
    [SerializeField] public TMP_InputField otherNotes;
    public string selectedFolderPath;

    void Start()
    {
        toFile.onClick.AddListener(formatToFile);
        toFolder.onClick.AddListener(preOpenFolder);
        toMenu.onClick.AddListener(backToHome);
        savingToFile = false;
    }

    void Update()
    {
    }

    void formatToFile()
    {
        string dateText = DateTime.Now.ToString("yyyy-MM-dd T HH:mm:ss");
        string cofName = coffeeName.text;
        string rLevel = roastLevel.text;
        string tNotes = TastingNotes.text;
        string oNotes = otherNotes.text;
        formatToTxt = $" Date: {dateText}\n -------------------- \n Coffee Name:{cofName} \n -------------------- \n Roast Level:{rLevel} \n -------------------- \n Tasting Notes:{tNotes} \n -------------------- \n Other:{oNotes}";
        Debug.Log($"{formatToTxt}");
        savingToFile = true; //user is saving to file
        openFolder();
    }
    void preOpenFolder() {
        savingToFile = false; //user is not saving to file
        openFolder();
    }
    void openFolder()
    {
        Debug.Log($"openFolder saveToFile check1: {savingToFile}");

        if (!savingToFile && !string.IsNullOrEmpty(directory)) //check if user said false to save to file AND has selected a directory before
        {
            Debug.Log($"Opening directory: {directory}");
            Application.OpenURL(directory); // Opens the folder outside of Unity
            return;
        }

        if (!savingToFile && string.IsNullOrEmpty(directory)) //check if user said false to save to file AND has selected a directory before
        {
            Debug.Log($"Select a directory via save file.");
            errorText.text = "Directory not selected: Select a directory via EXPORT NOTES.";
             // Opens the folder outside of Unity
            return;
        }


        if (savingToFile)
        {

            if (string.IsNullOrEmpty(directory))
            {
                var paths = StandaloneFileBrowser.OpenFolderPanel("Select Directory", "", false);
                if (paths.Length == 0 || string.IsNullOrEmpty(paths[0]))
                {
                    Debug.Log("No directory selected.");
                    return;
                }
                directory = paths[0];
            }

            Debug.Log($"Directory chosen: {directory}");

            string baseFileName = $"{DateTime.Now:MM-dd-yyyy}-brew_note.txt";
            string filePathToSave = Path.Combine(directory, baseFileName);
            int counter = 1;

            while (File.Exists(filePathToSave))
            {
                filePathToSave = Path.Combine(directory, $"{DateTime.Now:MM-dd-yyyy}-brew_note({counter}).txt");
                errorText.text = $"File saved to {filePathToSave} as {baseFileName}";

                counter++;
            }

            try
            {
                File.WriteAllText(filePathToSave, formatToTxt);
                Debug.Log($"File saved successfully at: {filePathToSave}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving file: {ex.Message}");
            }
        }

    }

    void backToHome()
    {
        SceneManager.LoadScene("HomePage");
    }

}
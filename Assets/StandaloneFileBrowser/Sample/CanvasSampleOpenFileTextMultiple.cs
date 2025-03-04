using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;
using UnityEngine.Networking;


[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler {
    public Text output;

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnPointerDown(PointerEventData eventData) {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
    }

    // Called from browser
    public void OnFileUpload(string urls) {
        StartCoroutine(OutputRoutine(urls.Split(',')));
    }
#else
    //
    // Standalone platforms & editor
    //
    public void OnPointerDown(PointerEventData eventData) { }

    void Start() {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", true);

        if (paths.Length > 0)
        {
            var urlArr = new List<string>(paths.Length);

            for (int i = 0; i < paths.Length; i++)
            {
                // Ensure proper file path format for UnityWebRequest
                string filePath = "file:///" + paths[i].Replace("\\", "/");
                urlArr.Add(filePath);
            }

            StartCoroutine(OutputRoutine(urlArr.ToArray()));
        }
    }

#endif

    private IEnumerator OutputRoutine(string[] urlArr)
    {
        var outputText = new StringBuilder();

        foreach (string url in urlArr)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    outputText.Append(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError($"Failed to load file: {url} - Error: {request.error}");
                }
            }
        }

        output.text = outputText.ToString();
    }
}
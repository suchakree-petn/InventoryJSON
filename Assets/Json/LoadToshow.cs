using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadToshow : MonoBehaviour
{
    public static LoadToshow instance;
    void Awake()
    {
        instance = this;
    }



    [Header("Data")]
    [SerializeField] List<Item> playerScoreList = new List<Item>();

    [Header("Saving")]
    [SerializeField] string savePath;
    [SerializeField] string onlineLoadPath;

    private void Start()
    {
        LoadScoreFromGoogleDrive();
    }

    public List<Item> GetLoadedPlayerScoreList()
    {
        return playerScoreList;
    }

    /*
    [Header("UI")] [SerializeField] Transform scoreParent;
    [SerializeField] UIPlayerScore scoreUIPrefab;
    [SerializeField] List<UIPlayerScore> scoreUIList = new List<UIPlayerScore>();
    */
    [ContextMenu(nameof(TestJsonConvert))]
    void TestJsonConvert()
    {
        var scoreJson = JsonConvert.SerializeObject(playerScoreList);
        Debug.Log(scoreJson);
    }

    [ContextMenu(nameof(SaveScoreData))]
    void SaveScoreData()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("No save path ja");
            return;
        }

        var scoreJson = JsonConvert.SerializeObject(playerScoreList);
        var dataPath = Application.dataPath;
        var targetFilePath = Path.Combine(dataPath, savePath);

        var directoryPath = Path.GetDirectoryName(targetFilePath);
        if (Directory.Exists(directoryPath) == false)
            Directory.CreateDirectory(directoryPath);

        File.WriteAllText(targetFilePath, scoreJson);
    }

    [ContextMenu(nameof(LoadScoreData))]
    void LoadScoreData()
    {
        var dataPath = Application.dataPath;
        var targetFilePath = Path.Combine(dataPath, savePath);

        var directoryPath = Path.GetDirectoryName(targetFilePath);
        if (Directory.Exists(directoryPath) == false)
        {
            Debug.LogError("No save folder at provided path");
            return;
        }

        if (File.Exists(targetFilePath) == false)
        {
            Debug.LogError("No save file at provided path");
            return;
        }

        var scoreJson = File.ReadAllText(targetFilePath);
        playerScoreList = JsonConvert.DeserializeObject<List<Item>>(scoreJson);
    }

    [ContextMenu(nameof(LoadScoreFromGoogleDrive))]
    void LoadScoreFromGoogleDrive()
    {
        StartCoroutine(LoadScoreRoutine(onlineLoadPath));
    }

    IEnumerator LoadScoreRoutine(string url)
    {
        using (var webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                var downloadedText = webRequest.downloadHandler.text;
                Debug.Log("Received Data: " + downloadedText);

                // Save the downloaded text as a JSON file
                SaveToFile(downloadedText, "data.json");

                playerScoreList = JsonConvert.DeserializeObject<List<Item>>(downloadedText);
            }
        }
    }

    void SaveToFile(string data, string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);

        // Ensure the directory exists
        string dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        try
        {
            File.WriteAllText(path, data);
            Debug.Log($"Data saved to: {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save the file to {path} due to {e}");
        }
    }

    /*
    IEnumerator LoadPhotoFromUrl(string url, Action<Sprite> onReceivePhoto)
    {
        if (string.IsNullOrEmpty(url))
            yield break;

        var webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            byte[] bytes = webRequest.downloadHandler.data;
            Texture2D texture = new Texture2D(100, 100);
            texture.LoadImage(bytes);
            var sprite = SpriteCreator.CreateFromTexture(texture);
            onReceivePhoto?.Invoke(sprite);
        }
    }

    void Awake()
    {
        scoreUIPrefab.gameObject.SetActive(false);
        Refresh();
    }

    [ContextMenu(nameof(ClearUIs))]
    void ClearUIs()
    {
        foreach (var uiPlayerScore in scoreUIList)
            Destroy(uiPlayerScore.gameObject);

        scoreUIList.Clear();
    }

    public void SetData(List<Item> dataList)
    {
        playerScoreList = dataList;
        Refresh();
    }

    [ContextMenu(nameof(Refresh))]
    void Refresh()
    {
        ClearUIs();
        for (int i = 0; i < playerScoreList.Count; i++)
        {
            var newUI = Instantiate(scoreUIPrefab, scoreParent, false);
            newUI.gameObject.SetActive(true);
            newUI.SetData(playerScoreList[i]);

            //var photoUrl = playerScoreList[i].profileUrl;
            //StartCoroutine(LoadPhotoFromUrl(photoUrl, newUI.SetPhoto));
            scoreUIList.Add(newUI);
        }
    }*/
}


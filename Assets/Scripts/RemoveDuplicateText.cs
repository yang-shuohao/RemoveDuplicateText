using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RemoveDuplicateText : MonoBehaviour
{
    public TMP_Text tipsText;
    public Button loadButton;
    public Button convertButton;

    private string[] paths;
    private string content;

    void Start()
    {
        loadButton.onClick.AddListener(()=>{
            paths = StandaloneFileBrowser.OpenFilePanel("Open Text File", "", "txt", false);
            if (paths.Length > 0)
            {
                StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
            }
        });

        convertButton.onClick.AddListener(() =>
        {
            ProcessAndSave();
            convertButton.interactable = false;
        });
    }

    private IEnumerator OutputRoutine(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        AsyncOperation async = request.SendWebRequest();//读取数据

        while (!async.isDone)
        {
            yield return null;
        }

        content = request.downloadHandler.text;

        tipsText.text = "Load Text File Successful";
        convertButton.interactable = true;
    }

    public void ProcessAndSave()
    {
        if (!string.IsNullOrEmpty(paths[0]))
        {
            // 去重
            string uniqueText = RemoveDuplicates(content);

            // 新文件路径
            string newFilePath = Path.GetDirectoryName(paths[0]) + "/" + Path.GetFileNameWithoutExtension(paths[0]) + "_Out.txt";

            // 写入新文件
            File.WriteAllText(newFilePath, uniqueText);

            tipsText.text = "Convert Successful";

            Debug.Log("文件已处理并保存到：" + newFilePath);
        }
        else
        {
            Debug.Log("请先选择要处理的文件！");
        }
    }

    public string RemoveDuplicates(string input)
    {
        HashSet<char> uniqueChars = new HashSet<char>();
        string result = "";

        foreach (char c in input)
        {
            if (!uniqueChars.Contains(c))
            {
                uniqueChars.Add(c);
                result += c;
            }
        }

        return result;
    }
}

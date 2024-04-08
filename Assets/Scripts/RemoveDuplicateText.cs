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
        AsyncOperation async = request.SendWebRequest();//��ȡ����

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
            // ȥ��
            string uniqueText = RemoveDuplicates(content);

            // ���ļ�·��
            string newFilePath = Path.GetDirectoryName(paths[0]) + "/" + Path.GetFileNameWithoutExtension(paths[0]) + "_Out.txt";

            // д�����ļ�
            File.WriteAllText(newFilePath, uniqueText);

            tipsText.text = "Convert Successful";

            Debug.Log("�ļ��Ѵ������浽��" + newFilePath);
        }
        else
        {
            Debug.Log("����ѡ��Ҫ������ļ���");
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

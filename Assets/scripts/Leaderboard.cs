using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;
using TMPro;
using System;
using UnityEngine.SocialPlatforms.Impl;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    [SerializeField]
    private bool SendData;
    [SerializeField]
    private bool GetData;
    [SerializeField]
    private int GetTopX = 5;
    
    public string xmlString; // The XML string you want to parse
    public List<string> outputNameText = new List<string>();
    public List<int> outputScoreText = new List<int>();
    
    public List<TextMeshProUGUI> outputNameObjects = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> outputScoreObjects = new List<TextMeshProUGUI>();

    public void Start()
    {
        //
        //Reading of data is performed by using your public url.
        //Get your data as XML:
        //http://www.iotservice.nl/leaderboard/changescores.php?get&top=5&xml=1
        //

        StartCoroutine(GetScoreboard("http://www.iotservice.nl/leaderboard/changescores.php?get&top=" + GetTopX + "&xml=1"));

        if(SendData)
            {
            if (PlayerPrefs.GetString("InputName") != null)
            inputScore.text = "score: " + Variables.Application.Get("Score").ToString();

            if (PlayerPrefs.GetString("InputName") == "")
            Debug.Log("NoPrefiusNameEntered");
            else
            inputName.text = PlayerPrefs.GetString("InputName");
        }
    }

    public void StartGame()
    {
        Variables.Application.Set("Score", 0);
        SceneManager.LoadScene(1);
    }

    public void OnClick()
    {
        //
        //A player named Carmine got a score of 100. If the same name is added twice, we use the higher score.
        //http://www.iotservice.nl/leaderboard/changescores.php?add&name=Nick&score=105
        //

        string myURL= "http://www.iotservice.nl/leaderboard/changescores.php?add&name=" + inputName.text + "&score=" + Variables.Application.Get("Score");
        StartCoroutine(Upload(myURL));
        SendScore();
    }

    private IEnumerator Upload(string uri)
    {
        
        if (SendData == true)
        {
            //Debug.Log(inputName.text + " " + inputScore.text + " " + uri);
            WWWForm form = new WWWForm();
            UnityWebRequest www = UnityWebRequest.Post(uri, form);
            yield return www.SendWebRequest();
        }
    }
    
    private void SendScore()
    {
        //ResetScore
        Variables.Application.Set("Score", 0);

        //SaveNameinput
        PlayerPrefs.SetString("InputName", inputName.text);

        //LoadMenuScene
        SceneManager.LoadScene(0);
    }

    public void DiscardScore()
    {
        //ResetScore
        Variables.Application.Set("Score", 0);

        //LoadMenuScene
        SceneManager.LoadScene(0);
    }

    private IEnumerator GetScoreboard(string uri)
    {

        if (GetData)
        {
            while (true)
            {
                WWWForm form = new WWWForm();
                form.AddField("answer", "");

                using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
                {

                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result != UnityWebRequest.Result.Success)
                    {
                        //Debug.Log(webRequest.error);
                    }
                    else
                    {
                        //Debug.Log(webRequest.downloadHandler.text);
                    }
                    if (webRequest.downloadHandler.text.Contains("ERROR") == true)
                    yield return new WaitForSeconds(2);
                    
                    xmlString = webRequest.downloadHandler.text;
                    ParseXML();

                    int i=0; 
                    foreach(string stringNameText in outputNameText)
                    {
                        outputNameObjects[i].text = stringNameText;
                        i++;
                    }
                    i=0; 
                    foreach(int intScoreText in outputScoreText)
                    {
                        outputScoreObjects[i].text = intScoreText.ToString();
                        i++;
                    }
                }
                yield return new WaitForSeconds(5);
            }
        }
    }

    private void ParseXML()
    {
        outputNameText.Clear();
        outputScoreText.Clear();

        // Load the XML string into an XmlDocument
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        // Get all "entry" nodes within the "leaderboard" node
        XmlNodeList entryNodes = xmlDoc.SelectNodes("/leaderboard/player");

        foreach (XmlNode entryNode in entryNodes)
        {
            XmlNode nameNode = entryNode.SelectSingleNode("name");
            XmlNode scoreNode = entryNode.SelectSingleNode("score");

            if (nameNode != null && scoreNode != null)
            {
                // Extract the name and score values and add them to the lists
                string name = nameNode.InnerText;
                int score;

                if (int.TryParse(scoreNode.InnerText, out score))
                {
                    outputNameText.Add(name);
                    outputScoreText.Add(score);
                }
            }
        }
    }
}

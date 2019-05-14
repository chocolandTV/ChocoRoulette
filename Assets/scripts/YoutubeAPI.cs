// YoutubeAPI
using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class YoutubeAPI : MonoBehaviour
{
    private const string baseURL = "https://www.googleapis.com/youtube/v3/liveChat/messages?liveChatId=";

    private const string baseURL2 = "&part=snippet%2CauthorDetails&key=";

    private const string LiveChatIDURL = "https://www.googleapis.com/youtube/v3/liveStreams/?part=status,snippet&default=true&key=";

    private string chatID = "";

    private const string channelid = "UCyPls4ziyLUMyvw6IsLnjiw";

    private const string key = "AIzaSyCMvDz1MWInfV2iZC7gG4t-TFaLLGMZ6iA";

    public InputField inputtext;

    public GameObject songscript;

    private void Start()
    {
        StartCoroutine(GetChatID("https://www.googleapis.com/youtube/v3/liveStreams/?part=status,snippet&default=true&key=AIzaSyCMvDz1MWInfV2iZC7gG4t-TFaLLGMZ6iA"));
    }

    private IEnumerator GetChatID(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            string[] array = uri.Split('/');
            int num = array.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(array[num] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                JSONNode jSONNode = JSON.Parse(webRequest.downloadHandler.text);
                Debug.Log("CHANNELIDTEST " + jSONNode);
                Debug.Log(jSONNode["items"][0]["snippet"]["liveChatId"].Value.ToString());
                chatID = jSONNode["items"][0]["snippet"]["liveChatId"].Value.ToString();
            }
        }
        StartCoroutine(GetRequest("https://www.googleapis.com/youtube/v3/liveChat/messages?liveChatId=" + chatID + "&part=snippet%2CauthorDetails&key=AIzaSyCMvDz1MWInfV2iZC7gG4t-TFaLLGMZ6iA"));
    }

    private IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            string[] array = uri.Split('/');
            int num = array.Length - 1;
            if (webRequest.isNetworkError)
            {
                Debug.Log(array[num] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                JSONNode jSONNode = JSON.Parse(webRequest.downloadHandler.text);
                songscript.GetComponent<SongIconController>();
                for (int i = 0; i < jSONNode["items"].Count; i++)
                {
                    Debug.Log(i + ". message:  " + jSONNode["items"][i]["snippet"]["displayMessage"].Value.ToString());
                    inputtext.text = inputtext.text + "\n\r" + jSONNode["items"][i]["snippet"]["displayMessage"].Value;
                }
            }
        }
    }
}
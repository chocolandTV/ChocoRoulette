// YouTubeLiveController
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class YouTubeLiveController : MonoBehaviour
{
    private IEnumerator Start()
    {
        string clientId = "client_id";
        string clientSecret = "client_secret";
        string code = "AIzaSyCMvDz1MWInfV2iZC7gG4t-TFaLLGMZ6iA";
        LocalServer(delegate (string c)
        {
            code = c;
        });
        Application.OpenURL("https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id=" + clientId + "&redirect_uri=http://localhost:8080&scope=https://www.googleapis.com/auth/youtube.readonly&access_type=offline");
        yield return new WaitUntil(() => code != "");
        Debug.Log(code);
        string uri = "https://www.googleapis.com/oauth2/v4/token";
        Dictionary<string, string> formFields = new Dictionary<string, string>
        {
            {
                "code",
                code
            },
            {
                "client_id",
                clientId
            },
            {
                "client_secret",
                clientSecret
            },
            {
                "redirect_uri",
                "http://localhost:8080"
            },
            {
                "grant_type",
                "authorization_code"
            },
            {
                "access_type",
                "offline"
            }
        };
        UnityWebRequest request = UnityWebRequest.Post(uri, formFields);
        yield return request.SendWebRequest();
        JSONNode jSONNode = JSON.Parse(request.downloadHandler.text);
        string token = jSONNode["access_token"].Value;
        Debug.Log(token);
        string str = "https://www.googleapis.com/youtube/v3/liveBroadcasts?part=snippet";
        str += "&id=xxxxxxxxxxx";
        UnityWebRequest req2 = UnityWebRequest.Get(str);
        req2.SetRequestHeader("Authorization", "Bearer " + token);
        yield return req2.SendWebRequest();
        jSONNode = JSON.Parse(req2.downloadHandler.text);
        string text = jSONNode["items"][0]["snippet"]["liveChatId"].Value;
        Debug.Log(text);
        str = "https://www.googleapis.com/youtube/v3/liveChat/messages?part=snippet,authorDetails";
        str = str + "&liveChatId=" + text;
        req2 = UnityWebRequest.Get(str);
        req2.SetRequestHeader("Authorization", "Bearer " + token);
        yield return req2.SendWebRequest();
        jSONNode = JSON.Parse(req2.downloadHandler.text);
        JSONNode.Enumerator enumerator = jSONNode["items"].GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<string, JSONNode> current = enumerator.Current;
            JSONNode jSONNode2 = current.Value["snippet"];
            Debug.Log(current.Value["authorDetails"]["displayName"].Value + ": " + jSONNode2["displayMessage"].Value);
        }
        Debug.Log(jSONNode["nextPageToken"]);
    }

    private void LocalServer(Action<string> onReceive)
    {
        new Thread((ThreadStart)delegate
        {
            try
            {
                HttpListener httpListener = new HttpListener();
                httpListener.Prefixes.Add("http://*:8080/");
                httpListener.Start();
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                string obj = new Regex("/\\?code=(?<c>.*)").Match(request.RawUrl).Groups["c"].ToString();
                onReceive(obj);
                response.StatusCode = 200;
                response.Close();
            }
            catch (Exception message)
            {
                Debug.LogError(message);
            }
        }).Start();
    }
}

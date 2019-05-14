// SongIconController
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongIconController : MonoBehaviour
{
    public List<SongInfo> Chocolist = new List<SongInfo>();

    public GameObject inputfieldText;

    private GameObject objectPoolerParent;

    private ChocoManager chocoManager;

    public GameObject SongIcon;

    public GameObject mother;

    public GameObject Gameborder;

    public GameObject currentSongScript;

    private bool Gamestate;

    private bool motherMove;

    private float posx;

    public Vector3 motherHolderstartPosition = new Vector3(0f, 300f, 0f);

    public List<GameObject> spawnedPoolItems = new List<GameObject>();

    private string[] textSplit = new string[1]
    {
        "all"
    };

    public GameObject endGameObject;

    private int foundSongIcon;

    private float posy = 300f;

    private int Spawncounter;

    private IEnumerator Waitingseconds(int sec)
    {
        yield return new WaitForSeconds(sec);
        if (sec == 25)
        {
            ButtonPress();
        }
    }

    void Start()
    {
        
        Gameborder.SetActive(value: false);
    }

    private void EndGame()
    {
        motherMove = false;
        foreach (GameObject spawnedPoolItem in spawnedPoolItems)
        {
            if (spawnedPoolItem.name != currentSongScript.GetComponent<ActiveSong>().CurrentSong)
            {
                UnityEngine.Object.Destroy(spawnedPoolItem);
            }
            else if (foundSongIcon == 0)
            {
                foundSongIcon = 1;
                GameObject gameObject = UnityEngine.Object.Instantiate(spawnedPoolItem, new Vector3(0f, 0f, 0f), Quaternion.identity);
                gameObject.gameObject.transform.localScale += new Vector3(0.4f, 0.4f, 0.4f);
                gameObject.transform.SetParent(Gameborder.gameObject.transform);
                gameObject.transform.position = new Vector3(Gameborder.gameObject.transform.position.x, Gameborder.gameObject.transform.position.y - 200f, Gameborder.gameObject.transform.position.z);
                UnityEngine.Object.Destroy(spawnedPoolItem);
            }
            else
            {
                UnityEngine.Object.Destroy(spawnedPoolItem);
            }
        }
        StartCoroutine(Waitingseconds(5));
        GetComponentInChildren<Text>().text = "OK";
    }

    public void ButtonPress()
    {
        if (!Gamestate)
        {
            GetComponentInChildren<Text>().text = "Stop";
            Gameborder.SetActive(value: true);
            ChocoManager.FillSonglist(Chocolist);
            Gamestate = true;
            SpawnPooledItems();
            StartCoroutine(Waitingseconds(25));
        }
        else if (GetComponentInChildren<Text>().text == "OK")
        {
            Gameborder.SetActive(value: false);
            Gamestate = false;
            GetComponentInChildren<Text>().text = "Start";
            inputfieldText.GetComponent<Text>().text = inputfieldText.GetComponent<Text>().text.Replace(currentSongScript.GetComponent<ActiveSong>().CurrentSong, "");
            Chocolist.Clear();
            posx = 0f;
            UnityEngine.Object.Destroy(endGameObject);
            SceneManager.LoadScene("ChocoRoulette", new LoadSceneParameters(LoadSceneMode.Additive));
        }
        else
        {
            EndGame();
        }
    }

    public static List<SongInfo> Shuffle(List<SongInfo> aList)
    {
        System.Random random = new System.Random();
        int count = aList.Count;
        for (int i = 0; i < count; i++)
        {
            int index = i + (int)(random.NextDouble() * (double)(count - i - 1));
            SongInfo value = aList[index];
            aList[index] = aList[i];
            aList[i] = value;
        }
        return aList;
    }

    public IEnumerator AnimatePosition(GameObject moveObject)
    {
        motherMove = true;
        Rigidbody rb = moveObject.GetComponent<Rigidbody>();
        rb.AddForce(Quaternion.Euler(0f, 0f, 0f) * Vector3.left * 150f);
        while (motherMove)
        {
            rb.AddForce(Quaternion.Euler(0f, 0f, 0f) * Vector3.left * 5f);
            if ((float)(-100 * spawnedPoolItems.Count) > moveObject.transform.position.x)
            {
                moveObject.transform.position = new Vector3(150 * spawnedPoolItems.Count, moveObject.transform.position.y, 0f);
            }
            if (!motherMove)
            {
                motherMove = false;
                rb.velocity = new Vector2(0f, 0f);
            }
            yield return null;
        }
    }

    private bool Spawnroutiner(int id)
    {
        Spawncounter++;
        if (Spawncounter < id)
        {
            return false;
        }
        Spawncounter = 0;
        return true;
    }

    private void SpawnPooledItems()
    {
        List<GameObject> list = new List<GameObject>();
        string text = inputfieldText.GetComponent<Text>().text;
        if (text == null || text == "")
        {
            textSplit[0] = "all";
            Debug.Log(textSplit[0]);
        }
        else
        {
            textSplit = text.Split('\n');
        }
        for (int i = 0; i < textSplit.Length; i++)
        {
            textSplit[i] = textSplit[i].ToLowerInvariant();
        }
        List<SongInfo> list2 = Shuffle(Chocolist);
        objectPoolerParent = new GameObject("Mutter hat songs");
        objectPoolerParent.transform.SetParent(mother.gameObject.transform);
        objectPoolerParent.transform.position = new Vector3(mother.gameObject.transform.position.x + posx, mother.gameObject.transform.position.y, mother.gameObject.transform.position.z);
        Rigidbody rigidbody = objectPoolerParent.AddComponent<Rigidbody>();
        rigidbody.mass = 0.01f;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.constraints = (RigidbodyConstraints)12;
        list.Add(objectPoolerParent);
        string[] array = textSplit;
        foreach (string text2 in array)
        {
            if (text2 == "all" || text2 == "ALL")
            {
                Spawncounter = 0;
                for (int k = 0; k < list2.Count; k++)
                {
                    if (Spawnroutiner(50))
                    {
                        objectPoolerParent = new GameObject("Mutter hat songs");
                        objectPoolerParent.transform.SetParent(mother.gameObject.transform);
                        objectPoolerParent.transform.position = new Vector3(mother.gameObject.transform.position.x + posx, mother.gameObject.transform.position.y, mother.gameObject.transform.position.z);
                        Rigidbody rigidbody2 = objectPoolerParent.AddComponent<Rigidbody>();
                        rigidbody2.mass = 0.01f;
                        rigidbody2.constraints = RigidbodyConstraints.FreezeRotation;
                        rigidbody2.constraints = (RigidbodyConstraints)12;
                        list.Add(objectPoolerParent);
                    }
                    GameObject gameObject = UnityEngine.Object.Instantiate(SongIcon, new Vector3(posx, posy, 0f), Quaternion.identity);
                    gameObject.transform.SetParent(objectPoolerParent.gameObject.transform);
                    gameObject.name = list2[k].skinName;
                    gameObject.gameObject.transform.GetChild(0).GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
                    gameObject.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = (Resources.Load(list2[k].img, typeof(Sprite)) as Sprite);
                    gameObject.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = list2[k].skinName;
                    spawnedPoolItems.Add(gameObject);
                    posx += 250f;
                }
            }
            else
            {
                Spawncounter = 0;
                for (int l = 0; l < list2.Count; l++)
                {
                    if (text2 == list2[l].skinName.ToLowerInvariant() || text2 == list2[l].artist.ToLowerInvariant() || text2 == list2[l].game.ToLowerInvariant() || text2 == list2[l].year.ToLowerInvariant() || text2 == list2[l].mode.ToLowerInvariant())
                    {
                        if (Spawnroutiner(1))
                        {
                            objectPoolerParent = new GameObject("Mutter hat songs");
                            objectPoolerParent.transform.SetParent(mother.gameObject.transform);
                            objectPoolerParent.transform.position = new Vector3(mother.gameObject.transform.position.x + posx, mother.gameObject.transform.position.y, mother.gameObject.transform.position.z);
                            Rigidbody rigidbody3 = objectPoolerParent.AddComponent<Rigidbody>();
                            rigidbody3.mass = 0.01f;
                            rigidbody3.constraints = RigidbodyConstraints.FreezeRotation;
                            rigidbody3.constraints = (RigidbodyConstraints)12;
                            list.Add(objectPoolerParent);
                        }
                        GameObject gameObject2 = UnityEngine.Object.Instantiate(SongIcon, new Vector3(posx, posy, 0f), Quaternion.identity);
                        gameObject2.transform.SetParent(objectPoolerParent.gameObject.transform);
                        gameObject2.name = list2[l].skinName;
                        gameObject2.gameObject.transform.GetChild(0).GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
                        gameObject2.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = (Resources.Load(list2[l].img, typeof(Sprite)) as Sprite);
                        gameObject2.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = list2[l].skinName;
                        spawnedPoolItems.Add(gameObject2);
                        posx += 250f;
                    }
                }
            }
            if (!text2.Contains("random"))
            {
                continue;
            }
            Spawncounter = 0;
            int num = int.Parse(text2.Replace("random", ""));
            for (int m = 0; m < num; m++)
            {
                int index = UnityEngine.Random.Range(1, Chocolist.Count);
                if (Spawnroutiner(num))
                {
                    objectPoolerParent = new GameObject("Mutter hat songs");
                    objectPoolerParent.transform.SetParent(mother.gameObject.transform);
                    objectPoolerParent.transform.position = new Vector3(mother.gameObject.transform.position.x + posx, mother.gameObject.transform.position.y, mother.gameObject.transform.position.z);
                    Rigidbody rigidbody4 = objectPoolerParent.AddComponent<Rigidbody>();
                    rigidbody4.mass = 0.01f;
                    rigidbody4.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidbody4.constraints = (RigidbodyConstraints)12;
                    list.Add(objectPoolerParent);
                }
                GameObject gameObject3 = UnityEngine.Object.Instantiate(SongIcon, new Vector3(posx, posy, 0f), Quaternion.identity);
                gameObject3.transform.SetParent(objectPoolerParent.gameObject.transform);
                gameObject3.name = list2[index].skinName;
                gameObject3.gameObject.transform.GetChild(0).GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
                gameObject3.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = (Resources.Load(list2[index].img, typeof(Sprite)) as Sprite);
                gameObject3.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = list2[index].skinName;
                spawnedPoolItems.Add(gameObject3);
                posx += 250f;
            }
        }
        if (spawnedPoolItems.Count < 40)
        {
            SpawnPooledItems();
        }
        foreach (GameObject item in list)
        {
            StartCoroutine(AnimatePosition(item));
        }
        objectPoolerParent.name = "Mutter hat  " + spawnedPoolItems.Count + " songs";
    }
}

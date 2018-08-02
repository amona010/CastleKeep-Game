using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class jsonDataHandler : MonoBehaviour {

    string filePath;
    string contents;
    public int currentMapIndex;
    public gameData data = new gameData();

    public void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "data.json");
        currentMapIndex = 0;
        StartCoroutine(initFile());
    }

    //public void Update()
    //{
    //    if (contents != null && firstRun)
    //    {
    //        readFromJSON();
    //    }
    //}

    public void addNewMap(string mapName, int roundNum, int numAttackers, int waves)
    {
        data.maps.Add(new gameData.mapData(mapName, roundNum, numAttackers, waves));
    }

    public void addNewPoint(string pointName, bool host, bool mainHost, bool spawnPoint, bool isSwitch, bool upper, bool lower, bool left, bool right, float x, float y)
    {
        data.maps[currentMapIndex].vertices.Add(new gameData.mapData.point(pointName, host, mainHost, spawnPoint, isSwitch, upper, lower, left, right, x, y));
    }

    public void addNewEdge(string v1, string v2)
    {
        data.maps[currentMapIndex].edges.Add(new gameData.mapData.edge(v1, v2));
    }

    public void saveToJSON()
    {
        string contents = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, string.Empty);
        File.WriteAllText(filePath, contents);
    }

    public void readFromJSON()
    {
        data = JsonUtility.FromJson<gameData>(contents);
    }

    public void nextMap()
    {
        if(currentMapIndex == data.maps.Count - 1)
        {
            currentMapIndex = 0;
        }
        else
        {
            currentMapIndex++;
        }
    }

    private IEnumerator initFile()
    {
        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            contents = www.text;
            Debug.Log("WWW ");
            readFromJSON();
        }
        else
        {
            contents = File.ReadAllText(filePath);
            Debug.Log("NOT WWW: ");
            readFromJSON();
        }
    }
}

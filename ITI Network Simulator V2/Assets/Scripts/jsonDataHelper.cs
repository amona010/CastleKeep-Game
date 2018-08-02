using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class jsonDataHelper : MonoBehaviour
{
    private jsonDataHandler handler;
    private waveHandler gameCTRL;
    private Graph gameMap;
    private sendDataToForm gameData;
    private GameObjectHolder gameComponents;
    private currencyCounter money;
    public Transform[] objects;
    public Text restartOrNewMap;

    private void Start()
    {
        handler = GameObject.Find("userData").GetComponent<jsonDataHandler>();
        gameMap = gameObject.GetComponent<Graph>();
        gameCTRL = gameObject.GetComponent<waveHandler>();
        gameData = gameObject.GetComponent<sendDataToForm>();
        gameComponents = gameObject.GetComponent<GameObjectHolder>();
        money = gameComponents.HealthAndCurrency.GetComponent<currencyCounter>();
        fillMapWithPoints();
        initializeMapSettings();

        if(handler.currentMapIndex == handler.data.maps.Count - 1)
        {
            restartOrNewMap.text = "Restart";
        }
        else
        {
            restartOrNewMap.text = "Next Map";
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        gameCTRL.saveMapSettingsToJSON(handler);
    //        gameMap.saveAllPointsToJSON(handler);
    //        gameMap.saveAllEdgesToJSON(handler);
    //        handler.saveToJSON();
    //    }
    //}

    public void fillMapWithPoints()
    {
        foreach(gameData.mapData.point p in handler.data.maps[handler.currentMapIndex].vertices)
        {
            string pointName = p.name;
            bool host = p.isHost;
            bool mainHost = p.isMainHost;
            bool spawnPoint = p.isSpawnPoint;
            bool isASwitch  = p.isSwitch;
            bool upper = p.hasUpperTile;
            bool lower = p.hasLowerTile;
            bool left = p.hasLeftTile;
            bool right = p.hasRightTile;
            float x = p.location.x;
            float y = p.location.y;

            gameMap.initializePointOnMap(pointName, host, mainHost, spawnPoint, isASwitch, upper, lower, left, right, x, y);
        }

        gameMap.initializeMap();

        foreach (gameData.mapData.point p in handler.data.maps[handler.currentMapIndex].vertices)
        {
            string pointName = p.name;
            bool host = p.isHost;
            bool mainHost = p.isMainHost;
            bool spawnPoint = p.isSpawnPoint;

            if(host)
            {
                gameMap.setHost(pointName);
            }

            if(mainHost)
            {
                gameMap.setMainHost(pointName);
            }

            if(spawnPoint)
            {
                gameMap.setSpawnPoint(pointName);
            }
        }

        foreach(gameData.mapData.edge e in handler.data.maps[handler.currentMapIndex].edges)
        {
            gameMap.setEdge(e.v1, e.v2);
        }
    }

    public void initializeMapSettings()
    {
        gameData.setMapName(handler.data.maps[handler.currentMapIndex].name);
        money.gainMoney(handler.data.maps[handler.currentMapIndex].startingCurrency);
        for (int i = 0; i < handler.data.maps[handler.currentMapIndex].waves; i++)
        {
            gameCTRL.waves.Add(new waveHandler.wave(gameComponents.attackers, gameComponents.citizen, handler.data.maps[handler.currentMapIndex].packetsPerWave));
        }
        gameCTRL.attackerChance = handler.data.maps[handler.currentMapIndex].attackerProbability;
    }

    public bool isMapCompleted(int currentRound)
    {
        return currentRound == handler.data.maps[handler.currentMapIndex].rounds;
    }

    public void setNextMap()
    {
        handler.nextMap();
    }

    public int getRedAccuracyLowHealth()
    {
        return 100 - handler.data.maps[handler.currentMapIndex].redTileLowHealthAccuracyAttackers;
    }

    public int getRedAccuracyCriticalHealth()
    {
        return 100 - handler.data.maps[handler.currentMapIndex].redTileCriticalHealthAccuracyAttackers;
    }

    public int getRedCitizenAccuracyLowHealth()
    {
        return 100 - handler.data.maps[handler.currentMapIndex].redTileLowHealthAccuracyCitizens;
    }

    public int getRedCitizenAccuracyCriticalHealth()
    {
        return 100 - handler.data.maps[handler.currentMapIndex].redTileLowHealthAccuracyCitizens;
    }

    public int getOrangeCitizenAccuracy()
    {
        return 100 - handler.data.maps[handler.currentMapIndex].orangeTileAccuracyCitizens;
    }

    public int getOrangeAttackerLimit()
    {
        return handler.data.maps[handler.currentMapIndex].orangeTileAttackerLimit;
    }

    public int getRoundCurrency()
    {
        return handler.data.maps[handler.currentMapIndex].currencyGainedPerRound;
    }

    public int getRedMaxHealth()
    {
        return handler.data.maps[handler.currentMapIndex].redTileMaximumHealth;
    }
}

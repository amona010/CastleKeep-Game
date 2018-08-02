using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    public List<mapData> maps = new List<mapData>();

    [System.Serializable]
    public class mapData
    {
        public string name;
        public int rounds;
        public int startingCurrency;
        public int currencyGainedPerRound;
        public int waves;
        public int redTileMaximumHealth;
        public int redTileLowHealthAccuracyAttackers;
        public int redTileCriticalHealthAccuracyAttackers;
        public int redTileLowHealthAccuracyCitizens;
        public int redTileCriticalHealthAccuracyCitizens;
        public int orangeTileAccuracyCitizens;
        public int orangeTileAttackerLimit;
        public int attackerProbability;
        public int packetsPerWave;
        public List<point> vertices = new List<point>();
        public List<edge> edges = new List<edge>();

        [System.Serializable]
        public class point
        {
            public string name;
            public bool isHost;
            public bool isMainHost;
            public bool isSpawnPoint;
            public bool isSwitch;
            public bool hasUpperTile;
            public bool hasLowerTile;
            public bool hasLeftTile;
            public bool hasRightTile;
            
            public vector location;

            public point(string pointName, bool host, bool mainHost, bool spawnPoint, bool isASwitch, bool upper, bool lower, bool left, bool right, float x, float y)
            {
                name = pointName;
                isHost = host;
                isMainHost = mainHost;
                isSpawnPoint = spawnPoint;
                isSwitch = isASwitch;
                hasUpperTile = upper;
                hasLowerTile = lower;
                hasLeftTile = left;
                hasRightTile = right;
                location = new vector(x, y);
            }

            [System.Serializable]
            public class vector
            {
                public float x;
                public float y;

                public vector(float xVal, float yVal)
                {
                    x = xVal;
                    y = yVal;
                }
            }
        }

        [System.Serializable]
        public class edge
        {
            public string v1;
            public string v2;

            public edge(string vertex1, string vertex2)
            {
                v1 = vertex1;
                v2 = vertex2;
            }
        }

        public mapData(string mapName, int roundNum, int numPackets, int wavesPerRound)
        {
            name = mapName;
            rounds = roundNum;
            packetsPerWave = numPackets;
            waves = wavesPerRound;
        }
    }
}

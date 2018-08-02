using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [System.Serializable]
    public class Vertex
    {
        public GameObject point;
        public bool connection;
        public bool isSpawnPoint;
        public bool host;
        public bool mainHost;

        public void setVertex(string pointName)
        {
            point = GameObject.Find(pointName);
        }

        public GameObject getPoint()
        {
            return point;
        }

        public void setConnection(bool value)
        {
            connection = value;
        }

        public bool isConnected()
        {
            return connection;
        }

        public void setSpawnable(bool value)
        {
            isSpawnPoint = value;
        }

        public void setHost(bool value)
        {
            host = value;
        }

        public void setMainHost(bool value)
        {
            mainHost = value;
        }

        public bool isHost()
        {
            return host;
        }

        public bool isMainHost()
        {
            return mainHost;
        }

        public bool spawnable()
        {
            return isSpawnPoint;
        }

        public string getName()
        {
            return point.name;
        }
    }

    public GameObject[] points;
    public Vertex[,] matrix;
    private GameObjectHolder objs;

    private void Start() //In the future this won't be needed
    {
        objs = gameObject.GetComponent<GameObjectHolder>();
    }

    public void initializeMap()
    {
        points = GameObject.FindGameObjectsWithTag("Point");
        matrix = new Vertex[points.Length + 1, points.Length + 1];
        objs = gameObject.GetComponent<GameObjectHolder>();
        matrix[0, 0] = new Vertex();

        for (int i = 1; i < points.Length + 1; i++)
        {
            matrix[0, i] = new Vertex();
            matrix[0, i].setVertex(points[i - 1].name);
            matrix[i, 0] = new Vertex();
            matrix[i, 0].setVertex(points[i - 1].name);

        }

        for (int i = 1; i < points.Length + 1; i++)
        {
            for (int k = 1; k < points.Length + 1; k++)
            {
                matrix[i, k] = new Vertex();
                matrix[i, k].setVertex(points[k - 1].name);

                //Debug.Log("[" + i + ", " + k + "]" + " = " + matrix[i, k].getName());
            }
        }
    }

    public Vertex[] findConnectedVertices(string name)
    {
        int total = 0;
        int loc = 0;
        Vertex[] vertices = new Vertex[points.Length];
        Vertex[] connections = new Vertex[points.Length];

        for(int i = 1; i < points.Length + 1; i++)
        {
            if(matrix[i, 0].getName().Equals(name))
            {
                for(int k = 1; k < points.Length + 1; k++)
                {
                    vertices[k-1] = matrix[i, k];
                }
            }
        }

        for(int i = 0; i < points.Length; i++)
        {
            if(vertices[i].isConnected())
            {
                connections[i] = vertices[i];
                total++;
            }
        }

        Vertex[] adj = new Vertex[total];

        for (int i = 0; i < connections.Length; i++)
        {
            if(connections[i] != null)
            {
                adj[loc] = connections[i];
                loc++;
            }
        }

        return adj;
    }

    public void setEdge(string point1, string point2)
    {
        int i, k;
        bool success = false;
        GameObject line;

        for (i = 1; i < points.Length + 1; i++)
        {
            if (matrix[i, 0].getName().Equals(point1))
            {
                for (k = 1; k < points.Length + 1; k++)
                {
                    if (matrix[i, k].getName().Equals(point2))
                    {
                        success = true;
                        matrix[i, k].setConnection(true);
                        matrix[k, i].setConnection(true);
                        line = Instantiate(objs.lineRenderer);
                        line.GetComponent<trailMaker>().drawLine(matrix[i, 0].point.GetComponent<Transform>(), (matrix[i, k].point.GetComponent<Transform>()));
                        Debug.Log(point1 + " + " + point2 + " successfully connected");
                        break;
                    }
                }
            }

            if (success)
                break;
        }
    }

    public void setSpawnPoint(string name)
    {
        for (int k = 1; k < points.Length + 1; k++)
        {
            if(matrix[0, k].getName().Equals(name))
            {
               matrix[0, k].setSpawnable(true);
            }
        }
    }

    public Vertex[] getSpawnPoints()
    {
        int total = 0;
        int loc = 0;

        for (int k = 1; k < points.Length + 1; k++)
        {
            if(matrix[0, k].spawnable())
            {
                total++;
            }
        }


        Vertex[] adj = new Vertex[total];

        for (int k = 1; k < points.Length + 1; k++)
        {
            if (matrix[0, k].spawnable())
            {
                adj[loc] = matrix[0, k];
                loc++;
            }
        }

        return adj;
    }

    public Transform[] initializeSpawns()
    {
        Vertex[] spawnPoints = getSpawnPoints();
        Transform[] adj = new Transform[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            adj[i] = spawnPoints[i].getPoint().transform;
        }

        return adj;
    }

    public string getStartingPath(Vector3 position)
    {
        Vertex[] spawnPoints = getSpawnPoints();
        string ans = "";

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].getPoint().transform.position.Equals(position))
                ans = spawnPoints[i].getName();
        }

        return ans;
    }

    public string getNextPosition(string current, Stack<string> visited)
    {
        Stack<string> movementOptions = new Stack<string>();
        Vertex[] adj = findConnectedVertices(current);

        for(int i = 0; i < adj.Length; i++)
        {
            if(!visited.Contains(adj[i].getName()))
            {
                movementOptions.Push(adj[i].getName());
            }
        }

        if(movementOptions.Count == 0)
        {
            return "err";
        }
        else
        {
            string[] possibleNext = movementOptions.ToArray();
            return possibleNext[Random.Range(0, possibleNext.Length)];
        }
    }

    public void setHost(string name)
    {
        for (int k = 1; k < points.Length + 1; k++)
        {
            if (matrix[0, k].getName().Equals(name))
            {
                matrix[0, k].setHost(true);
                Debug.Log(matrix[0, k].getName() + " is now a host");
            }
        }
    }

    public void setMainHost(string name)
    {
        for (int k = 1; k < points.Length + 1; k++)
        {
            if (matrix[0, k].getName().Equals(name))
            {
                matrix[0, k].setMainHost(true);
                Debug.Log(matrix[0, k].getName() + " is now a Main host");
            }
        }
    }

    public string statusOfCurrentPosition(string name)
    {
        string status = "None";

        for (int k = 1; k < points.Length + 1; k++)
        {
            if (matrix[0, k].getName().Equals(name))
            {
                if(matrix[0, k].isHost())
                {
                    status = "isHost";
                }
                else if(matrix[0, k].isMainHost())
                {
                    status = "isMainHost";
                }
            }
        }

        return status;
    }

    public void saveAllPointsToJSON(jsonDataHandler handler)
    {
        string pointName = "";
        float x = 0, y = 0;
        Transform[] children;
        for (int k = 1; k < points.Length + 1; k++)
        {
            pointName = matrix[0, k].getName();
            children = matrix[0, k].getPoint().GetComponentsInChildren<Transform>(true);
            x = matrix[0, k].getPoint().GetComponent<RectTransform>().position.x;
            y = matrix[0, k].getPoint().GetComponent<RectTransform>().position.y;
            bool host = false, mainHost = false, spawnPoint = false, upper = false, lower = false, left = false, right = false, isSwitch = false;

            if (matrix[0, k].isHost())
            {
                host = true;
            }
            if (matrix[0, k].isMainHost())
            {
                mainHost = true;
            }
            if (matrix[0, k].spawnable())
            {
                spawnPoint = true;
            }
            if(children[1].gameObject.activeInHierarchy)
            {
                upper = true;
            }
            if(children[8].gameObject.activeInHierarchy)
            {
                lower = true;
            }
            if(children[15].gameObject.activeInHierarchy)
            {
                right = true;
            }
            if(children[22].gameObject.activeInHierarchy)
            {
                left = true;
            }
            if(children[29].gameObject.activeInHierarchy)
            {
                children[29].gameObject.SetActive(false);
                isSwitch = true;
            }

            handler.addNewPoint(pointName, host, mainHost, spawnPoint, isSwitch, upper, lower, left, right, x, y);
        }
    }

    public void saveAllEdgesToJSON(jsonDataHandler handler)
    {
        int i, k;
        string vertex1 = "", vertex2 = "";

        for (i = 1; i < points.Length + 1; i++)
        {
            for (k = 1; k < points.Length + 1; k++)
            {
                if (matrix[i, k].isConnected())
                {
                    vertex1 = matrix[i, 0].getName();
                    vertex2 = matrix[i, k].getName();
                    handler.addNewEdge(vertex1, vertex2);
                }
            }
        }
    }

    public void initializePointOnMap(string pointName, bool host, bool mainHost, bool spawnPoint, bool isASwitch, bool upper, bool lower, bool left, bool right, float x, float y)
    {
        GameObject node = Instantiate(objs.point);
        Transform[] children = node.GetComponentsInChildren<Transform>(true);
        node.GetComponent<Transform>().position = new Vector3(x, y, 90);
        node.name = pointName;
        int rand = Random.Range(30, 34);

        if (host)
        {
            children[rand].gameObject.SetActive(true);
        }

        if(mainHost)
        {
            children[34].gameObject.SetActive(true);
        }

        if(isASwitch)
        {
            children[29].gameObject.SetActive(true);
        }

        if(spawnPoint)
        {
            children[rand].gameObject.SetActive(false);
            children[35].gameObject.SetActive(true);
        }

        if(upper)
        {
            children[1].gameObject.SetActive(true);
        }

        if (lower)
        {
            children[8].gameObject.SetActive(true);
        }

        if (right)
        {
            children[15].gameObject.SetActive(true);
        }

        if (left)
        {
            children[22].gameObject.SetActive(true);
        }
    }

}

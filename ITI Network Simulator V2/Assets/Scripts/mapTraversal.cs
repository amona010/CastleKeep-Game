using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapTraversal : MonoBehaviour {

    [SerializeField]
    private float speed;

    private sendDataToForm dataHandler;
    private string next;
    private int ID; 
    public int current;
    private string startingPoint;
    private Graph currentMap;
    private int stepCounter;
    private Stack<string> pathData;
    private Stack<string> correctPath;
    private bool success;
    private GameObject GM;

    private int[] currentPath;

    void Start()
    {
        GM = GameObject.Find("GM");
        dataHandler = GM.GetComponent<sendDataToForm>();
        currentMap =GM.GetComponent<Graph>();
        pathData = new Stack<string>();
        correctPath = new Stack<string>();
        setRandomID();
        selectCurrentPath();
    }

    void Update ()
    {
        //Not at next position, must move.
        if (transform.position != GameObject.Find(next).transform.position)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, GameObject.Find(next).transform.position, speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(pos);
        }
        else //Made it to next position, must determine where to go now
        {
            if(!success)
            {
                think();
            }
            else
            {
                escape();
            }
        }
	}

    void think()
    {
        stepCounter++;
        pathData.Push(next);
        correctPath.Push(next);

        if (!currentMap.statusOfCurrentPosition(next).Equals("isMainHost"))
        {
            if (stepCounter > 7)
            {
                if (currentMap.statusOfCurrentPosition(next).Equals("isHost"))
                {
                    success = true;
                    sendPathToGameData();
                    return;
                }
            }
            next = currentMap.getNextPosition(next, pathData);
            if (next.Equals("err"))
            {
                correctPath.Pop();
                next = correctPath.Pop();
            }
        }
        else
        {
            success = true;
            sendPathToGameData();
            return;
        }
    }

    void escape()
    {
        string test;
        if (correctPath.Count != 0)
        {
            test = correctPath.Pop();
            if (correctPath.Count != 0 && correctPath.Peek() == test)
            {
                correctPath.Pop();
                next = correctPath.Pop();
            }
            else
            {
                next = test;
            }

            if (pathData.Count > 0)
            {
                if (pathData.Peek() != next)
                    pathData.Push(next);
            }
            else
            {
                pathData.Push(next);
            }

        }
        else
        {
            Destroy(gameObject);
        }

    }

    void selectCurrentPath()
    {
        next = currentMap.getStartingPath(transform.position);
    }

    void setRandomID()
    {
        int ones, tens, hundreds, thousands, tenThousands;

        ones = Random.Range(1, 10);
        tens = Random.Range(1, 10) * 10;
        hundreds = Random.Range(1, 10) * 100;
        thousands = Random.Range(1, 10) * 1000;
        tenThousands = Random.Range(1, 10) * 10000;

        ID = ones + tens + hundreds + thousands + tenThousands;
    }

    void sendPathToGameData()
    {
        string pathAndCitizenData = ID + ": ";
        string[] steps = getSteps();

        for (int i = steps.Length - 1; i > -1; i--)
        {
            if (i == 0)
            {
                pathAndCitizenData += steps[i];
            }
            else
            {
                pathAndCitizenData += steps[i] + ", ";
            }
        }

        dataHandler.addCitizenPath(pathAndCitizenData);
    }

    public int getID()
    {
        return ID;
    }

    public string[] getSteps()
    {
        string[] steps = correctPath.ToArray();
        return steps;
    }

    public string getNext()
    {
        return next;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapTraversalAttacker : MonoBehaviour {

    [SerializeField]
    private float speed;
    private int ID;
    private bool success;
    private Stack<string> correctPath;
    private Stack<string> pathData;   
    private int current;
    private int previous;
    private int stepCounter;
    private sendDataToForm dataHandler;
    private healthBar gameHealth;
    private identityHandler attackerIdentity;
    private Graph currentMap;
    private GameObject GM;

    public GameObject healthCTRL;
    public string next;
    


    void Start()
    {
        GM = GameObject.Find("GM");
        stepCounter = 0;
        success = false;
        pathData = new Stack<string>();
        correctPath = new Stack<string>();
        attackerIdentity = gameObject.GetComponent<identityHandler>();
        healthCTRL = GM.GetComponent<GameObjectHolder>().HealthAndCurrency;
        gameHealth = healthCTRL.GetComponent<healthBar>();
        dataHandler = GM.GetComponent<sendDataToForm>();
        currentMap = GM.GetComponent<Graph>();
        setRandomID();
        next = currentMap.getStartingPath(transform.position);
        Debug.Log(next);
    }

    void Update()
    {
            if (transform.position != GameObject.Find(next).transform.position) //Not at target position, must move
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

        if(!currentMap.statusOfCurrentPosition(next).Equals("isMainHost"))
        {
            if (stepCounter > 10)
            {
                if (currentMap.statusOfCurrentPosition(next).Equals("isHost"))
                {
                    success = true;
                    attackerIdentity.setSuccessful();
                    sendPathToGameData();
                    gameHealth.loseWealth(30);
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
            attackerIdentity.setSuccessful(); 
            sendPathToGameData();
            gameHealth.loseWealth(50);
            return;
        }
    }

    void escape()
    {
        string test;
        if(correctPath.Count != 0)
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

            if(pathData.Count > 0)
            {
                if(pathData.Peek() != next)
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

    void selectCurrentPath()
    {
        next = currentMap.getStartingPath(transform.position);
    }

    private int coinFlip()
    {
        int coin = Random.Range(0, 2);
        return coin;
    }

    void sendPathToGameData()
    {
        string color = "";

        if (gameObject.tag.Equals("Attacker_red"))
        {
            color = "R";
        }
        else if (gameObject.tag.Equals("Attacker_blue"))
        {
            color = "B";
        }
        else if (gameObject.tag.Equals("Attacker_orange"))
        {
            color = "O";
        }
        else if (gameObject.tag.Equals("Attacker_gray"))
        {
            color = "G";
        }

        string pathAndAttackerData = color + ID + ": ";
        string[] steps = getSteps();

        for(int i = steps.Length - 1; i > -1; i--)
        {
            if(i == 0)
            {
                pathAndAttackerData += steps[i];
            }
            else
            {
                pathAndAttackerData += steps[i] + ", ";
            }
        }

        dataHandler.addAttackerPath(pathAndAttackerData);
    }

    public string[] getSteps()
    {
        string[] steps = correctPath.ToArray();
        return steps;
    }

    public int getID()
    {
        return ID;
    }

    public string getNext()
    {
        return next;
    }
}

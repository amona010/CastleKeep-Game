using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class identityHandler : MonoBehaviour {
    private Transform[] renderers;
    private Stack<string> locationsVisited, redsPassed;
    private sendDataToForm formData;
    private mapTraversalAttacker pathData;
    private bool risk = false;
    private bool marked = false;

	// Use this for initialization
	void Start ()
    {
        formData = GameObject.Find("GM").GetComponent<sendDataToForm>();
        pathData = gameObject.GetComponent<mapTraversalAttacker>();
        renderers = GetComponentsInChildren<Transform>(true);
        renderers[2].gameObject.SetActive(true);
        locationsVisited = new Stack<string>();
        redsPassed = new Stack<string>();
	}

    public void exposed()
    {
        risk = true;
        renderers[3].gameObject.SetActive(false);
        renderers[2].gameObject.SetActive(false);
        renderers[1].gameObject.SetActive(true);
    }

    public void dieIfAtRisk()
    {
        if(risk)
        {
            formData.attackerKilled();
            sendUnsuccessfulPathToGameData();
            Destroy(gameObject);
        }
    }

    public void setMarked(string location)
    {
        if(marked && !locationsVisited.Contains(location))
        {
            sendUnsuccessfulPathToGameData();
            Destroy(gameObject);
        }
        else
        {
            marked = true;
            renderers[3].gameObject.SetActive(true);
            renderers[2].gameObject.SetActive(false);
            renderers[1].gameObject.SetActive(false);
            locationsVisited.Push(location);
        }
    }

    public void setSuccessful()
    {
        renderers[4].gameObject.SetActive(true);
    }

    public void redLocationAccepted(string location)
    {
        redsPassed.Push(location);
    }

    public void testIfAccepted(string location)
    {
        if(!redsPassed.Contains(location))
        {
            exposed();
        }
    }

    public void sendUnsuccessfulPathToGameData()
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

        string pathAndAttackerData = color + pathData.getID() + ": ";
        string[] steps = pathData.getSteps();

        for (int i = steps.Length - 1; i > -1; i--)
        {
            pathAndAttackerData += steps[i] + ", ";
        }

        pathAndAttackerData += pathData.getNext();

        formData.addUnsuccessfulAttackerPath(pathAndAttackerData);
    }
}

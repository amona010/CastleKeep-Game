using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class citizenIdentityHandler : MonoBehaviour {
    private Transform[] renderers;
    private Stack<string> locationsVisited, redsPassed, orangesPassed;
    private bool marked = false;
    private healthBar health;
    private sendDataToForm formData;
    private GameObject GM;
    private mapTraversal pathHolder;

    // Use this for initialization
    void Start()
    {
        GM = GameObject.Find("GM");
        health = GM.GetComponent<GameObjectHolder>().HealthAndCurrency.GetComponent<healthBar>();
        formData = GM.GetComponent<sendDataToForm>();
        pathHolder = gameObject.GetComponent<mapTraversal>();
        renderers = GetComponentsInChildren<Transform>(true);
        renderers[1].gameObject.SetActive(true);
        locationsVisited = new Stack<string>();
        redsPassed = new Stack<string>();
        orangesPassed = new Stack<string>();
    }

    public void setMarked(string location)
    {
        if (marked && !locationsVisited.Contains(location))
        {
            health.loseWealth(5);
            formData.citizenStopped();
            sendDestroyedCitizenToGameData();
            Destroy(gameObject);
        }
        else
        {
            marked = true;
            renderers[2].gameObject.SetActive(true);
            renderers[1].gameObject.SetActive(false);
            locationsVisited.Push(location);
        }
    }

    public void redLocationAccepted(string location)
    {
        redsPassed.Push(location);
    }

    public void testIfAccepted(string location)
    {
        if (!redsPassed.Contains(location))
        {
            health.loseWealth(5);
            formData.citizenStopped();
            sendDestroyedCitizenToGameData();
            Destroy(gameObject);
        }
    }

    public void orangeLocationAccepted(string location)
    {
        orangesPassed.Push(location);
    }

    public void testIfOrangeAccepted(string location)
    {
        if (!orangesPassed.Contains(location))
        {
            health.loseWealth(5);
            formData.citizenStopped();
            sendDestroyedCitizenToGameData();
            Destroy(gameObject);
        }
    }

    public void sendDestroyedCitizenToGameData()
    {
        string pathAndCitizenData = pathHolder.getID() + ": ";
        string[] steps = pathHolder.getSteps();

        for (int i = steps.Length - 1; i > -1; i--)
        {
            pathAndCitizenData += steps[i] + ", ";
        }

        pathAndCitizenData += pathHolder.getNext();

        formData.addUnsuccessfulCitizenPath(pathAndCitizenData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendDataToForm : MonoBehaviour
{
    private int attackersStopped;
    private int citizensStopped;
    private int moneySpent;
    private int moneyRemaining;
    private string mapName;
    private string success;
    private string attackerPaths;
    private string unsuccessfulAttackerPaths;
    private string citizenPaths;
    private string unsuccessfulCitizenPaths;
    private string actionsMade;
    private userNameHolder userData;

    private void Start()
    {
        userData = GameObject.Find("userData").GetComponent<userNameHolder>();
        attackersStopped = 0;
        citizensStopped = 0;
        moneySpent = 0;
        success = "";
        attackerPaths = "";
        unsuccessfulAttackerPaths = "";
        unsuccessfulCitizenPaths = "";
        citizenPaths = "";
        actionsMade = "";
    }

    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScHwAT0k7-MdrunjojOUcH81nVvOGvkneQ2yLuQoUDSvSo9ig/formResponse";

    public void sendToGoogle(int round)
    {
        StartCoroutine(upload(round)); 
    }

    IEnumerator upload(int round)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1826401929", mapName);
        form.AddField("entry.1697422561", userData.getUserName());
        form.AddField("entry.799448094", round);
        form.AddField("entry.1510332244", attackersStopped.ToString());
        form.AddField("entry.1379884615", citizensStopped.ToString());
        form.AddField("entry.1976763584", moneySpent.ToString());
        form.AddField("entry.2082434026", moneyRemaining.ToString());
        form.AddField("entry.1659844989", success);
        form.AddField("entry.1562228525", actionsMade);
        form.AddField("entry.146423012", attackerPaths);
        form.AddField("entry.1249998310", unsuccessfulAttackerPaths);
        form.AddField("entry.1847006100", citizenPaths); 
        form.AddField("entry.1894224386", unsuccessfulCitizenPaths);


        byte[] rawData = form.data;

        WWW www = new WWW(BASE_URL, rawData);

        yield return www;
    }

    public void attackerKilled()
    {
        attackersStopped++;
    }

    public void citizenStopped()
    {
        citizensStopped++;
    }

    public void spendMoney(int money)
    {
        moneySpent += money;
    }

    public void moneyLeftOver(int money)
    {
        moneyRemaining = money;
    }

    public void actionMade(string action)
    {
        actionsMade += action + "\n";
    }

    public void addAttackerPath(string path)
    {
        attackerPaths += path + "\n";
    }

    public void addUnsuccessfulAttackerPath(string path)
    {
        unsuccessfulAttackerPaths += path + "\n";
    }

    public void addCitizenPath(string path)
    {
        citizenPaths += path + "\n";
    }

    public void addUnsuccessfulCitizenPath(string path)
    {
        unsuccessfulCitizenPaths += path + "\n";
    }

    public void wasSuccessful(string ans)
    {
        success = ans;
    }

    public void setMapName(string name)
    {
        mapName = name;
    }

    public void resetParams()
    {
        attackersStopped = 0;
        citizensStopped = 0;
        moneySpent = 0;
        moneyRemaining = 0;
        success = "";
        attackerPaths = "";
        citizenPaths = "";
        unsuccessfulAttackerPaths = "";
        unsuccessfulCitizenPaths = "";
        actionsMade = "";
    }
}

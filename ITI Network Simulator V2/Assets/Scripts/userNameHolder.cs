using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userNameHolder : MonoBehaviour {

    private string userName;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        userName = "EmptyName";
    }

    public void setUserName(string name)
    {
        userName = name;
    }

    public string getUserName()
    {
        return userName;
    }

}

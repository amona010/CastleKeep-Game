using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour {

    public GameObject cam, startButton, beginButton, cancelButton, exitButton, infoButton;
    public InputField input;
    private userNameHolder userData;
    public Text txt;

    private void Start()
    {
        userData = GameObject.Find("userData").GetComponent<userNameHolder>();
    }

    public void buttonHandler(GameObject obj)
    {
        switch(obj.tag)
        {
            case "BeginDemo":
                txt.text = "Please enter a username";
                startButton.SetActive(false);
                exitButton.SetActive(false);
                infoButton.SetActive(false);
                beginButton.SetActive(true);
                cancelButton.SetActive(true);
                input.gameObject.SetActive(true);
                break;
            case "MainMenu":
                cam.transform.position = new Vector2(0, 0);
                break;
            case "Page1":
                cam.transform.position = new Vector2(29, 0);
                break;
            case "Page2":
                cam.transform.position = new Vector2(58, 0);
                break;
            case "Page3":
                cam.transform.position = new Vector2(87.28f, 0);
                break;
            case "Page4":
                cam.transform.position = new Vector2(110.77f, 0);
                break;
            case "Page5":
                cam.transform.position = new Vector2(138.48f, 0);
                break;
            case "Page6":
                cam.transform.position = new Vector2(165.23f, 0);
                break;
            case "Page7":
                cam.transform.position = new Vector2(192.2f, 0);
                break;
            case "Page8":
                cam.transform.position = new Vector2(219.01f, 0);
                break;
            case "Page9":
                cam.transform.position = new Vector2(245.87f, 0);
                break;
            case "EnterGame":
                if(string.IsNullOrEmpty(input.text.Trim(' ')))
                {
                    input.text = string.Empty;
                    txt.text = "Name cannot be empty";
                }
                else
                {
                    userData.setUserName(input.text);
                    SceneManager.LoadScene("SmallNetwork");
                }
                break;
            case "Cancel":
                txt.text = "Please select an option";
                startButton.SetActive(true);
                exitButton.SetActive(true);
                infoButton.SetActive(true);
                beginButton.SetActive(false);
                cancelButton.SetActive(false);
                input.gameObject.SetActive(false);
                break;
            case "Exit":
                Application.Quit();
                break;
        }
    }
}

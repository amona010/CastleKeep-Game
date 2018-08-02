using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelCTRL : MonoBehaviour {

    public GameObject tutorialPanel, healthAndCurrencyPanel, editPanel;
    private GameObject[] panels;

    public void closeMe()
    {
        gameObject.SetActive(false);
    }

    public void closeAllPanels()
    {
        panels = GameObject.FindGameObjectsWithTag("Panel");

        foreach(GameObject p in panels)
        {
            p.SetActive(false);
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("Starting Scene");
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void openTutorial()
    {
        closeAllPanels();
        healthAndCurrencyPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        

    }

    public void closeTutorial()
    {
        
        tutorialPanel.SetActive(false);
        editPanel.SetActive(true);
        healthAndCurrencyPanel.SetActive(true);
    }

}

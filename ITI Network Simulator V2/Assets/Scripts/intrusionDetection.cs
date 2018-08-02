using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intrusionDetection : MonoBehaviour
{

    public bool isRed = false, isBlue = false, isOrange = false, isGold = false;
    private GameObject obj;
    public GameObject statPanel;
    public GameObject GM;
    private waveHandler waveCTRL;
    private statusHandler statsBar;
    private jsonDataHelper helper;
    private float timer = 1f, delay = 1f, redMax, redHealth = 0;
    private int checkIndex = 0, redCounter = 0, blueCounter = 0, orangeCounter = 0, grayCounter = 0;
    public Transform[] renderers;
    private void Start()
    {
        GM = GameObject.Find("GM");
        waveCTRL = GM.GetComponent<waveHandler>();
        statPanel = GM.GetComponent<GameObjectHolder>().StatusBar;
        statsBar = statPanel.GetComponent<statusHandler>();
        renderers = GetComponentsInChildren<Transform>(true);
        helper = GM.GetComponent<jsonDataHelper>();
        setRedMax();
        setRandomID();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        obj = other.gameObject;
        identityHandler attacker = null;
        citizenIdentityHandler citizen = null;

        if(obj.CompareTag("Attacker_red") || obj.CompareTag("Attacker_blue") || obj.CompareTag("Attacker_orange") || obj.CompareTag("Attacker_gray"))
        {
            attacker = obj.GetComponent<identityHandler>();
        }
        else if(obj.CompareTag("Citizen"))
        {
            citizen = obj.GetComponent<citizenIdentityHandler>();
        }

        if(isGold && attacker != null)
        {
            attacker.exposed();
        }
        else
        {

            if(isRed)
            {
                int accuracy = Random.Range(1, 101);
                if ((obj.CompareTag("Attacker_red") || obj.CompareTag("Attacker_blue") || obj.CompareTag("Attacker_orange")))
                {
                    if (redHealth > (redMax / 2f))
                    {
                        attacker.exposed();
                    }
                    else if (redHealth > (redMax / 4f))
                    {
                        if (accuracy > helper.getRedAccuracyLowHealth())
                        {
                            attacker.testIfAccepted(gameObject.name);
                        }
                        else
                        {
                            attacker.redLocationAccepted(gameObject.name);
                        }
                    }
                    else
                    {
                        if (accuracy > helper.getRedAccuracyCriticalHealth())
                        {
                            attacker.testIfAccepted(gameObject.name);
                        }
                        else
                        {
                            attacker.redLocationAccepted(gameObject.name);
                        }
                    }
                }
                else if(obj.CompareTag("Citizen"))
                {
                    if(redHealth > (redMax / 2f))
                    {
                        citizen.redLocationAccepted(gameObject.name);
                    }
                    else if (redHealth > (redMax / 4f))
                    {
                        if (accuracy > helper.getRedCitizenAccuracyLowHealth())
                        {
                            citizen.redLocationAccepted(gameObject.name);
                        }
                        else
                        {
                            citizen.testIfAccepted(gameObject.name);
                        }
                    }
                    else
                    {
                        if (accuracy > helper.getRedCitizenAccuracyCriticalHealth())
                        {
                            citizen.redLocationAccepted(gameObject.name);
                        }
                        else
                        {
                            citizen.testIfAccepted(gameObject.name);
                        }
                    }
                }
            }
            
            if(isBlue)
            {
                if(attacker != null)
                {
                    attacker.setMarked(gameObject.name);
                }
                else if(citizen != null)
                {
                    citizen.setMarked(gameObject.name);
                }
            }

            if(isOrange && attacker != null)
            {
                if(obj.CompareTag("Attacker_red") && redCounter < helper.getOrangeAttackerLimit())
                {
                    redCounter++;
                    attacker.exposed();
                }
                else if(obj.CompareTag("Attacker_blue") && blueCounter < helper.getOrangeAttackerLimit())
                {
                    blueCounter++;
                    attacker.exposed();
                }
                else if(obj.CompareTag("Attacker_orange") && orangeCounter < helper.getOrangeAttackerLimit())
                {
                    orangeCounter++;
                    attacker.exposed();
                }
                else if(obj.CompareTag("Attacker_gray") && grayCounter < helper.getOrangeAttackerLimit())
                {
                    grayCounter++;
                    attacker.exposed();
                }
            }

            if(isOrange && citizen != null)
            {
                int accuracy = Random.Range(1, 101);

                if (accuracy > helper.getOrangeCitizenAccuracy())
                {
                    citizen.orangeLocationAccepted(gameObject.name);
                }
                else
                {
                    citizen.testIfOrangeAccepted(gameObject.name);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
         obj = null;
    }

    private void OnMouseDown()
    {
        if(waveCTRL.inEditMode())
        {
            statPanel.SetActive(true);
            statsBar.initializePanel(gameObject.GetComponent<intrusionDetection>());
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        

        if(timer <= 0)
        {
            checkIndex++;

            if (redHealth > 0f)
            {
                redHealth--;
            }
            else
            {
                redHealth = 0f;
            }

            if (statsBar.isDisplaying() != null && statsBar.isDisplaying().Equals(gameObject.GetComponent<intrusionDetection>()))
            {
                statsBar.initializePanel(gameObject.GetComponent<intrusionDetection>());
            }

            if (!isRed && !isBlue && !isOrange && !isGold)
            {
                renderers[2].gameObject.SetActive(false);
                renderers[3].gameObject.SetActive(false);
                renderers[4].gameObject.SetActive(false);
                renderers[5].gameObject.SetActive(false);
                renderers[1].gameObject.SetActive(true);
                timer = delay;
                return;
            }

            //if(isGold) Gold tile might not be possible
            //{
            //    renderers[1].gameObject.SetActive(false);
            //    renderers[2].gameObject.SetActive(false);
            //    renderers[3].gameObject.SetActive(false);
            //    renderers[4].gameObject.SetActive(false);
            //    renderers[5].gameObject.SetActive(true);
            //    timer = delay;
            //    return;
            //}

            if(checkIndex == 1)
            {
                if(isRed)
                {
                    renderers[1].gameObject.SetActive(false);
                    renderers[2].gameObject.SetActive(true);
                    renderers[3].gameObject.SetActive(false);
                    renderers[4].gameObject.SetActive(false);
                    renderers[5].gameObject.SetActive(false);
                    timer = delay;
                    return;
                }
            }

            if (checkIndex == 2)
            {
                if (isBlue)
                {
                    renderers[1].gameObject.SetActive(false);
                    renderers[2].gameObject.SetActive(false);
                    renderers[3].gameObject.SetActive(true);
                    renderers[4].gameObject.SetActive(false);
                    renderers[5].gameObject.SetActive(false);
                    timer = delay;
                    return;
                }
            }

            if (checkIndex == 3)
            {
                if (isOrange)
                {
                    renderers[1].gameObject.SetActive(false);
                    renderers[2].gameObject.SetActive(false);
                    renderers[3].gameObject.SetActive(false);
                    renderers[4].gameObject.SetActive(false);
                    renderers[5].gameObject.SetActive(true);
                    timer = delay;
                    return;
                }
            }
           
            timer = delay;
            
            if(checkIndex > 3)
            {
                checkIndex = 0;
            }
        }
    }

    public void setRed()
    {
        isRed = true;
        redHealth = redMax;
    }

    public void setBlue()
    {
        isBlue = true;
    }

    public void setOrange()
    {
        redCounter = 0;
        blueCounter = 0;
        orangeCounter = 0;
        grayCounter = 0;
        isOrange = true;
    }

    public int perception()
    {
        int perception = 0;

        if(redCounter < helper.getOrangeAttackerLimit())
        {
            perception++;
        }

        if(blueCounter < helper.getOrangeAttackerLimit())
        {
            perception++;
        }

        if(orangeCounter < helper.getOrangeAttackerLimit())
        {
            perception++;
        }

        if(grayCounter < helper.getOrangeAttackerLimit())
        {
            perception++;
        }

        return perception;
    }

    public float redHealthRatio()
    {
        float ratio = redHealth / redMax;
        return ratio;
    }

    public int redsVisited()
    {
        return redCounter;
    }

    public int bluesVisited()
    {
        return blueCounter;
    }

    public int orangesVisited()
    {
        return orangeCounter;
    }

    public int graysVisited()
    {
        return grayCounter;
    }

    public bool isRedActive()
    {
        return isRed;
    }

    public bool isBlueActive()
    {
        return isBlue;
    }

    public bool isOrangeActive()
    {
        return isOrange;
    }

    public float redHealthCount()
    {
        return redHealth;
    }

    public void killRed()
    {
        isRed = false;
        redHealth = 0;
    }

    public void killBlue()
    {
        isBlue = false;
    }

    public void killOrange()
    {
        isOrange = false;
    }

    void setRandomID()
    {
        int ones, tens, hundreds, thousands, tenThousands;

        ones = Random.Range(1, 10);
        tens = Random.Range(1, 10) * 10;
        hundreds = Random.Range(1, 10) * 100;
        thousands = Random.Range(1, 10) * 1000;
        tenThousands = Random.Range(1, 10) * 10000;

        gameObject.name = gameObject.name + (ones + tens + hundreds + thousands + tenThousands).ToString();
    }

    void setRedMax()
    {
        redMax = helper.getRedMaxHealth();
    }
}

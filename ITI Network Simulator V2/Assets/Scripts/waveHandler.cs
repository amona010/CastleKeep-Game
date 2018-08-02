using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class waveHandler : MonoBehaviour
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class wave
    {
        public Transform[] attackers;
        public Transform citizen;
        public int count;
        public float rate;

        public wave(Transform[] attackerList, Transform friendlyCitizen, int num)
        {
            attackers = attackerList;
            citizen = friendlyCitizen;
            count = num;
            rate = 1.5f;
        }
    }

    public List<wave> waves = new List<wave>();
    public Transform[] spawns;
    public Text text, text2, wavesSurvivedText, gameOverText;
    public GameObject panel;
    public GameObject endGameScreen;
    public GameObject tutorial;
    public GameObject healthAndWealthCTRL;
    public GameObject statBar;
    private sendDataToForm formData;
    private jsonDataHelper helper;
    private bool editMode = false;
    private bool mapCompleted = false;
    private int nextWave = -1, wavesWon = 0, roundPassed = 1;
    public int attackerChance;

    public float timeBetweenWaves = 1f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        spawns = new Transform[0];
        formData = gameObject.GetComponent<sendDataToForm>();
        helper = gameObject.GetComponent<jsonDataHelper>();
        waveCountDown = timeBetweenWaves;
        state = SpawnState.WAITING;
        panel.SetActive(true);
        text.text = "Prepare Defenses";
        text2.text = "Click on X Tiles to add defenses. Click Continue to begin new round!";
        editMode = true;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!allIsAlive() && !panel.activeInHierarchy && !endGameScreen.activeInHierarchy && !tutorial.activeInHierarchy)
            {
                editMode = false;
                startNewRound();
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    void startNewRound()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Count - 1)
        {
            nextWave = -1;
            state = SpawnState.WAITING;
            healthAndWealthCTRL.SendMessage("gainWealth", 10);
            panel.SetActive(true);
            text.text = "Round Complete!";
            text2.text = "Click on X Tiles to add defenses. Click Continue to begin new round!";
            editMode = true;
            formData.wasSuccessful("Yes");
            formData.sendToGoogle(roundPassed);
            formData.resetParams();
            healthAndWealthCTRL.SendMessage("gainMoney", helper.getRoundCurrency());
            roundPassed++;
            mapCompleted = helper.isMapCompleted(roundPassed - 1);
            if(mapCompleted)
            {
                endGame();
            }
        }
        else
        {
            wavesWon++;
            nextWave++;
        }


    }


    bool allIsAlive()
    {
        searchCountDown -= Time.deltaTime;

        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectsWithTag("Attacker_red").Length == 0 && GameObject.FindGameObjectsWithTag("Attacker_blue").Length == 0 && GameObject.FindGameObjectsWithTag("Attacker_orange").Length == 0 && GameObject.FindGameObjectsWithTag("Attacker_gray").Length == 0)
            {
                if (GameObject.FindGameObjectsWithTag("Citizen").Length == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator SpawnWave(wave _wave)
    {
        int chance;
        Debug.Log("Spawning wave");
        state = SpawnState.SPAWNING;

        //Spawn
        for (int i = 0; i < _wave.count; i++)
        {
            chance = Random.Range(1, 101);
            if (state != SpawnState.WAITING)
            {
                if (chance < attackerChance)
                {
                    SpawnAttacker(_wave.attackers[Random.Range(0, 4)]);
                }
                else
                {
                    SpawnCitizen(_wave.citizen);
                }
            }
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnAttacker(Transform _attacker)
    {
        if(spawns.Length == 0)
        {
            spawns = gameObject.GetComponent<Graph>().initializeSpawns();
        }
        Debug.Log("Spawning attacker");
        Transform sp = spawns[Random.Range(0, spawns.Length)];
        Instantiate(_attacker, sp.position, sp.rotation);
    }

    void SpawnCitizen(Transform _citizen)
    {
        if (spawns.Length == 0)
        {
            spawns = gameObject.GetComponent<Graph>().initializeSpawns();
        }
        Debug.Log("Spawning citizen");
        Transform sp = spawns[Random.Range(0, spawns.Length)];
        Instantiate(_citizen, sp.position, sp.rotation);
    }

    public bool inEditMode()
    {
        return editMode;
    }

    public void endGame()
    {
        if(!endGameScreen.activeInHierarchy)
        {
            state = SpawnState.WAITING;
            endGameScreen.SetActive(true);
            if(!mapCompleted)
            {
                wavesSurvivedText.text = "You lasted through: " + wavesWon + " waves";
                helper.setNextMap();
                formData.wasSuccessful("No");
                formData.sendToGoogle(roundPassed);
                formData.resetParams();
            }
            else
            {
                panel.SetActive(false);
                gameOverText.text = "Congratulations!";
                wavesSurvivedText.text = "You completed this map!";
                helper.setNextMap();
            }
        }
    }

    public void saveMapSettingsToJSON(jsonDataHandler handler)
    {
        string mapName = SceneManager.GetActiveScene().name;
        handler.addNewMap(mapName, -1, waves[0].count, waves.Count);
    }
}

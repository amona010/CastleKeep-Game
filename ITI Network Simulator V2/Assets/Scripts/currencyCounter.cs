using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class currencyCounter : MonoBehaviour {

    public Text currency;
    private sendDataToForm formData;
    private GameObject GM;
    private int currentAMT;

    // Use this for initialization
    void Start()
    {
        GM = GameObject.Find("GM");
        formData = GM.GetComponent<sendDataToForm>();
        currentAMT = 0;
        updateCurrency();
    }

    private void updateCurrency()
    {
        currency.text = currentAMT.ToString();
        formData.moneyLeftOver(currentAMT);
    }

    public void loseMoney(int loss)
    {
        currentAMT -= loss;
        if (currentAMT < 0)
        {
            currentAMT = 0;
        }
        formData.spendMoney(loss);
        updateCurrency();
    }

    public void gainMoney(int gain)
    {
        currentAMT += gain;
        updateCurrency();
    }

    public bool isPurchaseable(int cost)
    {
        int difference = currentAMT - cost;
        return difference > -1;
    }
}

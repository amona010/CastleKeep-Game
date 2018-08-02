using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour {

    public Image castleWealth;
    public GameObject gameMaster;

    private float HP = 100, maxHP = 100;

	// Use this for initialization
	void Start ()
    {
        updateHealthBar();
	}

    private void updateHealthBar()
    {
        float ratio = HP / maxHP;

        castleWealth.rectTransform.localScale = new Vector2(ratio, 1);
    }

    public void loseWealth(int loss)
    {
        HP -= loss;
        if(HP < 1)
        {
            HP = 0;
            gameMaster.SendMessage("endGame");
        }
        updateHealthBar();
    }

    public void gainWealth(int gain)
    {
        HP += gain;
        if(HP > maxHP)
        {
            HP = maxHP;
        }
        updateHealthBar();
    }
}

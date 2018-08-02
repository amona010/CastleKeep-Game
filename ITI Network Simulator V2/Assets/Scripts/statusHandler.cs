using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statusHandler : MonoBehaviour {

    private intrusionDetection tile;
    public Text blueActive, orangeVision, redVisited, blueVisited, orangeVisited, grayVisited, redButtonText, blueButtonText, orangeButtonText;
    public Image redHealthBar;
    public GameObject currencyCTRL;
    private currencyCounter money;
    private sendDataToForm formData;
    private int healRedPrice;

    private void Start()
    { 
        formData = GameObject.Find("GM").GetComponent<sendDataToForm>();
    }

    public void initializePanel(intrusionDetection selected)
    {
        tile = selected;
        money = currencyCTRL.GetComponent<currencyCounter>();
        healRedPrice = 200 - (int)(tile.redHealthRatio() * 200);

        if(!tile.isRedActive())
        {
            redHealthBar.rectTransform.localScale = new Vector2(tile.redHealthRatio(), 1);
            if (money.isPurchaseable(200))
                redButtonText.text = "Purchase Red Archer \n$200";
            else
                redButtonText.text = "Insufficient Funds";
        }
        else
        {
            if(healRedPrice < 75)
            {
                if (money.isPurchaseable(75))
                    redButtonText.text = "Heal Red Archer \n$75";
                else
                    redButtonText.text = "Insufficient Funds";
            }
            else
            {
                if (money.isPurchaseable(healRedPrice))
                    redButtonText.text = "Heal Red Archer \n$" + healRedPrice;
                else
                    redButtonText.text = "Insufficient Funds";
            }
            redHealthBar.rectTransform.localScale = new Vector2(tile.redHealthRatio(), 1);
        }

        if(tile.isBlueActive())
        {
            blueActive.text = "Active";
            blueButtonText.text = "Blue Archer Active";
        }
        else
        {
            blueActive.text = "Inactive";
            if (money.isPurchaseable(100))
                blueButtonText.text = "Purchase Blue Archer \n$100";
            else
                blueButtonText.text = "Insufficient Funds";
        }

        if(!tile.isOrangeActive())
        {
            orangeVision.text = "Inactive";
            if (money.isPurchaseable(500))
                orangeButtonText.text = "Purchase Orange Archer \n$500";
            else
                orangeButtonText.text = "Insufficient Funds";
        }
        else
        {
            if (money.isPurchaseable(500))
                orangeButtonText.text = "Heal Orange Archer \n$200";
            else
                orangeButtonText.text = "Insufficient Funds";

            switch (tile.perception())
            {
                case 4:
                    orangeVision.text = "Peerless";
                    break;
                case 3:
                    orangeVision.text = "Okay";
                    break;
                case 2:
                    orangeVision.text = "Blurry";
                    break;
                case 1:
                    orangeVision.text = "Almost Blind";
                    break;
            }
        }
        redVisited.text = tile.redsVisited().ToString();
        blueVisited.text = tile.bluesVisited().ToString();
        orangeVisited.text = tile.orangesVisited().ToString();
        grayVisited.text = tile.graysVisited().ToString();
    }

    public void RedClicked()
    {
        if(!tile.isRedActive())
        {
            if(money.isPurchaseable(200))
            {
                money.loseMoney(200);
                formData.actionMade(tile.name + " - Red Tile Added");
                tile.setRed();
            }        
        }
        else
        {
            if (healRedPrice < 75)
            {
                if (money.isPurchaseable(75))
                {
                    money.loseMoney(75);
                    formData.actionMade(tile.name + " - Red Tile Updated");
                    tile.setRed();
                }
            }
            else
            {
                if (money.isPurchaseable(healRedPrice))
                {
                    money.loseMoney(healRedPrice);
                    formData.actionMade(tile.name + " - Red Tile Updated");
                    tile.setRed();
                }
            }
        }

    }

    public void RemRedClicked()
    {
        formData.actionMade(tile.name + " - Red Tile Removed");
        tile.killRed();
        if (money.isPurchaseable(200))
            redButtonText.text = "Purchase Red Archer \n$200";
        else
            redButtonText.text = "Insufficient Funds";
    }

    public void BlueClicked()
    {
        if (!tile.isBlueActive())
        {
            if(money.isPurchaseable(100))
            {
                formData.actionMade(tile.name + " - Blue Tile Added");
                money.loseMoney(100);
                tile.setBlue();
            }
        }
    }

    public void RemBlueClicked()
    {
        if (tile.isBlueActive())
        {
            formData.actionMade(tile.name + " - Blue Tile Removed");
            tile.killBlue();
            if (money.isPurchaseable(100))
                blueButtonText.text = "Purchase Blue Archer \n$100";
            else
                blueButtonText.text = "Insufficient Funds";
        }
    }

    public void OrangeClicked()
    {
        if(money.isPurchaseable(500))
        {
            money.loseMoney(500);

            if (tile.isOrangeActive())
            {
                formData.actionMade(tile.name + " - Orange Tile Updated");
            }
            else
            {
                formData.actionMade(tile.name + " - Orange Tile Added");
            }

            tile.setOrange();
            orangeButtonText.text = "Heal Orange Archer \n$500";
        }
    }

    public void RemOrangeClicked()
    {
        formData.actionMade(tile.name + " - Orange Tile Removed");
        tile.killOrange();
        if (money.isPurchaseable(500))
            orangeButtonText.text = "Purchase Orange Archer \n$500";
        else
            orangeButtonText.text = "Insufficient Funds";
    }


    public void closePanel()
    {
        tile = null;
        gameObject.SetActive(false);
    }

    public intrusionDetection isDisplaying()
    {
        return tile;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyButtonBehaviour : MonoBehaviour
{
    [Header("BaseValues")]
    [SerializeField] private int maxLevel;
    [SerializeField] private int priceBV;
    [SerializeField] private int levelBV;
    [SerializeField] private float actualValueBV, valueAugmentBV, valueAdditionBV, priceAugmentBV;

    [Header("BuyButton")]

    [SerializeField] private string playerPrefBaseName;
    [SerializeField] private bool isAugmentWithPercent = true;
    private TMP_Text priceText, levelText, augmentText;
    private int level ,price;
    private float priceAugment ,actualValue, valueAugment, valueAddition;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        priceText = transform.GetChild(0).GetComponent<TMP_Text>();
        levelText = transform.GetChild(1).GetComponent<TMP_Text>();
        augmentText = transform.GetChild(2).GetComponent<TMP_Text>();   

        UpdateValuesAndText();

        SetPlayerPrefsIfNotExists();
        
    }
    private void UpdateValuesAndText()
    {
        price = PlayerPrefs.GetInt(playerPrefBaseName + "Price", priceBV);
        priceAugment = PlayerPrefs.GetFloat(playerPrefBaseName + "PriceAugment", priceAugmentBV);
        level = PlayerPrefs.GetInt(playerPrefBaseName + "Level", levelBV);
        actualValue = PlayerPrefs.GetFloat(playerPrefBaseName + "Value", actualValueBV);
        valueAddition = PlayerPrefs.GetFloat(playerPrefBaseName + "ValueAddition", valueAdditionBV);
        valueAugment = PlayerPrefs.GetFloat(playerPrefBaseName + "ValueAugment", valueAugmentBV);

        priceText.text = price.ToString();
        levelText.text = "LVL" + level.ToString();

        if(level >= maxLevel)
        {
            priceText.text = "MAX";
            levelText.text = "LVL" + maxLevel.ToString();
            augmentText.text = actualValue.ToString();
            if(isAugmentWithPercent)
                augmentText.text += "%";
            GetComponent<Button>().interactable = false;
            return;
        }
        if(isAugmentWithPercent)
            augmentText.text = actualValue.ToString() + "% > " + (Mathf.Ceil((actualValue+valueAddition) * valueAugment*10)/10).ToString() + "%"; 
        else 
            augmentText.text = actualValue.ToString() +  " > " + (Mathf.Ceil((actualValue+valueAddition) * valueAugment*10)/10).ToString();
        
    }
    public void BuyUpgrade()
    {
        if(gameManager.BuyUpgrade(price))
        {
            level++;
            PlayerPrefs.SetInt(playerPrefBaseName + "Level", level);
            PlayerPrefs.SetInt(playerPrefBaseName + "Price", (int)Mathf.Ceil(price * priceAugment));
            PlayerPrefs.SetFloat(playerPrefBaseName + "Value", Mathf.Ceil((actualValue+valueAddition) * valueAugment*10)/10);
            UpdateValuesAndText();
        }
        else
        {
            StartCoroutine(NotEnoughMoney());
        }
    }
    private IEnumerator NotEnoughMoney()
    {
        priceText.text = "Not enough money";
        yield return new WaitForSeconds(1f);
        priceText.text = price.ToString();
    }

    private void SetPlayerPrefsIfNotExists()
    {
        if(!PlayerPrefs.HasKey(playerPrefBaseName + "Price"))
        {
            PlayerPrefs.SetInt(playerPrefBaseName + "Price", priceBV);
            PlayerPrefs.SetFloat(playerPrefBaseName + "PriceAugment", priceAugmentBV);
            PlayerPrefs.SetInt(playerPrefBaseName + "Level", levelBV);
            PlayerPrefs.SetFloat(playerPrefBaseName + "Value", actualValueBV);
            PlayerPrefs.SetFloat(playerPrefBaseName + "ValueAddition", valueAdditionBV);
            PlayerPrefs.SetFloat(playerPrefBaseName + "ValueAugment", valueAugmentBV);
        }
    }

}

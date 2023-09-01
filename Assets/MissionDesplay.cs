using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionDesplay : MonoBehaviour
{
    [Header("Mission Desplay")]
    private GameManager gameManager;
    [SerializeField] private GameObject star;
    [SerializeField] private Slider missionSlider;
    [SerializeField] private TMP_Text missionTitle, missionValue, missionCoin;
    [SerializeField] private Button claimButton;
    [SerializeField] private string missionTitleInput, missionTypeInput;
    [SerializeField] private int missionBaseValueInput, missionBaseCoinInput;
    [SerializeField] private float missionAdditionValueInput, missionAdditionCoinInput, missionMultiplierValueInput, missionMultiplierCoinInput;

    private int missionLevel, currentValue, startValue, maxValue, coins;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdateTexts();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt(missionTypeInput + "Value", PlayerPrefs.GetInt(missionTypeInput + "Value", 0) + 20);
            UpdateTexts();
        }
    }
    private void UpdateTexts()
    {
        missionTitle.text = missionTitleInput;

        missionLevel = PlayerPrefs.GetInt(missionTypeInput + "Level", 1);
        //Value when the missions starts
        startValue = PlayerPrefs.GetInt(missionTypeInput + "StartValue", 0);
        //Value of the mission right now
        maxValue = PlayerPrefs.GetInt(missionTypeInput + "MaxValue", missionBaseValueInput);
        currentValue = Mathf.Min(PlayerPrefs.GetInt(missionTypeInput + "Value", 0) - startValue, maxValue);
        

        missionSlider.maxValue = maxValue;
        missionSlider.value = currentValue;
        
        if(currentValue >= maxValue)
        {
            claimButton.interactable = true;
            star.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            claimButton.interactable = false;
            star.GetComponent<Image>().color = Color.grey;
        }


        missionValue.text = currentValue.ToString() + " / " + maxValue.ToString();

        coins = PlayerPrefs.GetInt(missionTypeInput + "Coin", missionBaseCoinInput);
        missionCoin.text = coins.ToString();
    }

    public void Claim()
    {
        gameManager.EarnCoin(coins);

        missionLevel = missionLevel +1;
        PlayerPrefs.SetInt(missionTypeInput + "Level", missionLevel);

        PlayerPrefs.SetInt(missionTypeInput + "StartValue", PlayerPrefs.GetInt(missionTypeInput + "Value", 0));

        PlayerPrefs.SetInt(missionTypeInput + "MaxValue", (int)Mathf.Ceil((maxValue + missionAdditionValueInput) * missionMultiplierValueInput/10)*10);

        PlayerPrefs.SetInt(missionTypeInput + "Coin", (int)Mathf.Ceil((coins + missionAdditionCoinInput) * missionMultiplierCoinInput/10)*10);

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + coins);

        UpdateTexts();
    }
}

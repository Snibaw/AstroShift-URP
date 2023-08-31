using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayerPrefsValues : MonoBehaviour
{
    private GameObject[] buyButton;
    // Start is called before the first frame update
    private void Awake() {
        if(PlayerPrefs.GetInt("PlayerPrefsInit",0) == 1) return;
        buyButton = GameObject.FindGameObjectsWithTag("BuyButton");
        foreach(GameObject button in buyButton)
        {
            button.GetComponent<BuyButtonBehaviour>().SetPlayerPrefsIfNotExists();
        }
        PlayerPrefs.SetInt("PlayerPrefsInit",1);
    }
}

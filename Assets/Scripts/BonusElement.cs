using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BonusElement : MonoBehaviour
{
    [SerializeField] private GameObject[] bonusList;
    [SerializeField] private TMP_Text timeText; 
    [SerializeField] private float limitTimeBeforeBlinking = 3f;
    private float time;
    private bool isInit = false;
    private Image borderImage;
    private Image bonusImage;
    public int bonusIndex;
    void FixedUpdate()
    {
        if(time >0) time -= Time.deltaTime;
        UpdateTimeText();
        if(time <= 0 && time > -1)
        {
            transform.parent.GetComponent<BonusContainer>().ElementHasBeenRemoved(bonusIndex);
            Destroy(gameObject);
        }
        else if(time <= limitTimeBeforeBlinking && time > -1)
        {
            timeText.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
            //Make the image of the border and the image of the bonus blink
            borderImage.color = Color.Lerp(Color.white, new Color(255,255,255,0.2f), Mathf.PingPong(Time.time, 0.5f));
            bonusImage.color = Color.Lerp(Color.white, new Color(255,255,255,0.2f), Mathf.PingPong(Time.time, 0.5f));
        }
    }
    public void InitBonusElement(int bonusIndex, float time)
    {
        foreach(GameObject bonus in bonusList)
        {
            bonus.SetActive(false);
        }
        bonusList[bonusIndex].SetActive(true);        
        this.time = time;
        isInit = true;
        this.bonusIndex = bonusIndex;
        bonusImage = bonusList[bonusIndex].GetComponent<Image>();
        borderImage = transform.GetChild(0).GetComponent<Image>();
        UpdateTimeText();
    }
    private void UpdateTimeText()
    {
        timeText.text = time.ToString("0.0");
        if(time < 0)
            timeText.text = "∞";
    }
    public void SetTime(float time)
    {
        this.time = time;
    }
}

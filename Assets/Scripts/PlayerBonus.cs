using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBonus : MonoBehaviour
{
    [SerializeField] private GameObject[] Shields;
    [SerializeField] private float gravityMultiplierGreenForm = 2f;
    [SerializeField] private float scaleMultiplierBlueForm = 0.5f;
    private Color[] colors;
    private Rigidbody2D rb;
    private int indexColor;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject shield in Shields)
        {
            shield.SetActive(false);
        }
        colors = GameObject.Find("Obstacles").GetComponent<SpawnObstacles>().colors;
        rb = GetComponent<Rigidbody2D>();

    }

    public void ActivateShield(int i)
    {
        Shields[i].SetActive(true);
        if(i !=3) Shields[i].GetComponent<Animator>().SetTrigger("On");
    }
    public void DesactivateShield(int i)
    {
        if(i!=3) Shields[i].GetComponent<Animator>().SetTrigger("Off");
        StartCoroutine(WaitBeforeDesactivate(i));
    }
    private IEnumerator WaitBeforeDesactivate(int i)
    {
        yield return new WaitForSeconds(1f);
        Shields[i].SetActive(false);
    }
    public void TakePortal(Color color)
    {
        switch(indexColor)
        {
            case 0:
                Debug.Log("Yellow Out");
                break;
            case 1:
                GetComponent<PlayerMovement>().canStayTouching = false;
                break;
            case 2:
                rb.gravityScale = rb.gravityScale / gravityMultiplierGreenForm;
                break;
            case 3:
                transform.localScale = transform.localScale / scaleMultiplierBlueForm;
                break;
        }

        GetComponent<SpriteGlow.SpriteGlowEffect>().GlowColor = color;
        GetComponent<TrailRenderer>().startColor = new Color(color.r, color.g, color.b, 0.3f);


        //Get the index of the color
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == color)
            {
                indexColor = i;
                break;
            }
        }
        GameObject.Find("Obstacles").GetComponent<SpawnObstacles>().colorIndex = indexColor;
        
        switch (indexColor)
        {
            case 0:
                ActivateYellowForm();
                break;
            case 1:
                ActivateBlueForm();
                break;
            case 2:
                ActivateGreenForm();
                break;
            case 3:
                ActivatePurpleForm();
                break;
        }
    }
    private void ActivateYellowForm()
    {
        Debug.Log("Yellow");
    }
    private void ActivateBlueForm()
    {
        GetComponent<PlayerMovement>().canStayTouching = true;
    }
    private void ActivateGreenForm()
    {
        rb.gravityScale = rb.gravityScale * gravityMultiplierGreenForm;
    }
    private void ActivatePurpleForm()
    {
        transform.localScale = transform.localScale * scaleMultiplierBlueForm;
    }
}

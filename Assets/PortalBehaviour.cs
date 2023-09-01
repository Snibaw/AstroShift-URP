using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    private Color color;
    public void ChangeColor(Color newColor)
    {
        color = newColor;
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<SpriteGlow.SpriteGlowEffect>().GlowColor = color;
        Destroy(gameObject,10f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt("M_ColorValue", PlayerPrefs.GetInt("M_ColorValue", 0) + 1);
            collision.gameObject.GetComponent<PlayerBonus>().TakePortal(color);
        }
    }
}

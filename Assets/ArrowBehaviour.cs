using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public void ChangeGlowColor(Color newColor)
    {
        foreach (SpriteGlow.SpriteGlowEffect g in GetComponentsInChildren<SpriteGlow.SpriteGlowEffect>())
        {
            g.GlowColor = newColor;
        }
        Destroy(gameObject, 10f);
    }
}

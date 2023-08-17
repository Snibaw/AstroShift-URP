using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBonus : MonoBehaviour
{
    [SerializeField] private GameObject[] Shields;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject shield in Shields)
        {
            shield.SetActive(false);
        }
    }

    public void ActivateShield(int i)
    {
        Shields[i].SetActive(true);
        Shields[i].GetComponent<Animator>().SetTrigger("On");
    }
    public void DesactivateShield(int i)
    {
        Shields[i].GetComponent<Animator>().SetTrigger("Off");
        StartCoroutine(WaitBeforeDesactivate(i));
    }
    private IEnumerator WaitBeforeDesactivate(int i)
    {
        yield return new WaitForSeconds(1f);
        Shields[i].SetActive(false);
    }
}

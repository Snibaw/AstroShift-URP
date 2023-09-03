using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] helpBgObjects;
    private bool hasBeenManuallyChanged = false;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Change bg every 5 seconds
        index = Random.Range(0,helpBgObjects.Length);
        StartCoroutine(ChangeBg());
    }
    private IEnumerator ChangeBg()
    {
        if(hasBeenManuallyChanged)
        {
            yield return new WaitForSeconds(3f);
            hasBeenManuallyChanged = false;
        }
        foreach(GameObject bg in helpBgObjects)
        {
            bg.SetActive(false);
        }
        index = (index+1)%helpBgObjects.Length;
        helpBgObjects[index].SetActive(true);
        yield return new WaitForSeconds(5f);
        StartCoroutine(ChangeBg());
    }
    public void ChangeBgManually(int indexAddition)
    {
        hasBeenManuallyChanged = true;
        foreach(GameObject bg in helpBgObjects)
        {
            bg.SetActive(false);
        }
        index = (index+indexAddition)%helpBgObjects.Length;
        helpBgObjects[index].SetActive(true);
    }
}

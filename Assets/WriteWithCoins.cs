using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteWithCoins : MonoBehaviour
{
    [SerializeField] private GameObject[] coinPrefab;
    private float lengthOfALetter = 2f;
    [SerializeField] private float spaceBetweenLetters = 1f;
    [SerializeField] private float spaceBetweenWords = 2f;
    // Start is called before the first frame update
    // private void Start() {
    //     // word = System.DateTime.Now.ToString("dd MM yyyy");
    //     WriteWord();
    // }
    public void WriteWord(string word, Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float z = position.z;
        for(int i = 0; i < word.Length; i++)
        {
            if(word[i] == ' ')
            {
                x += spaceBetweenWords;
                continue;
            }
            GameObject coin = Instantiate(coinPrefab[FindLetter(word[i])],new Vector3(x,y,z),Quaternion.identity);
            coin.transform.parent = transform;
            Destroy(coin, 15f);
            if(word[i] == '!') x += 2*spaceBetweenLetters;
            else x += lengthOfALetter + spaceBetweenLetters;
        }
    }
    private int FindLetter(char c)
    {
        Debug.Log(c);
        foreach (var item in coinPrefab)
        {
            if(item.name[0] == c)
                return System.Array.IndexOf(coinPrefab,item);
        }
        return 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserParentBehaviour : MonoBehaviour
{
    public bool isHorizontal;
    public int length;
    [SerializeField] private GameObject laserExtremityPrefab;
    [SerializeField] private GameObject laserMiddlePrefab;
    [SerializeField] private int laserHorizontalMaxLength;
    [SerializeField] private int laserVerticalMaxLength;
    private float laserMiddleLength;
    [SerializeField] private float[] YBoderPositions;
    private float yPosition;
    void Start()
    {
        laserMiddleLength = laserMiddlePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        isHorizontal = Random.Range(0, 2) == 0 ? true : false;


        if(isHorizontal)
        {
            length = Random.Range(2, laserHorizontalMaxLength+1);
            yPosition = Random.Range(YBoderPositions[0], YBoderPositions[1]);
            CreateHorizontalLaser();
        }
        else
        {
            length = Random.Range(2, laserVerticalMaxLength+1);
            float totalLength = length * laserMiddleLength + 2*laserExtremityPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            yPosition = Random.Range(YBoderPositions[0] + totalLength/2, YBoderPositions[1]- totalLength/2);
            CreateVerticalLaser();
        }
        Destroy(gameObject, 20f);
    }

    private void CreateHorizontalLaser()
    {
        GameObject laserExtremityLeft = Instantiate(laserExtremityPrefab, transform.position, Quaternion.identity);
        laserExtremityLeft.transform.parent = transform;
        laserExtremityLeft.transform.localPosition = new Vector3(-length*laserMiddleLength / 2, yPosition, 0);

        GameObject laserExtremityRight = Instantiate(laserExtremityPrefab, transform.position, Quaternion.identity);
        laserExtremityRight.transform.parent = transform;
        laserExtremityRight.transform.localPosition = new Vector3(length*laserMiddleLength / 2, yPosition, 0);
        laserExtremityRight.transform.rotation = Quaternion.Euler(0, 0, 180);

        for (int i = 1; i < length; i++)
        {
            GameObject laserMiddle = Instantiate(laserMiddlePrefab, transform.position, Quaternion.identity);
            laserMiddle.transform.parent = transform;
            laserMiddle.transform.localPosition = new Vector3(-length*laserMiddleLength / 2 + laserMiddleLength * i, yPosition, 0);
        }
    }
    private void CreateVerticalLaser()
    {
        GameObject laserExtremityTop = Instantiate(laserExtremityPrefab, transform.position, Quaternion.identity);
        laserExtremityTop.transform.parent = transform;
        laserExtremityTop.transform.localPosition = new Vector3(0, length*laserMiddleLength / 2 + yPosition, 0);
        laserExtremityTop.transform.rotation = Quaternion.Euler(0, 0, 270);

        GameObject laserExtremityBottom = Instantiate(laserExtremityPrefab, transform.position, Quaternion.identity);
        laserExtremityBottom.transform.parent = transform;
        laserExtremityBottom.transform.localPosition = new Vector3(0, -length*laserMiddleLength / 2 + yPosition, 0);
        laserExtremityBottom.transform.rotation = Quaternion.Euler(0, 0, 90);

        for (int i = 1; i < length; i++)
        {
            GameObject laserMiddle = Instantiate(laserMiddlePrefab, transform.position, Quaternion.identity);
            laserMiddle.transform.parent = transform;
            laserMiddle.transform.localPosition = new Vector3(0, -length*laserMiddleLength / 2 + laserMiddleLength * i + yPosition, 0);
            laserMiddle.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}

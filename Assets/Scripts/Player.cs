using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform leftSide;

    [SerializeField]
    private Transform rightSide;

    

    public bool isSmall;
    public bool isBig;
    public bool isNormal;

    private Vector3 normalSize;
    private Vector3 smallSize;
    private Vector3 bigSize;
    private Vector3 actualSize;

    private float animationElapsed;
    [SerializeField]
    private float animationDuration;

    private float bonusMallusElapsed;
    [SerializeField]
    private float bonusMallusDuration;

    public List<Bonus> bonusObejcts;
    

    // Start is called before the first frame update
    void Start()
    {
        normalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        smallSize = new Vector3(1f, transform.localScale.y, transform.localScale.z);
        bigSize = new Vector3(3f, transform.localScale.y, transform.localScale.z);
        actualSize = normalSize;

        animationElapsed = 0;
        bonusMallusElapsed = 0;
        bonusObejcts = new List<Bonus>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bonusObejcts.Count; i++)
        {
            if (BonusCollider(bonusObejcts[i].transform))
            {
                bonusObejcts[i].isCollide = true;
                bonusObejcts.RemoveAt(i);
            }
        }

        // End of bonus or mallus
        if (bonusMallusElapsed > bonusMallusDuration)
        {
            isSmall = false;
            isBig = false;
            bonusMallusElapsed = 0;
            isNormal = true;
            animationElapsed = 0;
        }

        // Smaller size player
        if (isSmall)
        {
            if (animationElapsed < animationDuration)
                SmallPlayer();
            else
            {
                animationElapsed = 0;
                actualSize = smallSize;
            }
            bonusMallusElapsed += Time.deltaTime;
        }

        // Bigger size player
        if (isBig)
        {
            if (animationElapsed < animationDuration)
                BigPlayer();
            else
            {
                animationElapsed = 0;
                actualSize = bigSize;
            }
            bonusMallusElapsed += Time.deltaTime;
        }

        // Normal size player
        if (isNormal)
        {
            if (animationElapsed < animationDuration)
                NormalPlayer();
            else
            {
                isNormal = false;
                animationElapsed = 0;
                actualSize = normalSize;
            }
        }

        Move();
    }

    private void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Vector3 add_pos = move * speed * Time.deltaTime;
        Vector3 new_pos = base.transform.position + add_pos;

        if (((leftSide.position.x - (leftSide.localScale.x / 2)) + leftSide.localScale.x) > new_pos.x - (transform.localScale.x / 2))
        {
            float x = (leftSide.position.x - (leftSide.localScale.x / 2));
            float width = leftSide.localScale.x;
            new_pos.x = x + width + (transform.localScale.x / 2);
        }
        else if (rightSide.position.x - (leftSide.localScale.x / 2) < (new_pos.x - (transform.localScale.x / 2)) + transform.localScale.x)
        {
            new_pos.x = (rightSide.position.x - (leftSide.localScale.x / 2)) - (transform.localScale.x / 2);
        }

        base.transform.position = new_pos;
    }


    private void SmallPlayer()
    {
        float a = (smallSize.x - actualSize.x) / (animationDuration - 0);
        float b = actualSize.x - a * 0;
        float y = a * animationElapsed + b;
        transform.localScale =  new Vector3(y, actualSize.y, actualSize.z);
        animationElapsed += Time.deltaTime;
    }

    private void BigPlayer()
    {
        float a = (bigSize.x - actualSize.x) / (animationDuration - 0);
        float b = actualSize.x - a * 0;
        float y = a * animationElapsed + b;
        transform.localScale = new Vector3(y, actualSize.y, actualSize.z);
        animationElapsed += Time.deltaTime;
    }

    private void NormalPlayer()
    {
        float a = (normalSize.x - actualSize.x) / (animationDuration - 0);
        float b = actualSize.x - a * 0;
        float y = a * animationElapsed + b;
        transform.localScale = new Vector3(y, actualSize.y, actualSize.z);
        animationElapsed += Time.deltaTime;
    }

    private bool Overlapping(float minA, float maxA, float minB, float maxB)
    {
        return minB <= maxA && minA <= maxB;
    }

    private bool BonusCollider(Transform bonus)
    {
        float aLeft = transform.position.x - (transform.localScale.x /2);
        float aRight = aLeft + transform.localScale.x;

        float bLeft = bonus.position.x - (bonus.localScale.x / 2);
        float bRight = bLeft + bonus.localScale.x;

        float aBottom = transform.position.z - (transform.localScale.z / 2);
        float aTop = aBottom + transform.localScale.z;

        float bBottom = bonus.position.z - (bonus.localScale.z / 2); ;
        float bTop = bBottom + bonus.localScale.z;

        return Overlapping(aLeft, aRight, bLeft, bRight) && Overlapping(aBottom, aTop, bBottom, bTop);
    }

}

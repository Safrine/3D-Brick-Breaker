using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    private float speed;

    private Player player;
    private Ball ball;

    public bool isCollide;
    public Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2;
        player = GameObject.Find("Player").GetComponent<Player>();
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        player.bonusObejcts.Add(this);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(0, 0, -1);
        Vector3 add_pos = move * speed * Time.deltaTime;
        Vector3 new_pos = base.transform.position + add_pos;

        base.transform.position = new_pos;

        if (isCollide)
        {
            string method = this.gameObject.name;
            int pos = method.IndexOf('(');
            method = method.Remove(pos);
            Invoke(method, 0);
            Destroy(gameObject);
        }
    }

    private void PlayerBonus()
    {
        player.isBig = true;
        player.isSmall = false;
    }

    private void PlayerMallus()
    {
        player.isBig = false;
        player.isSmall = true;
    }

    private void MultiBall()
    {
        ball.MultiBallBonus();
    }
}

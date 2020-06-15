using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField]
    private GameObject playerObject;
    private Transform playerTransform;

    [SerializeField]
    private GameObject brickbreakertrayObject;
    private List<Side> brickbreakertray;
    private List<Transform> brickbreakertrayTransform;

    [SerializeField]
    private Transform ground;

    [SerializeField]
    private GameObject bricksObjects;
    private List<Brick> bricks;
    private List<Transform> bricksTransform;


    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    public float minForwardStep;

    [SerializeField]
    private GameManager gameManager;

    private Vector3 new_pos;

    public Vector3 direction;

    public bool space_bar_pressed = false;

    public static List<Ball> balls = new List<Ball>();

    private AudioSource collision;

    private void Awake()
    {
        balls.Add(this);
        print("Coco");
        print(balls.Count);
        /*for(int i = 0; i < balls.Count; i++)
        {
            if (balls[i] == null)
                balls.RemoveAt(i);
        }*/
        balls.RemoveAll(item => item == null);
    }

    private void Start()
    {
        
        collision = GetComponent<AudioSource>();
        brickbreakertray = new List<Side>();
        brickbreakertrayTransform = new List<Transform>();
        foreach (Transform side in brickbreakertrayObject.transform)
        {
            if (side.name != "Ground")
            {
                brickbreakertray.Add(side.gameObject.GetComponent<Side>());
                brickbreakertrayTransform.Add(side);
            }
            else
            {
                ground = side;
            }

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        playerTransform = playerObject.GetComponent<Transform>();

        bricks = new List<Brick>();
        bricksTransform = new List<Transform>();
        foreach (Transform side in bricksObjects.transform)
        {
            bricks.Add(side.gameObject.GetComponent<Brick>());
            bricksTransform.Add(side);
        }

        direction = new Vector3(0, 0, 1);


        if (balls.Count == 2)
            direction = new Vector3(0.5f, 0, 1);
        if (balls.Count == 3)
            direction = new Vector3(-0.5f, 0, 1);

        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!space_bar_pressed)
            return;

        Vector3 move = direction * speed * Time.deltaTime;
        new_pos = transform.position + move;
        
        if (transform.localPosition.z < playerTransform.localPosition.z - (playerTransform.localScale.z / 2))
        {
            balls.Remove(this);
            if (balls.Count == 0)
                gameManager.isGameOver = true;
            Destroy(gameObject);
            
        }
        

        for (int i = 0; i < bricksTransform.Count; i++)
        {
            if (bricks[i] != null && bricksTransform[i] != null && bricks[i].isRigid && Collide(bricksTransform[i], new_pos))
            {
                collision.Play();
                bricks[i].isRigid = false;
                bricks.RemoveAt(i);
                bricksTransform.RemoveAt(i);
                bricksObjects.GetComponent<GameManager>().numberOfCube--;
            }
        }

        for (int i = 0; i < brickbreakertrayTransform.Count; i++)
            if (Collide(brickbreakertrayTransform[i], new_pos))
            {
                collision.Play();
                brickbreakertray[i].isCollide = true;
            } 

        if(Collide(playerTransform, new_pos))
        {
            float center = playerTransform.position.x;
            float max = playerTransform.localScale.x / 2;
            float pos = transform.position.x - center;

            float a = (minForwardStep - 1) / (max - 0);
            float b = minForwardStep - a;
            float y = a * Mathf.Abs(pos) + b;
            direction.z = y;

            a = 1 / max;
            y = a * pos;
            direction.x = y;
            collision.Play();
        }

        for (int i = 0; i < bricksTransform.Count; i++)
        {
            if (bricks[i] != null && bricksTransform[i] != null && bricks[i].isRigid && Collide(bricksTransform[i], new_pos))
            {
                bricks[i].isRigid = false;
                collision.Play();
            }
        }

        transform.position = new_pos;
    }

    private void LateUpdate()
    {
        if (!space_bar_pressed)
        {
            transform.position = playerTransform.position + offset;
            if (Input.GetKeyDown("space"))
                space_bar_pressed = true;
        }
    }

    private bool Collide(Transform objectTransform, Vector3 new_pos)
    {
        bool collide = false;
        Vector3 pos = new Vector3(new_pos.x, new_pos.y, new_pos.z);

        Vector2 circle_pos = new Vector2(pos.x, pos.z);
        float radius = transform.localScale.z / 2;

        float height = objectTransform.localScale.z;
        float width = objectTransform.localScale.x;

        float topLineX1 = objectTransform.position.x - (width / 2);
        float topLineY1 = objectTransform.position.z + (height / 2);
        float topLineX2 = objectTransform.position.x + (width / 2);
        float topLineY2 = objectTransform.position.z + (height / 2);

        Vector2 top_point1 = new Vector2(topLineX1, topLineY1);
        Vector2 top_point2 = new Vector2(topLineX2, topLineY2);

        float bottomLineX1 = objectTransform.position.x - (width / 2); 
        float bottomLineY1 = objectTransform.position.z - (height / 2);
        float bottomLineX2 = objectTransform.position.x + (width / 2);
        float bottomLineY2 = objectTransform.position.z - (height / 2);

        Vector2 bottom_point1 = new Vector2(bottomLineX1, bottomLineY1);
        Vector2 bottom_point2 = new Vector2(bottomLineX2, bottomLineY2);

        float leftLineX1 = objectTransform.position.x - (width / 2);
        float leftLineY1 = objectTransform.position.z - (height / 2);
        float leftLineX2 = objectTransform.position.x - (width / 2);
        float leftLineY2 = objectTransform.position.z + (height / 2);

        Vector2 left_point1 = new Vector2(leftLineX1, leftLineY1);
        Vector2 left_point2 = new Vector2(leftLineX2, leftLineY2);

        float rightLineX1 = objectTransform.position.x + (width / 2);
        float rightLineY1 = objectTransform.position.z - (height / 2);
        float rightLineX2 = objectTransform.position.x + (width / 2);
        float rightLineY2 = objectTransform.position.z + (height / 2);

        Vector2 right_point1 = new Vector2(rightLineX1, rightLineY1);
        Vector2 right_point2 = new Vector2(rightLineX2, rightLineY2);


        if (direction.z < 0 && CircleSegmentCollision(top_point1, top_point2, circle_pos, radius))
        {
            direction.z *= -1;
            new_pos.z = topLineY1 + radius + 0.00001f;
            collide = true;
        }
        else if (direction.z > 0 && CircleSegmentCollision(bottom_point1, bottom_point2, circle_pos, radius))
        {
            direction.z *= -1;
            new_pos.z = bottomLineY1 - radius - 0.00001f;
            collide = true;
        }
        else if (direction.x < 0 && CircleSegmentCollision(right_point1, right_point2, circle_pos, radius))
        {
            direction.x *= -1;
            new_pos.x = rightLineX1 + radius + 0.00001f;
            collide = true;
        }
        else if (direction.x > 0 && CircleSegmentCollision(left_point1, left_point2, circle_pos, radius))
        {
            direction.x *= -1;
            new_pos.x = leftLineX1 - radius - 0.00001f;
            collide = true;
        }

        return collide;
    }

    private float VectorLength(Vector2 v)
    {
        return Mathf.Sqrt((v.x * v.x) + (v.y * v.y));
    }

    private float DotProduct(Vector2 v1, Vector2 v2)
    {
        return ((v1.x * v2.x) + (v1.y * v2.y));
    }

    private Vector2 SubstractVector(Vector2 v1, Vector2 v2)
    {
        return new Vector2((v1.x - v2.x), (v1.y - v2.y));
    }

    private Vector2 AddVector(Vector2 v1, Vector2 v2)
    {
        return new Vector2((v1.x + v2.x), (v1.y + v2.y));
    }

    private Vector2 ProjectVector(Vector2 project, Vector2 onto)
    {
        float d = DotProduct(onto, onto);

        if (d > 0)
        {
            float dp = DotProduct(project, onto);
            return onto * (dp / d);
        }
        return onto;
    }

    private bool CirclePointCollision(Vector2 point, Vector2 circle_pos, float r)
    {
        Vector2 d = SubstractVector(circle_pos, point);
        return VectorLength(d) <= r;
    }

    private bool CircleSegmentCollision(Vector2 point1, Vector2 point2, Vector2 circle_pos, float r)
    {

        if (CirclePointCollision(point1, circle_pos, r))
            return true;
        if (CirclePointCollision(point2, circle_pos, r))
            return true;

        Vector2 d = SubstractVector(point2, point1);
        Vector2 lc = SubstractVector(circle_pos, point1);
        Vector2 p = ProjectVector(lc, d);
        Vector2 nearest = AddVector(point1, p);

        float len_p = VectorLength(p);
        float len_d = VectorLength(d);
        float dot_p_d = DotProduct(p, d);

        return CirclePointCollision(nearest, circle_pos, r) && len_p <= len_d && dot_p_d >= 0;
    }

    public void MultiBallBonus()
    {
        Ball ball2 = Instantiate(this);
        Ball ball3 = Instantiate(this);
    }
}

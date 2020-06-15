using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBricks : MonoBehaviour
{

    [SerializeField]
    private Shader shader;

    [SerializeField]
    private GameObject cube;

    [SerializeField]
    private GameObject empty;


    // Start is called before the first frame update
    void Start()
    {
        GameObject emptyObejct = Instantiate(cube, transform);
        emptyObejct.AddComponent<Brick>();
        emptyObejct.transform.position = new Vector3(-4.5f, 0.5f, 4f);

        Renderer rend = emptyObejct.GetComponent<Renderer>();
        rend.material = new Material(shader);

        emptyObejct = Instantiate(cube, transform);
        emptyObejct.AddComponent<Brick>();
        emptyObejct.transform.position = new Vector3(4.5f, 0.5f, 4f);

        rend = emptyObejct.GetComponent<Renderer>();
        rend.material = new Material(shader);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

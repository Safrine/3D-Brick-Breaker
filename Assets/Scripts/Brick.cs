using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private Material dissolveMaterial;

    [SerializeField]
    private float duration;

    private float elapsed;

    public bool isRigid;

    private bool bonusDrop;

    public GameObject bonus;

    // Start is called before the first frame update
    void Start()
    {
        dissolveMaterial = GetComponent<Renderer>().material;
        dissolveMaterial.SetFloat("_Dissolve", -0.1f);
        duration = 1f;
        elapsed = 0f;
        isRigid = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRigid && elapsed == 0)
        {
            bonusDrop = true;
        }
            

        if (!isRigid && elapsed < duration)
            Dissolve();
        else if (!isRigid && elapsed >= duration)
            Destroy(this);

        if (bonusDrop)
            DropBonus();
    }

    private void Dissolve()
    {
        float dissolve = 0;
        dissolve = ((elapsed * 1f) / duration);
        dissolveMaterial.SetFloat("_Dissolve", dissolve);
        elapsed += Time.deltaTime;
    }

    private void DropBonus()
    {
        if (bonus == null)
            return; 

        bonusDrop = false;
        GameObject bonusObject = Instantiate(bonus);
        bonusObject.AddComponent<Bonus>();
        bonusObject.GetComponent<Bonus>().position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}

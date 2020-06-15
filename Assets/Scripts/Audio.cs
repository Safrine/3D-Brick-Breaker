using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] audios = GameObject.FindGameObjectsWithTag("Audio");
        if (audios.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(transform.gameObject);
    }
}

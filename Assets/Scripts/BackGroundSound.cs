using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSound : MonoBehaviour
{
    public static BackGroundSound instance;
    public AudioSource musicAudioSource;
    void Awake()
    {
        if (BackGroundSound.instance != null)
            Destroy(this.gameObject);
    }

    void Start()
    {
        instance = this;        
        DontDestroyOnLoad(this);
    }


}

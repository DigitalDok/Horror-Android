using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUp : MonoBehaviour {

    public enum CleanType
    {
        Audio,
        Particle
    }

    public CleanType MyType;
    private AudioSource LeAudio;

    private void Start()
    {
        switch (MyType)
        {
            case CleanType.Audio:
                LeAudio = GetComponent<AudioSource>();
                break;
            case CleanType.Particle:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {

        switch (MyType)
        {
            case CleanType.Audio:
                if (!LeAudio.isPlaying) Destroy(gameObject);
                break;
            case CleanType.Particle:
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public GameObject SFX_Prefab;

    public List<string> SFX_Names;
    public List<AudioClip> SFXs;

    public Dictionary<string, AudioClip> SFX_Library = new Dictionary<string, AudioClip>();

    public static SFX_Manager SFX;

    public void Start()
    {
        SFX = this;

        for (int i = 0; i < SFX_Names.Count; i++)
        {
            SFX_Library.Add(SFX_Names[i], SFXs[i]);
        }
    }

    public void PlaySFX(string Name)
    {
        if (SFX_Library.ContainsKey(Name))
        {
            GameObject MySFX = Instantiate(SFX_Prefab);
            MySFX.GetComponent<AudioSource>().clip = SFX_Library[Name];
            MySFX.GetComponent<AudioSource>().Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorDoor : MonoBehaviour
{

    public GameObject Target;
    public GameObject TargetCamera;

    public bool IsLocked;

    private GameObject LePlayer;
    private GameObject MainButtons;

    public Dialogue AssociatedEffectDialogue;
    
    public float DelayBeforeEffect;
    public string FlagToRunIt;
    

    public void Start()
    {
        LePlayer = GameObject.Find("Player");
        MainButtons = GameObject.Find("MainPanel");
    }

    public void Navigate()
    {
        if (IsLocked) return;

        GameObject CurCam = Camera.main.gameObject;

        Camera.main.gameObject.tag = "Untagged";
        TargetCamera.gameObject.tag = "MainCamera";

        CurCam.SetActive(false);
        TargetCamera.gameObject.SetActive(true);

        LePlayer.transform.position = Target.transform.position;

        if (AssociatedEffectDialogue)
        {
            if(FlagToRunIt=="")
            {
                StartCoroutine(NavEffect());
            }
            else
            {
                if(FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(FlagToRunIt)])
                {
                    StartCoroutine(NavEffect());
                }
            }
        }
    }

    IEnumerator NavEffect()
    {
        yield return new WaitForSeconds(DelayBeforeEffect);
        AssociatedEffectDialogue.DialogueInit();
        MainButtons.SetActive(false);
        
    }
}

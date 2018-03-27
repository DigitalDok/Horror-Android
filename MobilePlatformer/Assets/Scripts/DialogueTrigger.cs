using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {
    
    internal Dialogue MyDialogue;
    


	void Start ()
    {
        MyDialogue = transform.parent.GetComponent<Dialogue>();
	}
	
}

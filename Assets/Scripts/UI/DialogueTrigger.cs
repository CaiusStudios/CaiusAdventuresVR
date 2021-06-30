using UnityEngine;

//
// This script is in some GameObject and enable a dialogue to start
// inspired by Brackeys tutorials (https://brackeys.com/) 
// 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }


}

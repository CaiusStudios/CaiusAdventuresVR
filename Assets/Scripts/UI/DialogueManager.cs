using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//
// inspired by Brackeys tutorials (https://brackeys.com/) 
//
public class DialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartDialogue(TextAsset jsonName)
    {
        _sentences = new Queue<string>();
        
        // load dialogue from json
        Dialogue dialogue = LoadDialogueFromJson(jsonName);
        
        // place the box of dialogues on the screen
        animator.SetBool("IsOpen", true);
        
        nameText.text = dialogue.name;
        _sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            FindObjectOfType<GameManagerMain>().screenBusy = false;  // end of dialogue, screen is free
            return;
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines(); // stop animating previous sentence, before starting a new sentence
        StartCoroutine(TypeSentence(sentence));
        
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null; // wait a single frame in between each letter added.
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
    
    //
    // From a JSON file, load and return a Dialogue object
    //
    private static Dialogue LoadDialogueFromJson(TextAsset jsonName)
    {
        Story storyInJson = JsonUtility.FromJson<Story>(jsonName.text);
        Dialogue dialogue = storyInJson.story[0];
        return dialogue;
    }
}

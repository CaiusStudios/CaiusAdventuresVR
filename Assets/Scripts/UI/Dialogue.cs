using UnityEngine;

//
// All info needed about one dialogue
// inspired by Brackeys tutorials (https://brackeys.com/) 
//
[System.Serializable]
public class Dialogue
{
    public string name;  // name of the character talking
    
    [TextArea(3, 10)]
    public string[] sentences; // what we want into our dialogue
}

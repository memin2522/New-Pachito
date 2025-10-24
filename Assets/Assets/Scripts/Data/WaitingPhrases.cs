using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitingPhrases", menuName = "ScriptableObjects/WaitingPhrases")]
public class WaitingPhrases : ScriptableObject
{
    public Phrases[] phrases;
}

[Serializable]
public class Phrases
{
    public int phraseId;
    [TextArea] public string phrase;
}

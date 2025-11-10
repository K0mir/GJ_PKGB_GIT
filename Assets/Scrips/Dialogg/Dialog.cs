using UnityEngine;

[System.Serializable]
public class Dialog 
{

    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public Sprite speakerPortrait;
        [TextArea(3, 10)]
        public string sentence;
    }

    public DialogueLine[] dialogueLines;
}

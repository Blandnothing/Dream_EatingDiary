using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventText : SingletonMono<EventText>
{
    [SerializeField] EventPrompt prompt;
    [SerializeField] EventDialogue dialogue;
    private string npcName;
    private Sprite npcSprite;
    [SerializeField] EventToast toast;

    private GameObject currentUI;

    public void SetNpc(string name, Sprite sprite)
    {
        npcName = name;
        npcSprite = sprite;
    }
    public void ShowDialogue(string text)
    {
        toast.gameObject.SetActive(false);
        dialogue.gameObject.SetActive(true);
        currentUI = dialogue.gameObject;
        dialogue.Parse(text,npcName, npcSprite);
    }

    public void ShowToast(string text)
    {
        toast.gameObject.SetActive(true);
        dialogue.gameObject.SetActive(false);
        currentUI = toast.gameObject;
        toast.Parse(text);
    }

    public void ShowPrompt(string text)
    {
        prompt.Parse(text);
    }
}

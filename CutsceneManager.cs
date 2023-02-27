using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class CutsceneManager : CommonGameManager
{
    static CutsceneManager main;
    public DialogueManager1 dialogueManager;
    public Dialogue[] dialoguesByID;
    public static void StartDialogue(int id)
    {
        main.dialogueManager.lines = main.dialoguesByID[id].speakerLines.ToArray();
        main.dialogueManager.Activate();
    }

    private void Start()
    {
        main = this;
        ScreenEffects.FadeScreen(this, 1f, Color.black, Color.clear);
        print(dialogueManager);
        dialogueManager.Disable();
        print("main:"+ main);
    }
    protected override void GameUpdate()
    {

    }
}

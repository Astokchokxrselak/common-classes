using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneActor : MonoBehaviour
{
    public void PlayDialogue(int id)
    {
        CutsceneManager.StartDialogue(id);
    }
}

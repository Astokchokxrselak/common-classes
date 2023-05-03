using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Common.UI;

[System.Serializable]
public struct Speaker
{
    public static Speaker Null => new Speaker();
    public string name;
    public Sprite sprite;
    public Speaker(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
    public override bool Equals(object obj)
    {
        Speaker speaker = (Speaker)obj;
        return obj is Speaker && speaker.name == this.name && speaker.sprite == this.sprite;
        // return obj is Speaker speaker && speaker.name == this.name && speaker.sprite.name == this.sprite.name;
    }
    public static bool IsNull(Speaker speaker)
    {
        return speaker.sprite == null && string.IsNullOrEmpty(speaker.name);
    }
    public static implicit operator bool(Speaker s) => !IsNull(s);
    public static bool operator ==(Speaker s1, Speaker s2) => s1.Equals(s2);
    public static bool operator !=(Speaker s1, Speaker s2) => !s1.Equals(s2);
}

[System.Serializable]
public struct SpeakerLine
{
    public Speaker speaker1;
    public string line1;
    public Speaker speaker2;
    public string line2;
    public AudioClip voiceLine;
    [System.Flags]
    public enum SpeakerEvent
    {
        None = 0,
        FadeToWhite = 1,
        SkipAfterDelay = 2,
        ShakeScreen = 4,
        Rotate180Degrees = 8,
        RunAway = 16
    }
    public SpeakerEvent event1, event2;
    public SpeakerLine(Speaker speaker1, Speaker speaker2, string line1, string line2, SpeakerEvent event1 = SpeakerEvent.None, SpeakerEvent event2= SpeakerEvent.None, AudioClip voiceLine = null)
    {
        this.speaker1 = speaker1;
        this.speaker2 = speaker2;
        this.line1 = line1;
        this.line2 = line2;
        this.voiceLine = voiceLine;
        this.event1 = event1;
        this.event2 = event2;
    }
}
[System.Serializable]
public class Dialogue { public List<SpeakerLine> speakerLines; }
public class DialogueManager1 : MonoBehaviour
{
    public static KeyCode SkipKey = KeyCode.Tab;
    public int index = -1;
    public bool disableOnFinish = true;
    public float resetOffset = 100f;
    public SpeakerLine[] lines;
    // Start is called before the first frame update
    Transform speaker1, speaker2;
    RectTransform box1, box2;
    Text name1, name2;
    Text text1, text2;
    [HideInInspector]
    public Image person1, person2;
    Image backdrop1, backdrop2;
    float boxY;
    float initPersonX;
    
    public void Activate() => gameObject.SetActive(true);
    public void Disable() => gameObject.SetActive(false);

    void Awake()
    {
        speaker1 = transform.Find("Speaker1");
        speaker2 = transform.Find("Speaker2");

        box1 = speaker1.Find("Textbox1") as RectTransform;
        box2 = speaker2.Find("Textbox2") as RectTransform;

        name1 = box1.Find("Name").GetComponent<Text>();
        name2 = box2.Find("Name").GetComponent<Text>();
        text1 = box1.Find("Text").GetComponent<Text>();
        text2 = box2.Find("Text").GetComponent<Text>();

        person1 = transform.Find("Speaker1/Image1").GetComponent<Image>();
        person2 = transform.Find("Speaker2/Image2").GetComponent<Image>();

        backdrop1 = transform.Find("Speaker1/Backdrop1").GetComponent<Image>();
        backdrop2 = transform.Find("Speaker2/Backdrop2").GetComponent<Image>();

        initPersonX = person1.rectTransform.anchoredPosition.x;
        boxY = box1.anchoredPosition.y;

        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        index = -1;
        // Update();
        backdrop1.color = backdrop2.color = Color.clear;
        PauseManager.canBePaused = false;
        ScreenEffects.Clear();
    }

    int frameCounter;
    AudioSource source;
    int state = LerpWait;
    const int LerpWait = 0, InputWait = 1;
    Timer pauseTimer = new Timer(0f, 0.5f);
    // Update is called once per frame
    void Update()
    {
        CheckSkipAll();
        MoveToNextLineIfDone();
        moveToNextLine = Input.GetKeyDown(KeyCode.Space);
        if (index < lines.Length)
        {
            PauseManager.canBePaused = false;
            LerpBoxPositions();
            LerpPeoplePositions();
            LerpPeopleColors();
            LerpBackdropColors();
            UpdateNames();
            UpdateLines();
            UpdateTimerAndCheckDone();
            UpdateSpeakerEffects();
            IncrementFrameCounter();
        }
    }
    void CheckSkipAll()
    {
        if (Input.GetKeyDown(SkipKey))
        {
            index = lines.Length;
            ForceNext();
        }
    }
    void IncrementFrameCounter()
    {
        frameCounter++;
    }


    const float FrameDelayBeforeFinish = 75;
    const float TPower = 2;
    const float LerpAccel = 0.05f;
    void LerpBoxPositions()
    {
        if (!string.IsNullOrEmpty(lines[index].line1) || !string.IsNullOrEmpty(lines[index].speaker1.name))
        {
            box1.anchoredPosition = Vector2.Lerp(box1.anchoredPosition, new Vector2(box1.anchoredPosition.x, boxY), LerpAccel);
        }
        if (!string.IsNullOrEmpty(lines[index].line2) || !string.IsNullOrEmpty(lines[index].speaker2.name))
        {
            box2.anchoredPosition = Vector2.Lerp(box2.anchoredPosition, new Vector2(box2.anchoredPosition.x, boxY), LerpAccel);
        }
    }

    void LerpPeoplePositions()
    {
        var personRect1 = person1.rectTransform;
        personRect1.anchoredPosition = Vector2.Lerp(personRect1.anchoredPosition, new Vector2(initPersonX, personRect1.anchoredPosition.y), LerpAccel);

        var personRect2 = person2.rectTransform;
        personRect2.anchoredPosition = Vector2.Lerp(personRect2.anchoredPosition, new Vector2(-initPersonX, personRect2.anchoredPosition.y), LerpAccel);
    }

    void LerpPeopleColors()
    {
        if (person1.color != Color.white)
        {
            person1.color = Color.Lerp(person1.color, Color.white, LerpAccel);
        }
        if (person2.color != Color.white) 
        {
            person2.color = Color.Lerp(person2.color, Color.white, LerpAccel);
        } 
    }

    void LerpBackdropColors()
    {
        backdrop1.color = Color.Lerp(backdrop1.color, Color.black / 2.5f, LerpAccel);
        backdrop2.color = Color.Lerp(backdrop2.color, Color.black / 2.5f, LerpAccel);
    }

    void UpdateNames()
    {
        name1.text = lines[index].speaker1.name;
        name2.text = lines[index].speaker2.name;
    }

    void UpdateLines()
    {
        text1.text = lines[index].line1;
        text2.text = lines[index].line2;
    }

    void UpdateTimerAndCheckDone()
    {
        if (pauseTimer.IncrementHit(true, false))
        {
            state = InputWait;
        }
    }
    void UpdateSpeakerEffects()
    {
        UpdateFadeToWhite();
        CheckIfShouldSkip();
        StartScreenShake();
        Rotate180Degrees();
        UpdateLeaveEffect();
    }
    const float DelayPerLetter = 0.2f;
    void CheckIfShouldSkip()
    {
        if ((lines[index].event1 & SpeakerLine.SpeakerEvent.SkipAfterDelay) != 0)
        {
            if (pauseTimer.timer > DelayPerLetter * lines[index].line1.Length)
            {
                ForceNext();
            }
        }
        if ((lines[index].event2 & SpeakerLine.SpeakerEvent.SkipAfterDelay) != 0)
        {
            if (pauseTimer.timer > DelayPerLetter * lines[index].line2.Length)
            {
                ForceNext();
            }
        }
    }

    const float FadeFrameLength = 120;
    void UpdateFadeToWhite()
    {
        if ((lines[index].event1 & SpeakerLine.SpeakerEvent.FadeToWhite) != 0)
        {
            person1.color = Color.Lerp(person1.color, new Color(1, 1, 1, 0), (float)frameCounter / FadeFrameLength);
        }
        if ((lines[index].event2 & SpeakerLine.SpeakerEvent.FadeToWhite) != 0)
        {
            person2.color = Color.Lerp(person2.color, new Color(1, 1, 1, 0), (float)frameCounter / FadeFrameLength);
        }
    }
    void StartScreenShake()
    {
        if (frameCounter == 0)
        {
            if ((lines[index].event1 & SpeakerLine.SpeakerEvent.ShakeScreen) != 0 
            ||  (lines[index].event2 & SpeakerLine.SpeakerEvent.ShakeScreen) != 0)
            {
                ScreenEffects.Activate(ScreenEffects.Type.ScreenShake, 1f, 4f);
            }
        }
    }

    void Rotate180Degrees()
    {
        if ((lines[index].event1 & SpeakerLine.SpeakerEvent.Rotate180Degrees) != 0) 
        {
            person1.transform.eulerAngles = Vector3.zero;
        }
        if ((lines[index].event2 & SpeakerLine.SpeakerEvent.Rotate180Degrees) != 0)
        {
            person2.transform.eulerAngles = Vector3.up * 180;
        }
    }

    const float LeaveMultiplier = 4;
    void UpdateLeaveEffect()
    {
        if ((lines[index].event1 & SpeakerLine.SpeakerEvent.RunAway) != 0)
        {
            var personRect = person1.rectTransform;
            Vector2 startPosition = new Vector2(initPersonX, personRect.anchoredPosition.y),
                    endPosition = new Vector2(initPersonX * LeaveMultiplier, personRect.anchoredPosition.y);
            personRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, Mathf.Pow(frameCounter / FrameDelayBeforeFinish, 2));
        }
        if ((lines[index].event2 & SpeakerLine.SpeakerEvent.RunAway) != 0)
        {
            var personRect = person2.rectTransform;
            Vector2 startPosition = new Vector2(initPersonX, personRect.anchoredPosition.y),
                    endPosition = new Vector2(initPersonX * LeaveMultiplier, personRect.anchoredPosition.y);
            personRect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, Mathf.Pow(frameCounter / FrameDelayBeforeFinish, 2));
        }
    }

    bool done;
    public bool Done => done || !isActiveAndEnabled;

    bool moveToNextLine = false;
    public void ForceNext()
    {
        moveToNextLine = true;
        state = InputWait;
    }
    void MoveToNextLineIfDone()
    {
        if (state == InputWait && moveToNextLine || index == -1)
        {
            frameCounter = 0;
            pauseTimer.SetZero();
            index++;
            if (index >= lines.Length)
            {
                done = true;
                if (disableOnFinish)
                {
                    gameObject.SetActive(false);
                }
                return;
            }

            if (lines[index].voiceLine && VoiceTrigger.voicesEnabled)
            {
                source.PlayOneShot(lines[index].voiceLine);
            }
            
            speaker1.gameObject.SetActive(lines[index].speaker1);
            if (lines[index].speaker1) 
            {
                if (lines[index].speaker1.sprite)
                {
                    person1.enabled = true;
                    person1.sprite = lines[index].speaker1.sprite;
                }
                else person1.enabled = false;
                box1.gameObject.SetActive(!string.IsNullOrEmpty(lines[index].line1) || !string.IsNullOrEmpty(lines[index].speaker1.name));
                if (index == 0 || !lines[index - 1].speaker1 || lines[index - 1].speaker1 != lines[index].speaker1)
                {
                    person1.color = Color.black;
                    person1.rectTransform.eulerAngles = Vector3.up * 180;
                    ResetSpeaker(true);
                }
            }
            speaker2.gameObject.SetActive(lines[index].speaker2);
            if (lines[index].speaker2)
            {
                if (lines[index].speaker2.sprite)
                {
                    person2.enabled = true;
                    person2.sprite = lines[index].speaker2.sprite;
                }
                else person2.enabled = false;
                box2.gameObject.SetActive(!string.IsNullOrEmpty(lines[index].line2) || !string.IsNullOrEmpty(lines[index].speaker2.name));
                if (index == 0 || !lines[index - 1].speaker2 || lines[index - 1].speaker2 != lines[index].speaker2)
                {
                    person2.color = Color.black;
                    person2.rectTransform.eulerAngles = Vector3.zero;
                    ResetSpeaker(false);
                }
            }
        }
    }
    private void OnDisable()
    {
        frameCounter = 0;
        Time.timeScale = 1f;
        PauseManager.canBePaused = true;
        for (int i = 0; i < 2; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void ResetSpeaker(bool speaker1)
    {
        if (speaker1)
        {
            box1.anchoredPosition -= Vector2.up * resetOffset;
            person1.rectTransform.anchoredPosition += Vector2.right * resetOffset;
        }
        else
        {
            box2.anchoredPosition -= Vector2.up * resetOffset;
            person2.rectTransform.anchoredPosition -= Vector2.right * resetOffset;
        }
    }
}

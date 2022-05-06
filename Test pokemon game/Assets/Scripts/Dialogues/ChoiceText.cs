using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Crosstales.RTVoice;

public class ChoiceText : MonoBehaviour
{
    TextMeshProUGUI text;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;
    public bool choiceTextSpoken = false;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetSelected(bool selected)
    {
        text.color = (selected) ? GlobalSettings.i.HighlightedColor : Color.black;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            choiceTextSpoken = false;
        }


         if(selected)
         {
            if(!choiceTextSpoken)
            {
                Debug.Log(text.text);
                Speaker.Instance.Speak(text.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                choiceTextSpoken = true;
            }
        }
    }

    public TextMeshProUGUI TextField => text;
}

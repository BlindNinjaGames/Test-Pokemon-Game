using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;
    bool spoken = false;

    public List<string> Lines
    {
        get { return lines; }  
    }
}

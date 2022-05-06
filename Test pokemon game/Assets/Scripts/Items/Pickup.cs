using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class Pickup : MonoBehaviour, Interactable, ISavable
{
    [SerializeField] SO_ItemBase item;
    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;

    public bool Used { get; set; } = false;

    public IEnumerator Interact(Transform initiator)
    {
        if (!Used)
        {
            initiator.GetComponent<Inventory>().AddItem(item);

            Used = true;

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

            string playerName = initiator.GetComponent<PlayerController>().Name;

            Speaker.Instance.Speak(playerName + " found " + item.Name, audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText($"{playerName} found {item.Name}");
        }
    }

    public object CaptureState()
    {
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}

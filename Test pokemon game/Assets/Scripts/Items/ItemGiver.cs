using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] SO_ItemBase item;
    [SerializeField] int count = 1;
    [SerializeField] Dialog dialog;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;

    bool used = false;

    public IEnumerator GiveItem(PlayerController player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        player.GetComponent<Inventory>().AddItem(item, count);

        used = true;

        Speaker.Instance.Speak(player.Name + " received " + item.Name, audioSource, Speaker.Instance.VoiceForName(voiceName));
        string dialogText = player.Name + " received " + item.Name;

        if (count > 1)
        {
            Speaker.Instance.Speak(player.Name + " received " + count + " " + item.Name + "s.", audioSource, Speaker.Instance.VoiceForName(voiceName));
            dialogText = player.Name + " received " +count + " " + item.Name + "s";
        }

        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return item != null && count > 0 && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}

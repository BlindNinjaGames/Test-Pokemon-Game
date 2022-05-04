using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class Healer : MonoBehaviour
{
    [SerializeField] float fadeTimer = 0.5f;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;

    public IEnumerator Heal(Transform player, Dialog dialog)
    {
        int selectedChoice = 0;

        yield return DialogManager.Instance.ShowDialog(dialog,
            new List<string>() { "Yes please", "No thanks" },
            (choiceIndex) => selectedChoice = choiceIndex);

        if (selectedChoice == 0)
        {
            // Yes
            yield return Fader.i.FadeIn(fadeTimer);

            var playerParty = player.GetComponent<PokemonParty>();
            playerParty.Pokemons.ForEach(p => p.Heal());
            playerParty.PartyUpdated();

            yield return Fader.i.FadeOut(fadeTimer);

            Speaker.Instance.Speak("Your Pokemon should be fully healed now.", audioSource, Speaker.Instance.VoiceForName(voiceName));

            yield return DialogManager.Instance.ShowDialogText("Your Pokemon should be fully healed now.");
        }
        else if (selectedChoice == 1)
        {
            // No
            Speaker.Instance.Speak("Okay! Come back if you change your mind.", audioSource, Speaker.Instance.VoiceForName(voiceName));
            yield return DialogManager.Instance.ShowDialogText("Okay! Come back if you change your mind.");
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class PokemonStartSelection : MonoBehaviour, ISavable
{
    [SerializeField] List<Pokemon> startPokemon;
    [SerializeField] string introText;
    [SerializeField] string selectionText;

    int selectedChoice = 0;
    bool receivedFirstPokemon = false;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;

    public IEnumerator SelectStartPokemon(PlayerController player)
    {
        Speaker.Instance.Speak(introText, audioSource, Speaker.Instance.VoiceForName(voiceName));
        yield return DialogManager.Instance.ShowDialogText(introText);

        List<string> choices = new List<string>();
        foreach (Pokemon pokemon in startPokemon)
        {
            choices.Add(pokemon.Base.Name);
        }

        Speaker.Instance.Speak(selectionText, audioSource, Speaker.Instance.VoiceForName(voiceName));
        yield return DialogManager.Instance.ShowDialogText(
            selectionText,
            waitForInput: false,
            choices: choices,
            onChoiceSelected: choiceIndex => selectedChoice = choiceIndex
        );

        Pokemon pokemonToStartWith = startPokemon[selectedChoice];

        pokemonToStartWith.Init();
        player.GetComponent<PokemonParty>().AddPokemon(pokemonToStartWith);

        receivedFirstPokemon = true;

        //  AudioManager.Instance.PlaySfx(AudioId.PokemonObtained, pauseMusic: true);

        Speaker.Instance.Speak(player.Name + " received " + pokemonToStartWith.Base.Name, audioSource, Speaker.Instance.VoiceForName(voiceName));
        string dialogText = player.Name + " received " + pokemonToStartWith.Base.Name;
        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    public bool ReceivedFirstPokemon()
    {
        return receivedFirstPokemon;
    }

    public object CaptureState()
    {
        return receivedFirstPokemon;
    }

    public void RestoreState(object state)
    {
        receivedFirstPokemon = (bool)state;
    }
}
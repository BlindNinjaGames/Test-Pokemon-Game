using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Crosstales.RTVoice;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    int selection = 0;

    [SerializeField] AudioSource audioSource;
    [SerializeField] string voiceName;
    bool spoken = false;

    PartyMemberUI partyMemberUI;

    [SerializeField] TextMeshProUGUI pokemonNameText1;
    [SerializeField] TextMeshProUGUI pokemonLevelText1;

    [SerializeField] TextMeshProUGUI pokemonNameText2;
    [SerializeField] TextMeshProUGUI pokemonLevelText2;

    [SerializeField] TextMeshProUGUI pokemonNameText3;
    [SerializeField] TextMeshProUGUI pokemonLevelText3;

    [SerializeField] TextMeshProUGUI pokemonNameText4;
    [SerializeField] TextMeshProUGUI pokemonLevelText4;

    public Pokemon SelectedMember => pokemons[selection];

    /// <summary>
    /// Party screen can be called from different states like ActionSelection, RunningTurn, AboutToUse
    /// </summary>
    public BattleState? CalledFrom { get; set; }

    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

        party = PokemonParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    public void SetPartyData()
    {
        pokemons = party.Pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(pokemons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        UpdateMemberSelection(selection);


        messageText.text = "Choose a Pokemon";
    }

    public void HandleUpdate(Action onSelected, Action onBack)
    {
        var prevSelection = selection;

        if (!spoken)
        {
            if (selection == 0)
            {
                Debug.Log(pokemonNameText1.text + " level " + pokemonLevelText1.text);
                Speaker.Instance.Speak(pokemonNameText1.text + " level " + pokemonLevelText1.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 1)
            {
                Debug.Log(pokemonNameText2.text + " level " + pokemonLevelText2.text);
                Speaker.Instance.Speak(pokemonNameText2.text + " level " + pokemonLevelText2.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 2)
            {
                Speaker.Instance.Speak(pokemonNameText3.text + " level " + pokemonLevelText3.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 3)
            {
                Speaker.Instance.Speak(pokemonNameText4.text + " level " + pokemonLevelText4.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            spoken = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spoken = false;

            ++selection;

            if (!spoken)
            {
                if (selection == 0)
                {
                    Debug.Log(pokemonNameText1.text + " level " + pokemonLevelText1.text);
                    Speaker.Instance.Speak(pokemonNameText1.text + " level " + pokemonLevelText1.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 1)
                {
                    Debug.Log(pokemonNameText2.text + " level " + pokemonLevelText2.text);
                    Speaker.Instance.Speak(pokemonNameText2.text + " level " + pokemonLevelText2.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 2)
                {
                    Speaker.Instance.Speak(pokemonNameText3.text + " level " + pokemonLevelText3.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 3)
                {
                    Speaker.Instance.Speak(pokemonNameText4.text + " level " + pokemonLevelText4.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                spoken = true;
            }

        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spoken = false;

            --selection;

            if (!spoken)
            {
                if (selection == 0)
                {
                    Debug.Log(pokemonNameText1.text + " level " + pokemonLevelText1.text);
                    Speaker.Instance.Speak(pokemonNameText1.text + " level " + pokemonLevelText1.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 1)
                {
                    Debug.Log(pokemonNameText2.text + " level " + pokemonLevelText2.text);
                    Speaker.Instance.Speak(pokemonNameText2.text + " level " + pokemonLevelText2.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 2)
                {
                    Speaker.Instance.Speak(pokemonNameText3.text + " level " + pokemonLevelText3.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 3)
                {
                    Speaker.Instance.Speak(pokemonNameText4.text + " level " + pokemonLevelText4.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                spoken = true;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            spoken = false;            
            selection += 2;

            if (!spoken)
            {
                if (selection == 0)
                {
                    Debug.Log(pokemonNameText1.text + " level " + pokemonLevelText1.text);
                    Speaker.Instance.Speak(pokemonNameText1.text + " level " + pokemonLevelText1.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 1)
                {
                    Debug.Log(pokemonNameText2.text + " level " + pokemonLevelText2.text);
                    Speaker.Instance.Speak(pokemonNameText2.text + " level " + pokemonLevelText2.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 2)
                {
                    Speaker.Instance.Speak(pokemonNameText3.text + " level " + pokemonLevelText3.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                else if (selection == 3)
                {
                    Speaker.Instance.Speak(pokemonNameText4.text + " level " + pokemonLevelText4.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
                }

                spoken = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            spoken = false;

            selection -= 2;

            if (selection == 0)
            {
                Speaker.Instance.Speak(pokemonNameText1.text + pokemonLevelText1.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 1)
            {
                Speaker.Instance.Speak(pokemonNameText2.text + pokemonLevelText2.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 2)
            {
                Speaker.Instance.Speak(pokemonNameText3.text + pokemonLevelText3.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }

            else if (selection == 3)
            {
                Speaker.Instance.Speak(pokemonNameText4.text + pokemonLevelText4.text, audioSource, Speaker.Instance.VoiceForName(voiceName));
            }
        }

        selection = Mathf.Clamp(selection, 0, pokemons.Count - 1);

        if (selection != prevSelection)
            UpdateMemberSelection(selection);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            onSelected?.Invoke();
        }

        else if (Input.GetKeyDown(KeyCode.X))
        {
            spoken = false;
            onBack?.Invoke();
        }
    }

    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }



    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}


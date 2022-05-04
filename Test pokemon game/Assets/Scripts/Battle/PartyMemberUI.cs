using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField]  TextMeshProUGUI partyMemberUINameText;
    [SerializeField]  TextMeshProUGUI partyMemberUILevelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] bool isPlayer;

    Pokemon _pokemon;

    public void Init(Pokemon pokemon)
    {
        _pokemon = pokemon;
        UpdateData();

        _pokemon.OnHPChanged += UpdateData;
    }

    void UpdateData()
    {
        partyMemberUINameText.text = _pokemon.Base.Name;
        partyMemberUILevelText.text = _pokemon.Level.ToString();
        hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHp);
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            partyMemberUINameText.color = GlobalSettings.i.HighlightedColor;
        else
            partyMemberUINameText.color = Color.black;
    }



}
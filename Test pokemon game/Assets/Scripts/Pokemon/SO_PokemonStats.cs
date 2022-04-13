using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Stats", menuName = "Stats/Create new stats")]

public class SO_PokemonStats : ScriptableObject
{
    public List<SO_PokemonStats> statsForeachLevel;

    public List<SO_PokemonStats> StatsForeachLevel
    {
        get { return statsForeachLevel; }
    }

}


[System.Serializable]
class PokemonStats
{
    [SerializeField] public int hp;
    [SerializeField] public int attack;
    [SerializeField] public int defense;
    [SerializeField] public int speed;
    [SerializeField] public int critical;

    public int HP
    {
        get { return hp; }
    }

    public int Attack
    {
        get { return attack; }
    }


    public int Defense
    {
        get { return defense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public int Critical
    {
        get { return critical; }
    }

}
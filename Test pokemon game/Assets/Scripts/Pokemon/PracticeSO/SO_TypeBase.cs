using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Type Effectiveness", menuName = "Types/Create new attack type")]
public class SO_TypeBase : ScriptableObject
{
    [SerializeField] new string name;

    [Tooltip("The elemental type that will be super effective against the opponent.")]
    [SerializeField] List<SO_TypeBase> superEffective = new List<SO_TypeBase>();

    [Tooltip("The elemental type that will be weak against the opponent.")]
    [SerializeField] List<SO_TypeBase> notVeryEffective = new List<SO_TypeBase>();

    [Tooltip("The elemental type that will get buffed if hit with this elemental attack.")]
    [SerializeField] List<SO_TypeBase> buffPokemon = new List<SO_TypeBase>();

    public string Name
    {
        get { return name; }
    }

    public List<SO_TypeBase> SuperEffective
    {
        get { return superEffective; }
    }

    public List<SO_TypeBase> NotVeryEffective
    {
        get { return notVeryEffective; }
    }

    public List<SO_TypeBase> BuffPokemon
    {
        get { return buffPokemon; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] float price;
    [SerializeField] bool isSellable;
    [SerializeField] bool forPlayer;

    public virtual string Name => name;
    public string Description => description;
    public Sprite Icon => icon;
    public bool ForPlayer => forPlayer;

    public float Price => price;
    public bool IsSellable => isSellable;

    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }

    public virtual bool IsReusable => false;

    public virtual bool CanUseInBattle => true;
    public virtual bool CanUseOutsideBattle => true;
}

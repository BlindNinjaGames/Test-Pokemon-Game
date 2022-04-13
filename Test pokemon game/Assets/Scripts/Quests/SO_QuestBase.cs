using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Create a new quest")]
public class SO_QuestBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;

    [SerializeField] Dialog startDialogue;
    [SerializeField] Dialog inProgressDialogue;
    [SerializeField] Dialog completedDialogue;

    [SerializeField] SO_ItemBase requiredItem;
    [SerializeField] SO_ItemBase rewardItem;

    public string Name => name;
    public string Description => description;

    public Dialog StartDialogue => startDialogue;
    public Dialog InProgressDialogue => inProgressDialogue?.Lines?.Count > 0 ? inProgressDialogue : startDialogue;
    public Dialog CompletedDialogue => completedDialogue;

    public SO_ItemBase RequiredItem => requiredItem;
    public SO_ItemBase RewardItem => rewardItem;
}

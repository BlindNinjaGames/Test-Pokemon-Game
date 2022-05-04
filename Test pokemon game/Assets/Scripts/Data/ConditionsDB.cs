using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.RTVoice;

public class ConditionsDB
{

    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = " has been poisoned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    Speaker.Instance.SpeakNative(pokemon.Base.Name + " was hurt due to poison."); //, audioSource, Speaker.Instance.VoiceForName(voiceName));
                    pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " was hurt due to poison");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = " has been burned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 16);
                    Speaker.Instance.SpeakNative(pokemon.Base.Name + " was hurt due to burn.");
                    pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " was hurt due to burn");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = " has been paralyzed",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if  (Random.Range(1, 5) == 1)
                    {
                        Speaker.Instance.SpeakNative(pokemon.Base.Name + " is paralyzed and can't move!");
                        pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " is paralyzed and can't move!");
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = " has been frozen",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if  (Random.Range(1, 5) == 1)
                    {
                        pokemon.CureStatus();
                        Speaker.Instance.SpeakNative(pokemon.Base.Name + " is not frozen anymore.");
                        pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " is not frozen anymore");
                        return true;
                    }

                    return false;
                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = " has fallen asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1, 4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        Speaker.Instance.SpeakNative(pokemon.Base.Name + " woke up!");
                        pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " woke up!");
                        return true;
                    }

                    pokemon.StatusTime--;
                    Speaker.Instance.SpeakNative(pokemon.Base.Name + " is sleeping!");
                    pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " is sleeping!");
                    return false;
                }
            }
        },

        // Volatile Status Conditions
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = " has been confused",
                OnStart = (Pokemon pokemon) =>
                {
                    // Confused for 1 - 4 turns
                    pokemon.VolatileStatusTime = Random.Range(1, 5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        Speaker.Instance.SpeakNative(pokemon.Base.Name + " is no longer confused!");
                        pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " is no longer confused!");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;

                    // 50% chance to do a move
                    if (Random.Range(1, 3) == 1)
                        return true;

                    // Hurt by confusion
                    Speaker.Instance.SpeakNative(pokemon.Base.Name + " is confused.");
                    pokemon.StatusChanges.Enqueue(pokemon.Base.Name + " is confused");
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    Speaker.Instance.SpeakNative("It hurt itself in its confusion!");
                    pokemon.StatusChanges.Enqueue("It hurt itself in its confusion!");
                    return false;
                }
            }
        }
    };

    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if (condition.Id == ConditionID.slp || condition.Id == ConditionID.frz)
            return 2f;
        else if (condition.Id == ConditionID.par || condition.Id == ConditionID.psn || condition.Id == ConditionID.brn)
            return 1.5f;

        return 1f;
    }
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz,
    confusion
}

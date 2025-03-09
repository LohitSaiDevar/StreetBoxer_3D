using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Stats/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float maxHealth = 100;
    public float attackPower = 10;
    public float defense = 5;
}

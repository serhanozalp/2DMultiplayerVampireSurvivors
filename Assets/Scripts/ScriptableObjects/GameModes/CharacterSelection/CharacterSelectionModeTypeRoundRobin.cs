using UnityEngine;
using Abstracts;

[CreateAssetMenu(fileName = "RoundRobinMode", menuName = "GameMode/CharacterSelection/RoundRobin", order = 2)]
public class CharacterSelectionModeTypeRoundRobin : BaseCharacterSelectionGameMode
{
    public override CharacterSelection CharacterSelectionType => CharacterSelection.RoundRobin;

    public override string ModeName => "RoundRobin";
}

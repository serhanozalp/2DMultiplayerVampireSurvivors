using UnityEngine;
using Abstracts;

[CreateAssetMenu(fileName = "RandomMode", menuName = "GameMode/CharacterSelection/Random", order = 1)]
public class CharacterSelectionModeTypeRandom : BaseCharacterSelectionGameMode
{
    public override CharacterSelection CharacterSelectionType => CharacterSelection.Random;

    public override string ModeName => "Random";
}

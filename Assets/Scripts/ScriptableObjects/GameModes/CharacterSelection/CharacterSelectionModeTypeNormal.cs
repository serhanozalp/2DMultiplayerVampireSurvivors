using UnityEngine;
using Abstracts;

[CreateAssetMenu(fileName = "NormalMode", menuName = "GameMode/CharacterSelection/Normal", order = 0)]
public class CharacterSelectionModeTypeNormal : BaseCharacterSelectionGameMode
{
    public override CharacterSelection CharacterSelectionType => CharacterSelection.Normal;

    public override string ModeName => "Normal";
}

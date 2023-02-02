using UnityEngine;
using Abstracts;

[CreateAssetMenu(fileName = "NormalMode", menuName = "GameMode/Difficulty/Normal", order = 1)]
public class DifficultyModeTypeNormal : BaseDifficultyGameMode
{
    public override string ModeName => "Normal";

    public override Difficulty DifficultyType => Difficulty.Normal;
}

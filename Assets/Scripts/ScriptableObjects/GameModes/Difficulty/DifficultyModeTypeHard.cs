using UnityEngine;
using Abstracts;

[CreateAssetMenu(fileName = "HardMode", menuName = "GameMode/Difficulty/Hard", order = 2)]
public class DifficultyModeTypeHard : BaseDifficultyGameMode
{
    public override string ModeName => "Hard";

    public override Difficulty DifficultyType => Difficulty.Hard;
}

using UnityEngine;
using Abstracts;
using System;

[CreateAssetMenu(fileName = "EasyMode", menuName = "GameMode/Difficulty/Easy", order = 0)]
public class DifficultyModeTypeEasy : BaseDifficultyGameMode
{
    public override string ModeName => "Easy";

    public override Difficulty DifficultyType => Difficulty.Easy;
}

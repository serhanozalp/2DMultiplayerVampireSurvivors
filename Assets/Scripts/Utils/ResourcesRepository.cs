using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using System.Linq;

public static class ResourcesRepository
{
    public static List<BaseGameMode> LoadGameModes()
    {
        return Resources.LoadAll<BaseGameMode>("").ToList();
    }
}

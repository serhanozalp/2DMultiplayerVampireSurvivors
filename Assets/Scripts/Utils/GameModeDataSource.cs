using System.Collections.Generic;
using Abstracts;
using System;

public static class GameModeDataSource
{
    private static Dictionary<Type, List<BaseGameMode>> _gameModeDictionary;
    private static Dictionary<Type, List<string>> _gameModeNameDictionary;
    private static List<BaseGameMode> _gameModeList;

    public static Dictionary<Type, List<string>> GameModeNameDictionary
    {
        get
        {
            if (_gameModeNameDictionary == null)
            {
                _gameModeNameDictionary = new Dictionary<Type, List<string>>();
                foreach(var pair in GameModeDictionary)
                {
                    _gameModeNameDictionary.Add(pair.Key, pair.Value.ConvertAll(x => x.ModeName));
                }
            }
            return _gameModeNameDictionary;
        }
    }

    public static Dictionary<Type, List<BaseGameMode>> GameModeDictionary
    {
        get
        {
            if(_gameModeDictionary == null)
            {
                _gameModeDictionary = new Dictionary<Type, List<BaseGameMode>>();
                foreach (var gameMode in GameModeList)
                {
                    Type gameModeType = gameMode.GetType().BaseType;
                    if (!_gameModeDictionary.ContainsKey(gameModeType)) _gameModeDictionary.Add(gameModeType, new List<BaseGameMode>());
                    _gameModeDictionary[gameModeType].Add(gameMode);
                    _gameModeDictionary[gameModeType].Sort();
                }
            }
            return _gameModeDictionary;
        }
    }
    public static List<BaseGameMode> GameModeList
    {
        get
        {
            if(_gameModeList == null)
            {
                _gameModeList = ResourcesRepository.LoadGameModes();
            }
            return _gameModeList;
        }
    }
}

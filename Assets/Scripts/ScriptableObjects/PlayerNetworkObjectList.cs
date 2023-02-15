using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNetworkObjectList", menuName = "PlayerNetworkObjectList")]
public class PlayerNetworkObjectList : ScriptableObject
{
    [SerializeField]
    private List<PlayerNetworkObject> _playerNetworkObjectList;

    public Action<PlayerNetworkObject> OnPlayerNetworkObjectAdded;
    public Action<PlayerNetworkObject> OnPlayerNetworkObjectRemoved;


    public void AddPlayerNetworkObject(PlayerNetworkObject playerNetworkGameObject)
    {
        if(!_playerNetworkObjectList.Contains(playerNetworkGameObject)) _playerNetworkObjectList.Add(playerNetworkGameObject);
        else Debug.LogWarning("Already exists in PlayerNetworkObjectList");
    }

    public void RemovePlayerNetworkObject(PlayerNetworkObject playerNetworkGameObject)
    {
        if (_playerNetworkObjectList.Contains(playerNetworkGameObject)) _playerNetworkObjectList.Remove(playerNetworkGameObject);
        else Debug.LogWarning("Does not exists in PlayerNetworkObjectList");
    }
}

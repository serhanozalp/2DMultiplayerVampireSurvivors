using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNetworkObjectList", menuName = "PlayerNetworkObjectList")]
public class PlayerNetworkObjectList : ScriptableObject
{
    [SerializeField]
    private List<PlayerNetworkObject> _playerNetworkObjectList;


    public void AddPlayerNetworkObject(PlayerNetworkObject playerNetworkGameObject)
    {
        _playerNetworkObjectList.Add(playerNetworkGameObject);
    }

    public void RemovePlayerNetworkObject(PlayerNetworkObject playerNetworkGameObject)
    {
        _playerNetworkObjectList.Remove(playerNetworkGameObject);
    }
}

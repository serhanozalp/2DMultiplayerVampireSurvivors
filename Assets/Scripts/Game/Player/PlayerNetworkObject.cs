using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkObject : NetworkBehaviour
{
    [SerializeField]
    private PlayerNetworkObjectList _playerNetworkObjectList;

    private NetworkVariable<FixedString64Bytes> _playerName = new NetworkVariable<FixedString64Bytes>();

    public string PlayerName { get { return _playerName.Value.ToString(); } set { _playerName.Value = value; } }

    public override void OnNetworkSpawn()
    {
        _playerName.OnValueChanged += PlayerName_OnValueChanged;
        _playerNetworkObjectList.AddPlayerNetworkObject(this);
    }

    private void PlayerName_OnValueChanged(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
    }

    public override void OnDestroy()
    {
        _playerName.OnValueChanged -= PlayerName_OnValueChanged;
        _playerNetworkObjectList.RemovePlayerNetworkObject(this);
    }
}

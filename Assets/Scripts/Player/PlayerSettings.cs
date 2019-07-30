using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Player/Player Settings")]
public class PlayerSettings : ScriptableObject {

    [SerializeField]
    string[] _playerColors = { "#ff0000", "#00ff00", "#0000ff", "#ffff00" };

    [SerializeField]
    string _localPlayerColor;

    const string _playerColorPropertyKey = "PlayerColor";

    void StorePlayerColorOverNetwork(string colorHex) {
        Hashtable playerColor = new Hashtable {
            { _playerColorPropertyKey, colorHex }
        };
        PhotonNetwork.SetPlayerCustomProperties(playerColor);
    }

    public Color GetLocalPlayerColor() {
        Color playerColor;
        if (! string.IsNullOrEmpty(_localPlayerColor)) {
            ColorUtility.TryParseHtmlString(_localPlayerColor, out playerColor);
            Debug.Log("player color stored locally: " + playerColor);
        } else {
            string colorHex = (string)PhotonNetwork.LocalPlayer.CustomProperties[_playerColorPropertyKey];
            if (! string.IsNullOrEmpty(colorHex)) {
                _localPlayerColor = colorHex;
                ColorUtility.TryParseHtmlString(colorHex, out playerColor);
                Debug.Log("player color fetched from network: " + playerColor);
            } else {
                playerColor = Color.black;
                Debug.LogWarning("No player color set, using fallback color: " + playerColor);
            }
        }
        return playerColor;
    }

    public void SetPlayerColor(int playerNumber) {
        int colorIndex = playerNumber - 1;
        if (playerNumber >= 0 && playerNumber < _playerColors.Length) {
            _localPlayerColor = _playerColors[colorIndex];
            StorePlayerColorOverNetwork(_playerColors[colorIndex]);
        }
    }
}

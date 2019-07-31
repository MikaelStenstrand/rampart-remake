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
    Color _fallbackColor = Color.black;


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
        } else {
            string colorHex = (string)PhotonNetwork.LocalPlayer.CustomProperties[_playerColorPropertyKey];
            if (! string.IsNullOrEmpty(colorHex)) {
                _localPlayerColor = colorHex;
                ColorUtility.TryParseHtmlString(colorHex, out playerColor);
            } else {
                playerColor = _fallbackColor;
            }
        }
        return playerColor;
    }

    public Color GetPlayerColor(Photon.Realtime.Player player) {
        Color playerColor = _fallbackColor;
        string colorHex = (string)player.CustomProperties[_playerColorPropertyKey];
        if (!string.IsNullOrEmpty(colorHex)) {
            ColorUtility.TryParseHtmlString(colorHex, out playerColor);
        } else {
            playerColor = this.GetPlayerColor(player.ActorNumber);
        }
        return playerColor;
    }

    public Color GetPlayerColor(int playerNumber) {
        Color playerColor = _fallbackColor;
        int colorIndex = playerNumber - 1;
        if (playerNumber >= 0 && playerNumber < _playerColors.Length) {
            string colorHex = _playerColors[colorIndex];
            if (! string.IsNullOrEmpty(colorHex))
                ColorUtility.TryParseHtmlString(colorHex, out playerColor);
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

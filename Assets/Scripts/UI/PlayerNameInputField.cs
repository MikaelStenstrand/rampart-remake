using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour {

    const string playerNamePrefKey = "PlayerName";

    private InputField _inputField;

    void Start() {
        string defaultName = string.Empty;
        _inputField = this.GetComponent<InputField>();

        if (_inputField != null) {
            if (PlayerPrefs.HasKey(playerNamePrefKey)) {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }
        this.SetPlayerName(defaultName);
    }

    public void SetPlayerName(string playerName) {
        if (string.IsNullOrEmpty(playerName))
            return;
        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString(playerNamePrefKey, playerName);
    }

}

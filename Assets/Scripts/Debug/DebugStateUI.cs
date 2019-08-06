using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Rampart.Remake { 

    public class DebugStateUI : MonoBehaviour {

        [SerializeField]
        Text _player1TextUI;

        [SerializeField]
        Text _player2TextUI;

        [SerializeField]
        Text _gameStateTextUI;

        [SerializeField]
        Text _connectedTextUI;

        GameManager _gameManager;

        void Start() {
            _gameManager = GameManager.instance;
            _player1TextUI.text = "";
            _player2TextUI.text = "";
        }

        void Update() {
            _gameStateTextUI.text = _gameManager.GetGameMode().ToString();
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom) {
                _connectedTextUI.text = PhotonNetwork.IsConnected.ToString();
                Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
                _player1TextUI.text = players[0].NickName;
                _player2TextUI.text = players[1].NickName;
            } else {
                _connectedTextUI.text = PhotonNetwork.IsConnected.ToString();
            }
        }

    }
}
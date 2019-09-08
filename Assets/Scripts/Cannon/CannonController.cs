using System.Collections.Generic;
using UnityEngine;

namespace Rampart.Remake { 

    public class CannonController : MonoBehaviour {

        [SerializeField]
        LayerMask _layerMask;

        List<Cannon> _playerCannons;
        Camera _camera;
        GameManager _gameManager;


        void Start() {
            _playerCannons = new List<Cannon>();
            _camera = Camera.main;
            _gameManager = GameManager.instance;
        }

        void Update() {
            if (_gameManager.GetGameMode() == GameMode.ATTACK) {
                this.AimAtCursor();

                if (Input.GetMouseButtonDown(0)) {
                    this.FireCannon();
                }
            }
        }

        void AimAtCursor() {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _layerMask)) {
                foreach (Cannon cannon in _playerCannons) {
                    cannon.RotateCannonTo(hitInfo.point);
                }

            }
        }

        void FireCannon() {
            foreach(Cannon cannon in _playerCannons) {
                if (cannon.IsAvailableToFire()) {
                    cannon.Fire();
                    return;
                }
            }
        }

        public void AddCannon(Cannon cannon) {
            _playerCannons.Add(cannon);
        }


    }
}
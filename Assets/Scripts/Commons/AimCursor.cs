using System;
using UnityEngine;

namespace Rampart.Remake { 

    [RequireComponent(typeof(SpriteRenderer))]
    public class AimCursor : MonoBehaviour {

        [SerializeField]
        LayerMask _layerMask;

        [SerializeField]
        float _cursorUpModifier = 0.1f;

        Camera _camera;
        Rampart.Remake.GameManager _gameManager;
        SpriteRenderer _spriteRenderer;

        void Start() {
            _camera = Camera.main;
            _gameManager = GameManager.instance;
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        void Update() {
            if (_gameManager.GetGameMode() == GameMode.ATTACK) {
                this.AimSpirteAtCursor();
            } else {
                this.HideCursor();
            }
        }

        void HideCursor() {
            if (_spriteRenderer.isVisible)
                _spriteRenderer.enabled = false;
        }

        void AimSpirteAtCursor() {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _layerMask)) {
                this.ShowCursor();
                gameObject.transform.position = hitInfo.point + Vector3.up * _cursorUpModifier;
            } else {
                this.HideCursor();
            }
        }

        void ShowCursor() {
            if (!_spriteRenderer.isVisible)
                _spriteRenderer.enabled = true;
        }
    }
}
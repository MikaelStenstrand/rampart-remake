using UnityEngine;

namespace Rampart.Remake { 
    public class CameraController : MonoBehaviour {

        [Tooltip("Enable to pan the screen using mouse at the corners")]
        [SerializeField]
        bool _enableScreenPan = false;

        [SerializeField]
        float _panSpeed = 20f;

        [SerializeField]
        float _scrollSpeed = 20f;

        [SerializeField]
        float _panBorderThickness = 10f;

        [SerializeField]
        Vector3 _panLimit;
        [SerializeField]
        float _extraNegativeZLimit = -10;

        const string _MOUSEWHEELINPUT = "Mouse ScrollWheel";
        Vector3 _newPosition;
        float _scroll;


        void Start() {
            _newPosition = transform.position;
        }

        void Update() {
            _newPosition = transform.position;

            this.CheckPanInput();
            this.CheckZoomingInput();
            this.LimitCameraPosition();

            transform.position = _newPosition;
        }

        void CheckPanInput() {
            if (Input.GetKey(KeyCode.W) || (_enableScreenPan && Input.mousePosition.y >= Screen.height - _panBorderThickness)) {
                _newPosition.z += _panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) || (_enableScreenPan && Input.mousePosition.y <= _panBorderThickness)) {
                _newPosition.z -= _panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || (_enableScreenPan && Input.mousePosition.x >= Screen.width - _panBorderThickness)) {
                _newPosition.x += _panSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A) || (_enableScreenPan && Input.mousePosition.x <= _panBorderThickness)) {
                _newPosition.x -= _panSpeed * Time.deltaTime;
            }
        }

        void CheckZoomingInput() {
            _scroll = Input.GetAxis(_MOUSEWHEELINPUT);
            _newPosition.y -= _scroll * _scrollSpeed * Time.deltaTime * 100f;
        }

        void LimitCameraPosition() {
            _newPosition.x = Mathf.Clamp(_newPosition.x, -_panLimit.x, _panLimit.x);
            _newPosition.y = Mathf.Clamp(_newPosition.y, -_panLimit.y, _panLimit.y);
            _newPosition.z = Mathf.Clamp(_newPosition.z, -_panLimit.z + _extraNegativeZLimit, _panLimit.z);
        }
    }
}
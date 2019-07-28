using UnityEngine;

[RequireComponent(typeof(GridLayout))]
public class GroundPlacementController : MonoBehaviour {

    [SerializeField]
    GameObject _placeableObjectPrefab;

    [SerializeField]
    KeyCode _newObjectHotkey = KeyCode.N;

    [SerializeField]
    KeyCode _rotateHotkey = KeyCode.R;

    [SerializeField]
    LayerMask _environmentLayerMask;

    GameObject _currentPlaceableObject;
    float _currentObjectRotation = 0;
    GridLayout _gridLayout;

    void Awake() {
        _gridLayout = GetComponent<GridLayout>();
    }

    void Update() {
        HandleNewObjectHotkey();
        if (_currentPlaceableObject != null) {
            MoveCurrentPlaceableObjectToMouse();
            RotateCurrentPlaceableObject();
            ReleaseIfClicked();
        }
    }

    private void ReleaseIfClicked() {
        if (Input.GetMouseButtonDown(0)) {
            _currentPlaceableObject = null;
        }
    }

    private void RotateCurrentPlaceableObject() {
        if (Input.GetKeyDown(_rotateHotkey)) {
            AddRotation(90);
        }
    }

    void AddRotation(float degrees) {
        _currentObjectRotation += degrees;
        if (_currentObjectRotation >= 360) {
            _currentObjectRotation = 0;
        }
    }

    void MoveCurrentPlaceableObjectToMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _environmentLayerMask)) {
            Vector3Int coordinateOfCell = _gridLayout.WorldToCell(hitInfo.point);
            _currentPlaceableObject.transform.position = _gridLayout.CellToWorld(coordinateOfCell);
            _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            _currentPlaceableObject.transform.Rotate(Vector3.up, _currentObjectRotation);
        }
    }

    void HandleNewObjectHotkey() {
        if (Input.GetKeyDown(_newObjectHotkey))  {
            if (_currentPlaceableObject == null) {
                _currentPlaceableObject = Instantiate(_placeableObjectPrefab);
            } else {
                Destroy(_currentPlaceableObject);
            }
        }
    }
}

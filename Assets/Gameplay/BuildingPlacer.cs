using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _preview;

    private BuildingsConfig _config;
    private BuildingFactory _factory;
    private BuildingEntry _active;

    private Dictionary<Vector2Int, Building> _placed;

    private GameInput _input;
    private SpriteRenderer _previewSpriteRenderer;

    private Vector2Int _gridPos = Vector2Int.zero;

    private float _moveCooldown = 0.25f;
    private float _moveTimer = 0f;

    public enum Mode { Place, Delete }
    public Mode CurrentMode { get; private set; }

    public void Initialize(GameInput input, BuildingsConfig config)
    {
        _input = input;
        _config = config;

        _factory = new BuildingFactory();
        _placed = new();

        _previewSpriteRenderer = _preview.GetComponent<SpriteRenderer>();
    }

    public void SetModePlace() => CurrentMode = Mode.Place;
    public void SetModeDelete() => CurrentMode = Mode.Delete;

    public void SelectBuilding(string id)
    {
        _active = _config.buildings.Find(b => b.id == id);

        var prefab = Resources.Load<GameObject>(_active.prefabPath);

        _previewSpriteRenderer.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        _moveTimer -= Time.deltaTime;
        Vector2 mouseInput = _input.Gameplay.MoveBuildingMouse.ReadValue<Vector2>();
        Vector2 keyboardInput = _input.Gameplay.MoveBuildingKeyboard.ReadValue<Vector2>();

        if (keyboardInput != Vector2.zero && _moveTimer <= 0f)
        {
            // Смещаем от текущей позиции preview
            _gridPos += new Vector2Int(Mathf.RoundToInt(keyboardInput.x), Mathf.RoundToInt(keyboardInput.y));
            _moveTimer = _moveCooldown;
        }
        else if(_input.Gameplay.MoveBuildingMouse.triggered)
        {
            // Обновляем позицию по мыши
            Vector3 screenPoint = new Vector3(mouseInput.x, mouseInput.y, -_camera.transform.position.z);
            Vector3 world = _camera.ScreenToWorldPoint(screenPoint);
            _gridPos = _gridManager.WorldToGrid(world);
        }

        // Если выбрано здание
        if (CurrentMode == Mode.Place && _active != null)
        {
            _preview.SetActive(true);

            // Перемещаем preview на сетку
            _preview.transform.position = new Vector3(_gridPos.x, _gridPos.y, 0);

            if (_gridManager.IsAreaFree(_gridPos, _active.width, _active.height) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                _previewSpriteRenderer.color = new Color(0f, 1f, 0f, 0.5f);

                if (_input.Gameplay.Click.triggered)
                {
                    var b = _factory.Create(_active.prefabPath, _gridPos, _active.id, _active.width, _active.height);
                    _gridManager.OccupyArea(_gridPos, _active.width, _active.height);
                    _placed.Add(_gridPos, b);
                }
            }
            else
            {
                _previewSpriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
            }
        }
        else _preview.SetActive(false);
        
        if (CurrentMode == Mode.Delete)
        {
            if (_input.Gameplay.Click.triggered && _placed.TryGetValue(_gridPos, out var b))
            {
                _gridManager.FreeArea(b.gridPosition, b.width, b.height);
                Destroy(b.gameObject);
                _placed.Remove(_gridPos);
            }
        }
    }

    public PlacedBuildingsData GetSaveData()
    {
        var data = new PlacedBuildingsData();
        foreach (var kv in _placed)
        {
            var b = kv.Value;
            data.items.Add(new PlacedBuildingDTO { id = b.id, x = b.gridPosition.x, y = b.gridPosition.y, w = b.width, h = b.height });
        }
        return data;
    }

    public void Load(PlacedBuildingsData data)
    {
        // Очистка поял
        foreach (var kv in _placed)
        {
            Destroy(kv.Value.gameObject);
            _gridManager.FreeArea(kv.Key, kv.Value.width, kv.Value.height);
        }
        _placed.Clear();

        foreach (var d in data.items)
        {
            var entry = _config.buildings.Find(b => b.id == d.id);
            var b = _factory.Create(entry.prefabPath, new Vector2Int(d.x, d.y), entry.id, entry.width, entry.height);
            _gridManager.OccupyArea(new Vector2Int(d.x, d.y), entry.width, entry.height);
            _placed[new Vector2Int(d.x, d.y)] = b;
        }
    }
}

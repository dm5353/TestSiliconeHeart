using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private BuildingPlacer _buildingPlacer;

    private GameObject[] _imagesBuildings, _imagesModes;

    private GameInput _input;
    private BuildingsConfig _buildingsConfig;

    private void Awake()
    {
        Debug.Log("[Bootstrap] Initialization started...");

        _input = new GameInput();
        _input.Enable();

        _imagesBuildings = new GameObject[3];
        _imagesModes = new GameObject[2];

        _buildingsConfig = ConfigLoader.Load();

        _buildingPlacer.Initialize(_input, _buildingsConfig);
        _uIManager.Initialize(_buildingPlacer, _imagesBuildings, _imagesModes);

        Debug.Log("[Bootstrap] Initialization finished");
    }
}

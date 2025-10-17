using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _placeBtn, _deleteBtn, _saveBtn, _loadBtn;
    [SerializeField] private Button _b1Btn, _b2Btn, _b3Btn;

    private GameObject[] _imagesBuildings, _imagesModes;

    public void Initialize(BuildingPlacer buildingPlacer, GameObject[] imagesBuildings, GameObject[] imagesModes)
    {
        _imagesBuildings = imagesBuildings;
        _imagesModes = imagesModes;

        _imagesModes[0] = _placeBtn.transform.GetChild(0).gameObject;
        _imagesModes[1] = _deleteBtn.transform.GetChild(0).gameObject;

        _imagesBuildings[0] = _b1Btn.transform.GetChild(0).gameObject;
        _imagesBuildings[1] = _b2Btn.transform.GetChild(0).gameObject;
        _imagesBuildings[2] = _b3Btn.transform.GetChild(0).gameObject;

        _placeBtn.onClick.AddListener(() => {
            buildingPlacer.SetModePlace();
            SetActiveImagesModes(0);
        });

        _deleteBtn.onClick.AddListener(() => {
            buildingPlacer.SetModeDelete();
            SetActiveImagesModes(1);
        });

        _saveBtn.onClick.AddListener(() => {
            SaveLoadManager.Save(buildingPlacer.GetSaveData());
        });

        _loadBtn.onClick.AddListener(() => {
            buildingPlacer.Load(SaveLoadManager.Load());
        });

        _b1Btn.onClick.AddListener(() => {
            buildingPlacer.SelectBuilding("b1");
            SetActiveImagesBuildings(0);
        });

        _b2Btn.onClick.AddListener(() => {
            buildingPlacer.SelectBuilding("b2");
            SetActiveImagesBuildings(1);
        });

        _b3Btn.onClick.AddListener(() => {
            buildingPlacer.SelectBuilding("b3");
            SetActiveImagesBuildings(2);
        });
    }

    private void SetActiveImagesBuildings(int index)
    {
        for (int i = 0; i < _imagesBuildings.Length; i++)
            _imagesBuildings[i].SetActive(i == index);
    }
    private void SetActiveImagesModes(int index)
    {
        for (int i = 0; i < _imagesModes.Length; i++)
            _imagesModes[i].SetActive(i == index);
    }
}

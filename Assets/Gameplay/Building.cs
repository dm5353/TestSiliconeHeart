using UnityEngine;

public class Building : MonoBehaviour
{
    public string id;
    public int width, height;
    public Vector2Int gridPosition;

    public void Init(string id, int w, int h, Vector2Int pos)
    {
        this.id = id;
        width = w;
        height = h;
        gridPosition = pos;
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
}

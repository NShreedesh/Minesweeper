using UnityEngine;


public class Cell
{
    public enum Type
    {
        Invalid,
        Empty,
        Mine,
        Number
    }

    public Type type;
    public Vector2Int position;
    public SpriteRenderer tileSpriteRenderer;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}

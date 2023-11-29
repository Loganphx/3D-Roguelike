using UnityEngine;

public class NodeData
{
    public readonly Color color;
    public readonly int   health;
    public readonly ITEM_TYPE itemType;

    public NodeData(Color color, int health, ITEM_TYPE itemId)
    {
      this.color = color;
      this.health = health;
      itemType = itemId;
    }
}
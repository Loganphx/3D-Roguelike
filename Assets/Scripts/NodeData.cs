using UnityEngine;

public class NodeData
{
  public readonly Color Color;
  public readonly int Health;
  public readonly ITEM_TYPE ItemType;
  public readonly TOOL_TYPE RequiredToolType;
  public readonly int MinimumToolDamage;

  public NodeData(Color color, int health, ITEM_TYPE itemId, TOOL_TYPE requiredToolType, int minimumToolDamage)
  {
    Color = color;
    Health = health;
    ItemType = itemId;
    RequiredToolType = requiredToolType;
    MinimumToolDamage = minimumToolDamage;
  }
}
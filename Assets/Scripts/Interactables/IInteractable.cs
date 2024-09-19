
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
  public INTERACTABLE_TYPE GetInteractableType(); 
  public void Interact(IPlayer player);
}

public interface IDamager
{
  
}
public interface IDamagable
{
  public Transform Transform { get; }
  public void OnHit(IDamager damager, Vector3 hitDirection, Vector3 hitPosition, TOOL_TYPE toolType, int damage);
  
  
}
public static class InterfaceExtensionMethods {
  public static void Death(this Damagable damagable, Vector3 hitDirection, List<(ITEM_TYPE item, int amount)> items)
  {
    var position = damagable.transform.position;
    var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
    
    foreach (var droppedItem in items)
    {
      var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
      var itemTemplate = Object.Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
      var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[droppedItem.item]];

      var item = Object.Instantiate(itemPrefab, Vector3.zero,
        Quaternion.identity, itemTemplate.transform);

      item.transform.localPosition = Vector3.zero;
    
      itemTemplate.GetComponent<Item>().SetItemType(droppedItem.item, droppedItem.amount);

      // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
      hitDirection.y = 1;
      var rigidBody = itemTemplate.GetComponent<Rigidbody>();
      rigidBody.linearDamping = 0.5f;
      rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);
    }
    

    damagable.transform.gameObject.SetActive(false);

  }
}

public interface IHoverable
{
  public void OnHoverEnter(IPlayer player);
  public void OnHoverExit(IPlayer player);
}
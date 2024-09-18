
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
  public void TakeDamage(IDamager damager, Vector3 hitDirection, TOOL_TYPE toolType, int damage);
  
  
}
public static class InterfaceExtensionMethods {
  public static void Death(this IDamagable damagable, Vector3 hitDirection, ITEM_TYPE droppedItem)
  {
    var position = damagable.Transform.position;
    var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
    var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
    var itemTemplate = GameObject.Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
    var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[droppedItem]];

    var item = GameObject.Instantiate(itemPrefab, Vector3.zero,
      Quaternion.identity, itemTemplate.transform);

    item.transform.localPosition = Vector3.zero;
    
    itemTemplate.GetComponent<Item>().SetItemType(droppedItem);

    // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
    hitDirection.y = 1;
    var rigidBody = itemTemplate.GetComponent<Rigidbody>();
    rigidBody.linearDamping = 0.5f;
    rigidBody.AddForce(hitDirection * 3f, ForceMode.Impulse);

    damagable.Transform.gameObject.SetActive(false);

  }
}

public interface IHoverable
{
  public void OnHoverEnter(IPlayer player);
  public void OnHoverExit(IPlayer player);
}
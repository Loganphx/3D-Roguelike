using UnityEngine;
using Random = System.Random;

public class Totem : MonoBehaviour, IInteractable, IHoverable
{
  private Outline _outline;
  private Animation _animation;

  private bool started;

  private Animal[] _spawnedEnemies = new Animal[3];

  private int remainingEnemies;
  public static string[] Animations = new string[]
  {
    "Animations/Totem/Totem-0-Progress",
    "Animations/Totem/Totem-1-Progress",
    "Animations/Totem/Totem-2-Progress",
    "Animations/Totem/Totem-3-Progress",
  };

  private void Awake()
  {
    _outline = GetComponent<Outline>();
    _outline.enabled = false;
    _animation = GetComponent<Animation>();
  }

  private void FixedUpdate()
  {
    if (!started) return;

    int entityCount = 0;

    for (int i = 0; i < _spawnedEnemies.Length; i++)
    {
      if (_spawnedEnemies[i].State.CurrentHealth > 0)
      {
        entityCount++;
      }
    }

    if (remainingEnemies != entityCount)
    {
      // Update Visual 
      if (entityCount == 2)
      {
        if (_animation.clip.name != "Totem-1-Progress")
        {
          _animation.clip = AnimationPool.Animations[Animations[1]];
          _animation.Play();
        }
      }
      else if (entityCount == 1)
      {
        if (_animation.clip.name != "Totem-2-Progress")
        {
          _animation.clip = AnimationPool.Animations[Animations[2]];
          _animation.Play();
        }
      }
      else if (entityCount == 0)
      {
        if (_animation.clip.name != "Totem-3-Progress")
        {
          _animation.clip = AnimationPool.Animations[Animations[3]];
          _animation.Play();
        }
      }
      
      if (entityCount == 0)
      {
        Death();
      }
    }

    remainingEnemies = entityCount;
  }

  public INTERACTABLE_TYPE GetInteractableType()
  {
    return INTERACTABLE_TYPE.TOTEM;
  }

  public void Interact(IPlayer player)
  {
    Debug.Log("Interacting with totem");

    SpawnEnemies();

    // Disable Interaction Trigger

    var interaction = transform.Find("Interaction");
    interaction.gameObject.SetActive(false);

    _animation.clip = AnimationPool.Animations[Animations[0]];
    _animation.Play();
    
    started = true;
  }

  private void SpawnEnemies()
  {
    var transform1 = transform;
    var forward = transform1.forward;
    var right = transform1.right;
    var position1 = transform1.position;
    Vector3[] positions = new[]
    {
      position1 + (forward * 2) + (Vector3.up * 1.5f),
      position1 + (forward * 1 + right * 2) + (Vector3.up * 1.5f),
      position1 + (forward * 1 - right * 2) + (Vector3.up * 1.5f)
    };

    for (int i = 0; i < positions.Length; i++)
    {
      var position = positions[i];

      var prefab = PrefabPool.Prefabs["Prefabs/Entities/Animal"];
      if(prefab == null) Debug.LogError("Prefab is null");
      var animal =GameObject
        .Instantiate(prefab, position, Quaternion.identity);
      Physics.Raycast(position, Vector3.down, out var hit, 10f, 1 << LayerMask.NameToLayer("Ground"));
      animal.transform.position = hit.point;
      
      if(animal == null) Debug.LogError("Animal is null");
      _spawnedEnemies[i] = animal
        .GetComponent<Animal>();
    }
  }

  public void Death()
  {
    // GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = transform.position + (Vector3.up * 0.45f);
    var powerupPrefab =
      PrefabPool.Prefabs[
        PowerupPool.PowerupPrefabs[
          (POWERUP_TYPE)new Random().Next((int)(POWERUP_TYPE.NULL + 1), (int)(POWERUP_TYPE.LEGENDARY + 1))]];
    var powerup = Instantiate(powerupPrefab, transform.position + (Vector3.up * 0.45f), Quaternion.identity);
    gameObject.SetActive(false);
  }

  public void OnHoverEnter(IPlayer player)
  {
    _outline.enabled = true;
  }

  public void OnHoverExit(IPlayer player)
  {
    _outline.enabled = false;
  }
}
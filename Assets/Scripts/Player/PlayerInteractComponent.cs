using System;
using ECS.Movement.Services;
using Interactables;
using UnityEngine;

public enum INTERACTABLE_TYPE : byte
{
    NULL = 0,
    CRAFTING_STATION,
    FURNACE,
    CAULDRON,
    CHEST,
    FLETCHING_TABLE,
    ANVIL,

    HERB,
    NODE,
    CROP,
    ITEM,
    POWERUP,
    TOTEM,
}

public struct PlayerInteractState : IState
{
    public bool HasChanged;
    public int ColliderId;
    public INTERACTABLE_TYPE InteractableType;
}

internal class PlayerInteractComponent : IComponent<PlayerInteractState>
{
    private readonly IPlayer _player;
    private readonly Transform _cameraTransform;
    private PhysicsScene _physicsScene;

    private IHoverable lastHover;

    private RaycastHit[] _hits = new RaycastHit[5];
    private Collider[] _overlapHits = new Collider[5];

    private readonly int _groundLayer = 1 << LayerMask.NameToLayer("Ground");

    private Transform _hoverPrefab;

    public PlayerInteractState _state = new PlayerInteractState()
    {
        HasChanged = true,
        ColliderId = -1,
        InteractableType = INTERACTABLE_TYPE.NULL
    };

    public PlayerInteractComponent(IPlayer player, Transform cameraTransform,
        PhysicsScene physicsScene)
    {
        _player = player;
        _cameraTransform = cameraTransform;
        _physicsScene = physicsScene;
    }

    public bool ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState)
    {
        var hitCount = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
            1 << LayerMask.NameToLayer("Interactable"), QueryTriggerInteraction.Collide);

        ref var state = ref _state;
        state.HasChanged = false;

        if (weaponState.EquippedWeapon != ITEM_TYPE.BUILDING_FOUNDATION && _hoverPrefab != null &&
            _hoverPrefab.position != new Vector3(0, -100, 0))
        {
            _hoverPrefab.position = new Vector3(0, -100, 0);
        }

        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];
                var hoverable = hit.transform.GetComponentInParent<IHoverable>();
                var isHoverable = hoverable != null;

                if (isHoverable)
                {
                    if (lastHover != hoverable)
                    {
                        if (lastHover != null)
                        {
                            lastHover.OnHoverExit(_player);
                            lastHover = null;
                        }

                        hoverable.OnHoverEnter(_player);
                        state.ColliderId = -1;
                        state.InteractableType = INTERACTABLE_TYPE.NULL;
                        state.HasChanged = true;
                    }

                    lastHover = hoverable;
                }

                if (input.PrimaryAttackClicked)
                {
                    if (ItemPool.ItemDeployables.Contains(weaponState.EquippedWeapon))
                    {
                        var farmPlot = hit.transform.GetComponentInParent<IDeployable>();
                        farmPlot?.Deploy(_player, weaponState.EquippedWeapon);
                        return false;
                    }
                }

                if (!input.Interact) return false;
                // Debug.Log($"Hit {hit.collider}");
                var interactable = hit.transform.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(_player);
                    state.ColliderId = hit.collider.GetInstanceID();
                    state.InteractableType = interactable.GetInteractableType();
                    state.HasChanged = true;
                    return false;
                }
            }
        }
        else
        {
            if (weaponState.EquippedWeapon != ITEM_TYPE.NULL &&
                ItemPool.ItemDeployables.Contains(weaponState.EquippedWeapon))
            {
                if (IDeployable.ItemToDeployable[typeof(BuildingBlock)].ContainsKey(weaponState.EquippedWeapon))
                {
                    hitCount = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
                        _groundLayer, QueryTriggerInteraction.Ignore);

                    if (hitCount > 0)
                    {
                        if (weaponState.EquippedWeapon == ITEM_TYPE.BUILDING_FOUNDATION)
                        {
                            _hoverPrefab = PrefabPool
                                .SpawnedPrefabs[
                                    IDeployable.ItemToDeployable[typeof(BuildingBlock)][weaponState.EquippedWeapon]]
                                .transform;
                            var box = _hoverPrefab.GetChild(0);

                            var meshRenderer = box.GetComponent<MeshRenderer>();

                            var localScale = box.localScale;
                            var startPos = _hits[0].point;

                            _hoverPrefab.position = startPos;
                            _hoverPrefab.forward = _player.Transform.forward;
                            //Debug.DrawLine(startPos, startPos + box.up, Color.blue, 5f);
                            var hits = Physics.OverlapBoxNonAlloc(startPos, localScale / 2, _overlapHits, box.rotation,
                                ~_groundLayer, QueryTriggerInteraction.Ignore);
                            if (hits > 0)
                            {
                                Debug.Log($"Invalid placement: {_overlapHits[0].transform.root}");
                                meshRenderer.material = MaterialPool.Materials["Materials/InvalidBuildHover"];
                            }
                            else
                            {
                                meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];
                                if (input.PrimaryAttackClicked)
                                {
                                    _player.RemoveItem(weaponState.EquippedWeapon, 1);

                                    var buildingBlock = GameObject
                                        .Instantiate(PrefabPool.Prefabs[
                                            IDeployable.ItemToDeployable[typeof(BuildingBlock)][
                                                weaponState.EquippedWeapon]]).transform;
                                    buildingBlock.position = _hoverPrefab.position;
                                    buildingBlock.forward = _hoverPrefab.forward;

                                    _hoverPrefab.position = new Vector3(0, -100, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        _hoverPrefab = PrefabPool
                            .SpawnedPrefabs[
                                IDeployable.ItemToDeployable[typeof(BuildingBlock)][weaponState.EquippedWeapon]]
                            .transform;
                        _hoverPrefab.position = Vector3.down;
                    }
                }
                else if (IDeployable.ItemToDeployable[typeof(CraftingStation)].ContainsKey(weaponState.EquippedWeapon))
                {
                    HoverDeployable(typeof(CraftingStation), ref input, ref weaponState);
                }
                else if (IDeployable.ItemToDeployable[typeof(FletchingTable)].ContainsKey(weaponState.EquippedWeapon))
                {
                    HoverDeployable(typeof(FletchingTable), ref input, ref weaponState);
                }
                else if (IDeployable.ItemToDeployable[typeof(Anvil)].ContainsKey(weaponState.EquippedWeapon))
                {
                    HoverDeployable(typeof(Anvil), ref input, ref weaponState);
                }
                else if (IDeployable.ItemToDeployable[typeof(Furnace)].ContainsKey(weaponState.EquippedWeapon))
                {
                    HoverDeployable(typeof(Furnace), ref input, ref weaponState);
                }
                else if (IDeployable.ItemToDeployable[typeof(Cauldron)].ContainsKey(weaponState.EquippedWeapon))
                {
                    HoverDeployable(typeof(Cauldron), ref input, ref weaponState);
                }
            }
        }

        if (lastHover != null)
        {
            lastHover.OnHoverExit(_player);
            lastHover = null;
        }

        if (state.InteractableType != INTERACTABLE_TYPE.NULL)
        {
            state.ColliderId = -1;
            state.InteractableType = INTERACTABLE_TYPE.NULL;
            state.HasChanged = true;
        }

        return false;
    }

    private void HoverDeployable(Type deployableType, ref GameplayInput inputState, ref PlayerWeaponState weaponState)
    {
        var hitCount = _physicsScene.Raycast(_cameraTransform.position, _cameraTransform.forward, _hits, 5f,
            ~(1 << LayerMask.NameToLayer("Interactable") | 1 << LayerMask.NameToLayer("Player")),
            QueryTriggerInteraction.Ignore);

        if (hitCount > 0)
        {
            var hoverPrefab = PrefabPool
                .SpawnedPrefabs[
                    IDeployable.ItemToDeployable[deployableType][weaponState.EquippedWeapon]]
                .transform;
            var box = hoverPrefab.GetChild(0);

            var meshRenderer = box.GetChild(0).GetComponent<MeshRenderer>();

            var localScale = box.localScale;
            var startPos = _hits[0].point;

            hoverPrefab.position = startPos;
            hoverPrefab.forward = _player.Transform.forward;

            var hits = Physics.OverlapBoxNonAlloc(meshRenderer.bounds.center, meshRenderer.bounds.extents / 2,
                _overlapHits, box.rotation,
                ~_groundLayer, QueryTriggerInteraction.Ignore);
            if (hits > 0)
            {
                Debug.Log($"Invalid placement ({hitCount}): {_overlapHits[0].transform.root}");
                meshRenderer.material = MaterialPool.Materials["Materials/InvalidBuildHover"];
            }
            else
            {
                var isGrounded = Physics.OverlapBoxNonAlloc(meshRenderer.bounds.center, meshRenderer.bounds.extents / 2,
                    _overlapHits,
                    box.rotation);
                if (isGrounded == 0)
                {
                    meshRenderer.material = MaterialPool.Materials["Materials/InvalidBuildHover"];
                }
                else meshRenderer.material = MaterialPool.Materials["Materials/BuildHover"];

                if (inputState.PrimaryAttackClicked)
                {
                    _player.RemoveItem(weaponState.EquippedWeapon, 1);

                    var buildingBlock = GameObject
                        .Instantiate(PrefabPool.Prefabs[
                            IDeployable.ItemToDeployable[deployableType][
                                weaponState.EquippedWeapon]]).transform;
                    buildingBlock.position = hoverPrefab.position;
                    buildingBlock.forward = hoverPrefab.forward;

                    hoverPrefab.position = new Vector3(0, -100, 0);
                }
            }
        }
        else
        {
            var hoverPrefab = PrefabPool
                .SpawnedPrefabs[
                    IDeployable.ItemToDeployable[deployableType][weaponState.EquippedWeapon]]
                .transform;
            hoverPrefab.position = Vector3.down;
        }
    }

    public PlayerInteractState State => _state;
}

internal class PlayerDropComponent
{
    private readonly IPlayer _player;
    private readonly Transform _cameraTransform;

    internal PlayerDropComponent(IPlayer player, Transform cameraTransform)
    {
        _player = player;
        _cameraTransform = cameraTransform;
    }

    public bool ProcessInput(ref GameplayInput input, ref PlayerWeaponState weaponState,
        ref PlayerInventoryState inventoryState)
    {
        if (input.Drop && weaponState.EquippedWeapon != ITEM_TYPE.NULL)
        {
            var quantity = inventoryState.Items[weaponState.EquippedSlot].Amount;
            DropItem(weaponState.EquippedSlot, weaponState.EquippedWeapon, quantity);
            return true;
        }

        return false;
    }

    private void DropItem(int itemSlot, ITEM_TYPE itemType, int quantity)
    {
        var position = _cameraTransform.transform.position;
        var dropPosition = new Vector3(position.x, position.y + 0.15f, position.z);
        var itemTemplatePrefab = PrefabPool.Prefabs["Prefabs/Items/item_template"];
        var itemTemplate = GameObject.Instantiate(itemTemplatePrefab, dropPosition, Quaternion.identity);
        var itemPrefab = PrefabPool.Prefabs[ItemPool.ItemPrefabs[itemType]];

        var item = GameObject.Instantiate(itemPrefab, Vector3.zero,
            Quaternion.identity, itemTemplate.transform);

        item.transform.localPosition = Vector3.zero;

        itemTemplate.GetComponent<Item>().SetItemType(itemType, quantity);

        // var drop = GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
        // hitDirection.y = 1;
        itemTemplate.GetComponent<Rigidbody>().AddForce(_cameraTransform.transform.forward * 3f, ForceMode.Impulse);

        _player.RemoveItem(itemSlot, quantity);
    }
}
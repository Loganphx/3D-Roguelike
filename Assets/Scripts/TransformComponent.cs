using System;
using UnityEditor.VersionControl;
using UnityEngine;

namespace ECS.Core.Components
{
  [Flags]
  public enum TransformDirtyFlags : byte
  {
    Position      = 1 << 0,
    Rotation      = 1 << 1,
    LocalRotation = 1 << 2,
    Scale         = 1 << 3,
    LocalScale    = 1 << 4,
  }

  public interface ISerializable
  {
    public void Serialize(Message message);
    public int  Size { get; }
  }

  public interface IDeserializable<T>
  {
    public T Deserialize(Message message);
  }

  public struct TransformData /*: ISerializable, IDeserializable<TransformData>*/
  {
    public const int        size = 12 + 16 + 16 + 12;
    public       Vector3    Position;      // 12 bytes
    public       Quaternion Rotation;      // 16 bytes
    public       Quaternion LocalRotation; // 16 bytes
    public       Vector3    Scale;         // 12 bytes

    public int Size => size;

    // 40 bytes
    // public void Serialize(Message message)
    // {
    //   message.AddVector3(Position);
    //   message.AddQuaternion(Rotation);
    //   message.AddVector3(Scale);
    // }

    // public TransformData Deserialize(Message message)
    // {
    //   Position = message.GetVector3();
    //   Rotation = message.GetQuaternion();
    //   Scale    = message.GetVector3();
    //
    //   return this;
    // }
  }

  public class CameraTransformComponent : TransformComponent
  {
    public static readonly int HashId = typeof(CameraTransformComponent).GetHashCode();
    public CameraTransformComponent(Transform transform, int transformId) : base(transform, transformId) { }
  }

  /// <summary>
  /// Get the root Transform of the entity this component is on
  /// </summary>
  [Serializable]
  public class TransformComponent : IComponent /*ISerializable, IDeserializable<TransformComponent>*/
  {
    public const int size = sizeof(int) + TransformData.size;

    public static readonly int HashId = typeof(TransformComponent).GetHashCode();

    public int TransformId;

    public bool                IgnoreRotation; // :)
    public bool                IgnoreWorldPosition; // :)
    public bool                HasChanged;
    public TransformDirtyFlags DirtyFlags;

    public TransformData TransformData;

    public Vector3 Position => TransformData.Position;

    public Quaternion Rotation => TransformData.Rotation;

    public  Quaternion LocalRotation => TransformData.LocalRotation;
    private Vector3    _forward;
    public  Vector3    Forward => _forward;

    public int Size => size;

    public TransformComponent() { }

    public TransformComponent(Transform transform, int transformId)
    {
      TransformData.Position      = transform.position;
      TransformData.Rotation      = transform.rotation;
      TransformData.LocalRotation = transform.localRotation;
      TransformData.Scale         = transform.localScale;
      _forward                    = transform.forward;
      TransformId                 = transformId;
    }

    public void SetPosition(Vector3 position)
    {
      HasChanged             =  true;
      DirtyFlags             |= TransformDirtyFlags.Position;
      TransformData.Position =  position;
    }

    public void SetRotation(Quaternion rotation)
    {
      HasChanged             =  true;
      DirtyFlags             |= TransformDirtyFlags.Rotation;
      TransformData.Rotation =  rotation;
      _forward               =  rotation * Vector3.forward;
    }

    public void SetLocalRotation(Quaternion localRotation)
    {
      HasChanged                  =  true;
      DirtyFlags                  |= TransformDirtyFlags.LocalRotation;
      TransformData.LocalRotation =  localRotation;
      //_forward               =  rotation * Vector3.forward;
    }

    public void Serialize(Message message)
    {
      // message.AddInt(TransformId);
      // TransformData.Serialize(message);
    }

    public TransformComponent Deserialize(Message message)
    {
      // TransformId = message.GetInt();
      // TransformData.Deserialize(message);
      return this;
    }

    public void OnFixedUpdate()
    {
      
    }
  }
}

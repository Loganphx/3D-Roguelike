using UnityEngine;

public interface IPlayer
{
  public int       Gold      { get; set; }
  public Transform Transform { get; }
}
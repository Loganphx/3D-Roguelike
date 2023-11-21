using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneNode : MonoBehaviour, IDamagable
{
  public void Hit(IPlayer player)
  {
    Debug.Log("Hit stone node");
  }
}

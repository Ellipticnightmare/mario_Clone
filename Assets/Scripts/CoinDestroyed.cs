using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroyed : MonoBehaviour
{
    public float interval = 1;

  void Start()
  {
    Destroy (gameObject, interval);
  }

  // Update is called once per frame
  void Update()
  {

  }
}

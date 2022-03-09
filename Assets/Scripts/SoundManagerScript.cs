using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip playerHitCoin, jumpSound, playerHitBrick;
    static AudioSource audioSrc;

    void Start()
    {
        playerHitCoin = Resources.Load<AudioClip> ("Coin");
        jumpSound = Resources.Load<AudioClip> ("jump");
        playerHitBrick = Resources.Load<AudioClip> ("HitBrick");

        audioSrc = GetComponent <AudioSource> ();
    }

    public static void PlaySound (string clip)
    {
      switch (clip)
      {
        case "jump":
          audioSrc.PlayOneShot (jumpSound);
          break;
      }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "keyBindingDatabase", menuName = "kbDatabase", order = 0)]
public class keybinDatabase : ScriptableObject
{
    public keyBindingData[] keyBinds;
}
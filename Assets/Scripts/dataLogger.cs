using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dataLogger : MonoBehaviour
{
    public static void throwMessage(string output) => Debug.Log(output);
    public static void throwError(string output) => Debug.Log(output);
}
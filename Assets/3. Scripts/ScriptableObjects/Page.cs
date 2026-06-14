using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Page : ScriptableObject
{
    [TextArea]
    public string text;

    public string part;
}

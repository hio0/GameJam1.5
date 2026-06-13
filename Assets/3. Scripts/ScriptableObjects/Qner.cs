using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Qner : ScriptableObject
{
    public string qnerName;

    public Sprite qnerImage;

    public Question[] myQuestion;

    [TextArea]
    public string[] correctReact;

    [TextArea]
    public string[] wrondReact;
}

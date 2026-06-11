using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Question : ScriptableObject
{
    [TextArea]
    public string Qtext;

    public string answer;

    public string[] wrongAnswer;
}

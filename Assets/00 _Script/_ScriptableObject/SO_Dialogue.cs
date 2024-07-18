using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class SO_Dialogue : ScriptableObject
{
    public Dialogue_Content[] contents;
}

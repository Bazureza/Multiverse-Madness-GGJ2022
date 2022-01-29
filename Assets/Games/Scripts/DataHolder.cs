using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Data Holder", fileName = "Data Holder")]
public class DataHolder : ScriptableObject
{
    public List<string> levelsComplete;
}

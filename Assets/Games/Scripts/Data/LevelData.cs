using UnityEngine;

[CreateAssetMenu(menuName = "Create Level Data", fileName = "Level Data")]
public class LevelData : ScriptableObject
{
    public string level_id;
    public string level_name;
    public string scene_name;
}

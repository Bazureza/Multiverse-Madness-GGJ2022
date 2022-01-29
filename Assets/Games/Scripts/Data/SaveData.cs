using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<string> levelsComplete;

    public void AddLevel(string level_id)
    {
        if (!levelsComplete.Contains(level_id)) levelsComplete.Add(level_id);
    }
}
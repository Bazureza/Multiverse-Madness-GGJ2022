using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    public string GetLevelID()
    {
        return levelData.level_id;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="New Level", fileName ="newLevel")]
public class LevelData : ScriptableObject
{

    public string levelName;
    public int levelNumber;

    public int guarenteeDistance;

    public RoomData spawnRoom;

    public List<roomEntry> rooms = new List<roomEntry>();

}

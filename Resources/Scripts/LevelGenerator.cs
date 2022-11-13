using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public int positionMultiplier = 10;
    public int seed = 1;

    public Dictionary<Vector2, LiveRoom> rooms = new Dictionary<Vector2, LiveRoom>();
    public LevelData level;

    public List<roomEntry> possibleRooms = new List<roomEntry>();

    public bool randomSeed;

    private void Start()
    {
        seed = PlayerPrefs.GetInt("seed");

        if (randomSeed)
        {
            seed = Random.Range(-10000, 10000);
        }

        Random.InitState(seed);

        foreach (roomEntry entry in level.rooms)
        {
            if (entry.maxPerLevel > -1)
            {
            }
            else
            {
                for (int i = 0; i < entry.chance; i++)
                {
                    possibleRooms.Add(entry);
                }
            }
        }

        bool baseZoneGenerated = false;
        Vector2 startingCoords = new Vector2(-level.guarenteeDistance, -level.guarenteeDistance);
        Vector2 currentCoords = startingCoords;

        int maxBound = level.guarenteeDistance;
        
        while (baseZoneGenerated == false)
        {
            GenerateLocation(currentCoords);
            currentCoords.x += 1;

            if (currentCoords.x > maxBound)
            {
                currentCoords.x = -maxBound;
                currentCoords.y += 1;

                if (currentCoords.y > maxBound)
                {
                    baseZoneGenerated = true;
                }
            }
        }

        //Placed rooms required for the level that are required
        for (int i = 0; i < level.rooms.Count; i++)
        {
            print(1);
            if (level.rooms[i].required)
            {
                print(2);
                for (int x = 0; x < level.rooms[i].maxPerLevel; x++)
                {
                    print("Creating Room: " + level.rooms[i].room.name);
                    Vector2 replacePos = new Vector2();
                    bool posValid = false;

                    while (!posValid)
                    {
                        replacePos = new Vector2(Random.Range(-maxBound, maxBound + 1), Random.Range(-maxBound, maxBound + 1));

                        if (level.rooms[i].distanceFromSpawn > -1)
                        {
                            if (Vector2.Distance(replacePos * positionMultiplier, new Vector2(0, 0)) > possibleRooms[i].distanceFromSpawn)
                            {
                                if (rooms.ContainsKey(replacePos))
                                {
                                    if (!rooms[replacePos].unOverrideable)
                                    {
                                        posValid = true;
                                        GenerateLocation(replacePos, level.rooms[i]);
                                    }
                                }
                                else
                                {
                                    posValid = true;
                                    GenerateLocation(replacePos, level.rooms[i]);
                                }
                            }
                        }
                        else
                        {
                            if (replacePos != new Vector2(0, 0))
                            {
                                posValid = true;
                                GenerateLocation(replacePos, level.rooms[i]);
                            }
                        }
                    }
                }
                
            }
        }

    }

    public void GenerateLocation(Vector2 position, roomEntry overrided = null)
    {
        if (rooms.ContainsKey(position))
        {
            if (rooms[position].unOverrideable)
            {
                return;
            }
        }

        if (position == new Vector2(0, 0))
        {
            overrided = new roomEntry
            {
                room = level.spawnRoom
            };
        }

        roomEntry selectedToGenerate = null;
        if (overrided == null)
        {
            bool roomChecks = false;

            while (!roomChecks)
            {
                selectedToGenerate = possibleRooms[Random.Range(0, possibleRooms.Count)];

                if (selectedToGenerate.distanceFromSpawn > -1)
                {
                    //Check if room is far away enought from spawn
                    for (int i = 0; i < 10; i++)
                    {
                        if (Vector2.Distance(position * positionMultiplier, new Vector2(0, 0)) >= selectedToGenerate.distanceFromSpawn)
                        {
                            roomChecks = true;
                            possibleRooms.Remove(selectedToGenerate);
                            i = 10;
                        }
                        else if (i == 9)
                        {
                            roomChecks = false;
                            i = 10;
                        }
                    }
                }
                else
                {
                    roomChecks = true;
                }
            }
        }
        else
        {
            selectedToGenerate = overrided;

            if (rooms.ContainsKey(position))
            {
                Destroy(rooms[position].gameObject);
                rooms.Remove(position);

                if (rooms.ContainsKey(new Vector2(position.x, position.y - 1)))
                {
                    if (rooms[new Vector2(position.x, position.y - 1)].northAllowed)
                    {
                        rooms[new Vector2(position.x, position.y - 1)].DisableConnector(LiveRoom.directionData.North);
                    }
                }
                if (rooms.ContainsKey(new Vector2(position.x, position.y + 1)))
                {
                    if (rooms[new Vector2(position.x, position.y + 1)].southAllowed)
                    {
                        rooms[new Vector2(position.x, position.y + 1)].DisableConnector(LiveRoom.directionData.South);
                    }
                }
                if (rooms.ContainsKey(new Vector2(position.x - 1, position.y)))
                {
                    if (rooms[new Vector2(position.x - 1, position.y)].eastAllowed)
                    {
                        rooms[new Vector2(position.x - 1, position.y)].DisableConnector(LiveRoom.directionData.East);
                    }
                }
                if (rooms.ContainsKey(new Vector2(position.x + 1, position.y)))
                {
                    if (rooms[new Vector2(position.x + 1, position.y)].westAllowed)
                    {
                        rooms[new Vector2(position.x + 1, position.y)].DisableConnector(LiveRoom.directionData.West);
                    }
                }
            }
        }

        LiveRoom newRoom = Instantiate(selectedToGenerate.room.prefab, new Vector3(position.x, 0, position.y) * positionMultiplier, Quaternion.identity).GetComponent<LiveRoom>();
        newRoom.originRoom = selectedToGenerate.room;

        if (selectedToGenerate.required)
        {
            newRoom.unOverrideable = true;
        }

        rooms.Add(position, newRoom);

        //See if current room can have a north connector
        if (newRoom.northAllowed)
        {
            //See if we have a room generated north of the current room
            if (rooms.ContainsKey(new Vector2(position.x, position.y + 1)))
            {
                //If we do, then we check if that room is able to have a conenction to a room to the south of it
                if (rooms[new Vector2(position.x, position.y + 1)].southAllowed)
                {
                    //If so, then we enable the south conenctor of the room above and the north connector of the current room
                    newRoom.EnableConnector(LiveRoom.directionData.North);
                    rooms[new Vector2(position.x, position.y + 1)].EnableConnector(LiveRoom.directionData.South);
                }
            }
        }

        //See if current room can have a south connector
        if (newRoom.southAllowed)
        {
            //See if we have a room generated south of the current room
            if (rooms.ContainsKey(new Vector2(position.x, position.y - 1)))
            {
                //If we do, then we check if that room is able to have a conenction to a room to the north of it
                if (rooms[new Vector2(position.x, position.y - 1)].northAllowed)
                {
                    //If so, then we enable the north conenctor of the room below and the south connector of the current room
                    newRoom.EnableConnector(LiveRoom.directionData.South);
                    rooms[new Vector2(position.x, position.y - 1)].EnableConnector(LiveRoom.directionData.North);
                }
            }
        }

        //See if current room can have a west connector
        if (newRoom.westAllowed)
        {
            //See if we have a room generated west of the current room
            if (rooms.ContainsKey(new Vector2(position.x - 1, position.y)))
            {
                //If we do, then we check if that room is able to have a conenction to a room to the east of it
                if (rooms[new Vector2(position.x - 1, position.y)].eastAllowed)
                {
                    //If so, then we enable the east conenctor of the room west and the west connector of the current room
                    newRoom.EnableConnector(LiveRoom.directionData.West);
                    rooms[new Vector2(position.x - 1, position.y)].EnableConnector(LiveRoom.directionData.East);
                }
            }
        }

        //See if current room can have an east connector
        if (newRoom.eastAllowed)
        {
            //See if we have a room generated east of the current room
            if (rooms.ContainsKey(new Vector2(position.x + 1, position.y)))
            {
                //If we do, then we check if that room is able to have a conenction to a room to the west of it
                if (rooms[new Vector2(position.x + 1, position.y)].westAllowed)
                {
                    //If so, then we enable the west conenctor of the room east and the east connector of the current room
                    newRoom.EnableConnector(LiveRoom.directionData.East);
                    rooms[new Vector2(position.x + 1, position.y)].EnableConnector(LiveRoom.directionData.West);
                }
            }
        }
    }
}

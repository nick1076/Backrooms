using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardsClosure : MonoBehaviour
{
    public LiveRoom.directionData lookDir;
    public LiveRoom.directionData currentlyLooking;
    public LiveRoom parentRoom;

    public List<GameObject> otherClosures = new List<GameObject>();

    public bool inSight;

    private void Update()
    {
        if (GameObject.FindWithTag("Entity.Player.Camera") != null)
        {
            if (GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y >= 315 && GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y <= 360 || GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y <= 45 && GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y >= 0)
            {
                currentlyLooking = LiveRoom.directionData.North;
            }
            if (GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y >= 45 && GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y <= 135)
            {
                currentlyLooking = LiveRoom.directionData.East;
            }
            if (GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y >= 135 && GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y <= 225)
            {
                currentlyLooking = LiveRoom.directionData.South;
            }
            if (GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y >= 225 && GameObject.FindWithTag("Entity.Player.Camera").transform.eulerAngles.y <= 315)
            {
                currentlyLooking = LiveRoom.directionData.West;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 == 1)
        {
            if (currentlyLooking == LiveRoom.directionData.North)
            {
                if (lookDir == LiveRoom.directionData.South)
                {
                    inSight = false;
                }
                else
                {
                    inSight = true;
                }
            }
            if (currentlyLooking == LiveRoom.directionData.South)
            {
                if (lookDir == LiveRoom.directionData.North)
                {
                    inSight = false;
                }
                else
                {
                    inSight = true;
                }
            }
            if (currentlyLooking == LiveRoom.directionData.East)
            {
                if (lookDir == LiveRoom.directionData.West)
                {
                    inSight = false;
                }
                else
                {
                    inSight = true;
                }
            }
            if (currentlyLooking == LiveRoom.directionData.West)
            {
                if (lookDir == LiveRoom.directionData.East)
                {
                    inSight = false;
                }
                else
                {
                    inSight = true;
                }
            }
        }

        if (other.tag == "Entity.Player")
        {
            if (inSight)
            {
                return;
            }
            //If player not looking
            if (lookDir == LiveRoom.directionData.North)
            {
                if (parentRoom.connectorNorth.activeInHierarchy)
                {
                    parentRoom.DisableConnector(lookDir);
                    LevelGenerator generator = GameObject.Find("Generator").GetComponent<LevelGenerator>();
                    Vector2 roomPos = new Vector2(parentRoom.transform.position.x / generator.positionMultiplier, parentRoom.transform.position.z / generator.positionMultiplier);
                    Vector2 targetPos = roomPos;
                    targetPos.y += 1;
                    generator.rooms[targetPos].DisableConnector(LiveRoom.directionData.South);
                }
            }
            if (lookDir == LiveRoom.directionData.South)
            {
                if (parentRoom.connectorSouth.activeInHierarchy)
                {
                    parentRoom.DisableConnector(lookDir);
                    LevelGenerator generator = GameObject.Find("Generator").GetComponent<LevelGenerator>();
                    Vector2 roomPos = new Vector2(parentRoom.transform.position.x / generator.positionMultiplier, parentRoom.transform.position.z / generator.positionMultiplier);
                    Vector2 targetPos = roomPos;
                    targetPos.y -= 1;
                    generator.rooms[targetPos].DisableConnector(LiveRoom.directionData.North);
                }
            }
            if (lookDir == LiveRoom.directionData.East)
            {
                if (parentRoom.connectorEast.activeInHierarchy)
                {
                    parentRoom.DisableConnector(lookDir);
                    LevelGenerator generator = GameObject.Find("Generator").GetComponent<LevelGenerator>();
                    Vector2 roomPos = new Vector2(parentRoom.transform.position.x / generator.positionMultiplier, parentRoom.transform.position.z / generator.positionMultiplier);
                    Vector2 targetPos = roomPos;
                    targetPos.x += 1;
                    generator.rooms[targetPos].DisableConnector(LiveRoom.directionData.West);
                }
            }
            if (lookDir == LiveRoom.directionData.West)
            {
                if (parentRoom.connectorWest.activeInHierarchy)
                {
                    parentRoom.DisableConnector(lookDir);
                    LevelGenerator generator = GameObject.Find("Generator").GetComponent<LevelGenerator>();
                    Vector2 roomPos = new Vector2(parentRoom.transform.position.x / generator.positionMultiplier, parentRoom.transform.position.z / generator.positionMultiplier);
                    Vector2 targetPos = roomPos;
                    targetPos.x -= 1;
                    generator.rooms[targetPos].DisableConnector(LiveRoom.directionData.East);
                }
            }

            for (int i = 0; i < otherClosures.Count; i++)
            {
                if (otherClosures[i] != this.gameObject)
                {
                    Destroy(otherClosures[i]);
                }
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Entity.Player.Camera")
        {
            inSight = false;
        }
    }
}

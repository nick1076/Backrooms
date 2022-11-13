using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveRoom : MonoBehaviour
{
    public GameObject connectorNorth;
    public GameObject connectorSouth;
    public GameObject connectorEast;
    public GameObject connectorWest;

    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;

    public RoomData originRoom;

    public bool unOverrideable;

    public bool connectionsRandom;

    public bool northAllowed;
    public bool southAllowed;
    public bool eastAllowed;
    public bool westAllowed;

    public enum directionData
    {
        North,
        South,
        East,
        West
    };

    private void Awake()
    {
        if (connectionsRandom)
        {
            if (1 == 1)
            {
                int sel = Random.Range(0, 5);

                if (sel == 0)
                {
                    northAllowed = false;
                }
                else if (sel >= 1)
                {
                    northAllowed = true;
                }
            }
            if (1 == 1)
            {
                int sel = Random.Range(0, 5);

                if (sel == 0)
                {
                    southAllowed = false;
                }
                else if (sel >= 1)
                {
                    southAllowed = true;
                }
            }
            if (1 == 1)
            {
                int sel = Random.Range(0, 5);

                if (sel == 0)
                {
                    eastAllowed = false;
                }
                else if (sel >= 1)
                {
                    eastAllowed = true;
                }
            }
            if (1 == 1)
            {
                int sel = Random.Range(0, 5);

                if (sel == 0)
                {
                    westAllowed = false;
                }
                else if (sel >= 1)
                {
                    westAllowed = true;
                }
            }
        }
        else
        {
            northAllowed = originRoom.northConnectionAllowed;
            southAllowed = originRoom.southConnectionAllowed;
            eastAllowed = originRoom.eastConnectionAllowed;
            westAllowed = originRoom.westConnectionAllowed;
        }

        DisableConnector(directionData.North);
        DisableConnector(directionData.South);
        DisableConnector(directionData.East);
        DisableConnector(directionData.West);
    }

    public void EnableConnector(directionData direction)
    {
        if (direction == directionData.North)
        {
            if (connectorNorth != null)
            {
                connectorNorth.SetActive(true);
            }
            if (wallNorth != null)
            {
                wallNorth.SetActive(false);
            }
        }
        if (direction == directionData.South)
        {
            if (connectorSouth != null)
            {
                connectorSouth.SetActive(true);
            }
            if (wallSouth != null)
            {
                wallSouth.SetActive(false);
            }
        }
        if (direction == directionData.East)
        {
            if (connectorEast != null)
            {
                connectorEast.SetActive(true);
            }
            if (wallEast != null)
            {
                wallEast.SetActive(false);
            }
        }
        if (direction == directionData.West)
        {
            if (connectorWest != null)
            {
                connectorWest.SetActive(true);
            }
            if (wallWest != null)
            {
                wallWest.SetActive(false);
            }
        }
    }

    public void DisableConnector(directionData direction)
    {
        if (direction == directionData.North)
        {
            if (connectorNorth != null)
            {
                connectorNorth.SetActive(false);
            }
            if (wallNorth != null)
            {
                wallNorth.SetActive(true);
            }
        }
        if (direction == directionData.South)
        {
            if (connectorSouth != null)
            {
                connectorSouth.SetActive(false);
            }
            if (wallSouth != null)
            {
                wallSouth.SetActive(true);
            }
        }
        if (direction == directionData.East)
        {
            if (connectorEast != null)
            {
                connectorEast.SetActive(false);
            }
            if (wallEast != null)
            {
                wallEast.SetActive(true);
            }
        }
        if (direction == directionData.West)
        {
            if (connectorWest != null)
            {
                connectorWest.SetActive(false);
            }
            if (wallWest != null)
            {
                wallWest.SetActive(true);
            }
        }
    }
}

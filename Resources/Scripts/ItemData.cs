using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newItem", menuName="New Item")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite heldSprite;
    public Sprite uiSprite;
    public GameObject drop;

    public enum contrabandTierData
    {
        Safe,
        Banned,
        Dangerous
    };

    public contrabandTierData contrabandTier;

    public bool hasCreatedObject;
    public GameObject createdObject;
}

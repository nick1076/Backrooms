using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory attachedInventory;
    public InventoryDisplay invenDisplay;
    public SpriteRenderer itemRender;

    public Transform cameraTransform;
    public Transform dropTransform;
    public float reach = 2.0f;
    public float inventorySpread = 5.0f;

    public bool controlingInventory;
    public bool playerControlled = true;
    public bool player = false;

    public GameObject createdItem;

    private ItemData lastRendered;
    private bool dumpedInven;

    public void DumpAllItems()
    {
        dumpedInven = true;
        foreach (Item itemData in attachedInventory.RetrieveCurrentInventory())
        {
            Item potentialDrop = new Item
            {
                item = itemData.item,
                data = itemData.data
            };

            GameObject dropped = Instantiate(potentialDrop.item.drop, new Vector3(Random.Range(this.transform.position.x - inventorySpread, this.transform.position.x + inventorySpread), this.transform.position.y, Random.Range(this.transform.position.z - inventorySpread, this.transform.position.z + inventorySpread)), Quaternion.identity);
            dropped.GetComponent<PhysicalItem>().item = potentialDrop;
        }
        attachedInventory.ClearInventory();
    }

    public void UpdateItemRendering()
    {

        if (attachedInventory == null)
        {
            return;
        }

        if (player)
        {
            if (itemRender != null)
            {
                if (attachedInventory.RetrieveCurrentlySelected().item != null)
                {
                    if (lastRendered != null)
                    {
                        if (attachedInventory.RetrieveCurrentlySelected().item != lastRendered)
                        {
                            lastRendered = attachedInventory.RetrieveCurrentlySelected().item;
                            if (createdItem != null)
                            {
                                Destroy(createdItem);
                            }
                            if (attachedInventory.RetrieveCurrentlySelected().item.hasCreatedObject)
                            {
                                itemRender.sprite = null;
                                createdItem = Instantiate(attachedInventory.RetrieveCurrentlySelected().item.createdObject, itemRender.transform.position, itemRender.transform.rotation, itemRender.transform);
                            }
                            else
                            {
                                itemRender.sprite = attachedInventory.RetrieveCurrentlySelected().item.heldSprite;
                            }
                        }
                    }
                    else
                    {
                        lastRendered = attachedInventory.RetrieveCurrentlySelected().item;
                        if (createdItem != null)
                        {
                            Destroy(createdItem);
                        }
                        if (attachedInventory.RetrieveCurrentlySelected().item.hasCreatedObject)
                        {
                            itemRender.sprite = null;
                            createdItem = Instantiate(attachedInventory.RetrieveCurrentlySelected().item.createdObject, itemRender.transform.position, itemRender.transform.rotation, itemRender.transform);
                        }
                        else
                        {
                            itemRender.sprite = attachedInventory.RetrieveCurrentlySelected().item.heldSprite;
                        }
                    }
                }
                else
                {
                    if (createdItem != null)
                    {
                        Destroy(createdItem);
                    }
                    itemRender.sprite = null;
                    lastRendered = null;
                }
            }
        }
        else
        {
            GameObject oneToCreate = null;

            if (!dumpedInven)
            {
                if (attachedInventory.RetrieveCurrentInventory()[0].item != null)
                {
                    if (attachedInventory.RetrieveCurrentInventory().Count > 0)
                    {
                    }
                }
            }

            if (oneToCreate != null)
            {
                if (itemRender != null)
                {
                    if (attachedInventory.RetrieveCurrentlySelected().item != null)
                    {
                        if (lastRendered != null)
                        {
                            if (attachedInventory.RetrieveCurrentlySelected().item != lastRendered)
                            {
                                lastRendered = attachedInventory.RetrieveCurrentlySelected().item;
                                if (createdItem != null)
                                {
                                    Destroy(createdItem);
                                }
                                if (attachedInventory.RetrieveCurrentlySelected().item.hasCreatedObject)
                                {
                                    itemRender.sprite = null;
                                    createdItem = Instantiate(oneToCreate, itemRender.transform.position, itemRender.transform.rotation, itemRender.transform);
                                }
                                else
                                {
                                    itemRender.sprite = attachedInventory.RetrieveCurrentlySelected().item.heldSprite;
                                }
                            }
                        }
                        else
                        {
                            lastRendered = attachedInventory.RetrieveCurrentlySelected().item;
                            if (createdItem != null)
                            {
                                Destroy(createdItem);
                            }
                            if (attachedInventory.RetrieveCurrentlySelected().item.hasCreatedObject)
                            {
                                itemRender.sprite = null;
                                createdItem = Instantiate(oneToCreate, itemRender.transform.position, itemRender.transform.rotation, itemRender.transform);
                            }
                            else
                            {
                                itemRender.sprite = attachedInventory.RetrieveCurrentlySelected().item.heldSprite;
                            }
                        }
                    }
                    else
                    {
                        if (createdItem != null)
                        {
                            Destroy(createdItem);
                        }
                        itemRender.sprite = null;
                        lastRendered = null;
                    }
                }
            }
            else
            {
                if (createdItem != null)
                {
                    Destroy(createdItem);
                }
                if (itemRender != null)
                {
                    itemRender.sprite = null;
                }

                lastRendered = null;
            }

            if (lastRendered != null && attachedInventory.RetrieveCurrentInventory()[0].item == null)
            {
                lastRendered = null;
            }

        }
    }

    void Update()
    {
        UpdateItemRendering();

        if (!controlingInventory)
        {
            return;
        }


        if (playerControlled)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                attachedInventory.SelectItem(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                attachedInventory.SelectItem(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                attachedInventory.SelectItem(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                attachedInventory.SelectItem(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                attachedInventory.SelectItem(4);
            }

            if (Input.GetKeyDown(KeyCode.G) && attachedInventory.RetrieveCurrentlySelected() != null)
            {
                if (attachedInventory.RetrieveCurrentlySelected() != null)
                {
                    Item potentialDrop = new Item
                    {
                        item = attachedInventory.RetrieveCurrentlySelected().item,
                        data = attachedInventory.RetrieveCurrentlySelected().data
                    };

                    if (attachedInventory.RemoveItem(attachedInventory.RetrieveCurrentlySelected().item) == true)
                    {
                        if (potentialDrop.item.drop != null)
                        {
                            GameObject dropped = Instantiate(potentialDrop.item.drop, dropTransform.position, Quaternion.identity);
                            dropped.GetComponent<PhysicalItem>().item = potentialDrop;
                        }
                    }
                }
            }

            if (cameraTransform == null)
            {
                return;
            }

            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            RaycastHit hitForward;
            if (Physics.Raycast(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward), out hitForward, Mathf.Infinity, layerMask))
            {
                if (hitForward.distance <= this.reach)
                {
                    Debug.DrawRay(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward) * 1000, Color.green);
                    if (hitForward.collider.gameObject.GetComponent<PhysicalItem>() != null)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse1) && attachedInventory.RetrieveCurrentlySelected().item == null)
                        {
                            if (attachedInventory.AddItem(hitForward.collider.gameObject.GetComponent<PhysicalItem>().item) == true)
                            {
                                Destroy(hitForward.collider.gameObject);
                            }
                        }
                    }
                }
                else
                {
                    Debug.DrawRay(cameraTransform.transform.position, cameraTransform.transform.TransformDirection(Vector3.forward) * 1000, Color.red);
                }
            }
        }
    }
}

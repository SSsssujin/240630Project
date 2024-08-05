using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    public void Open()
    {
        transform.Find("Closed").gameObject.SetActive(false);
        transform.Find("Open").gameObject.SetActive(true);
    }
    
    public void Close()
    {
        transform.Find("Closed").gameObject.SetActive(true);
        transform.Find("Open").gameObject.SetActive(false);
    }
}

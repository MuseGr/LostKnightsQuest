using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGfollow : MonoBehaviour
{
    public GameObject mainCamera;

    private void Update()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 10);
    }
}

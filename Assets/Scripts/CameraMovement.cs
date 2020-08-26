using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerCar;
    public GameManager gameManager;

    private void Update()
    {
        transform.position = playerCar.transform.position + new Vector3(0, -0.1f, -25);
    }

    public void AssingCar(GameObject x)
    {
        playerCar = x;
        gameManager.playerCar = x;
    }
}

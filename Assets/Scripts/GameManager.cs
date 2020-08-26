using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerCar;
    public DrawBoard drawBoard;
    public int[] checkPoints;

    Vector3 checkpointPos = new Vector3();
    Quaternion checkpointRot = new Quaternion();   
    
    //Will be finish routine
    public IEnumerator FinishRoutine()
    {
        yield return new WaitForSeconds(0f);
    }

    //Sets new checkpoint
    public void SetCheckpointTra()
    {
        checkpointPos = playerCar.transform.position;
        checkpointRot = playerCar.transform.rotation;
    }
    
    //Spawns car
    public void SpawnFallingCar()
    {
        drawBoard.SpawnCar(checkpointPos,checkpointRot);
    }
}

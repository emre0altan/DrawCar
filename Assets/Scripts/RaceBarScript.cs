using UnityEngine;

public class RaceBarScript : MonoBehaviour
{
    public RectTransform playerKnob;
    public Transform finishLine;
    public GameManager gameManager;
    public Transform playerTra;

    private float barSize = 900;
    private float finishX;
    private float tmp = 0f;

    private void Start()
    {
        finishX = finishLine.position.x;
    }

    private void Update()
    {
        
        tmp = Mathf.Lerp(0, barSize, playerTra.position.x / finishLine.position.x);
        playerKnob.anchoredPosition = new Vector3(tmp, 0, 0);
        
    }
}

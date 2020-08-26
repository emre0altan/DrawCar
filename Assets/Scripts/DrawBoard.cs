using UnityEngine;
using UnityEngine.EventSystems;


public class DrawBoard : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform leadTra;
    public TrailRenderer draw;
    public MeshFilter testMesh;
    public float scale = 1;
    public GameObject wheelPrefab;
    public GameObject carRoot;
    public WheelCollider frontWheelCollider, rearWheelCollider;
    public GameObject frontWheel, rearWheel;
    public CameraMovement myCamera;
    public RaceBarScript raceBarScript;
    

    private bool m_Pressed = false,m_Dragged = false;
    private Mesh createdMesh;
    private Vector3[] baseVertices;
    private int[] baseTriangles;
    private float dragTimer = 0f;
    
    

    private void Start()
    {
        createdMesh = new Mesh();
        myCamera.AssingCar(carRoot);
    }

    private void Update()
    {
        if (m_Pressed)
            dragTimer += Time.deltaTime;
        
    }
    //Scales created mesh
    void UpdateVertices()
    {
        baseVertices = createdMesh.vertices;
        Vector3[] vertices = new Vector3[baseVertices.Length];

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            vertex.x *= scale;
            vertex.y *= scale;
            vertex.z *= scale;
            vertices[i] = vertex;
        }

        createdMesh.vertices = vertices;
        createdMesh.RecalculateNormals();
        createdMesh.RecalculateBounds();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_Pressed)
            return;

        m_Pressed = true;
        draw.emitting = true;

        if (eventData.position.x > Screen.width / 10 && eventData.position.x < Screen.width * 0.9f &&
            eventData.position.y > Screen.height / 50 && eventData.position.y < Screen.height * 0.35f)
        {
            leadTra.anchoredPosition3D = new Vector3(eventData.position.x-100, eventData.position.y-30, -51);
        }
        draw.Clear();
        dragTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!m_Pressed || !m_Dragged)
            return;
        

        if(dragTimer > 0.05f)
            CreateMesh();

        dragTimer = 0f;
        draw.emitting = false;
        m_Pressed = false;
        m_Dragged = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_Dragged = true;

        if (eventData.position.x -100> Screen.width*0.05f && eventData.position.x -100< Screen.width*0.75f && 
            eventData.position.y-30 > Screen.height*0.03f && eventData.position.y-30 < Screen.height*0.32f)
        {
            leadTra.anchoredPosition3D = new Vector3(eventData.position.x-100, eventData.position.y-30, -51);
        }
    }
    //Creates drawed car to a mesh 
    void CreateMesh()
    {
        Mesh tmpmesh = new Mesh();
        draw.BakeMesh(tmpmesh, Camera.main, true);
        if (tmpmesh.vertexCount < 7) return;
        createdMesh = tmpmesh;

        if (testMesh.GetComponent<MeshCollider>() != null)
            Destroy(testMesh.GetComponent<MeshCollider>());

        UpdateVertices();
        baseVertices = createdMesh.vertices;
        baseTriangles = createdMesh.triangles;

        Vector3[] addedVertices = new Vector3[baseVertices.Length * 2];
        int[] addedTriangles = new int[baseTriangles.Length * 2 + baseTriangles.Length*6 +12];
        for(int i = 0; i < addedVertices.Length; i++)
        {
            addedVertices[i] = baseVertices[i % baseVertices.Length];
            if(i >= baseVertices.Length)
            {
                addedVertices[i].z += 0.2f;
            }
        }

        int[] upTriang = new int[baseTriangles.Length * 3], lowTriang = new int[baseTriangles.Length *3];
        for (int i = 0; i < baseTriangles.Length*4; i++)
        {
            if (i < baseTriangles.Length) addedTriangles[i] = baseTriangles[i];
            else if(i < baseTriangles.Length*2) addedTriangles[i] = baseTriangles[i % baseTriangles.Length] + baseVertices.Length;
            else
            {    
                upTriang[i - baseTriangles.Length * 2] = baseTriangles[(i) % baseTriangles.Length];
                upTriang[i + 1 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length];
                upTriang[i + 2 - baseTriangles.Length * 2] = baseTriangles[(i) % baseTriangles.Length] + baseVertices.Length;

                upTriang[i + 3 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length] + baseVertices.Length;
                upTriang[i + 4 - baseTriangles.Length * 2] = baseTriangles[i % baseTriangles.Length] + baseVertices.Length;
                upTriang[i + 5 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length];

                lowTriang[i - baseTriangles.Length * 2] = baseTriangles[(i + 1) % baseTriangles.Length] + baseVertices.Length;
                lowTriang[i + 1 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length] + 1;
                lowTriang[i + 2 - baseTriangles.Length * 2] = baseTriangles[(i + 1) % baseTriangles.Length];

                lowTriang[i + 3 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length] + 1;
                lowTriang[i + 4 - baseTriangles.Length * 2] = baseTriangles[(i + 1) % baseTriangles.Length] + baseVertices.Length;
                lowTriang[i + 5 - baseTriangles.Length * 2] = baseTriangles[(i + 2) % baseTriangles.Length] + baseVertices.Length + 1;

                i += 5; 
            }
        }    
        for(int i = baseTriangles.Length * 2; i < baseTriangles.Length * 5; i++)
        {
            addedTriangles[i] = upTriang[i - baseTriangles.Length * 2];
        }

        for (int i = baseTriangles.Length * 5; i < baseTriangles.Length * 8; i++)
        {
            addedTriangles[i] = lowTriang[i - baseTriangles.Length * 5];
        }

        int tmp = addedTriangles.Length;
        addedTriangles[tmp - 12] = addedTriangles[baseTriangles.Length];
        addedTriangles[tmp - 11] = baseTriangles[1];
        addedTriangles[tmp - 10] = baseTriangles[0];
        addedTriangles[tmp - 9] = baseTriangles[1];
        addedTriangles[tmp - 8] = addedTriangles[baseTriangles.Length];
        addedTriangles[tmp - 7] = addedTriangles[1 + baseTriangles.Length];
        addedTriangles[tmp - 6] = baseTriangles[baseTriangles.Length - 3];
        addedTriangles[tmp - 5] = baseTriangles[baseTriangles.Length - 1];
        addedTriangles[tmp - 4] = addedTriangles[baseTriangles.Length * 2 - 3];
        addedTriangles[tmp - 3] = addedTriangles[baseTriangles.Length * 2 - 1];
        addedTriangles[tmp - 2] = addedTriangles[baseTriangles.Length * 2 - 3];
        addedTriangles[tmp - 1] = baseTriangles[baseTriangles.Length - 1];

        int tmp1;
        for (int i = baseTriangles.Length; i < baseTriangles.Length*2; i += 3)
        {
            tmp1 = addedTriangles[i + 2];
            addedTriangles[i + 2] = addedTriangles[i];
            addedTriangles[i] = tmp1;
        }

        createdMesh.vertices = addedVertices;
        createdMesh.triangles = addedTriangles;
        createdMesh.RecalculateBounds();

        Vector3 dif = -createdMesh.bounds.center;
        for (int i = 0; i < addedVertices.Length; i++)
        {
            addedVertices[i] = addedVertices[i] + dif;
        }       

        createdMesh.vertices = addedVertices;    
        createdMesh.RecalculateNormals();
        createdMesh.RecalculateBounds();

        testMesh.transform.position = new Vector3(-createdMesh.bounds.center.x, -createdMesh.bounds.center.y, -createdMesh.bounds.center.z);
        testMesh.transform.rotation = Quaternion.Euler(0, testMesh.transform.rotation.y, 0);

        frontWheel.transform.position = (createdMesh.vertices[0] + createdMesh.vertices[1] + createdMesh.vertices[baseVertices.Length] + createdMesh.vertices[1+baseVertices.Length])/4;
        rearWheel.transform.position = (createdMesh.vertices[baseVertices.Length-1] + createdMesh.vertices[baseVertices.Length - 2] + createdMesh.vertices[baseVertices.Length*2-1] + createdMesh.vertices[baseVertices.Length*2-2])/4;

        testMesh.mesh = createdMesh;
        MeshCollider x = testMesh.gameObject.AddComponent<MeshCollider>();
        x.convex = true;
        x.sharedMesh = createdMesh;

        ResetCar();     
        draw.Clear();
        carRoot.SetActive(true);
    }

    //Creates new object with rigidbody and make paren to car objects
    private void ResetCar()
    {
        frontWheelCollider.GetComponent<WheelCollider>().enabled = false;
        rearWheelCollider.GetComponent<WheelCollider>().enabled = false;
        carRoot.transform.DetachChildren();
        Vector3 tmpPos = new Vector3(carRoot.transform.position.x, carRoot.transform.position.y+0.5f,0);

        Destroy(carRoot);

        carRoot = new GameObject();
        carRoot.transform.position = tmpPos;
        carRoot.transform.rotation *= Quaternion.Euler(0, 90, 0);
        carRoot.transform.localScale = new Vector3(1, 1, -1);
        myCamera.AssingCar(carRoot);
        testMesh.transform.parent = carRoot.transform;
        testMesh.transform.localPosition = Vector3.zero;
        testMesh.transform.localRotation = Quaternion.Euler(0, 90, 0);

        Rigidbody newRigidbody = carRoot.AddComponent<Rigidbody>();
        newRigidbody.mass = createdMesh.vertexCount * 20;
        newRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;        
        newRigidbody.ResetCenterOfMass();
        newRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        frontWheelCollider.GetComponent<WheelCollider>().enabled = true;
        rearWheelCollider.GetComponent<WheelCollider>().enabled = true;
    }
    
    //Spawns car in related position
    public void SpawnCar(Vector3 pos, Quaternion rot)
    {
        carRoot.SetActive(false);
        carRoot.transform.position = pos;
        carRoot.transform.rotation = rot;
        carRoot.SetActive(true);
    }
    
}

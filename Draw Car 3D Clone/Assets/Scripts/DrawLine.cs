using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

//Added on Panel in Canvas
public class DrawLine : MonoBehaviour
{
    //Access other scripts
    private GenerateShape generateShapeScript;
    private GameManager gameManagerScript;

    public GameObject lineCreatorPrefab; //it draws line on canvas, and crate path

    public GameObject linePrefab; //dot prefab for drawing on canvas
    public GameObject currentLine; //total line, gets deleted after darawing

    public LineRenderer lineRenderer;
    public List<Vector2> touchPositions;

    private Camera orthogonalCamera; //shows lines on canvas in 2D, DrawLine layer selected

    //private Transform car;

    private bool isPointerOnCanvas = false;

    private void Awake()
    {
        gameManagerScript = Camera.main.GetComponent<GameManager>();
        orthogonalCamera = GameObject.FindGameObjectWithTag("DrawLine").GetComponent<Camera>();
        generateShapeScript = GameObject.FindGameObjectWithTag("Car").GetComponent<GenerateShape>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPointerOnCanvas) //only drawn in rectangle area
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateLine();                           
            }
            if (Input.GetMouseButton(0))
            {
                Vector2 touchPosition = orthogonalCamera.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(touchPosition, touchPositions[touchPositions.Count - 1]) > 0.1f)
                {
                    UpdateLine(touchPosition);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                gameManagerScript.GamePlay = true; //car falls down
                
                generateShapeScript.AddDrawingObjectsToPool();

                GameObject lineCreator = Instantiate(lineCreatorPrefab, Vector3.zero, Quaternion.identity);
                lineCreator.GetComponent<GeneratePath>().CreatePath(touchPositions);

                DestroyLine();
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                DestroyLine();
            }
        }
        
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        touchPositions.Clear();
        touchPositions.Add(orthogonalCamera.ScreenToWorldPoint(Input.mousePosition));
        touchPositions.Add(orthogonalCamera.ScreenToWorldPoint(Input.mousePosition));
        lineRenderer.SetPosition(0, touchPositions[0]);
        lineRenderer.SetPosition(1, touchPositions[1]);
    }

    void UpdateLine(Vector2 touchPosition)
    {
        touchPositions.Add(touchPosition);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, touchPosition);
    }

    public void SetIsPointerOnCanvas(bool value) //Event Trigger checks pointer
    {
        isPointerOnCanvas = value;
    }
    void DestroyLine() //lines deleted after drawing
    {
        if (lineRenderer != null)
            Destroy(currentLine);
    }
}
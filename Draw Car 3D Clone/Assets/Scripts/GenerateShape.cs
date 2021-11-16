using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

//Added on Car GameObject
//Genearates shapes of car
public class GenerateShape : MonoBehaviour
{
    public GameObject drawingPrefab;//drawing parts

    private Transform lineHolder;
    public Transform leftWheel;
    public Transform rightWheel;
    public Transform centerMass;

    private Transform car;
    private float carOffsetY = 1f;//instantiating

    private float space = 0.1f;//intervals of shapes

    private PathCreator pathCreator;
    private GameObject lineCreator;

    //Active shapes
    private List<Transform> drawingObjectActiveList = new List<Transform>();
    //Object Pooling shapes
    private List<Transform> drawingObjectPool = new List<Transform>();

    private void Awake()
    {
        car = GameObject.FindGameObjectWithTag("Car").transform;
        lineHolder = GameObject.FindGameObjectWithTag("LineHolder").transform;
    }
    public void Generate()
    {
        //They are always instantiated before drawing and then removed 
        lineCreator = GameObject.FindGameObjectWithTag("LineCreator");
        pathCreator = GameObject.FindGameObjectWithTag("LineCreator").GetComponent<PathCreator>();
    

        VertexPath path = pathCreator.path;

        float distance = 0;

        while (distance < path.length)
        {
           Vector3 point = path.GetPointAtDistance(distance);
           Quaternion rotation = path.GetRotationAtDistance(distance);
           
           if (drawingObjectPool.Count == 0)
           {
                GameObject drawingObject = Instantiate(drawingPrefab, point, rotation, lineHolder.transform);
                drawingObject.transform.localPosition = point;
                drawingObjectActiveList.Add(drawingObject.transform);

            }
           else
           {
               Transform drawingObject = drawingObjectPool[drawingObjectPool.Count - 1];
               drawingObjectPool.RemoveAt(drawingObjectPool.Count - 1);
               drawingObjectActiveList.Add(drawingObject.transform);
               drawingObject.localPosition = point;
               drawingObject.localRotation = rotation;
           
               drawingObject.gameObject.SetActive(true);
            }
            distance += space;
        }

        leftWheel.localPosition = path.GetPointAtDistance(0);
        rightWheel.localPosition = path.GetPointAtDistance(distance-space);      
        centerMass.position = (leftWheel.position + rightWheel.position) / 2f;
        centerMass.localScale = new Vector3(centerMass.localScale.x, centerMass.localScale.y, Mathf.Abs(leftWheel.position.x - rightWheel.position.x));

        float angle = AngleInDeg(leftWheel.position, rightWheel.position);
        centerMass.eulerAngles = new Vector3(angle, -90, 0);
        car.position = new Vector3(car.position.x, car.position.y + carOffsetY, 0f);

        Destroy(lineCreator);//It deletes line creator to delete old paths
    }

    public void AddDrawingObjectsToPool(){

        while (drawingObjectActiveList.Count != 0)
        {
            
            Transform drawingObject = drawingObjectActiveList[drawingObjectActiveList.Count - 1];
            drawingObject.gameObject.SetActive(false);
            drawingObjectActiveList.RemoveAt(drawingObjectActiveList.Count-1);

            drawingObjectPool.Add(drawingObject);
        }
    }

    //They are used for the calculations of the angle of CenterMass

    //This returns the angle in radians
    private static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }
    //This returns the angle in degrees
    private static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Added on Line Creator prefab
//Works with PathCreator.cs
namespace PathCreation
{
    [RequireComponent(typeof(PathCreator))]
    public class GeneratePath : MonoBehaviour
    {
        //Access other script
        private GenerateShape generateShapeScript;

        public bool closedLoop = false; //path loop

        public List<Vector3> wayPoints =new List<Vector3>(); //path points

        private float offsetY = 4.6f;

        private int decreasePointsRate = 4; //number of points divided by

        private void Awake()
        {
            generateShapeScript = GameObject.FindGameObjectWithTag("Car").GetComponent<GenerateShape>();
        }
        public void CreatePath(List<Vector2> points2d)
        {
            List<Vector3> points3 = new List<Vector3>();
            int averageNumberOfPoints = points2d.Count / decreasePointsRate;// division by rate because of too much points
            for(int i=0; i<points2d.Count; i+=averageNumberOfPoints)
            {
                Vector3 point3d = new Vector3(points2d[i].x, points2d[i].y+offsetY,0);
                wayPoints.Add(point3d);


                if(i >=  points2d.Count - averageNumberOfPoints)
                {
                    if(i != points2d.Count - 1)
                    {
                        i = points2d.Count - 1 - averageNumberOfPoints;
                    }
                }
            }
            if (wayPoints.Count > 0)
            {
                // Create a new bezier path from the waypoints.
                BezierPath bezierPath = new BezierPath(wayPoints, closedLoop, PathSpace.xyz);               
                GetComponent<PathCreator>().bezierPath = bezierPath;
            }

            generateShapeScript.Generate();
            GameObject.FindGameObjectWithTag("Car").GetComponent<Rigidbody>().useGravity = true;

        }
    }
}


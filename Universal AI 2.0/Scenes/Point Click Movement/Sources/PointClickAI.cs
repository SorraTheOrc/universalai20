using UnityEngine;

namespace UniversalAI
{
    public class PointClickAI : MonoBehaviour
    {
        public Transform FollowObj;
        public Transform Point;
        public UniversalAISystem System;
        public bool OnlySmoothFollow = false;
        public Vector3 Offset = new Vector3(0, 6.5f, -9.4f);
        Vector3 NewPos;
        void Update()
        {
            Vector3 Vel = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, FollowObj.position + Offset, ref Vel, 0.065f);

            if(OnlySmoothFollow)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray;
                RaycastHit hit;
                ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 30))
                {
                    if (hit.transform != null)
                    {
                        Point.gameObject.SetActive(true);
                        Point.transform.position = hit.point;
                        System.UniversalAICommandManager.SetDestination(hit.point);
                    }
                }
            }

            if (Point.gameObject.activeSelf)
            {
                if (!System.OvverideWandering)
                {
                    Point.gameObject.SetActive(false);
                }
            }
        }
    }
    
}
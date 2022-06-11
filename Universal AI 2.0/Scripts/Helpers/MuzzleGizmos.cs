using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleGizmos : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        ForGizmo(transform.position, transform.forward, 0.1f, 20, 0.2f);
    }

    public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
    {
        ForGizmo(pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle, arrowPosition);
    }
 
    public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);
        DrawArrowEnd(true, pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
    }
    
    private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
    {
        Vector3 right = (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
        Vector3 left = (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
        Vector3 up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;
        Vector3 down = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;
 
        Vector3 arrowTip = pos + (direction*arrowPosition);
 
        if (gizmos)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(arrowTip, right);
            Gizmos.DrawRay(arrowTip, left);
            Gizmos.DrawRay(arrowTip, up);
            Gizmos.DrawRay(arrowTip, down);
        }
        else {
            Debug.DrawRay(arrowTip, right, color);
            Debug.DrawRay(arrowTip, left, color);
            Debug.DrawRay(arrowTip, up, color);
            Debug.DrawRay(arrowTip, down, color);
        }
    }
}

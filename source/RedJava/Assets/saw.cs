using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
public class saw : MonoBehaviour
{
    GameObject[] path;
    bool control = true; // aradaki mesafe bir kere çalışsın
    bool direction = true; // gidilecek yön 
    Vector3 distance;
    int count = 0;


    void Start()
    {
        path = new GameObject[transform.childCount];
        for(int i = 0; i < path.Length; i++)
        {
            path[i] = transform.GetChild(0).gameObject;
            path[i].transform.SetParent(transform.parent);
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
        goOnPath();
    }
    void goOnPath()
    {
        if (control)
        {
            distance = (path[count].transform.position - transform.position).normalized;
            control = false;
        }
        float a = Vector3.Distance(transform.position, path[count].transform.position);
        transform.position += distance * Time.deltaTime * 5;
        if(a < 0.5f)
        {
            control = true;
            if(count == path.Length - 1)
            {
                direction = false;
            }
            else if(count == 0)
            {
                direction = true;
            }
            if (direction)
            {
                count++;
            }
            else
            {
                count--;
            }
        }

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 0.3f);
        }
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor (typeof(saw))]
[System.Serializable]

class sawEditor : Editor
{
    public override void OnInspectorGUI()
    {
        saw script = (saw)target;
        if(GUILayout.Button("CreateObject", GUILayout.MinWidth(100), GUILayout.Width(100)))
        {
            GameObject newObject = new GameObject();
            newObject.transform.parent = script.transform;
            newObject.transform.position = script.transform.position;
            newObject.name = script.transform.childCount.ToString();
        }

    }
}
#endif
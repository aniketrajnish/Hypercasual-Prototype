using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class waypointManagerWindow : EditorWindow{
 
    [MenuItem("Tools/waypoints Editor")]

    public static void Open(){
        GetWindow<waypointManagerWindow>();
    }

    public Transform waypointRoot;

    void OnGUI(){
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if(waypointRoot == null){
            EditorGUILayout.HelpBox("transform not assigned",MessageType.Warning);
        }
        else{
            EditorGUILayout.BeginVertical("Box");
            DrawButtons();
            EditorGUILayout.EndVertical();

        }

        obj.ApplyModifiedProperties();
    }

    void DrawButtons(){
        if(GUILayout.Button("Create waypoint")){
            createWaypoint();
        }


        if(Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<waypoint>()){
            if(GUILayout.Button("Create Waypoint before")){
                createWaypointBefore();
            }
            
            if(GUILayout.Button("Create Waypoint after")){
                createWaypointAfter();
            }
            if(GUILayout.Button("remove Waypoint")){
                removeWaypoint();
            }

        }
    }

    void createWaypointBefore(){
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount , typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot , false);
        
        waypoint newWaypoint = waypointObject.GetComponent<waypoint>();

        waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if(selectedWaypoint.previousWaypoint != null){
            newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
        }

        newWaypoint.nextWaypoint = selectedWaypoint;
        selectedWaypoint.previousWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void createWaypointAfter(){
        GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount , typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot , false);
        
        waypoint newWaypoint = waypointObject.GetComponent<waypoint>();

        waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        newWaypoint.previousWaypoint = selectedWaypoint;

        if(selectedWaypoint.nextWaypoint != null){
            selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
            newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        }

        
        
        selectedWaypoint.nextWaypoint = newWaypoint;

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = newWaypoint.gameObject;
    }

    void removeWaypoint(){
        waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<waypoint>();
        if(selectedWaypoint.nextWaypoint != null){
            selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
        }
        if(selectedWaypoint.previousWaypoint != null){
            selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
        }
        DestroyImmediate(selectedWaypoint.gameObject);
    }

    void createWaypoint(){
        GameObject waypointObject = new GameObject("waypoint "+ waypointRoot.childCount,typeof(waypoint));
        waypointObject.transform.SetParent(waypointRoot,false);

        waypoint waypoint = waypointObject.GetComponent<waypoint>();
        if(waypointRoot.childCount > 1){
            waypoint.previousWaypoint = waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<waypoint>();
            waypoint.previousWaypoint.nextWaypoint = waypoint;
 
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;

    }


}

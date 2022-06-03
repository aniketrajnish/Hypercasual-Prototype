using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class generateWaypoints : EditorWindow{

    [MenuItem("Tools/waypoints Generator")]

    public static void Open(){
        GetWindow<generateWaypoints>();
    }

    public Transform RootObject;
    private Transform[] nodes;
    public int nodeAmount = 0;

    private bool rev = false;
    void OnGUI(){
        SerializedObject obj = new SerializedObject(this);
        SerializedObject nodesArray = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("RootObject"));
        EditorGUILayout.PropertyField(nodesArray.FindProperty("nodeAmount"));

        if(nodes != null)
        nodeAmount = nodes.Length;

        if(RootObject == null){
            EditorGUILayout.HelpBox("transform not assigned",MessageType.Warning);
        }
        else{
            EditorGUILayout.BeginVertical("Box");
            DrawButtons();
            initializeArray();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    void DrawButtons(){
        if(GUILayout.Button("generate Pedestrian Node")){
            generatePedestrianNode();
        }
        if(GUILayout.Button("generate Vehicle Node")){
            generateVehicleNode();
        }

        if(GUILayout.Button("assign Pedestrian loop Node")){
            assingLoopNode();
        }

        if(GUILayout.Button("assign open Node")){
            assingNodeOpen();
        }

        if(GUILayout.Button("assign Vehicle open Node")){
            assingVehicleNodeOpen();
        }

        if(GUILayout.Button("reverse CarNodes array")){
            reverseCarNode();
        }

    }

    void reverseCarNode(){
        for (int i = 0; i < nodes.Length; i++){

            carNode e = nodes[i].GetComponent<carNode>();
            if(e != null && e.nextWaypoint != null && e.previousWaypoint != null){
                carNode tmp = e.nextWaypoint;
                e.nextWaypoint = e.previousWaypoint;
                e.previousWaypoint = tmp;
            }
        }
        rev = rev ? false : true;
    }

    void generatePedestrianNode(){
        
        for (int i = 0; i < nodes.Length; i++){

            MeshRenderer mesh = nodes[i].GetComponent<MeshRenderer>();
            if(mesh != null) DestroyImmediate(nodes[i].GetComponent<MeshRenderer>());

            MeshFilter filter = nodes[i].GetComponent<MeshFilter>();
            if(filter != null) DestroyImmediate(nodes[i].GetComponent<MeshFilter>());

            nodes[i].gameObject.tag = "pedestrianNode";

            waypoint w = nodes[i].GetComponent<waypoint>();
            if(w == null){
                nodes[i].gameObject.AddComponent<waypoint>();
            }
        }
    }

    void generateVehicleNode(){
        
        for (int i = 0; i < nodes.Length; i++){

            MeshRenderer mesh = nodes[i].GetComponent<MeshRenderer>();
            if(mesh != null) DestroyImmediate(nodes[i].GetComponent<MeshRenderer>());

            MeshFilter filter = nodes[i].GetComponent<MeshFilter>();
            if(filter != null) DestroyImmediate(nodes[i].GetComponent<MeshFilter>());

            //nodes[i].gameObject.tag = "pedestrianNode";

            carNode w = nodes[i].GetComponent<carNode>();
            if(w == null){
                nodes[i].gameObject.AddComponent<carNode>();
            }
        }
    }

    void assingLoopNode(){
        for (int i = 0; i < nodes.Length; i++){
            waypoint w = nodes[i].GetComponent<waypoint>();
            if(w != null){
                nodes[i].gameObject.GetComponent<waypoint>().previousWaypoint = (i == 0)? nodes[nodes.Length-1].GetComponent<waypoint>() : nodes[i-1].GetComponent<waypoint>();
                nodes[i].gameObject.GetComponent<waypoint>().nextWaypoint = (i == nodes.Length-1)? nodes[0].GetComponent<waypoint>() : nodes[i+1].GetComponent<waypoint>();
            }
        }
    }

    void assingNodeOpen(){
        for (int i = 0; i < nodes.Length; i++){
            waypoint w = nodes[i].GetComponent<waypoint>();
            if(w != null){
                nodes[i].gameObject.GetComponent<waypoint>().previousWaypoint = (i == 0)? null : nodes[i-1].GetComponent<waypoint>();
                nodes[i].gameObject.GetComponent<waypoint>().nextWaypoint = (i == nodes.Length-1)? null: nodes[i+1].GetComponent<waypoint>();
            }
        }
    }
    void assingVehicleNodeOpen(){
        for (int i = 0; i < nodes.Length; i++){
            carNode w = nodes[i].GetComponent<carNode>();
            if(w != null){
                nodes[i].gameObject.GetComponent<carNode>().previousWaypoint = (i == 0)? null : nodes[i-1].GetComponent<carNode>();
                nodes[i].gameObject.GetComponent<carNode>().nextWaypoint = (i == nodes.Length-1)? null: nodes[i+1].GetComponent<carNode>();
            }
        }
    }

    void initializeArray(){
        nodes = null;
        nodes = new Transform [RootObject.childCount];

        for (int i = 0; i < nodes.Length; i++){
            nodes[i] = RootObject.GetChild(i);
        }
    }

        [DrawGizmo( GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawGizmos(carNode waypoint , GizmoType gizmoType){
        if((gizmoType & GizmoType.Selected) != 0)
            Gizmos.color = Color.white;
        else
            Gizmos.color = Color.white ;


        Gizmos.DrawSphere(waypoint.transform.position , 0.1f);
        if(waypoint.nextWaypoint != null && waypoint.previousWaypoint != null){
            Gizmos.DrawLine(waypoint.transform.position ,waypoint.previousWaypoint.transform.position );

        }
        else if(waypoint.previousWaypoint == null && waypoint.nextWaypoint != null){
            Gizmos.color = Color.green ;
            Gizmos.DrawLine(waypoint.transform.position ,waypoint.nextWaypoint.transform.position );
        }
        else{
            Gizmos.color = Color.yellow ;
            if(waypoint.previousWaypoint != null)
            Gizmos.DrawLine(waypoint.transform.position ,waypoint.previousWaypoint.transform.position );
        }

        if(waypoint.link != null)
            Gizmos.DrawLine(waypoint.transform.position ,waypoint.link.transform.position );
        
        
        }

}

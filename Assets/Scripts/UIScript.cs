using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    //This script all of the user interface.

    //Zooming
    float ZoomSensitivity = 0.5f; //Multplies input change to InputZoom.
    float ZoomMin = 0.7f; //Minimum Zoom (Smaller == Bigger Size). Effects InputZoom.
    float ZoomMax = 25; //Maximum Zoom (Bigger == Smaller Size). Effects InputZoom.
    float ZoomSwitchOver = 20; //Point at which pixels tiles are hidden and indivdual tiles take over (or vice versa), this is compared to the outputzoom not input zoom.
    float LastZoomOutput = 10; //The Zoom Output from the last frame, used to check if zoom has changed (and the tiles need to be updated.
    float ZoomInput = 10; //Input zoom that scroll wheel changes linearlly.
    public float ZoomOuput = 10; //Output zoom that changes exponetially according to input zoom. (Public so that it can be displayed on screen).

    
    bool MouseDrag = false;
    Vector2 MousePosOnMouseDown;
    
    Text TileCoordinatesText;
    Text TileTypeText;
    Text TileWater;
    Text TileWood;
    Text TileStone;
    Text TileMetal;
    Text FPSCount;
    Text Zoom;

    MapScript MapScriptComponent;
    Camera CameraComponent;

    Manager ManagerScript;

    // Use this for initialization
    void Start()
    {
        TileTypeText = GameObject.Find("TileType").GetComponent<Text>();
        TileCoordinatesText = GameObject.Find("TileCoordinates").GetComponent<Text>();
        TileWater = GameObject.Find("TileWater").GetComponent<Text>();
        TileWood = GameObject.Find("TileWood").GetComponent<Text>();
        TileStone = GameObject.Find("TileStone").GetComponent<Text>();
        TileMetal = GameObject.Find("TileMetal").GetComponent<Text>();
        FPSCount = GameObject.Find("FPS Count").GetComponent<Text>();
        Zoom = GameObject.Find("Zoom").GetComponent<Text>();
        ManagerScript = GetComponent<Manager>();
        CameraComponent = GetComponent<Camera>();
        MapScriptComponent = GetComponent<MapScript>();

    }

    // Update is called once per frame
    void Update()
    {
        bool UpdateTiles = false;
        ZoomInput += Input.GetAxis("Zoom") * ZoomSensitivity; //Uses zoom input (scrollwheel) to change zoominput.
                                                              //Makes sure it is in boundries.
        if (ZoomInput < ZoomMin)
        {
            ZoomInput = ZoomMin;
        }
        if (ZoomInput > ZoomMax)
        {
            ZoomInput = ZoomMax;
        }
        //This makes rotation of scroll wheel have bigger effect when zoom out, which makes it look normal
        ZoomOuput = ZoomInput * ZoomInput;
        CameraComponent.orthographicSize = ZoomOuput;
        if (ZoomOuput != LastZoomOutput)
        {
            UpdateTiles = true;
        }

        FPSCount.text = "FPS: " + Mathf.Round((1 / Time.deltaTime)).ToString();
        Zoom.text = "Zoom: " + ZoomOuput;
        
        if (Input.GetMouseButtonDown(0))
        {
            MouseDrag = true;
            MousePosOnMouseDown = CameraComponent.ScreenToWorldPoint(Input.mousePosition);
        }
        if (MouseDrag)
        {
            UpdateTiles = true;
            
            Vector2 _MouseChange = CameraComponent.ScreenToWorldPoint(Input.mousePosition); ;
            _MouseChange = new Vector2(_MouseChange.x - MousePosOnMouseDown.x, _MouseChange.y - MousePosOnMouseDown.y);
             transform.position = new Vector3(transform.position.x - _MouseChange.x, transform.position.y -_MouseChange.y, transform.position.z);
            MousePosOnMouseDown = CameraComponent.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseDrag = false;
        }
        LastZoomOutput = ZoomOuput;
        if (UpdateTiles)
        {
            MapScriptComponent.TryUpdateTiles(ZoomOuput < ZoomSwitchOver);
        }
    }


    

}

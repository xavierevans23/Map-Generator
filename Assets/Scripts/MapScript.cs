using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/14663168/an-integer-array-as-a-key-for-dictionary
public class BlockIntArray : IEqualityComparer<int[]>
{
    public bool Equals(int[] x, int[] y)
    {
        if (x.Length != y.Length)
        {
            return false;
        }
        for (int i = 0; i < x.Length; i++)
        {
            if (x[i] != y[i])
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(int[] obj)
    {
        int result = 17;
        for (int i = 0; i < obj.Length; i++)
        {
            unchecked
            {
                result = result * 23 + obj[i];
            }
        }
        return result;
    }
}

public class MapScript : MonoBehaviour {

    //Generating Map
    GraphicsMap GraphicsMapload;
    public int LoadMapStage = 0;
    bool OnceLoaded = false;


    //Tiles
    GameObject[,] Tiles;
    int MapWidth; //Map Width in blocks.
    int MapHeight; //Map Height in blocks.
    float AllowanceForHiding = 2; //How many blocks to the side of what can be seen are shown when individal blocks are shown.
    float TileScaleFactor = 1; //Enlarges the side of entire grid. Not used but works.
    int LargeTilePixelSideLength = 250;

    //Pixel Tiles
    GameObject[,] Chunks;
    int PixelTileSideLength = 15; //Side length of each of the smaller pixel tile images.
    int PixelTileChunkSideLength = 25; //Side length of chunks of pixel tiles. Extras are rounded off!!

    

    //GameObjects, textures, infomation and Components
    Camera CameraComponent;
    public Texture2D[] GroundImages = new Texture2D[73]; //Array containg sprites with each texure.
    public Texture2D[] MapTextures = new Texture2D[6]; //Used in editor to define images.
    public int[] TextureListStart = new int[6];//where each new tile starts in the array of tile secions.
    GameObject TileObject; //Default tile that ground image is added to. 
    GameObject ChunksHolder; //Gameobject that individual chunks go into.
    GameObject MapHolder; //Gameobject that individual tiles go into.
    GameObject DefaultChunk; //Empty Chunk to have textured added to.
    public Texture2D Error; //Error Texture, set in editor.
    Dictionary<int[], Texture2D> RenderedBlocks = new Dictionary<int[], Texture2D>(new BlockIntArray());

    float TimeTakenCounter = 0;

    private void Start()
    {


        //Setting componetns and gameobjects.
        CameraComponent = GetComponent<Camera>();

        DefaultChunk = GameObject.Find("DefaultChunk");
        ChunksHolder = GameObject.Find("Chunks");
        TileObject = GameObject.Find("Tile");
        MapHolder = GameObject.Find("Map");

        //Sets all things that shouldn't be shown unactive.
        GameObject.Find("Ground Images").SetActive(false);
        DefaultChunk.SetActive(false);
        TileObject.SetActive(false);

    }

    //Gets tile no. and thus texture from list of tile sections
    private Texture2D RotationNumbersToTexture2D(int _TileNo, int _TileRotation, int _TileDirection)
    {
        int LengthFindCounter = TextureListStart[_TileNo];//Gets start

        if (_TileNo > 0 && _TileNo < 5)
        {
            if (_TileDirection == 0)
            {
                return GroundImages[LengthFindCounter]; //If the direction is 0, it must be the first one.
            }
            else
            {
                return GroundImages[LengthFindCounter + ((_TileRotation - 1) * 4) + _TileDirection];
            }

        }
        if (_TileNo == 5)
        {
            return GroundImages[LengthFindCounter + _TileDirection];
        }
        if (_TileNo == 0)
        {
            return GroundImages[0]; //Grass only has one state.
        }

        return Error; //Returns error if it wasn't found.
    }

    private void CheckTilesActive()
    {
        //Calculates sides of camera in real world view by finding the x or y postion in pixels of the camera positon, then adding turing that into a vector3 so that the camera can use ScreenToWorldPoint to turn into real world coordinates. Camera widht/Height is added if it is the max.
        float _CameraMinX = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(GetComponent<Camera>().pixelRect.x, 0, 0)).x;
        float _CameraMinY = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, GetComponent<Camera>().pixelRect.y, 0)).y;
        float _CameraMaxX = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(GetComponent<Camera>().pixelRect.x + GetComponent<Camera>().pixelRect.width, 0, 0)).x;
        float _CameraMaxY = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0, GetComponent<Camera>().pixelRect.y + GetComponent<Camera>().pixelRect.height, 0)).y;

        //Goes through every block
        for (int a = 0; a < MapWidth; a++)
        {
            for (int b = 0; b < MapHeight; b++)
            {
                //Each block is always one apart * scalefactor.
                float _TileMinX = ((a - AllowanceForHiding) * TileScaleFactor);
                float _TileMinY = ((b - AllowanceForHiding) * TileScaleFactor);
                float _TileMaxX = ((a + AllowanceForHiding) * TileScaleFactor) + TileScaleFactor;
                float _TileMaxY = ((b + AllowanceForHiding) * TileScaleFactor) + TileScaleFactor;
                if ((_TileMaxX < _CameraMinX || _TileMinX > _CameraMaxX) || (_TileMaxY < _CameraMinY || _TileMinY > _CameraMaxY)) //If left side of block is smaller than right side of camera, it must be out, this is repeated for the other sides.
                {
                    if (Tiles[a, b].activeSelf) //If active, make not active.
                    {
                        Tiles[a, b].SetActive(false);
                    }

                }
                else
                {
                    if (Tiles[a, b].activeSelf == false)
                    {
                        Tiles[a, b].SetActive(true);
                    }
                }
            }
        }
    }

    public void TryUpdateTiles(bool _ZoomMode)
    {
        if (LoadMapStage == 0)
        {
            //Checks if ouput says to switch over, if so it activates and deactives correpsoningly.
            if (_ZoomMode)
            {
             CheckTilesActive();
                

                if (ChunksHolder.activeSelf)
                {
                    ChunksHolder.SetActive(false);
                }
                if (MapHolder.activeSelf == false)
                {
                    MapHolder.SetActive(true);
                }
            }
            else
            {
                if (ChunksHolder.activeSelf == false)
                {
                    ChunksHolder.SetActive(true);
                }
                if (MapHolder.activeSelf)
                {
                    MapHolder.SetActive(false);
                }
            }

           
        }
    }

    private void Update()
    {
        OnceLoaded = true;
        //Loads world bit by bit
        if (LoadMapStage == 1 && OnceLoaded)
        {
            TimeTakenCounter += Time.unscaledDeltaTime;
            SetMapStage(GraphicsMapload, 0);
            LoadMapStage = 2;
            OnceLoaded = false;
        }
        if (LoadMapStage == 2 && OnceLoaded)
        {
            TimeTakenCounter += Time.unscaledDeltaTime;
            SetMapStage(GraphicsMapload, 1);
            LoadMapStage = 3;
            OnceLoaded = false;
        }
        if (LoadMapStage == 3 && OnceLoaded)
        {
            TimeTakenCounter += Time.unscaledDeltaTime;
            SetMapStage(GraphicsMapload, 2);
            LoadMapStage = 4;
            OnceLoaded = false;
        }
        if (LoadMapStage == 4 && OnceLoaded)
        {
            TimeTakenCounter += Time.unscaledDeltaTime;
            print(TimeTakenCounter);
            LoadMapStage = 0;
            OnceLoaded = false;
        }
      
        
        
    }

    //Combines textures using copytexture.
    Texture2D CombineTexturesForTiles(Texture2D[] _TextureList, int _Width, int _Height, int _SideLength)
    {
        Texture2D _CopyDest = new Texture2D(_SideLength * _Width, _SideLength * _Height);
        if (_Width * _Height == _TextureList.Length) //Checks to make sure correct number of textures given.
        {
            for (int _ImageCount = 0; _ImageCount < _TextureList.Length; _ImageCount++) //Counts through each image.
            {
               
                Graphics.CopyTexture(_TextureList[_ImageCount], 0, 0, 0, 0, _SideLength, _SideLength, _CopyDest, 0, 0, ((_ImageCount % _Width) * _SideLength), (((_ImageCount - (_ImageCount % _Width)) / _Width) * _SideLength));                
            }
            
            _CopyDest.wrapMode = TextureWrapMode.Clamp; //Makes sure it doesn't loop.
            _CopyDest.filterMode = FilterMode.Bilinear; //Make sure it blurres instead of pixelates.
            _CopyDest.Apply();
            return _CopyDest;
        }
        else
        {
            print("Incorrect number of textures given");
            return Error;
        }
    }

    public void ClearMap()
    {
        ChunksHolder.SetActive(true);
        Chunks = null; //Clears chunks array
        foreach (GameObject _chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            GameObject.Destroy(_chunk);
        }

        foreach (Transform child in MapHolder.transform)
        {
            Transform.Destroy(child.gameObject);
        }
    }

    public void GenerateChunks(GraphicsMap _GraphicsMap)
    {
        int PerfectWidth = MapWidth - (MapWidth % PixelTileChunkSideLength); //If chunks size does fit into map size, the remainder is ignored.
        int PerfectHeight = MapHeight - (MapHeight % PixelTileChunkSideLength);
        if(PerfectWidth!= MapWidth || PerfectHeight!= MapHeight)
        {
            print("Warning!! Incorrect chunk size for map size.");
        }
        
        Chunks = new GameObject[PerfectWidth, PerfectHeight]; //Creates 2d gameobject array.

        for (int _MapWidthC = 0; _MapWidthC < PerfectWidth / PixelTileChunkSideLength; _MapWidthC++) //Counts up through each future chunk.
        {
            for (int _MapHeightC = 0; _MapHeightC < PerfectHeight / PixelTileChunkSideLength; _MapHeightC++)
            { 
                Texture2D[] TexturesToUse = new Texture2D[PixelTileChunkSideLength * PixelTileChunkSideLength]; //Creates list of textures that will be combined.

                for (int _ChunkX = 0; _ChunkX < PixelTileChunkSideLength; _ChunkX++) //Counts through each tile within chunk.
                {
                    for (int _ChunkY = 0; _ChunkY < PixelTileChunkSideLength; _ChunkY++)
                    {
                        //Adds each texture to array using (Y*W)+X
                        TexturesToUse[(_ChunkY * PixelTileChunkSideLength) + _ChunkX] = MapTextures[_GraphicsMap.GraphicsMapMatrix[(_MapWidthC * PixelTileChunkSideLength) + _ChunkX, (_MapHeightC * PixelTileChunkSideLength) + _ChunkY].Type];
                    }
                }

                Texture2D NewTexture = CombineTexturesForTiles(TexturesToUse, PixelTileChunkSideLength, PixelTileChunkSideLength, PixelTileSideLength); //Combines textures using pixel fucntion
                Chunks[_MapWidthC, _MapHeightC] = GameObject.Instantiate(DefaultChunk); //Adds new GameObject to array
                Chunks[_MapWidthC, _MapHeightC].name = "Chunk (" + _MapWidthC + "," + _MapHeightC + ")"; //Names
                Chunks[_MapWidthC, _MapHeightC].tag = "Chunk"; //Tags
                Chunks[_MapWidthC, _MapHeightC].transform.parent = ChunksHolder.transform; //Sets parent

                Chunks[_MapWidthC, _MapHeightC].transform.position = new Vector3(_MapWidthC* TileScaleFactor*PixelTileChunkSideLength, _MapHeightC* TileScaleFactor * PixelTileChunkSideLength, 0); //Postion, tile should be one apart
                Chunks[_MapWidthC, _MapHeightC].transform.localScale = new Vector3((float)(decimal.Divide(1, System.Convert.ToDecimal((float)(decimal.Divide(PixelTileSideLength, 100))))) * TileScaleFactor, (float)(decimal.Divide(1, System.Convert.ToDecimal((float)(decimal.Divide(PixelTileSideLength, 100))))) * TileScaleFactor, 1); //If scale was set to one, the actual size would be pixel size over 100, so 1/(pixel size over 100) makes true size. Note: special function used becuase doesn't work with "/".

                Chunks[_MapWidthC, _MapHeightC].GetComponent<SpriteRenderer>().sprite = Sprite.Create(NewTexture, new Rect(0, 0, NewTexture.width, NewTexture.height), Vector2.one * 0f);
                Chunks[_MapWidthC, _MapHeightC].SetActive(true);
            }
        }
        ChunksHolder.transform.localScale = new Vector3(1, 1, 1); //Makes sure scale is set right.
    }

    private void SetMapStage(GraphicsMap _GraphicsMap, int _Stage)
    {
        if(_Stage ==0)
        {
            MapWidth = _GraphicsMap.Width;
            MapHeight = _GraphicsMap.Height;
            ClearMap(); //Deletes all chunks and all tiles.
        }
        if(_Stage == 1)
        {
            GenerateChunks(_GraphicsMap);
        }
        if (_Stage == 2)
        {
            CreatesIndividualTiles(_GraphicsMap);
        }
    }

    public void SetMap(GraphicsMap _GraphicsMap, bool _Split) 
    {
        TimeTakenCounter = 0;
        if (_Split)
        {
            LoadMapStage = 1;
            GraphicsMapload = _GraphicsMap;
            
        }
        else
        {
            SetMapStage(_GraphicsMap,0);
            SetMapStage(_GraphicsMap, 1);
            SetMapStage(_GraphicsMap, 2);
        }
       
    }

    void CreatesIndividualTiles(GraphicsMap _GraphicsMap)
    {
        Tiles = new GameObject[MapWidth, MapHeight]; //Resets array for Tiles.
        for (int a = 0; a < MapWidth; a++) //Counts each tile
        {
            for (int b = 0; b < MapHeight; b++)
            {
                GameObject _NewTile = GameObject.Instantiate(TileObject); //Creates new tile from template.
                _NewTile.SetActive(false);
                Texture2D _NewTexture; //Creates new texture
                _NewTexture = Error; //Sets it as eroor to start off with

                //Gets rotation and direction numbers from direction array, and so texture
                if (_GraphicsMap.GraphicsMapMatrix[a, b].Directions[0] == -1)
                {
                    //If special case
                    _NewTexture = RotationNumbersToTexture2D(_GraphicsMap.GraphicsMapMatrix[a, b].Type, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[1], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[2]);
                }
                else
                {
                    if (RenderedBlocks.ContainsKey(new int[] { _GraphicsMap.GraphicsMapMatrix[a, b].Type, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[0], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[1], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[2], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[3] }))
                    {
                        _NewTexture = RenderedBlocks[new int[] { _GraphicsMap.GraphicsMapMatrix[a, b].Type, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[0], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[1], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[2], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[3] }];
                    }
                    else
                    {
                        Texture2D _TopLeftTexture = RotationNumbersToTexture2D(_GraphicsMap.GraphicsMapMatrix[a, b].Type, 1, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[0]);
                        Texture2D _TopRightTexture = RotationNumbersToTexture2D(_GraphicsMap.GraphicsMapMatrix[a, b].Type, 2, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[1]);
                        Texture2D _BottomLeftTexture = RotationNumbersToTexture2D(_GraphicsMap.GraphicsMapMatrix[a, b].Type, 3, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[2]);
                        Texture2D _BottomRightTexture = RotationNumbersToTexture2D(_GraphicsMap.GraphicsMapMatrix[a, b].Type, 4, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[3]);
                        _NewTexture = CombineTexturesForTiles(new Texture2D[] { _BottomRightTexture, _BottomLeftTexture, _TopLeftTexture, _TopRightTexture }, 2, 2, 250); //Combines Texture into one.
                        RenderedBlocks.Add(new int[] { _GraphicsMap.GraphicsMapMatrix[a, b].Type, _GraphicsMap.GraphicsMapMatrix[a, b].Directions[0], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[1], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[2], _GraphicsMap.GraphicsMapMatrix[a, b].Directions[3] }, _NewTexture);
                    }
                }
                _NewTile.name = "Tile (" + a + ", " + b + ")";
                _NewTile.transform.parent = MapHolder.transform;
                _NewTile.transform.position = new Vector3(a * TileScaleFactor, b * TileScaleFactor, 0);
                _NewTile.transform.localScale = new Vector3(0.2f * TileScaleFactor, 0.2f * TileScaleFactor, 1);
                _NewTile.GetComponent<SpriteRenderer>().sprite = Sprite.Create(_NewTexture, new Rect(new Vector2(0, 0), new Vector2(_NewTexture.width, _NewTexture.height)),new Vector2(0.5f,0.5f),100f,(uint)0,SpriteMeshType.FullRect);
                Tiles[a, b] = _NewTile;
            }
        }

    }

}

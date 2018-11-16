using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsTile
{
    public int[] Directions;
    public int Type;
    public int Settlement; //If -1, no settlement applied.
    public int Empire; //If -1, no empire applied.

    public GraphicsTile(int[] _Directions, int _Type, int _Settlement, int _Empire)
    {
        Directions = _Directions;
        Settlement = _Settlement;
        Empire = _Empire;
        Type = _Type;
    }
}

public class Tile
{
    public int Empire;
    public int Settlement;
    public float Water;
    public float Metal;
    public float Wood;
    public float Stone;
    public float Fertility;
    public int Troops;
    public int Type;
    public MilitaryItems[] militaryItems;
    public Building[] Buildings;

    public Tile()
    {

    }
}

public class TileType
{
    public string Name;
    public float WaterMax;
    public float WaterMin;
    public float MetalMax;
    public float MetalMin;
    public float WoodMax;
    public float WoodMin;
    public float StoneMax;
    public float StoneMin;
    public float FertilityMax;
    public float FertilityMin;
}

public class Building
{
    public int Type;
    public int Upgrade;

    public Building(int _Type)
    {
        Type = _Type;
    }
}

public class MilitaryItems
{
    public int Type;
    public int Number;

    public MilitaryItems(int _Type, int _Number)
    {
        Type = _Type;
        Number = _Number;
    }
}

public class Settlement
{
    public int Population;
    public int Troops;
    public int Water;
    public int Metal;
    public int Wood;
    public int Food;
    public int Stone;
    public string Name;
    public Vector2 Location;
    public List<Vector2> Tiles;
    public int Level;

    public Settlement(string _Name, Vector2 _Location)
    {
        Location = _Location;
        Name = _Name;
    }
}

public class Map
{
    public Tile[,] MapMatrix;
    public int Width;
    public int Height;

    public Map(Tile[,] _MapMatrix, int _Width, int _Height)
    {
        MapMatrix = _MapMatrix;
        Width = _Width;
        Height = _Height;
    }
}

public class GraphicsMap
{
    public GraphicsTile[,] GraphicsMapMatrix;
    public int Width;
    public int Height;

    public GraphicsMap(GraphicsTile[,] _GraphicsMapMatrix, int _Width, int _Height)
    {
        GraphicsMapMatrix = _GraphicsMapMatrix;
        Width = _Width;
        Height = _Height;
    }
}

public class Empire
{
    public string Name;
    public Settlement[] Settlements;
    public Empire(string _Name)
    {
        Name = _Name;
    }
}

public class Manager : MonoBehaviour
{
    TileType[] TileInfo = new TileType[6]; //Infomation About types of tiles setup.
    Map WorldMap;

    List<int> SpecialTiles = new List<int>(); //Tiles which don't match pattern
    Dictionary<int, List<int>> AcceptedTiles = new Dictionary<int, List<int>>();

    public void Start()
    {
        //Adds special tiles
        SpecialTiles.Add(0);
        SpecialTiles.Add(5);
        //Adds Accepted tiles
        for (int TileCount = 0; TileCount < 6; TileCount++)
        {
            AcceptedTiles.Add(TileCount, new List<int>());
            AcceptedTiles[TileCount].Add(TileCount);

        }
        AcceptedTiles[3].Add(2);
        AcceptedTiles[3].Add(5);
        //Creates list of blank tile infomation then adds it all.
        for (int i = 0; i < TileInfo.Length; i++)
        {
            TileInfo[i] = new TileType();
        }
        TileInfo[0].Name = "Grass";
        TileInfo[0].FertilityMax = 1f;
        TileInfo[0].FertilityMin = 0.2f;
        TileInfo[0].MetalMax = 0.6f;
        TileInfo[0].MetalMin = 0.1f;
        TileInfo[0].StoneMax = 0.6f;
        TileInfo[0].StoneMin = 0.1f;
        TileInfo[0].WaterMax = 0.6f;
        TileInfo[0].WaterMin = 0.1f;
        TileInfo[0].WoodMax = 0.4f;
        TileInfo[0].WoodMin = 0.0f;

        TileInfo[1].Name = "Stone";
        TileInfo[1].FertilityMax = 0.2f;
        TileInfo[1].FertilityMin = 0.0f;
        TileInfo[1].MetalMax = 1f;
        TileInfo[1].MetalMin = 0.6f;
        TileInfo[1].StoneMax = 1f;
        TileInfo[1].StoneMin = 0.8f;
        TileInfo[1].WaterMax = 0.3f;
        TileInfo[1].WaterMin = 0.0f;
        TileInfo[1].WoodMax = 0.2f;
        TileInfo[1].WoodMin = 0.0f;

        TileInfo[2].Name = "Water";
        TileInfo[2].FertilityMax = 0.1f;
        TileInfo[2].FertilityMin = 0.0f;
        TileInfo[2].MetalMax = 0.2f;
        TileInfo[2].MetalMin = 0.0f;
        TileInfo[2].StoneMax = 0.0f;
        TileInfo[2].StoneMin = 0.0f;
        TileInfo[2].WaterMax = 1f;
        TileInfo[2].WaterMin = 1f;
        TileInfo[2].WoodMax = 0.2f;
        TileInfo[2].WoodMin = 0.0f;

        TileInfo[3].Name = "Sand";
        TileInfo[3].FertilityMax = 0.1f;
        TileInfo[3].FertilityMin = 0.0f;
        TileInfo[3].MetalMax = 0.2f;
        TileInfo[3].MetalMin = 0.0f;
        TileInfo[3].StoneMax = 0.6f;
        TileInfo[3].StoneMin = 0.0f;
        TileInfo[3].WaterMax = 0.7f;
        TileInfo[3].WaterMin = 0.5f;
        TileInfo[3].WoodMax = 0.2f;
        TileInfo[3].WoodMin = 0.0f;

        TileInfo[4].Name = "Woodland";
        TileInfo[4].FertilityMax = 1f;
        TileInfo[4].FertilityMin = 0.5f;
        TileInfo[4].MetalMax = 0.4f;
        TileInfo[4].MetalMin = 0.2f;
        TileInfo[4].StoneMax = 0.6f;
        TileInfo[4].StoneMin = 0.3f;
        TileInfo[4].WaterMax = 0.6f;
        TileInfo[4].WaterMin = 0.1f;
        TileInfo[4].WoodMax = 1f;
        TileInfo[4].WoodMin = 0.7f;

        TileInfo[5].Name = "Sea";
        TileInfo[5].FertilityMax = 0.1f;
        TileInfo[5].FertilityMin = 0.0f;
        TileInfo[5].MetalMax = 0.2f;
        TileInfo[5].MetalMin = 0.0f;
        TileInfo[5].StoneMax = 0.0f;
        TileInfo[5].StoneMin = 0.0f;
        TileInfo[5].WaterMax = 1f;
        TileInfo[5].WaterMin = 1f;
        TileInfo[5].WoodMax = 0.2f;
        TileInfo[5].WoodMin = 0.0f;
    }



    public void Update()
    {

        if (Input.GetKey("space") && GetComponent<MapScript>().LoadMapStage == 0)
        {

            WorldMap = GenerateMap(200, 200);

            //To calc time taken (x*y)*0.00991157 secs
            //To calc time taken (x*y)*0.000084 secs
            GetComponent<MapScript>().SetMap(GenerateGraphicsMap(WorldMap), true);

        }
    }

    private GraphicsMap GenerateGraphicsMap(Map _Map)
    {
        GraphicsTile[,] _GraphicsMap = new GraphicsTile[_Map.Width, _Map.Height]; //Sets up new graphics map
        for (int _MapCountX = 0; _MapCountX < _Map.Width; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _Map.Height; _MapCountY++) //Goes through each block
            {
                int _type = _Map.MapMatrix[_MapCountX, _MapCountY].Type; //Gets type of block

                //Gets if surroudings block are of some material
                bool[] _TileNumbers = new bool[8];
                if (_MapCountX > 0 && _MapCountY < _Map.Height - 1) //Makes sure its not looking past the end of map, if it is it just reports true.
                {
                    _TileNumbers[0] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX - 1, _MapCountY + 1].Type);
                }
                else
                {
                    _TileNumbers[0] = true;
                }
                if (_MapCountY < _Map.Height - 1)
                {
                    _TileNumbers[1] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX, _MapCountY + 1].Type);
                }
                else
                {
                    _TileNumbers[1] = true;
                }
                if (_MapCountX < _Map.Width - 1 && _MapCountY < _Map.Height - 1)
                {
                    _TileNumbers[2] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX + 1, _MapCountY + 1].Type);
                }
                else
                {
                    _TileNumbers[2] = true;
                }
                if (_MapCountX > 0)
                {
                    _TileNumbers[3] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX - 1, _MapCountY].Type);
                }
                else
                {
                    _TileNumbers[3] = true;
                }
                if (_MapCountX < _Map.Width - 1)
                {
                    _TileNumbers[4] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX + 1, _MapCountY].Type);
                }
                else
                {
                    _TileNumbers[4] = true;
                }
                if (_MapCountY > 0 && _MapCountX > 0)
                {
                    _TileNumbers[5] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX - 1, _MapCountY - 1].Type);
                }
                else
                {
                    _TileNumbers[5] = true;
                }
                if (_MapCountY > 0)
                {
                    _TileNumbers[6] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX, _MapCountY - 1].Type);
                }
                else
                {
                    _TileNumbers[6] = true;
                }
                if (_MapCountY > 0 && _MapCountX < _Map.Width - 1)
                {
                    _TileNumbers[7] = AcceptedTiles[_type].Contains(_Map.MapMatrix[_MapCountX + 1, _MapCountY - 1].Type);
                }
                else
                {
                    _TileNumbers[7] = true;
                }

                //Decides for each corner of block what it should be. Since this is based on how they are arranged, this can be simplified.
                int _TopLeft = 0;
                int _TopRight = 0;
                int _BottomLeft = 0;
                int _BottomRight = 0;
                if (!SpecialTiles.Contains(_Map.MapMatrix[_MapCountX, _MapCountY].Type))
                {
                    if (_TileNumbers[0] == false && _TileNumbers[1] == false && _TileNumbers[3] == false)
                    {
                        _TopLeft = 1;
                    }
                    if (_TileNumbers[0] == false && _TileNumbers[1] == false && _TileNumbers[3] == true)
                    {
                        _TopLeft = 2;
                    }
                    if (_TileNumbers[0] == false && _TileNumbers[1] == true && _TileNumbers[3] == false)
                    {
                        _TopLeft = 4;
                    }
                    if (_TileNumbers[0] == false && _TileNumbers[1] == true && _TileNumbers[3] == true)
                    {
                        _TopLeft = 3;
                    }
                    if (_TileNumbers[0] == true && _TileNumbers[1] == false && _TileNumbers[3] == false)
                    {
                        _TopLeft = 1;
                    }
                    if (_TileNumbers[0] == true && _TileNumbers[1] == false && _TileNumbers[3] == true)
                    {
                        _TopLeft = 2;
                    }
                    if (_TileNumbers[0] == true && _TileNumbers[1] == true && _TileNumbers[3] == false)
                    {
                        _TopLeft = 4;
                    }
                    if (_TileNumbers[0] == true && _TileNumbers[1] == true && _TileNumbers[3] == true)
                    {
                        _TopLeft = 0;
                    }
                    if (_TileNumbers[3] == false && _TileNumbers[5] == false && _TileNumbers[6] == false)
                    {
                        _BottomLeft = 1;
                    }
                    if (_TileNumbers[3] == false && _TileNumbers[5] == false && _TileNumbers[6] == true)
                    {
                        _BottomLeft = 2;
                    }
                    if (_TileNumbers[3] == false && _TileNumbers[5] == true && _TileNumbers[6] == false)
                    {
                        _BottomLeft = 1;
                    }
                    if (_TileNumbers[3] == false && _TileNumbers[5] == true && _TileNumbers[6] == true)
                    {
                        _BottomLeft = 2;
                    }
                    if (_TileNumbers[3] == true && _TileNumbers[5] == false && _TileNumbers[6] == false)
                    {
                        _BottomLeft = 4;
                    }
                    if (_TileNumbers[3] == true && _TileNumbers[5] == false && _TileNumbers[6] == true)
                    {
                        _BottomLeft = 3;
                    }
                    if (_TileNumbers[3] == true && _TileNumbers[5] == true && _TileNumbers[6] == false)
                    {
                        _BottomLeft = 4;
                    }
                    if (_TileNumbers[3] == true && _TileNumbers[5] == true && _TileNumbers[6] == true)
                    {
                        _BottomLeft = 0;

                    }
                    if (_TileNumbers[1] == false && _TileNumbers[2] == false && _TileNumbers[4] == false)
                    {
                        _TopRight = 1;
                    }
                    if (_TileNumbers[1] == false && _TileNumbers[2] == false && _TileNumbers[4] == true)
                    {
                        _TopRight = 4;
                    }
                    if (_TileNumbers[1] == false && _TileNumbers[2] == true && _TileNumbers[4] == false)
                    {
                        _TopRight = 1;
                    }
                    if (_TileNumbers[1] == false && _TileNumbers[2] == true && _TileNumbers[4] == true)
                    {
                        _TopRight = 4;
                    }
                    if (_TileNumbers[1] == true && _TileNumbers[2] == false && _TileNumbers[4] == false)
                    {
                        _TopRight = 2;
                    }
                    if (_TileNumbers[1] == true && _TileNumbers[2] == false && _TileNumbers[4] == true)
                    {
                        _TopRight = 3;
                    }
                    if (_TileNumbers[1] == true && _TileNumbers[2] == true && _TileNumbers[4] == false)
                    {
                        _TopRight = 2;
                    }
                    if (_TileNumbers[1] == true && _TileNumbers[2] == true && _TileNumbers[4] == true)
                    {
                        _TopRight = 0;
                    }
                    if (_TileNumbers[4] == false && _TileNumbers[6] == false && _TileNumbers[7] == false)
                    {
                        _BottomRight = 1;
                    }
                    if (_TileNumbers[4] == true && _TileNumbers[6] == false && _TileNumbers[7] == false)
                    {
                        _BottomRight = 2;
                    }
                    if (_TileNumbers[4] == false && _TileNumbers[6] == false && _TileNumbers[7] == true)
                    {
                        _BottomRight = 1;
                    }
                    if (_TileNumbers[4] == true && _TileNumbers[6] == false && _TileNumbers[7] == true)
                    {
                        _BottomRight = 2;
                    }
                    if (_TileNumbers[4] == false && _TileNumbers[6] == true && _TileNumbers[7] == false)
                    {
                        _BottomRight = 4;
                    }
                    if (_TileNumbers[4] == true && _TileNumbers[6] == true && _TileNumbers[7] == false)
                    {
                        _BottomRight = 3;
                    }
                    if (_TileNumbers[4] == false && _TileNumbers[6] == true && _TileNumbers[7] == true)
                    {
                        _BottomRight = 4;
                    }
                    if (_TileNumbers[4] == true && _TileNumbers[6] == true && _TileNumbers[7] == true)
                    {
                        _BottomRight = 0;
                    }
                }
                else
                {

                    _TopLeft = -1;
                    if (_Map.MapMatrix[_MapCountX, _MapCountY].Type == 5)
                    {
                        if (_TileNumbers[1] == false && _TileNumbers[3] == false && _TileNumbers[4] == false)
                        {
                            _BottomRight = 1;
                        }
                        if (_TileNumbers[1] == false && _TileNumbers[3] == true && _TileNumbers[4] == false)
                        {
                            _BottomRight = 2;
                        }
                        if (_TileNumbers[1] == false && _TileNumbers[3] == true && _TileNumbers[4] == true)
                        {
                            _BottomRight = 3;
                        }
                        if (_TileNumbers[1] == false && _TileNumbers[3] == false && _TileNumbers[4] == true)
                        {
                            _BottomRight = 4;
                        }
                        if (_TileNumbers[0] == false && _TileNumbers[1] == true && _TileNumbers[2] == false)
                        {
                            _BottomRight = 5;
                        }
                        if (_TileNumbers[0] == false && _TileNumbers[1] == true && _TileNumbers[2] == true)
                        {
                            _BottomRight = 6;
                        }
                        if (_TileNumbers[0] == true && _TileNumbers[1] == true && _TileNumbers[2] == false)
                        {
                            _BottomRight = 7;
                        }
                    }
                }
                //Sets new block

                _GraphicsMap[_MapCountX, _MapCountY] = new GraphicsTile(new int[] { _TopLeft, _TopRight, _BottomRight, _BottomLeft }, _Map.MapMatrix[_MapCountX, _MapCountY].Type, -1, -1);


            }
        }

        return new GraphicsMap(_GraphicsMap, _Map.Width, _Map.Height);
    }

    Map GenerateMap(int _MapWidth, int _MapHeight)
    {
        Tile[,] _OutMap = new Tile[_MapWidth, _MapHeight]; //Final Map to be outputed

        int[,] _BasicMap = new int[_MapWidth, _MapHeight]; //Map of types only for simplification.

        bool[,] _BubbleOut = GenerateBubbleMap(0.5f, _MapWidth, _MapHeight, 200); //Simple map set up just for grass and trees

        //Grass
        for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
            {
                if (_BubbleOut[_MapCountX, _MapCountY])
                {
                    _BasicMap[_MapCountX, _MapCountY] = 0;
                }
                else
                {
                    _BasicMap[_MapCountX, _MapCountY] = 4;
                }
            }
        }

        _BubbleOut = GenerateBubbleMap(0.4f, _MapWidth, _MapHeight, 5); //Simple map set up just for rock areas
        //Rocks
        for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
            {
                if (_BubbleOut[_MapCountX, _MapCountY])
                {
                    _BasicMap[_MapCountX, _MapCountY] = 1;
                }

            }
        }

        _BubbleOut = GenerateBubbleMap(0.55f, _MapWidth, _MapHeight, 2); //Simple map set up just for water areas
        for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
            {
                if (!_BubbleOut[_MapCountX, _MapCountY])
                {
                    _BubbleOut[_MapCountX, _MapCountY] = Random.value > 0.4f;
                }

            }
        }
        _BubbleOut = ApplyBubbleFilter(_BubbleOut, _MapWidth, _MapHeight, 1);
        bool[,] _BubbleOutFalse = _BubbleOut;
        for (int _RepeatCount = 0; _RepeatCount < 0; _RepeatCount++)
        {
            
            _BubbleOut = _BubbleOutFalse;
            for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
            {
                for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
                {
                    if (!_BubbleOut[_MapCountX, _MapCountY])
                    {
                        if (_MapCountX < _MapWidth - 1)
                        {
                            if (_MapCountY < _MapHeight - 1)
                            {
                                if (!_BubbleOut[_MapCountX + 1, _MapCountY + 1])
                                {
                                    _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                                }
                            }
                            if (_MapCountY > 0)
                            {
                                if (!_BubbleOut[_MapCountX + 1, _MapCountY - 1])
                                {
                                    _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                                }
                            }
                            if (!_BubbleOut[_MapCountX + 1, _MapCountY])
                            {
                                _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                            }
                        }
                        if (_MapCountX > 0)
                        {
                            if (_MapCountY < _MapHeight - 1)
                            {
                                if (!_BubbleOut[_MapCountX - 1, _MapCountY + 1])
                                {
                                    _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                                }
                            }
                            if (_MapCountY > 0)
                            {
                                if (!_BubbleOut[_MapCountX - 1, _MapCountY - 1])
                                {
                                    _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                                }
                            }
                            if (!_BubbleOut[_MapCountX - 1, _MapCountY])
                            {
                                _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                            }

                        }
                        if (_MapCountY < _MapHeight - 1)
                        {
                            if (!_BubbleOut[_MapCountX, _MapCountY + 1])
                            {
                                _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                            }
                        }
                        if (_MapCountY > 0)
                        {
                            if (!_BubbleOut[_MapCountX, _MapCountY - 1])
                            {
                                _BubbleOutFalse[_MapCountX, _MapCountY] = true;
                            }
                        }
                        

                    }

                }
            }
        }

        for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
            {
                if (!_BubbleOut[_MapCountX, _MapCountY])
                {
                    if (_MapCountX < _MapWidth - 1)
                    {
                        if (_MapCountY < _MapHeight - 1)
                        {
                            if (_BubbleOut[_MapCountX + 1, _MapCountY + 1])
                            {
                                _BasicMap[_MapCountX, _MapCountY] = 3;
                            }
                        }
                        if (_MapCountY > 0)
                        {
                            if (_BubbleOut[_MapCountX + 1, _MapCountY - 1])
                            {
                                _BasicMap[_MapCountX, _MapCountY] = 3;
                            }
                        }
                        if (_BubbleOut[_MapCountX + 1, _MapCountY])
                        {
                            _BasicMap[_MapCountX, _MapCountY] = 3;
                        }
                    }
                    if (_MapCountX > 0)
                    {
                        if (_MapCountY < _MapHeight - 1)
                        {
                            if (_BubbleOut[_MapCountX - 1, _MapCountY + 1])
                            {
                                _BasicMap[_MapCountX, _MapCountY] = 3;
                            }
                        }
                        if (_MapCountY > 0)
                        {
                            if (_BubbleOut[_MapCountX - 1, _MapCountY - 1])
                            {
                                _BasicMap[_MapCountX, _MapCountY] = 3;
                            }
                        }
                        if (_BubbleOut[_MapCountX - 1, _MapCountY])
                        {
                            _BasicMap[_MapCountX, _MapCountY] = 3;
                        }
                    }
                    if (_MapCountY < _MapHeight - 1)
                    {
                        if (_BubbleOut[_MapCountX, _MapCountY + 1])
                        {
                            _BasicMap[_MapCountX, _MapCountY] = 3;
                        }
                    }
                    if (_MapCountY > 0)
                    {
                        if (_BubbleOut[_MapCountX, _MapCountY - 1])
                        {
                            _BasicMap[_MapCountX, _MapCountY] = 3;
                        }
                    }

                }
                else
                {
                    _BasicMap[_MapCountX, _MapCountY] = 2;
                }
            }
        }

        //Sea and beach
        for (int _MapCountX = 0; _MapCountX < _MapHeight; _MapCountX++)
        {
            if (Random.Range(0, 2) == 1)
            {

                _BasicMap[_MapCountX, 3] = 5;
            }
            else
            {
                _BasicMap[_MapCountX, 3] = 3;

            }
        }
        for (int _MapCountX = 0; _MapCountX < _MapHeight; _MapCountX++)
        {
            _BasicMap[_MapCountX, 2] = 5;
            _BasicMap[_MapCountX, 1] = 5;
            _BasicMap[_MapCountX, 0] = 5;
        }
        for (int _MapCountX = 0; _MapCountX < _MapHeight; _MapCountX++)
        {
            _BasicMap[_MapCountX, 4] = 3;
        }

        ////Rivers
        //for (int i = 0; i < 10; i++) //River Count
        //{
        //    //Starts by using sine wave to generate path
        //}

        //Convert to map
        for (int _MapCountX = 0; _MapCountX < _MapWidth; _MapCountX++)
        {
            for (int _MapCountY = 0; _MapCountY < _MapHeight; _MapCountY++)
            {
                _OutMap[_MapCountX, _MapCountY] = new Tile();
                _OutMap[_MapCountX, _MapCountY].Fertility = Random.Range(TileInfo[_BasicMap[_MapCountX, _MapCountY]].FertilityMin, TileInfo[_BasicMap[_MapCountX, _MapCountY]].FertilityMax);
                _OutMap[_MapCountX, _MapCountY].Metal = Random.Range(TileInfo[_BasicMap[_MapCountX, _MapCountY]].MetalMin, TileInfo[_BasicMap[_MapCountX, _MapCountY]].MetalMax);
                _OutMap[_MapCountX, _MapCountY].Wood = Random.Range(TileInfo[_BasicMap[_MapCountX, _MapCountY]].WoodMin, TileInfo[_BasicMap[_MapCountX, _MapCountY]].WoodMax);
                _OutMap[_MapCountX, _MapCountY].Stone = Random.Range(TileInfo[_BasicMap[_MapCountX, _MapCountY]].StoneMin, TileInfo[_BasicMap[_MapCountX, _MapCountY]].StoneMax);
                _OutMap[_MapCountX, _MapCountY].Type = _BasicMap[_MapCountX, _MapCountY];
            }
        }

        return new Map(_OutMap, _MapWidth, _MapHeight);
    }

    private bool[,] ApplyBubbleFilter(bool[,] _Seed, int _MapWidth, int _MapHeight, int _Repeats)
    {
        bool[,] _MapOut = _Seed;
        for (int i = 0; i < _Repeats; i++)
        {
            for (int _MapOutX = 0; _MapOutX < _MapWidth; _MapOutX++)
            {
                for (int _MapOutY = 0; _MapOutY < _MapHeight; _MapOutY++)
                {
                    int _EdgeCount = 0;

                    if (_MapOutY > 0)
                    {
                        if (_MapOut[_MapOutX, _MapOutY - 1] == _MapOut[_MapOutX, _MapOutY])
                        {
                            _EdgeCount++;
                        }
                    }

                    if (_MapOutY < _MapHeight - 2)
                    {
                        if (_MapOut[_MapOutX, _MapOutY + 1] == _MapOut[_MapOutX, _MapOutY])
                        {
                            _EdgeCount++;
                        }
                    }

                    if (_MapOutX > 0)
                    {
                        if (_MapOut[_MapOutX - 1, _MapOutY] == _MapOut[_MapOutX, _MapOutY])
                        {
                            _EdgeCount++;
                        }

                        if (_MapOutY > 0)
                        {
                            if (_MapOut[_MapOutX - 1, _MapOutY - 1] == _MapOut[_MapOutX, _MapOutY])
                            {
                                _EdgeCount++;
                            }
                        }

                        if (_MapOutY < _MapHeight - 2)
                        {
                            if (_MapOut[_MapOutX - 1, _MapOutY + 1] == _MapOut[_MapOutX, _MapOutY])
                            {
                                _EdgeCount++;
                            }
                        }
                    }

                    if (_MapOutX < _MapWidth - 2)
                    {
                        if (_MapOut[_MapOutX + 1, _MapOutY] == _MapOut[_MapOutX, _MapOutY])
                        {
                            _EdgeCount++;
                        }

                        if (_MapOutY > 0)
                        {
                            if (_MapOut[_MapOutX + 1, _MapOutY - 1] == _MapOut[_MapOutX, _MapOutY])
                            {
                                _EdgeCount++;
                            }
                        }

                        if (_MapOutY < _MapHeight - 2)
                        {
                            if (_MapOut[_MapOutX + 1, _MapOutY + 1] == _MapOut[_MapOutX, _MapOutY])
                            {
                                _EdgeCount++;
                            }
                        }
                    }

                    if (_EdgeCount < 4)
                    {
                        _MapOut[_MapOutX, _MapOutY] = !_MapOut[_MapOutX, _MapOutY];
                    }
                }
            }
        }
        return _MapOut;

    }

    private bool[,] GenerateBubbleMap(float Percentage, int _MapWidth, int _MapHeight, int _Repeats)
    {
        bool[,] _MapOut = new bool[_MapWidth, _MapHeight];

        for (int _MapOutX = 0; _MapOutX < _MapWidth; _MapOutX++)
        {
            for (int _MapOutY = 0; _MapOutY < _MapHeight; _MapOutY++)
            {

                _MapOut[_MapOutX, _MapOutY] = Random.Range(0f, 1f) < Percentage;
            }
        }


        return ApplyBubbleFilter(_MapOut, _MapWidth, _MapHeight, _Repeats);
    }
}

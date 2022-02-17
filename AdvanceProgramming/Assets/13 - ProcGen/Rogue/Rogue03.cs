using UnityEngine;
using UnityEngine.Tilemaps;

/* [ROGUE 03]
 * 
 * Divides the level in 9 regions and creates rooms in each,
 * with a given probability.
 * 
 * Connected rooms.
 */
// https://github.com/mikeyk730/Rogue-Collection
public class Rogue03 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;


    [Header("Rooms")]
    public Vector2Int RoomsCount; // how many rooms on x and y axes
    private Vector2Int[,] Rooms; // The position of each room;

    [Header("Room Tiles")]
    public Tile RoomH;
    public Tile RoomV;
    public Tile RoomNE;
    public Tile RoomSE;
    public Tile RoomSW;
    public Tile RoomNW;

    [Space]
    public Tile FloorTile;

    [Header("Corridor Tiles")]
    public Tile DoorTile;
    public Tile CorridorTile;


    [Button(Editor = true)]
    void Start()
    {
        Tilemap.ClearAllTiles();

        // Creates the rooms
        Rooms = new Vector2Int[RoomsCount.x, RoomsCount.y];
        for (int x = 0; x < RoomsCount.x; x++)
            for (int y = 0; y < RoomsCount.y; y++)
                Rooms[x,y] = CreateRandomRoom
                (
                    new Vector2Int
                    (
                        (int)(x * (Size.x / (float)RoomsCount.x)),
                        (int)(y * (Size.y / (float)RoomsCount.y))
                    ),
                    new Vector2Int
                    (
                        Size.x / RoomsCount.x,
                        Size.y / RoomsCount.y
                    )
                );





        for (int x = 0; x < RoomsCount.x - 1; x++)
            for (int y = 0; y < RoomsCount.y - 1; y++)
            {
                CreateCorridor(Rooms[x, y], Rooms[x + 1, y]); // Corridor to room to the right
                CreateCorridor(Rooms[x, y], Rooms[x, y + 1]); // Corridor to room to the top
            }

        //CreateCorridor(new Vector2Int(10, 10), new Vector2Int(20, 15));
        //CreateCorridor(new Vector2Int(20, 10), new Vector2Int(25, 30));

        //for (int x = 0; x < Size.x; x++)
        //    for (int y = 0; y < Size.y; y++)
        //        Tilemap.SetTile(new Vector3Int(x, y, 0), FloorTile);
    }

    #region Rooms
    // Creates a random room inside a region
    // areaStart: The start of the region
    // areaSize:  The size of the region
    // Returns the center of the room
    //  (used to create corridors)
    Vector2Int CreateRandomRoom(Vector2Int areaOffset, Vector2Int areaSize)
    {
        // Room size in Rogue:
        // 4x4 to 25x7
        Vector2Int size = new Vector2Int
        (
            Random.Range(4, areaSize.x),
            Random.Range(4, areaSize.y)
        );
        Vector2Int offset = areaOffset + new Vector2Int
        (
            Random.Range(0, areaSize.x - size.x),
            Random.Range(0, areaSize.y - size.y)
        );

        CreateRoom(offset, size);

        // Center of the room
        return offset + size / 2;
    }



    // Creates a room, given a location (offset) and size
    void CreateRoom(Vector2Int offset, Vector2Int size)
    {
        // Walls and floor
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                Vector3Int position = (Vector3Int)(offset + new Vector2Int(x, y));
                Tile tile;
                if (x == 0 || x == size.x - 1)
                    tile = RoomV;
                else
                if (y == 0 || y == size.y - 1)
                    tile = RoomH;
                else
                    tile = FloorTile;

                // Floor
                Tilemap.SetTile(position, tile);
            }

        // Corners
        Tilemap.SetTile
        (
            new Vector3Int
            (
                offset.x + size.x - 1,
                offset.y + size.y - 1,
                0
            ),
            RoomNE
        );
        Tilemap.SetTile
        (
            new Vector3Int
            (
                offset.x + size.x - 1,
                offset.y,
                0
            ),
            RoomSE
        );
        Tilemap.SetTile
        (
            new Vector3Int
            (
                offset.x,
                offset.y,
                0
            ),
            RoomSW
        );
        Tilemap.SetTile
        (
            new Vector3Int
            (
                offset.x,
                offset.y + size.y - 1,
                0
            ),
            RoomNW
        );
    }
    #endregion

    #region Corridors
    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int delta = end - start;

        if (delta.x >= delta.y)
            CreateHorizontalCorridor(start, end);
        else
            CreateVerticalCorridor(start, end);
    }


    /* delta.x > delta.y
     * O++++++++
     *         +
     *         ++++++++O
     */
    void CreateHorizontalCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int delta = end - start;

        for (int x = 0; x <= delta.x / 2; x++)
            SetCorridorTile(start + new Vector2Int(x, 0));

        for (int y = 0; y <= delta.y; y++)
            SetCorridorTile(start + new Vector2Int(delta.x / 2, y));

        for (int x = delta.x / 2; x <= delta.x; x++)
            SetCorridorTile(start + new Vector2Int(x, delta.y));
    }

    /* delta.x < delta.y
     * O
     * +
     * +
     * +++
     *   +
     *   +
     *   O
     */
    void CreateVerticalCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int delta = end - start;

        for (int y = 0; y <= delta.y / 2; y++)
            SetCorridorTile(start + new Vector2Int(0, y));

        for (int x = 0; x <= delta.x; x++)
            SetCorridorTile(start + new Vector2Int(x, delta.y / 2));

        for (int y = delta.y / 2; y <= delta.y; y++)
            SetCorridorTile(start + new Vector2Int(delta.x, y));
    }


    // Sets a corridor tile
    // If it overlaps a room border, it creates a door
    // It does nothing inside rooms
    void SetCorridorTile (Vector2Int position)
    {
        // Does nothing inside rooms
        if (Tilemap.GetTile((Vector3Int)position) == FloorTile)
            return;

        
        Tile tile;

        // Wall?
        if (IsRoomWall(position))
            tile = DoorTile;
        else
            tile = CorridorTile;

        Tilemap.SetTile
        (
            (Vector3Int) position,
            tile
        );
    }

    // True on a room wall
    bool IsRoomWall(Vector2Int position)
    {
        Tile tile = Tilemap.GetTile((Vector3Int) position) as Tile;
        if (tile == RoomH ||
            tile == RoomV ||
            tile == RoomNE ||
            tile == RoomSE ||
            tile == RoomSW ||
            tile == RoomNW ||
            tile == DoorTile)
            return true;
        return false;
    }
    #endregion
}

using UnityEngine;
using UnityEngine.Tilemaps;

/* [ROGUE 02]
 * 
 * Divides the level in 9 regions and creates rooms in each,
 * with a given probability.
 */
// https://github.com/mikeyk730/Rogue-Collection
public class Rogue02 : MonoBehaviour
{
    public Tilemap Tilemap;
    public Vector2Int Size;


    [Header("Rooms")]
    public Vector2Int RoomsCount; // how many rooms on x and y axes

    [Header("Room Tiles")]
    public Tile RoomH;
    public Tile RoomV;
    public Tile RoomNE;
    public Tile RoomSE;
    public Tile RoomSW;
    public Tile RoomNW;

    [Space]
    public Tile FloorTile;


    [Button(Editor=true)]
    void Start()
    {
        Tilemap.ClearAllTiles();

        // Creates the rooms
        for (int x = 0; x < RoomsCount.x; x++)
            for (int y = 0; y < RoomsCount.y; y++)
                CreateRandomRoom
                (
                    new Vector2Int
                    (
                        (int) (x * (Size.x / (float) RoomsCount.x)),
                        (int) (y * (Size.y / (float) RoomsCount.y))
                    ),
                    new Vector2Int
                    (
                        Size.x / RoomsCount.x,
                        Size.y / RoomsCount.y
                    )
                );


        //for (int x = 0; x < Size.x; x++)
        //    for (int y = 0; y < Size.y; y++)
        //        Tilemap.SetTile(new Vector3Int(x, y, 0), FloorTile);
    }


    // Creates a random room inside a region
    // areaStart: The start of the region
    // areaSize:  The size of the region
    void CreateRandomRoom(Vector2Int areaOffset, Vector2Int areaSize)
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
}

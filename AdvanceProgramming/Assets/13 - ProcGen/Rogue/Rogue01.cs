using UnityEngine;
using UnityEngine.Tilemaps;

/* [ROGUE 01]
 * 
 * Creates a room in the level
 */
// https://github.com/mikeyk730/Rogue-Collection
public class Rogue01 : MonoBehaviour
{
    public Tilemap Tilemap;


    [Header("Room")]
    public Vector2Int Offset;
    public Vector2Int Size;

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

        // Creates the room
        CreateRoom(Offset, Size);
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

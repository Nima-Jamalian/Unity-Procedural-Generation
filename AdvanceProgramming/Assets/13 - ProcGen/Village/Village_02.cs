using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [VILLAGE 02]
 * Creates a random village/city.
 * 
 * Places one tree randomly within an area.
 * 
 * Places trees on terrain.
 */
public class Village_02 : MonoBehaviour
{
    [Header("Props")]
    public GameObject TreePrefab;
    public Transform Centre;
    [Range(0,100)]
    public float Radius;
    [Range(1,1000)]
    public int Trees;

    [Header("Terrain")]
    public LayerMask TerrainMask;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateTrees();
    }

    private void InstantiateTrees()
    {
        for (int i = 0; i < Trees; i ++)
        {
            InstantiateTree();
        }
    }

    private void InstantiateTree ()
    {
        // insideUnitCircle returns (x,y), but we need (x,z)
        Vector2 unitCircle = Random.insideUnitCircle * Radius;
        Vector3 position = Centre.position +
            new Vector3(unitCircle.x, 0f, unitCircle.y);

        // Position is on the sky
        // We need to raycast down to find the terrain height
        position = TerrainHeightAt(position);


        // Random quaternion around y axis
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0f);

        Instantiate(TreePrefab, position, rotation, transform);
    }

    private Vector3 TerrainHeightAt(Vector3 position)
    {
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(position, Vector3.down, out hitInfo, 10f, TerrainMask);
        // Tree is not on top of the ground! (error?)
        if (!hit)
            return position;

        position.y = hitInfo.point.y;
        return position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [VILLAGE 01]
 * Creates a random village/city.
 * 
 * Places one tree randomly within an area.
 */
public class Village_01 : MonoBehaviour
{
    [Header("Props")]
    public GameObject TreePrefab;
    public Transform Centre;
    [Range(0,100)]
    public float Radius;
    [Range(1,1000)]
    public int Trees;


    // Start is called before the first frame update
    void Start()
    {
        InstantiateTrees();
    }

    private void InstantiateTrees()
    {
        for (int i = 0; i < Trees; i ++)
        {
            // insideUnitCircle returns (x,y), but we need (x,z)
            Vector2 unitCircle = Random.insideUnitCircle * Radius;
            Vector3 position = Centre.position +
                new Vector3(unitCircle.x, 0f, unitCircle.y);

            // Random quaternion around y axis
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0f);

            Instantiate(TreePrefab, position, rotation, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

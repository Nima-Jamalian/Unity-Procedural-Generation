using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProGen01 : MonoBehaviour
{
    //[SerializeField] List<Color> colors;
    [SerializeField] int count = 10;

    //[Range(0f,1f)]
    //[SerializeField] float s;
    //[Range(0f, 1f)]
    //[SerializeField] float v;

    //[SerializeField] Gradient gradient;

    [SerializeField] List<GameObject> treePrefabs;
    [SerializeField] float boxSize;
    //animation curve can be used for randomise the size as well instead of definding in the code
    //[SerializeField] AnimationCurve animationCurve;
    // Start is called before the first frame update
    void Start()
    {
        //Random.InitState(123);
        for(int i =0; i<count; i++)
        {
            //Gnerating random colours
            //colors.Add(new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

            //Gnerating random colours that are similiar to each other
            //colors.Add(Color.HSVToRGB(Random.Range(0f, 1f), s, v));

            //Gnerating random colours using gradient 
            //colors.Add(gradient.Evaluate(Random.Range(0f, 1f)));

            //Defending a box area for spawning the objects
            Vector3 position = transform.position + new Vector3(Random.Range(-boxSize, +boxSize), 0, Random.Range(-boxSize, +boxSize));


            Quaternion rotation = Quaternion.Euler(0f, Random.Range(-180f, +180f), 0f);

            GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Count)];
            //Spawning the trees in random location defined inside the box
            GameObject tree = Instantiate(prefab, position, rotation);
            //Randomise the tree size
            tree.transform.localScale = Random.Range(0.5f,1f) * Vector3.one;
        }


        //Generate random event based on the perecntage of how lilkley it is that the event is going to happen
        //if(Random.Range(0f,1f) <= 0.75)
        //{
        //    Debug.Log("75%");
        //} else
        //{
        //    Debug.Log("25%");
        //}
    }

}

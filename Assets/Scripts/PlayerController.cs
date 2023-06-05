using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject box;


    [SerializeField] private Transform throwPoint;

    private GameObject throwableBoxPrefabs;

    public void ThrowBoxes()
    {
        //throw no. of boxes based on each word's length

        //instantiate
        throwableBoxPrefabs = Instantiate(box, throwPoint);

        //throw the box
        throwableBoxPrefabs.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1f, 1f), ForceMode2D.Impulse);

        Invoke("DestroyBoxes", 5f);
    }

    public void DestroyBoxes()
    {
        Destroy(throwableBoxPrefabs);
    }
}

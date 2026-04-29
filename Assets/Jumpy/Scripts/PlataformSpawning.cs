using UnityEngine;



public class PlatformSpawner : MonoBehaviour

{



    public GameObject platformPrefab;

    public Transform initialPlatform;

    public int numPlatforms = 7;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()

    {

        setPlatforms();

    }



    // Update is called once per frame

    void Update()

    {



    }



    void setPlatforms()

    {

        for (int i = 0; i < numPlatforms; i++)

        {

            float randomX = Random.Range(-4f, 6f);

            float desplazamientoVertical = (i * 3) + 7;

            float desplazamientoHorizontal = randomX;



            Vector3 platformPosition = initialPlatform.position + new Vector3(desplazamientoHorizontal, desplazamientoVertical, 0);



            Instantiate(platformPrefab, platformPosition, platformPrefab.transform.rotation);

        }

    }

}
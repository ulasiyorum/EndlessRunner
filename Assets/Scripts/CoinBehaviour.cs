using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] int point;
    private static LinkedList<GameObject> linkedCoins; 
    void Start()
    {
        LinkCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LinkCoins()
    {
        if(linkedCoins == null)
        {
            int size = UnityEngine.Random.Range(0, 8);
            linkedCoins = new LinkedList<GameObject>();
            linkedCoins.AddFirst(gameObject);

            for(int i = 0; i < size; i++)
            {
                GameObject go = Instantiate(gameObject);
                GameObject last = linkedCoins.Last.Value;
                float scale = go.GetComponent<MeshCollider>().bounds.size.z * 12;
                go.transform.position = new Vector3(transform.position.x, transform.position.y, last.transform.position.z + scale);
                linkedCoins.AddLast(go);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameHandler.Instance.profile.CollectCoin(point);
        // play collect animation
        Destroy(this.gameObject);
    }
}

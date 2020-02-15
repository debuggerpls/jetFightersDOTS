using System;
using System.Net.Mime;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public GameObject playerBullet0;
    public GameObject playerBullet1;
    
    [SerializeField]
    public Entity playerBullet0Entity;
    [SerializeField]
    public Entity playerBullet1Entity;


    [SerializeField] public Vector3 player0StartPosition;
    [SerializeField] public Vector3 player1StartPosition;
    [SerializeField] 
    public float bulletTTL = 3f;
    [SerializeField]
    public float xBound = 37f;
    [SerializeField]
    public float yBound = 20f;
    
    [SerializeField]
    public Text[] scoreTexts;
    [SerializeField]
    private int[] scores;

    private EntityManager manager;
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
        
        scores = new int[2];
        
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        playerBullet0Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerBullet0, settings);
        playerBullet1Entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerBullet1, settings);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    public void UpdateScores()
    {
        for (int i = 0; i < scores.Length && i < scoreTexts.Length ; i++)
        {
            scoreTexts[i].text = scores[i].ToString();
        }
    }

    public void Scored(int playerId)
    {
        scores[playerId]++;
        //UpdateScores();
    }

    private void LateUpdate()
    {
        UpdateScores();
    }

    public void Restart()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] mapParts;
    public GameObject mapPartsHolder;
    public GameObject envParts;
    public GameObject envPartsHolder;
    public GameObject[] blocks;
    public GameObject obstaclesHolder;
    public GameObject coin;
    public MeshRenderer startPart;
    public GameObject endCanvas;
    public Text endScore;
    private Vector3 mapSpawnPosition;
    private float tile = 3.75f;
    private System.Random random = new System.Random();
    private MeshRenderer envPartRenderer;
    void Start()
    {
        mapSpawnPosition.z = startPart.transform.parent.transform.position.z;
        envPartRenderer = envParts.transform.Find("GroundIsland").GetComponent<MeshRenderer>();
        StartCoroutine(MapSpawn());
    }

    void Update()
    {
        
    }

    IEnumerator MapSpawn()
    {
        yield return new WaitForSeconds(1);
        int index = random.Next(0, mapParts.Length);
        GameObject partToSpawn = mapParts[index];
        var renderer = partToSpawn.transform.Find("Ground").GetComponent<MeshRenderer>();
        mapSpawnPosition.z = mapSpawnPosition.z - renderer.bounds.extents.z-tile;
        Instantiate(mapParts[index], mapSpawnPosition, mapParts[index].transform.rotation).transform.parent = mapPartsHolder.transform;
        SpawnEnv(mapSpawnPosition, renderer);
        SpawnBlocks(mapSpawnPosition);
        SpawnCoins(mapSpawnPosition, renderer);
        StartCoroutine(MapSpawn());
    }

    void SpawnEnv(Vector3 platformPosition, MeshRenderer platformRenderer)
    {
        int index = random.Next(1, 3);
        if (index == 2)
        {
            SpawnTree(-1, platformPosition, platformRenderer);
            SpawnTree(1, platformPosition, platformRenderer);

        }
    }

    void SpawnTree(int x, Vector3 platformPosition, MeshRenderer platformRenderer)
    {
        var xPosition = x*(platformRenderer.bounds.extents.x + envPartRenderer.bounds.extents.x +
                        platformPosition.x);
        var yPosition = platformPosition.y - 2f;
        var zPosition =
            Random.Range(-platformRenderer.bounds.extents.z + 1, platformRenderer.bounds.extents.z - 1) +
            platformPosition.z;
        var pos = new Vector3(xPosition, yPosition, zPosition);
        var obj = Instantiate(envParts, pos, envParts.transform.rotation);
        var scale = Random.Range(1, 2f);
        obj.transform.localScale = new Vector3(scale, scale, scale);
    }

    void SpawnBlocks(Vector3 platformPosition)
    {
        var positionArray  = new List<int>();
        positionArray.Add(-1);
        positionArray.Add(0);
        positionArray.Add(1);
        int x = random.Next(1, 3);
        switch (x)
        {
            case 1:
                SpawnBlock(platformPosition, random.Next(-1,2));
                break;
            case 2:
                int secondIndex = random.Next(0, positionArray.Count);
                SpawnBlock(platformPosition, positionArray[secondIndex]);
                positionArray.Remove(secondIndex);
                secondIndex = random.Next(0, positionArray.Count);
                SpawnBlock(platformPosition, positionArray[secondIndex]);
                break;

        }

    }

    void SpawnBlock(Vector3 platformPosition, int xPos)
    {
        int index = random.Next(0, blocks.Length);
        var pos = new Vector3(xPos, 0, platformPosition.z);
        Instantiate(blocks[index], pos, blocks[index].transform.rotation).transform.parent = obstaclesHolder.transform;
    }

    void SpawnCoins(Vector3 platformPosition, MeshRenderer platformRenderer)
    {
        int i = random.Next(0, 2);
        int xPos = random.Next(-1, 2);
        var pos = new Vector3();
        switch (i)
        {
            case 0:
                pos = new Vector3(xPos, platformPosition.y + 0.5f, (platformPosition.z-platformRenderer.bounds.extents.z/2f));
                Instantiate(coin, pos, coin.transform.rotation);
                break;
            case 1:
                pos = new Vector3(xPos, platformPosition.y + 0.5f, (platformPosition.z+platformRenderer.bounds.extents.z/2f));
                Instantiate(coin, pos, coin.transform.rotation);
                break;

        }
        
    }

    void ShowCanvas()
    {
        endCanvas.SetActive(true);
        endScore.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>().ScoreValue.text;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    void OnEnable()
    {
        EventManager.StartListening(EventManager.EndGame, ShowCanvas);

    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.EndGame, ShowCanvas);


    }

}

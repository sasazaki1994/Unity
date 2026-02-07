using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject fallingPrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float destroyY = -6f;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverText;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();
    private float elapsedTime;
    private bool isGameOver;

    private void Start()
    {
        if (!ValidateSetup())
        {
            enabled = false;
            return;
        }

        elapsedTime = 0f;
        isGameOver = false;

        UpdateScoreText();
        gameOverText.text = "Game Over\nPress R to Restart";
        gameOverText.gameObject.SetActive(false);

        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime;
            UpdateScoreText();
        }

        CleanupFallingObjects();
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
    }

    private IEnumerator SpawnLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);

        while (!isGameOver)
        {
            float spawnX = Random.Range(minX, maxX);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
            GameObject fallingObject = Instantiate(fallingPrefab, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(fallingObject);

            yield return wait;
        }
    }

    private void CleanupFallingObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject obj = spawnedObjects[i];

            if (obj == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (obj.transform.position.y < destroyY)
            {
                Destroy(obj);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {elapsedTime:F1}";
    }

    private bool ValidateSetup()
    {
        bool isValid = true;

        if (fallingPrefab == null)
        {
            Debug.LogError("GameController: Falling prefab reference is not set.", this);
            isValid = false;
        }

        if (scoreText == null)
        {
            Debug.LogError("GameController: Score Text reference is not set.", this);
            isValid = false;
        }

        if (gameOverText == null)
        {
            Debug.LogError("GameController: Game Over Text reference is not set.", this);
            isValid = false;
        }

        if (spawnInterval <= 0f)
        {
            Debug.LogError("GameController: Spawn interval must be greater than 0.", this);
            isValid = false;
        }

        if (minX > maxX)
        {
            Debug.LogError("GameController: minX must be less than or equal to maxX.", this);
            isValid = false;
        }

        return isValid;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager sharedInstance;
    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>();
    public Transform levelStartPosition;

    private void Awake()
    {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    void Start()
    {
        generateInitialBlocks();
    }


    void Update()
    {
        
    }

    public void AddLevelBlock()
    {
        int randomIDx =  UnityEngine.Random.Range(0, allTheLevelBlocks.Count);
        LevelBlock block;
        Vector3 spawnPosition = Vector3.zero;

        if (currentLevelBlocks.Count == 0)
        {
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = levelStartPosition.position;
        }
        else
        {
            block = Instantiate(allTheLevelBlocks[randomIDx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].exitPoint.position;
        }
        block.transform.SetParent(this.transform, false);
        Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x,
            spawnPosition.y - block.startPoint.position.y, 0);
        block.transform.position = correction;
        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelBlock() {
        while (currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }

    public void generateInitialBlocks() {
        for (int i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}

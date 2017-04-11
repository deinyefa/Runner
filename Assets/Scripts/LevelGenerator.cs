using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private int levelLength;
    [SerializeField]
    private int startPlatformLength = 5, endPlatformLength = 5;
    [SerializeField]
    private float distance_between_platforms;
    [SerializeField]
    private Transform platformPrefab, platform_parent;
    [SerializeField]
    private Transform monster, monster_parent;
    [SerializeField]
    private Transform health_collectable, healthCollectable_parent;
    [SerializeField]
    private float platformPosition_minY = 0, platformPosition_maxY = 10f;
    [SerializeField]
    private int platformLength_min = 1, platformLength_max = 4;
    [SerializeField]
    private float chanceForMonsterExistence = 0.25f, chanceForCollectableExistence = 0.1f;
    [SerializeField]
    private float healthCollectable_minY = 1f, healthCollectable_maxY = 3f;

    private float platformLastPositionX;

    private enum PlatformType
    {
        None,
        Flat
    }
	
    private class PlatformPositionInfo
    {
        public PlatformType platformType;
        public float positionY;
        public bool hasMonster;
        public bool hasHealthCollectable;

        public PlatformPositionInfo (PlatformType type, float posY, bool has_monster, bool has_collectable)
        {
            platformType = type;
            positionY = posY;
            hasMonster = has_monster;
            hasHealthCollectable = has_collectable;
        }
    }   //PlatformPositionInfo class

    private void Start()
    {
        GenerateLevel();
    } 

    void FillOutPositionInfo(PlatformPositionInfo[] platformInfo) {
        int currentPlatformInfoIndex = 0;

        for (int i = 0; i < startPlatformLength; i++)
        {
            platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
            platformInfo[currentPlatformInfoIndex].positionY = 0f;

            currentPlatformInfoIndex++;
        }

        while (currentPlatformInfoIndex < levelLength - endPlatformLength)
        {
            if (platformInfo[currentPlatformInfoIndex - 1].platformType != PlatformType.None)
            {
                currentPlatformInfoIndex++;
                continue;
            }

            float platformPositionY = Random.Range(platformPosition_minY, platformPosition_maxY);
            int platformLength = Random.Range(platformLength_min, platformLength_max);

            for (int i = 0; i < platformLength; i++)
            {
                bool has_Monster = (Random.Range(0, 1f) < chanceForMonsterExistence);
                bool has_healthCollectable = (Random.Range(0, 1f) < chanceForCollectableExistence);

                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = platformPositionY;
                platformInfo[currentPlatformInfoIndex].hasMonster = has_Monster;
                platformInfo[currentPlatformInfoIndex].hasHealthCollectable = has_healthCollectable;

                currentPlatformInfoIndex++;

                if (currentPlatformInfoIndex > (levelLength - endPlatformLength))
                {
                    currentPlatformInfoIndex = levelLength - endPlatformLength;
                    break;
                }
            }

            for (int i = 0; i < endPlatformLength; i++)
            {
                platformInfo[currentPlatformInfoIndex].platformType = PlatformType.Flat;
                platformInfo[currentPlatformInfoIndex].positionY = 0f;

                currentPlatformInfoIndex++;
            }
        }
    }

    void CreatePlatformsFromPositionInfo (PlatformPositionInfo[] platformPositionInfo)
    {
        for (int i = 0; i < platformPositionInfo.Length; i++)
        {
            PlatformPositionInfo positionInfo = platformPositionInfo[i];
            if (positionInfo.platformType == PlatformType.None)
            {
                continue;
            }

            Vector3 platformPosiiton = new Vector3(distance_between_platforms * i, positionInfo.positionY, 0);
            Transform createBlock = (Transform)Instantiate(platformPrefab, platformPosiiton, Quaternion.identity);
            createBlock.parent = platform_parent;

            // if current block has a collectable and if block has a monster

        }
    }

    public void GenerateLevel ()
    {
        PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];
        for (int i = 0; i < platformInfo.Length; i++)
        {
            platformInfo[i] = new PlatformPositionInfo(PlatformType.None, -1f, false, false);
        }

        FillOutPositionInfo(platformInfo);
        CreatePlatformsFromPositionInfo(platformInfo);
    }

}   // LevelGeneratorClass

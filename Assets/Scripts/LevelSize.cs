using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class LevelSize : MonoBehaviour
{
    public int NumberOfTilesWide;
    public int NumberOfTilesDeep;

    private int _defaultTileWidth = 3;
    private int _defaultTileDepth = 3;
    private float _blockSize = 5f;
    

    public void SetLevelSize()
    {
        transform.position = new Vector3((NumberOfTilesWide-_defaultTileWidth )*_blockSize/6,0,(_defaultTileDepth - NumberOfTilesDeep)*_blockSize/2);
        transform.localScale = new Vector3((float)NumberOfTilesWide/(float)_defaultTileWidth,1f,(float)NumberOfTilesDeep/(float)_defaultTileDepth);
    }

}

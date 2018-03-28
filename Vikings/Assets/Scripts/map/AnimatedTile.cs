using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimatedTile : Tile
{
    [SerializeField]
    private Sprite[] animationFrames;

    [SerializeField]
    private Sprite preview;

    public float speed = 1f;

    public float minStartTime;
    public float maxStartTime;

    public Tile.ColliderType m_TileColliderType;

    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
    {
        tileData.transform = Matrix4x4.identity;
        tileData.color = Color.white;
        if (animationFrames != null && animationFrames.Length > 0)
        {
            tileData.sprite = animationFrames[animationFrames.Length - 1];
            tileData.colliderType = m_TileColliderType;
        }
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        if (animationFrames.Length > 0)
        {
            tileAnimationData.animatedSprites = animationFrames;
            tileAnimationData.animationSpeed = speed;
            tileAnimationData.animationStartTime = Random.Range(minStartTime,maxStartTime);
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/AnimatedTile")]
    public static void CreateAnimatedTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save AnimatedTile", "New AnimatedTile", "asset", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AnimatedTile>(), path);
    }
#endif

}

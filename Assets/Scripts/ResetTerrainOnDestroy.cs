using UnityEngine;

public class ResetTerrainOnDestroy : MonoBehaviour
{
    #region Fields

    public Terrain Terrain;

    private float[,] originalHeights;

    #endregion

    #region Methods

    private void OnDestroy()
    {
        this.Terrain.terrainData.SetHeights(0, 0, this.originalHeights);
    }

    private void Start()
    {
        this.originalHeights = this.Terrain.terrainData.GetHeights(
            0, 0, this.Terrain.terrainData.heightmapWidth, this.Terrain.terrainData.heightmapHeight);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    bool stepped = false;

    public Material tileMaterial;
    public Material steppedTileMaterial;

    public LayerMask layerMask;

    private MeshRenderer meshRenderer;

    private ScoreManager scoreManager;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stepped)
        {
            stepped = true;
            meshRenderer.material = steppedTileMaterial;

            scoreManager.AddScore(1);
        }
    }
}

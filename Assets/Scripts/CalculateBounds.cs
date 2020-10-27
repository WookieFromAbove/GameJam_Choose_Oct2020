using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBounds : MonoBehaviour
{
    [SerializeField]
    private GameObject _coffin;
    private Mesh _coffinMesh;
    private Vector3 _coffinMeshBounds;
    private Vector3[] _coffinVertices;

    [SerializeField]
    private GameObject _block;
    private Vector2[] _uvs;

    private float _targetX = 3.75f;
    private float _targetY = 0.75f;
    private float _targetZ = 1.25f;

    private Vector3 _targetScale;


    private void Start()
    {
        _coffinMesh = _coffin.GetComponent<MeshFilter>().mesh;
        //CalculateSize();
        _targetX = _block.transform.localScale.x;
        _targetY = _block.transform.localScale.y;
        _targetZ = _block.transform.localScale.z;

        NewScale(_coffin, _targetX, _targetY, _targetZ);
    }

    private void NewScale(GameObject obj, float newSizeX, float newSizeY, float newSizeZ)
    {
        float sizeX = obj.GetComponent<Renderer>().bounds.size.x;
        float sizeY = obj.GetComponent<Renderer>().bounds.size.y;
        float sizeZ = obj.GetComponent<Renderer>().bounds.size.z;

        Vector3 rescale = obj.transform.localScale;

        rescale.x = newSizeX * rescale.x / sizeX;
        rescale.y = newSizeY * rescale.y / sizeY;
        rescale.z = newSizeZ * rescale.z / sizeZ;

        obj.transform.localScale = rescale;
    }

    private void CalculateSize()
    {
        _targetScale = new Vector3(_targetX, _targetY, _targetZ);

        _coffinMeshBounds = _coffinMesh.bounds.size;

        var invertedBounds = new Vector3(1 / _coffinMeshBounds.x, 1 / _coffinMeshBounds.y, 1 / _coffinMeshBounds.z);

        var finalScale = Vector3.Scale(invertedBounds, _targetScale);

        var min = Mathf.Min(finalScale.x, finalScale.y);
        min = Mathf.Min(min, finalScale.z);


        transform.localScale = Vector3.one * min;
    }
}

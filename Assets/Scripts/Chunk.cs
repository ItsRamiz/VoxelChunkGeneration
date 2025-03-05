using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Overlays;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float x_offset;

    public float x_padding;
    public float y_padding;
    public float z_padding;

    public int chunkSize_X;
    public int chunkSize_Y;
    public int chunkSize_Z;

    public Vector3 vector1;
    public Vector3 vector2;
    public Vector3 vector3;

    public GameObject[,,] blocks;

    public Vector3 chunkOrigin;

    public GameObject blockPrefab;

    public bool Startup = true;

    void Start()
    {
        x_offset = World.Instance.x_offset;

        x_padding = World.Instance.x_padding;
        y_padding = World.Instance.y_padding;
        z_padding = World.Instance.z_padding;

        chunkSize_X = World.Instance.chunkSize_X;
        chunkSize_Y = World.Instance.chunkSize_Y;
        chunkSize_Z = World.Instance.chunkSize_Z;

        vector1 = World.Instance.vector1;
        vector2 = World.Instance.vector2;
        vector3 = World.Instance.vector3;

        blocks = new GameObject[chunkSize_X, chunkSize_Y, chunkSize_Z];

        chunkOrigin = new Vector3(0,1,0);

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        if (Startup)
        {
            InitializeBlockGrid();
            Startup = false;
        }
        else
        {
            // You can add implementation to handle Grid updates after the first initialzation.
        }
    }


    private void InitializeBlockGrid()
    {
        for (int x = 0; x < chunkSize_X; x++)
        {
            for (int y = 0; y < chunkSize_Y; y++)   
            {
                for (int z = 0; z < chunkSize_Z; z++)
                {
                    if (IsHidden(x, y, z))
                        continue;  // Skip instantiation for hidden blocks

                    Vector3 blockPosition = CalcBlockPosition(x, y, z);
                    GameObject block = CreateBlockInstance(blockPosition); // Create the block instance
                    blocks[x, y, z] = block;
                    TransformObject(block); // Transform mesh based on World vectors
                    AddCollidersToChildren(block); // Add colliders based on the object's new form
                }
            }
        }
    }

    private Vector3 CalcBlockPosition(int x, int y, int z) // Calculates block position based on world parameters.
    {                                                      // Parameters must be fine-tuned based on new passed objects.
            Vector3 blockPosition = new Vector3(
            x * x_padding + (y * x_offset),
            y * y_padding, 
            z * z_padding
        ) + chunkOrigin;

        return blockPosition;
    }

    private GameObject CreateBlockInstance(Vector3 blockPosition) // Creates block instance, as title says
    {
        GameObject blockGO = Instantiate(blockPrefab, blockPosition, Quaternion.identity, transform);
        AddCollidersToChildren(blockGO);
        return blockGO;
    }
    private bool IsInBounds(int x, int y, int z) // Returns whether block is in bounds of the chunk
    {
        if (x >= 0 && y >= 0 && z >= 0)
        {
            if (x < chunkSize_X && y < chunkSize_Y && z < chunkSize_Z)
                return true;
        }
        return false;
    }
    private bool IsHidden(int x, int y, int z)
    {

        if (y == chunkSize_Y - 1) // Topmost layer is never hidden
            return false;

        // Check all 6 adjacent neighbors if in bounds.
        int[][] directions = new int[][]
        {
        new int[] { -1,  0,  0 }, // Left
        new int[] {  1,  0,  0 }, // Right
        new int[] {  0, -1,  0 }, // Down
        new int[] {  0,  1,  0 }, // Up
        new int[] {  0,  0, -1 }, // Back
        new int[] {  0,  0,  1 }  // Front
        };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];
            int nz = z + dir[2];

            // If a neighbor is out of bounds, this block is exposed and NOT hidden
            if (!IsInBounds(nx, ny, nz))
                return false;
        }

        // If all surrounding blocks are inside the chunk, this block is hidden
        return true;
    }
    void AddCollidersToChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.mesh != null) // Ensure there's a valid mesh
            {
                MeshCollider collider = child.gameObject.GetComponent<MeshCollider>();
                if (collider == null)
                {
                    collider = child.gameObject.AddComponent<MeshCollider>(); // Add if missing
                }

                collider.sharedMesh = meshFilter.mesh; // Reassign transformed mesh to collider.
                collider.convex = true;
            }

            if (child.childCount > 0) // Recursive checking for nested children inside base children. 
            {
                AddCollidersToChildren(child.gameObject);
            }
        }
    }

    public void DestroyBlock(GameObject block)
    {
        for (int x = 0; x < chunkSize_X; x++)
        {
            for (int y = 0; y < chunkSize_Y; y++)
            {
                for (int z = 0; z < chunkSize_Z; z++)
                {
                    if (blocks[x, y, z] != null)
                    {
                        // If the given block is a direct child of the stored block
                        if (block.transform.parent == blocks[x, y, z].transform)
                        {
                            Destroy(block); // Destroy specefic child

                            modifiedBlocks.Add(new Vector3Int(x, y, z)); // Mark block as modified. (To prevent duplicates)

                            if (blocks[x, y, z].transform.childCount == 1) // Last remaining child
                            {
                                Destroy(blocks[x, y, z]); // Destroy parent if empty
                                blocks[x, y, z] = null;
                            }

                            UpdateBlocksAround(x, y, z);  // Show adjacent hidden blocks
                            return;
                        }
                    }
                }
            }
        }
    }

    private HashSet<Vector3Int> modifiedBlocks = new HashSet<Vector3Int>(); // Prevents duplicate blocks inside chunks.
    private void UpdateBlocksAround(int x, int y, int z)
    {
        int[][] directions = new int[][]
        {
        new int[] { -1,  0,  0 }, // Left
        new int[] {  1,  0,  0 }, // Right
        new int[] {  0, -1,  0 }, // Down
        new int[] {  0,  1,  0 }, // Up
        new int[] {  0,  0, -1 }, // Back
        new int[] {  0,  0,  1 }  // Front
        };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];
            int nz = z + dir[2];

            if (!IsInBounds(nx, ny, nz))
                continue;

            Vector3Int neighborPos = new Vector3Int(nx, ny, nz);

            // If the adjacent block was modified, do NOT regenerate it
            if (modifiedBlocks.Contains(neighborPos))
            {
                continue;
            }

            if (blocks[nx, ny, nz] == null) // Only generate missing blocks
            {
                Vector3 newBlockPosition = CalcBlockPosition(nx, ny, nz);
                GameObject block = CreateBlockInstance(newBlockPosition);
                blocks[nx, ny, nz] = block;
                TransformObject(block);
                AddCollidersToChildren(block); // Hi
            }
            else if (IsHidden(nx, ny, nz) == false) // If previously hidden, reveal it
            {
                blocks[nx, ny, nz].SetActive(true);
            }
        }
    }
    private Vector3 TransformVertex(Vector3 vertex) // Vertex transformation based on World Vectors.
    {
        Matrix4x4 transformationMatrix = new Matrix4x4(
            new Vector4(vector1.x, vector1.y, vector1.z, 0),
            new Vector4(vector2.x, vector2.y, vector2.z, 0),
            new Vector4(vector3.x, vector3.y, vector3.z, 0),
            new Vector4(0, 0, 0, 1)
        );

        return transformationMatrix.MultiplyPoint3x4(vertex);
    }
    private void TransformObject(GameObject myobject)
    {
        if (myobject == null)
        {
            Debug.LogError("No object assigned to Chunk");
            return;
        }

        MeshFilter[] meshFilters = myobject.GetComponentsInChildren<MeshFilter>(); // Mesh for all children.
        if (meshFilters.Length == 0)
        {
            Debug.LogError("No MeshFilters found on assigned object or children!");
            return;
        }

        foreach (MeshFilter meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices; // Get current vertices

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = TransformVertex(vertices[i]); // Apply custom transformation
            }

            mesh.vertices = vertices;  // Apply modified vertices
            mesh.RecalculateNormals(); // Recalculate normals
            mesh.RecalculateBounds();  // Recalculate bounds
        }
    }

}

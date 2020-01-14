using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for creating and placing a randomly generated stage (moon surface) with platforms
public class StageBuilder : MonoBehaviour
{
    public Material moonMat;
    private GameObject stage;

    private void Start()
    {
        //Set the gravity to match the moon's gravity
        Physics.gravity = new Vector3(0, -1.628F, 0);
        RebuildStage();
    }

    // Creates a stage with the specified material with valleys no lower than minY
    // and peaks no higher than maxY, in world space
    public void CreateStage(float minY, float maxY, Material mat)
    {
        stage = new GameObject("Stage");
        MeshFilter mf = stage.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = stage.AddComponent(typeof(MeshRenderer)) as MeshRenderer;

        int minX = -25;
        int maxX = 100;
        float minZ = -5f;
        float maxZ = 5f;
        int rangeX = Mathf.Abs(maxX - minX);
        float rangeY = Mathf.Abs(maxY - minY);

        Mesh m = new Mesh();
        
        Vector3[] vertices = new Vector3[rangeX * 4];
        Vector2[] uv = new Vector2[rangeX * 4];
        int[] tri = new int[(rangeX + 1) * 18];

        // Used for creating flat spots where the player can land
        var (platformsIndices, platformSizes) = generatePlatforms();
        float prevHeight = 0;
        int flatCount = 0;

        //verts and UVs
        for(int i = 0; i < rangeX; i++)
        {
            float heightLowerBound = Mathf.Max(0, prevHeight - 6f);
            float heightUpperBound = Mathf.Min(rangeY, prevHeight + 6f);
            float height = Random.Range(heightLowerBound, heightUpperBound);
            int vertIndex = i * 4;

            // If true, this is where a platform is, since the height remains consistent (flat)
            if (flatCount > 0)
            {
                height = prevHeight;
                flatCount--;
            }

            prevHeight = height;
            float x = minX + i;
            float y = minY + height;

            // If there is supposed to be a platform starting at this x index,
            // create the platform and ensure the next few indices have the same flat height
            if (platformsIndices.Contains(i))
            {
                float platformSize = platformSizes[platformsIndices.IndexOf(i)];
                flatCount = (int) platformSize;

                Vector3 platformPos = new Vector3(x + (platformSize / 2f), height, 0f);
                var platform = Instantiate(Resources.Load("Prefabs/Platform"), stage.transform) as GameObject;
                platform.GetComponent<Platform>().Place(platformPos, platformSize);
            }

            vertices[vertIndex] = new Vector3(x, minY - 20, minZ);
            vertices[vertIndex + 1] = new Vector3(x, y, minZ);
            vertices[vertIndex + 2] = new Vector3(x, y, maxZ);
            vertices[vertIndex + 3] = new Vector3(x, minY - 20, maxZ);

            float u = (float) i / (float) rangeX;
            float v = (height + 20) / (rangeY + 20);

            uv[vertIndex] = new Vector2(u, 0);
            uv[vertIndex + 1] = new Vector2(u, v);
            uv[vertIndex + 2] = new Vector2(u, v);
            uv[vertIndex + 3] = new Vector2(u, 0);
        }

        //triangles
        int j = 0;
        for (int i = 0; i < vertices.Length - 4; i += 4)
        {
            tri[j] = i + 5; tri[j + 1] = i + 4; tri[j + 2] = i;
            tri[j + 3] = i + 1; tri[j + 4] = i + 5; tri[j + 5] = i;
            tri[j + 6] = i + 6; tri[j + 7] = i + 5; tri[j + 8] = i + 1;
            tri[j + 9] = i + 2; tri[j + 10] = i + 6; tri[j + 11] = i + 1;
            tri[j + 12] = i + 2; tri[j + 13] = i + 3; tri[j + 14] = i + 7;
            tri[j + 15] = i + 6; tri[j + 16] = i + 2; tri[j + 17] = i + 7;
            j += 18;
        }
        
        mf.mesh = m;
        m.Clear();
        m.vertices = vertices;
        m.triangles = tri;
        m.uv = uv;

        (stage.AddComponent(typeof(MeshCollider)) as MeshCollider).sharedMesh = m;
        stage.GetComponent<MeshFilter>().sharedMesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();

        stage.AddComponent(typeof(Collidable));
        mr.material = mat;
    }

    private (List<int>, float[]) generatePlatforms()
    {
        var platformIndices = new List<int>();

        //Randomly chooses 4 stage x axis indices to generate platforms
        platformIndices.Add(Random.Range(30, 40));
        platformIndices.Add(Random.Range(45, 55));
        platformIndices.Add(Random.Range(65, 75));
        platformIndices.Add(Random.Range(80, 90));

        // Used to randomize two things:
        // If a platform is excluded from this stage, 
        // and which one out of the four
        int excludeIndex = Random.Range(0, 8);
        if(platformIndices.Count > excludeIndex)
        {
            platformIndices.RemoveAt(excludeIndex);
        }

        // Used to randomize the size and value of each platform
        float[] platformSizes = new float[platformIndices.Count];
        for(int i = 0; i < platformSizes.Length; i++)
        {
            platformSizes[i] = Random.Range(2, 5);
        }

        return (platformIndices, platformSizes);
    }

    public void RebuildStage()
    {
        Destroy(stage);
        CreateStage(0, 25, moonMat);
    }
}

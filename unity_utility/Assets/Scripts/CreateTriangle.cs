using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 三角ポリゴン描画のテストクラス
/// </summary>
public class CreateTriangle : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private Mesh mesh;

    // 頂点座標
    private Vector3[] localVertices = {
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(1.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, -1.0f, 0.0f)
    };

    // 頂点インデックスの指定
    private int[] localTriangles = { 0, 1, 2 };

    // 法線
    private Vector3[] localNormals = {
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, -1.0f)
    };

    private void Awake()
    {
        mesh = new Mesh();

        // 三角ポリゴン
        //SetForTriangle();

        // トーラス
        SetForTorus();

        mesh.RecalculateBounds();
    }

    private void SetForTriangle()
    {
        mesh.vertices = localVertices;
        mesh.triangles = localTriangles;
        mesh.normals = localNormals;
    }

    private void SetForTorus()
    {
        float r1 = 3.0f;
        float r2 = 1.0f;
        // 頂点の設定数(箇所)、この数値を変えると形が変わる。
        int n = 20;

        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();

        for(int i = 0; i <= n; i++)
        {
            // 外側
            var phi = 2.0f * Mathf.PI * i / n;
            var tr = Mathf.Cos(phi) * r2;
            var y = Mathf.Sin(phi) * r2;

            for(int j = 0; j <= n; j++)
            {
                // 内側
                var theta = 2.0f * Mathf.PI * j / n;
                var x = Mathf.Cos(theta) * (r1 + tr);
                var z = Mathf.Sin(theta) * (r1 + tr);

                vertices.Add(new Vector3(x, y, z));

                // 法線の計算
                normals.Add(new Vector3(tr * Mathf.Cos(theta), y, tr * Mathf.Sin(theta)));
            }
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < n; j++)
            {
                int count = ((n + 1) * j) + i;

                // 頂点インデックスを指定
                triangles.Add(count);
                triangles.Add(count + n + 2);
                triangles.Add(count + 1);

                triangles.Add(count);
                triangles.Add(count + n + 1);
                triangles.Add(count + n + 2);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
    }

    private void Update()
    {
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
    }
}

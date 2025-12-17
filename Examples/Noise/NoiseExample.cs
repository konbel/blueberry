using Godot;
using Blueberry.Noise;

[Tool]
public partial class NoiseExample : Node3D {
    [Export] public int Resolution { get; set; } = 10;
    [Export] public Vector2 Size { get; set; } = new Vector2(10, 10);
    [Export] public NoiseSettings2D Settings { get; set; }

    [ExportToolButton("Create Mesh with Noise")] private Callable CreateMeshButton => Callable.From(CreatePlaneMesh);

    private MeshInstance3D _meshInstance;
    private NoiseFilter2D _noiseFilter;

    public override void _Ready() {
        CreatePlaneMesh();
    }

    private void CreatePlaneMesh() {
        // Create mesh instance if it doesn't exist
        if (_meshInstance == null) {
            _meshInstance = new MeshInstance3D();
            AddChild(_meshInstance);
        }

        // Create noise instance if it doesn't exist
        _noiseFilter = NoiseFilterFactory.CreateNoiseFilter2D(Settings);

        // Create the plane mesh
        ArrayMesh arrayMesh = new ArrayMesh();
        Godot.Collections.Array arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);

        // Calculate vertex count
        int vertexCount = (Resolution + 1) * (Resolution + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector2[] uvs = new Vector2[vertexCount];

        // Generate vertices, normals, and UVs
        int index = 0;
        for (int y = 0; y <= Resolution; y++) {
            for (int x = 0; x <= Resolution; x++) {
                float xPercent = x / (float)Resolution;
                float yPercent = y / (float)Resolution;

                float worldX = (xPercent - 0.5f) * Size.X;
                float worldY = (yPercent - 0.5f) * Size.Y;

                // Position centered at origin
                vertices[index] = new Vector3(
                    worldX,
                    _noiseFilter.GetNoise(worldX, worldY),
                    worldY
                );

                // Normal pointing up
                normals[index] = Vector3.Up;

                // UV coordinates
                uvs[index] = new Vector2(xPercent, yPercent);

                index++;
            }
        }

        // Generate indices for triangles
        int triangleCount = Resolution * Resolution * 2;
        int[] indices = new int[triangleCount * 3];
        int triIndex = 0;

        for (int y = 0; y < Resolution; y++) {
            for (int x = 0; x < Resolution; x++) {
                int i = y * (Resolution + 1) + x;

                // First triangle
                indices[triIndex++] = i;
                indices[triIndex++] = i + 1;
                indices[triIndex++] = i + Resolution + 1;

                // Second triangle
                indices[triIndex++] = i + 1;
                indices[triIndex++] = i + Resolution + 2;
                indices[triIndex++] = i + Resolution + 1;
            }
        }

        // Assign arrays to mesh
        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        arrays[(int)Mesh.ArrayType.Normal] = normals;
        arrays[(int)Mesh.ArrayType.TexUV] = uvs;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        // Create the mesh
        arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        // Assign mesh to instance
        _meshInstance.Mesh = arrayMesh;
    }
}

using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// ScriptableObject que cria um renderer baseado em um MeshRenderer com um cubo.
    /// Configura o material e a malha do cubo, além de definir cores baseadas nas configurações.
    /// </summary>
    [CreateAssetMenu(fileName = "CubeMeshRendererCell", menuName = "ScriptableObjects/CubeMeshRendererCell", order = 2)]
    public class CubeMeshRendererCell : ARendererCellScriptableObject
    {
        [SerializeField] private Material _material;
        private MeshRenderer _meshRenderer;

        /// <summary>
        /// Cria o renderer da célula utilizando um MeshRenderer com cubo.
        /// </summary>
        /// <param name="root">O transform pai para a célula.</param>
        /// <param name="size">Tamanho da célula.</param>
        /// <param name="x">Coordenada X da célula na grade.</param>
        /// <param name="y">Coordenada Y da célula na grade.</param>
        /// <returns>A instância de <see cref="RendererCellBehaviour"/> configurada para a célula.</returns>
        public override RendererCellBehaviour CreateCellRenderer(Transform root, float size, int x, int y)
        {
            if (_material == null)
            {
                Debug.LogError("Material não definido em CubeMeshRendererCell.");
                return null;
            }
            if (root == null)
            {
                Debug.LogError("Transform raiz não pode ser nulo.");
                return null;
            }

            // Cria o GameObject para a célula e adiciona os componentes necessários
            GameObject cell = new GameObject($"({x}, {y})GridCell");
            MeshFilter meshFilter = cell.AddComponent<MeshFilter>();
            _meshRenderer = cell.AddComponent<MeshRenderer>();
            _meshRenderer.material = new Material(_material);
            _meshRenderer.material.color = IsOffset(x, y) ? _offsetColor : _baseColor;

            // Cria a malha do cubo e atribui ao MeshFilter
            Mesh cubeMesh = CreateCubeMesh(size);
            meshFilter.mesh = cubeMesh;

            // Define a posição e o parent da célula
            cell.transform.position = new Vector3(x * size, y * size, 0);
            cell.transform.parent = root;

            // Adiciona o componente de comportamento e o configura com as cores
            var rendererBehaviour = cell.AddComponent<RendererCellBehaviour>();
            rendererBehaviour.SetData(_meshRenderer.material.color, _highlitColor, _selectedColor, _previewColor);

            return rendererBehaviour;
        }

        /// <summary>
        /// Cria uma malha de cubo com o tamanho especificado.
        /// </summary>
        /// <param name="size">Tamanho do cubo.</param>
        /// <returns>Uma instância de <see cref="Mesh"/> representando um cubo.</returns>
        private Mesh CreateCubeMesh(float size)
        {
            Mesh mesh = new Mesh();

            // Define os vértices (8 cantos do cubo)
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-size / 2, -size / 2, -size / 2),
                new Vector3(size / 2, -size / 2, -size / 2),
                new Vector3(size / 2, size / 2, -size / 2),
                new Vector3(-size / 2, size / 2, -size / 2),
                new Vector3(-size / 2, -size / 2, size / 2),
                new Vector3(size / 2, -size / 2, size / 2),
                new Vector3(size / 2, size / 2, size / 2),
                new Vector3(-size / 2, size / 2, size / 2)
            };

            // Define os triângulos que formam as faces do cubo
            int[] triangles = new int[]
            {
                0, 2, 1, // Face frontal
                0, 3, 2,
                4, 6, 5, // Face traseira
                4, 7, 6,
                0, 1, 5, // Face inferior
                0, 5, 4,
                1, 2, 6, // Face direita
                1, 6, 5,
                2, 3, 7, // Face superior
                2, 7, 6,
                0, 4, 7, // Face esquerda
                0, 7, 3
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            // Recalcula as normais para iluminação correta
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}

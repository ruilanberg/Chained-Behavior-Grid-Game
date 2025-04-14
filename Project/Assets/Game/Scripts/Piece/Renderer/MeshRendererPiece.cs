using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Implementação de ARendererPieceScriptableObject utilizando MeshRenderer para renderizar peças via malha 3D de cubos.
    /// Configura cores e dimensões dos cubos através da estrutura MeshInfo.
    /// </summary>
    [CreateAssetMenu(fileName = "MeshRendererPiece", menuName = "ScriptableObjects/MeshRendererPiece", order = 5)]
    public class MeshRendererPiece : ARendererPieceScriptableObject
    {
        [SerializeField] private MeshInfo _playerMesh;
        [SerializeField] private MeshInfo _enemyMesh;
        [SerializeField] private MeshInfo _obstacleMesh;

        /// <summary>
        /// Cria o renderizador para uma peça inimiga utilizando MeshRenderer.
        /// </summary>
        /// <param name="enemy">Instância da peça inimiga.</param>
        /// <returns>Renderizador configurado com uma malha de cubo.</returns>
        public override Renderer CreateEnemyPieceRenderer(APiece enemy)
        {
            return CreateCubeRenderer(ref enemy, ref _enemyMesh);
        }

        /// <summary>
        /// Cria o renderizador para uma peça de obstáculo utilizando MeshRenderer.
        /// </summary>
        /// <param name="obstacle">Instância da peça de obstáculo.</param>
        /// <returns>Renderizador configurado com uma malha de cubo.</returns>
        public override Renderer CreateObstaclePieceRenderer(APiece obstacle)
        {
            return CreateCubeRenderer(ref obstacle, ref _obstacleMesh);
        }

        /// <summary>
        /// Cria o renderizador para uma peça do jogador utilizando MeshRenderer.
        /// </summary>
        /// <param name="player">Instância da peça do jogador.</param>
        /// <returns>Renderizador configurado com uma malha de cubo.</returns>
        public override Renderer CreatePlayerPieceRenderer(APiece player)
        {
            return CreateCubeRenderer(ref player, ref _playerMesh);
        }

        /// <summary>
        /// Cria um renderizador baseado em MeshRenderer com uma malha de cubo.
        /// Esse método instancia um GameObject com MeshFilter e MeshRenderer, gera uma malha de cubo
        /// com o tamanho especificado e configura o material com a cor base definida.
        /// </summary>
        /// <param name="piece">Referência à peça para a qual o renderizador será criado.</param>
        /// <param name="info">Informações do cubo, como tamanho e cor base.</param>
        /// <returns>Instância configurada de MeshRenderer.</returns>
        private Renderer CreateCubeRenderer(ref APiece piece, ref MeshInfo info)
        {
            if (piece == null)
            {
                Debug.LogError("APiece não pode ser nulo ao criar MeshRenderer.");
                return null;
            }

            GameObject rendererObject = new GameObject("CubeRenderer");
            rendererObject.transform.SetParent(piece.transform, false);
            rendererObject.transform.localPosition = Vector3.zero;

            MeshFilter meshFilter = rendererObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = rendererObject.AddComponent<MeshRenderer>();

            // Cria a malha do cubo usando o tamanho definido
            Mesh cubeMesh = CreateCubeMesh(info.CubeSize);
            meshFilter.mesh = cubeMesh;

            // Cria um material padrão utilizando o shader Standard
            Material material = new Material(Shader.Find("Standard"));
            material.color = info.BaseColor;
            meshRenderer.material = material;

            return meshRenderer;
        }

        /// <summary>
        /// Gera uma malha de cubo com o tamanho especificado.
        /// </summary>
        /// <param name="size">Tamanho do cubo.</param>
        /// <returns>Uma instância de Mesh representando um cubo.</returns>
        private Mesh CreateCubeMesh(float size)
        {
            Mesh mesh = new Mesh();

            // Define os vértices do cubo (8 pontos)
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

            // Define os triângulos que formam cada face do cubo (2 triângulos por face)
            int[] triangles = new int[]
            {
                // Frente
                0, 2, 1, 0, 3, 2,
                // Traseira
                4, 5, 6, 4, 6, 7,
                // Inferior
                0, 1, 5, 0, 5, 4,
                // Superior
                2, 3, 7, 2, 7, 6,
                // Direita
                1, 2, 6, 1, 6, 5,
                // Esquerda
                0, 4, 7, 0, 7, 3
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }
    }

    /// <summary>
    /// Estrutura que contém as informações de configuração do cubo para renderização de peças.
    /// </summary>
    [System.Serializable]
    public struct MeshInfo
    {
        /// <summary>
        /// Tamanho (comprimento de cada lado) do cubo.
        /// </summary>
        public float CubeSize;
        /// <summary>
        /// Cor base do material do cubo.
        /// </summary>
        public Color BaseColor;
    }
}

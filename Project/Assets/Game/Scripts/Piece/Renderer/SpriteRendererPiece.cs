using GridSystem;
using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Implementação de ARendererPieceScriptableObject utilizando SpriteRenderer para renderizar peças.
    /// Permite configurar sprites e cores para peças do jogador, inimigas e de obstáculo.
    /// </summary>
    [CreateAssetMenu(fileName = "SpriteRendererPiece", menuName = "ScriptableObjects/SpriteRendererPiece", order = 4)]
    public class SpriteRendererPiece : ARendererPieceScriptableObject
    {
        [SerializeField] private SpriteInfo _playerSprite;
        [SerializeField] private SpriteInfo _enemySprite;
        [SerializeField] private SpriteInfo _obstacleSprite;

        /// <summary>
        /// Cria e retorna o renderizador para uma peça inimiga utilizando SpriteRenderer.
        /// </summary>
        /// <param name="enemy">Instância da peça inimiga.</param>
        /// <returns>Instância de SpriteRenderer configurada.</returns>
        public override Renderer CreateEnemyPieceRenderer(APiece enemy)
        {
            return CreateSpriteRenderer(ref enemy, ref _enemySprite);
        }

        /// <summary>
        /// Cria e retorna o renderizador para uma peça de obstáculo utilizando SpriteRenderer.
        /// </summary>
        /// <param name="obstacle">Instância da peça de obstáculo.</param>
        /// <returns>Instância de SpriteRenderer configurada.</returns>
        public override Renderer CreateObstaclePieceRenderer(APiece obstacle)
        {
            return CreateSpriteRenderer(ref obstacle, ref _obstacleSprite);
        }

        /// <summary>
        /// Cria e retorna o renderizador para uma peça do jogador utilizando SpriteRenderer.
        /// </summary>
        /// <param name="player">Instância da peça do jogador.</param>
        /// <returns>Instância de SpriteRenderer configurada.</returns>
        public override Renderer CreatePlayerPieceRenderer(APiece player)
        {
            return CreateSpriteRenderer(ref player, ref _playerSprite);
        }

        /// <summary>
        /// Cria um objeto com SpriteRenderer e o configura com os dados especificados.
        /// </summary>
        /// <param name="piece">Referência da peça para a qual o renderizador será criado.</param>
        /// <param name="info">Informações do sprite e cores a serem utilizadas.</param>
        /// <returns>Instância de SpriteRenderer configurada.</returns>
        private Renderer CreateSpriteRenderer(ref APiece piece, ref SpriteInfo info)
        {
            if (piece == null)
            {
                Debug.LogError("APiece não pode ser nulo ao criar SpriteRenderer.");
                return null;
            }
            if (info.Sprite == null)
            {
                Debug.LogError("SpriteInfo não possui um sprite definido.");
                return null;
            }

            GameObject rendererObject = new GameObject("Renderer");
            rendererObject.transform.SetParent(piece.transform, false);
            rendererObject.transform.localPosition = Vector3.zero;

            SpriteRenderer spriteRenderer = rendererObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = info.Sprite;
            spriteRenderer.material.color = info.BaseColor;
            spriteRenderer.sortingOrder = 1;

            return spriteRenderer;
        }
    }

    /// <summary>
    /// Estrutura que contém as informações de configuração do sprite para renderização de peças.
    /// </summary>
    [System.Serializable]
    public struct SpriteInfo
    {
        /// <summary>
        /// Sprite a ser utilizado.
        /// </summary>
        public Sprite Sprite;
        /// <summary>
        /// Cor base do sprite.
        /// </summary>
        public Color BaseColor;
        /// <summary>
        /// Cor utilizada para o estado de destaque (highlight).
        /// </summary>
        public Color HighlitColor;
    }
}

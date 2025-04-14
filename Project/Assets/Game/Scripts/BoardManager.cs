using Game.Piece;
using GridSystem;
using Reflex.Attributes;
using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Gerencia o tabuleiro, a interação dos jogadores com as peças e a comunicação com o sistema de grid.
    /// Responsável por instanciar peças, gerenciar seleções e atualizar o estado visual das células.
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        [Inject] private Grid<APiece> _grid;
        [Inject] private GridSettings _gridSettings;
        [Inject] private GameSettings _gameSettings;

        private ARendererPieceScriptableObject _rendererPieceScriptableObject;

        private Cell<APiece> _overMouseLastCell;
        private Cell<APiece> _previewInteractionCell;

        /// <summary>
        /// A célula atualmente selecionada.
        /// </summary>
        public Cell<APiece> SelectedCell { get; private set; }

        /// <summary>
        /// Evento acionado para executar o comportamento de uma peça.
        /// </summary>
        public Action<APiece> OnRunBehaviourPiece;

        /// <summary>
        /// Define o renderer que será usado pelas peças.
        /// </summary>
        /// <param name="rendererPiece">Instância do renderer para as peças.</param>
        public void SetRenderer(ARendererPieceScriptableObject rendererPiece)
        {
            _rendererPieceScriptableObject = rendererPiece;
        }

        /// <summary>
        /// Instancia e posiciona as peças do jogo (inimigos, obstáculos e jogadores) nas células do grid.
        /// </summary>
        public void SpawnPieces()
        {
            if (_gameSettings == null)
            {
                Debug.LogError("GameSettings não estão definidos.");
                return;
            }

            if (_rendererPieceScriptableObject == null)
            {
                Debug.LogError("RendererPiece não foi definido. Use SetRenderer() para configurar.");
                return;
            }

            SpawnPiecesFromArray(_gameSettings.EnemyPieces);
            SpawnPiecesFromArray(_gameSettings.ObstaclePieces);
            SpawnPiecesFromArray(_gameSettings.PlayerPieces);
        }

        /// <summary>
        /// Itera sobre um array de peças e as instancia.
        /// </summary>
        /// <param name="pieces">Array de peças a serem instanciadas.</param>
        private void SpawnPiecesFromArray(APiece[] pieces)
        {
            if (pieces == null || pieces.Length == 0) return;

            foreach (var piecePrefab in pieces)
            {
                SpawnPiece(piecePrefab);
            }
        }

        /// <summary>
        /// Instancia uma peça, posiciona-a em uma célula aleatória e configura seu renderer.
        /// </summary>
        /// <param name="piecePrefab">Prefab da peça a ser instanciada.</param>
        private void SpawnPiece(APiece piecePrefab)
        {
            var pieceInstance = Instantiate(piecePrefab, transform);
            Cell<APiece> randomCell = GetRandomCell();
            if (randomCell != null)
            {
                randomCell.Value = pieceInstance;
                pieceInstance.transform.position = GetPositionOfCellInWorld(randomCell.X, randomCell.Y);
                pieceInstance.SetData(_rendererPieceScriptableObject);
            }
            else
            {
                Debug.LogWarning("Não foi possível encontrar uma célula vazia para a peça instanciada.");
            }
        }

        /// <summary>
        /// Seleciona uma célula e, dependendo do conteúdo (por exemplo, uma peça do jogador),
        /// dispara o comportamento associado ou previsualiza as interações.
        /// </summary>
        /// <param name="cell">Célula a ser selecionada.</param>
        public void SelectCell(Cell<APiece> cell)
        {
            if (cell == null)
            {
                DeselectCells();
                return;
            }

            if (SelectedCell != cell)
            {
                DeselectCells();
            }

            if (_previewInteractionCell == cell && SelectedCell?.Value is PlayerPiece)
            {
                OnRunBehaviourPiece?.Invoke(SelectedCell?.Value);
                return;
            }

            SelectedCell = cell;
            cell?.Selected();

            if (cell?.Value is PlayerPiece player)
            {
                PreviewInteractionCells(player.PreviewPositionsToMove());
            }
        }

        /// <summary>
        /// Restaura o estado visual (idle) das células selecionadas e em preview.
        /// </summary>
        private void DeselectCells()
        {
            SelectedCell?.Idle();
            _previewInteractionCell?.Idle();
        }

        /// <summary>
        /// Atualiza o estado visual da célula sobre a qual o mouse está passando.
        /// Evita alterações se a célula for a selecionada ou estiver em estado de preview.
        /// </summary>
        /// <param name="cell">Célula sobre a qual o mouse está passando.</param>
        public void OverMouseCell(Cell<APiece> cell)
        {
            if (cell == null || cell == SelectedCell || cell == _overMouseLastCell || cell == _previewInteractionCell)
                return;

            if (_overMouseLastCell != cell && _overMouseLastCell != SelectedCell && cell != _previewInteractionCell)
                _overMouseLastCell?.Idle();

            cell?.MouseOver();
            _overMouseLastCell = cell;
        }

        /// <summary>
        /// Previsualiza a interação para a célula correspondente à posição informada.
        /// </summary>
        /// <param name="position">Posição onde ocorrerá a interação.</param>
        public void PreviewInteractionCells(Vector3 position)
        {
            var cell = _grid.GetCell(position);
            if (cell == null || cell == SelectedCell)
                return;

            cell.Preview();
            _previewInteractionCell = cell;
        }

        /// <summary>
        /// Restaura o estado visual normal de todas as células (idle).
        /// </summary>
        public void ResetViewBoard()
        {
            _overMouseLastCell?.Idle();
            _previewInteractionCell?.Idle();
            SelectedCell?.Idle();
        }

        /// <summary>
        /// Limpa as referências das células temporárias e da célula selecionada.
        /// </summary>
        public void ResetReferences()
        {
            _overMouseLastCell = null;
            _previewInteractionCell = null;
            SelectedCell = null;
        }

        /// <summary>
        /// Calcula a posição no mundo para uma célula de acordo com suas coordenadas.
        /// </summary>
        /// <param name="x">Coordenada X da célula.</param>
        /// <param name="y">Coordenada Y da célula.</param>
        /// <returns>Posição no mundo para a célula.</returns>
        public Vector3 GetPositionOfCellInWorld(int x, int y)
        {
            return new Vector3(x * _gridSettings.CellSize, y * _gridSettings.CellSize, 0);
        }

        /// <summary>
        /// Associa uma peça à célula correspondente à sua posição.
        /// </summary>
        /// <param name="piece">Referência à peça que deve ser posicionada na célula.</param>
        public void SetPieceInCell(ref APiece piece)
        {
            var cell = _grid.GetCell(piece.transform.position);
            if (cell != null)
            {
                cell.Value = piece;
            }
            else
            {
                Debug.LogWarning("Nenhuma célula encontrada para a posição da peça.");
            }
        }

        /// <summary>
        /// Obtém uma célula aleatória que esteja vazia.
        /// Nota: Se muitas células estiverem ocupadas, pode ocorrer recursão excessiva.
        /// </summary>
        /// <returns>Célula vazia aleatória ou null se não houver livre.</returns>
        private Cell<APiece> GetRandomCell()
        {
            int xRandom = UnityEngine.Random.Range(0, _gridSettings.GridWidth);
            int yRandom = UnityEngine.Random.Range(0, _gridSettings.GridHeight);
            Cell<APiece> cell = _grid.GetCell(xRandom, yRandom);

            if (cell.Value != null)
                return GetRandomCell();

            return cell;
        }
    }
}

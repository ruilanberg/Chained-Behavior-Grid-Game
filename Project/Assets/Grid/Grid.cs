using UnityEngine;
using System;

namespace GridSystem
{
    /// <summary>
    /// Representa uma grade composta por células que armazenam valores do tipo <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">O tipo de dado armazenado em cada célula da grade.</typeparam>
    public class Grid<T>
    {
        private readonly GridSettings _settings;
        private Cell<T>[,] cells;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Grid{T}"/>.
        /// Cria as células baseando-se nas configurações fornecidas e instancia o renderer de cada uma.
        /// </summary>
        /// <param name="root">O transform pai para os renderers das células.</param>
        /// <param name="settings">Configurações da grade.</param>
        /// <param name="rendererCell">ScriptableObject responsável pela criação dos renderers.</param>
        public Grid(Transform root, GridSettings settings, ARendererCellScriptableObject rendererCell)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "As configurações da grade não podem ser nulas.");
            }
            if (rendererCell == null)
            {
                throw new ArgumentNullException(nameof(rendererCell), "O scriptable object do renderer não pode ser nulo.");
            }
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root), "O transform raiz não pode ser nulo.");
            }

            _settings = settings;
            cells = new Cell<T>[_settings.GridWidth, _settings.GridHeight];

            for (int x = 0; x < _settings.GridWidth; x++)
            {
                for (int y = 0; y < _settings.GridHeight; y++)
                {
                    cells[x, y] = new Cell<T>(rendererCell, root, x, y, _settings.CellSize);
                }
            }
        }

        /// <summary>
        /// Obtém a célula nas coordenadas especificadas.
        /// </summary>
        /// <param name="x">Coordenada X da célula.</param>
        /// <param name="y">Coordenada Y da célula.</param>
        /// <returns>A célula localizada ou null se as coordenadas estiverem fora dos limites.</returns>
        public Cell<T> GetCell(int x, int y)
        {
            if (x < 0 || x >= _settings.GridWidth || y < 0 || y >= _settings.GridHeight)
            {
                Debug.LogWarning($"As coordenadas ({x}, {y}) estão fora dos limites da grade.");
                return null;
            }
            return cells[x, y];
        }

        /// <summary>
        /// Obtém a primeira célula que contém o valor especificado.
        /// </summary>
        /// <param name="val">O valor a ser procurado.</param>
        /// <returns>A célula encontrada ou null se o valor não for encontrado.</returns>
        public Cell<T> GetCell(T val)
        {
            for (int i = 0; i < cells.GetLength(0); i++) // Itera sobre as linhas
            {
                for (int j = 0; j < cells.GetLength(1); j++) // Itera sobre as colunas
                {
                    if (cells[i, j].Value != null && cells[i, j].Value.Equals(val))
                    {
                        return cells[i, j]; // Valor encontrado
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Obtém a célula correspondente à posição informada na tela.
        /// A posição da tela é convertida em coordenadas da grade.
        /// </summary>
        /// <param name="screenPosition">A posição na tela.</param>
        /// <returns>A célula correspondente ou null se a posição estiver fora da grade.</returns>
        public Cell<T> GetCell(Vector2 screenPosition)
        {
            int x = Mathf.RoundToInt(screenPosition.x / _settings.CellSize);
            int y = Mathf.RoundToInt(screenPosition.y / _settings.CellSize);

            if (x >= 0 && y >= 0 && x < _settings.GridWidth && y < _settings.GridHeight)
                return cells[x, y];
            else
            {
                //Debug.LogWarning($"A posição na tela {screenPosition} se traduz em coordenadas ({x}, {y}) fora dos limites da grade.");
                return null;
            }
        }
    }
}

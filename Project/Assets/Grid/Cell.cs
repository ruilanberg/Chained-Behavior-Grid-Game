using System;
using UnityEngine;

namespace GridSystem
{
    /// <summary>
    /// Representa uma célula em uma grade, armazenando um valor e um renderer para sua exibição.
    /// </summary>
    /// <typeparam name="T">Tipo do valor contido na célula.</typeparam>
    public class Cell<T>
    {
        /// <summary>
        /// A coordenada X da célula na grade.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// A coordenada Y da célula na grade.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// O valor armazenado na célula.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Responsável pela exibição e comportamento visual da célula.
        /// </summary>
        public RendererCellBehaviour Renderer { get; private set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Cell{T}"/>.
        /// Cria um renderer para a célula utilizando o scriptable object informado.
        /// </summary>
        /// <param name="rendererSO">O scriptable object usado para criar o renderer da célula.</param>
        /// <param name="root">O transform pai onde o renderer será instanciado.</param>
        /// <param name="x">Coordenada X da célula.</param>
        /// <param name="y">Coordenada Y da célula.</param>
        /// <param name="size">O tamanho da célula.</param>
        public Cell(ARendererCellScriptableObject rendererSO, Transform root, int x, int y, float size)
        {
            if (rendererSO == null)
            {
                throw new ArgumentNullException(nameof(rendererSO), "O scriptable object do renderer não pode ser nulo.");
            }
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root), "O transform raiz não pode ser nulo.");
            }

            X = x;
            Y = y;
            Renderer = rendererSO.CreateCellRenderer(root, size, x, y);

            if (Renderer == null)
            {
                Debug.LogError($"A criação do renderer falhou para a célula em ({x}, {y}).");
            }
        }

        /// <summary>
        /// Define o estado da célula como ocioso.
        /// </summary>
        public void Idle()
        {
            Renderer?.SetIdleState();
        }

        /// <summary>
        /// Define o estado da célula como ativado pelo mouse.
        /// </summary>
        public void MouseOver()
        {
            Renderer?.SetMouseOverState();
        }

        /// <summary>
        /// Define o estado da célula como selecionada.
        /// </summary>
        public void Selected()
        {
            Renderer?.SetSelectedState();
        }

        /// <summary>
        /// Define o estado da célula para pré-visualização.
        /// </summary>
        public void Preview()
        {
            Renderer?.SetPreviewState();
        }
    }
}

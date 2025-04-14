using Reflex.Core;
using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Instalador responsável por registrar as dependências relacionadas às peças.
    /// Este script pode ser utilizado para configurar a injeção de dependências para os componentes das peças.
    /// </summary>
    public class PieceInstaller : MonoBehaviour, IInstaller
    {
        /// <summary>
        /// Registra as dependências necessárias no contêiner.
        /// </summary>
        /// <param name="builder">Construtor do contêiner usado para adicionar as dependências.</param>
        public void InstallBindings(ContainerBuilder builder)
        {
            APiece piece = GetComponent<APiece>();

            if (piece == null)
            {
                Debug.LogWarning("Nenhuma APiece encontrada para instalar bindings.");
                return;
            }

            // Exemplo de binding; se não houver nós, não registra nada.
            if (piece.ChainedNode == null || piece.ChainedNode.Nodes.Count <= 0)
            {
                Debug.LogWarning("ChainedNode ou sua lista de nós está vazia em " + piece.name);
                return;
            }
        }
    }
}

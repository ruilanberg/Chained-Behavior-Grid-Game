using UnityEngine;
using UnityEditor;
using ChainedBehavior;
using ChainedBehavior.Editor;

namespace Game.Piece
{
    /// <summary>
    /// Custom Editor para objetos do tipo APiece.
    /// Permite editar a cadeia de comportamento associada à peça.
    /// </summary>
    [CustomEditor(typeof(APiece), true)]
    public class PieceEditor : Editor
    {
        /// <summary>
        /// Desenha o Inspector padrão e adiciona um botão para editar a cadeia de comportamento.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Desenha o Inspector padrão
            base.OnInspectorGUI();

            // Cria um botão para editar a cadeia de comportamento
            if (GUILayout.Button("Edit Piece Behavior Chain", GUILayout.Height(40)))
            {
                APiece piece = (APiece)target;
                EnsureChainedNode(piece);
                ChainedBehaviorEditor.ShowPanel();
            }
        }

        /// <summary>
        /// Garante que a peça possua uma instância válida de ChainedNodeData.
        /// Caso contrário, cria uma nova instância, adiciona como asset filho e salva as alterações.
        /// </summary>
        /// <param name="piece">Instância de APiece a ser verificada.</param>
        private void EnsureChainedNode(APiece piece)
        {
            if (piece.ChainedNode == null)
            {
                // Cria uma nova instância de ChainedNodeData
                piece.ChainedNode = ScriptableObject.CreateInstance<ChainedNodeData>();

                // Adiciona o objeto criado como subasset da peça para persistência
                AssetDatabase.AddObjectToAsset(piece.ChainedNode, piece);
                AssetDatabase.SaveAssets();
            }
        }
    }
}

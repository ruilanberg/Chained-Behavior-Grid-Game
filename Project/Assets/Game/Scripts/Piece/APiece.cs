using ChainedBehavior;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace Game.Piece
{
    /// <summary>
    /// Classe abstrata base para todas as peças do jogo.
    /// Gerencia a injeção de atributos nos nós encadeados da peça.
    /// </summary>
    public abstract class APiece : MonoBehaviour
    {
        [Inject] private Container _container;

        /// <summary>
        /// Dados que armazenam a cadeia de nós que definem o comportamento da peça.
        /// </summary>
        public ChainedNodeData ChainedNode = null;

        /// <summary>
        /// Configura os dados de renderização para a peça.
        /// Este método deve ser implementado pelas classes derivadas.
        /// </summary>
        /// <param name="rendererPiece">Instância do renderer para as peças.</param>
        public abstract void SetData(ARendererPieceScriptableObject rendererPiece);

        /// <summary>
        /// Método chamado durante o Awake para injetar os atributos necessários em cada nó.
        /// </summary>
        private void Awake()
        {
            InjectAttributes();
        }

        /// <summary>
        /// Realiza a injeção de dependências em cada nó contido em ChainedNode.
        /// </summary>
        private void InjectAttributes()
        {
            if (ChainedNode == null)
            {
                Debug.LogWarning("ChainedNode não está configurado em " + gameObject.name);
                return;
            }
            foreach (var node in ChainedNode.Nodes)
            {
                AttributeInjector.Inject(node, _container);
            }
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace ChainedBehavior
{
    /// <summary>
    /// Nó de comportamento simples que herda da classe Node.
    /// Realiza a execução de forma assíncrona retornando sucesso.
    /// </summary>
    public class BehaviourNode : Node
    {
        /// <summary>
        /// Método chamado ao iniciar o nó.
        /// Realize configurações ou inicializações aqui, se necessário.
        /// </summary>
        protected override void OnStart()
        {
            // Inicialização do nó, se necessário.
        }

        /// <summary>
        /// Método chamado para finalizar a execução do nó.
        /// Realize a limpeza ou encerramento dos recursos aqui, se necessário.
        /// </summary>
        protected override void OnStop()
        {
            // Finalização do nó, se necessário.
        }

        /// <summary>
        /// Atualiza o estado do nó de forma assíncrona.
        /// Aguarda um yield para garantir continuidade sem bloquear e retorna sucesso.
        /// </summary>
        /// <returns>O resultado da execução do nó.</returns>
        protected override async UniTask<StateResult> OnUpdate()
        {
            await UniTask.Yield();
            return StateResult.Success;
        }
    }
}

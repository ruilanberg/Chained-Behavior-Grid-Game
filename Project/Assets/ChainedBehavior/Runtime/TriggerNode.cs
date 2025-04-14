using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace ChainedBehavior
{
    /// <summary>
    /// Nó de gatilho que inicia a cadeia de execução dos nós.
    /// Simplesmente aguarda um yield assíncrono e retorna sucesso, permitindo que a execução continue.
    /// </summary>
    public class TriggerNode : Node
    {
        /// <summary>
        /// Inicializa o nó de gatilho. Pode ser usado para configurações iniciais ou logs.
        /// </summary>
        protected override void OnStart()
        {
            Debug.Log("TriggerNode OnStart");
        }

        /// <summary>
        /// Finaliza a execução do nó de gatilho. Use para limpar recursos ou registrar logs.
        /// </summary>
        protected override void OnStop()
        {
            Debug.Log("TriggerNode OnStop");
        }

        /// <summary>
        /// Atualiza a execução do nó de gatilho de forma assíncrona.
        /// Aguarda um yield e retorna sucesso, permitindo que a cadeia de execução continue.
        /// </summary>
        /// <returns>StateResult.Success para indicar que a execução pode continuar.</returns>
        protected override async UniTask<StateResult> OnUpdate()
        {
            await UniTask.Yield();
            return StateResult.Success;
        }
    }
}

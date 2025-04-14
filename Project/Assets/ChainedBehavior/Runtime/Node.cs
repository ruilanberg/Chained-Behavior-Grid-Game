using Cysharp.Threading.Tasks;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ChainedBehavior
{
    /// <summary>
    /// Classe base que define a estrutura e execução dos nós.
    /// Gerencia estados e possibilita o encadeamento através de nós filhos.
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        /// <summary>
        /// Enumeração que define o estado atual do nó.
        /// </summary>
        public enum StateBehaviour
        {
            None,
            Running,
        }

        /// <summary>
        /// Enumeração que define o resultado da execução do nó.
        /// </summary>
        public enum StateResult
        {
            Failure,
            Success
        }

        // Identificador único do nó
        public string Guid;

        // Posição do nó, utilizada para o layout visual no editor.
        public Vector2 Position;

        // Nó filho que será executado após a execução bem-sucedida deste nó.
        public Node ChildNode;

        /// <summary>
        /// Estado atual do nó.
        /// </summary>
        public StateBehaviour StateCurrent { get; protected set; } = StateBehaviour.None;

        /// <summary>
        /// Resultado da última execução do nó.
        /// </summary>
        public StateResult ResultState { get; protected set; } = StateResult.Failure;

        /// <summary>
        /// Inicializa o nó sempre que o ScriptableObject é habilitado.
        /// Reseta o estado e o resultado.
        /// </summary>
        private void OnEnable()
        {
            StateCurrent = StateBehaviour.None;
            ResultState = StateResult.Failure;
        }

        /// <summary>
        /// Executa o nó e, se bem-sucedido, seu nó filho.
        /// Realiza a sequência: OnStart → OnUpdate → OnStop e propaga a execução para o filho, se houver.
        /// </summary>
        /// <returns>O resultado da execução (Success ou Failure).</returns>
        public async UniTask<StateResult> Run()
        {
            if (StateCurrent == StateBehaviour.None)
            {
                OnStart();
                StateCurrent = StateBehaviour.Running;
                try
                {
                    ResultState = await OnUpdate();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Erro durante a execução do nó {name}: {ex.Message}");
                    ResultState = StateResult.Failure;
                }
            }

            OnStop();
            StateCurrent = StateBehaviour.None;

            if (ResultState == StateResult.Success && ChildNode != null)
            {
                return await ChildNode.Run();
            }

            return ResultState;
        }

        /// <summary>
        /// Define, via reflexão, o valor de um campo específico e salva as alterações no asset.
        /// </summary>
        /// <param name="fieldInfo">Informação do campo a ser alterado.</param>
        /// <param name="val">Novo valor a ser atribuído.</param>
        public void SetValueField(FieldInfo fieldInfo, object val)
        {
            var field = GetType().GetField(fieldInfo.Name, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(this, val);
#if UNITY_EDITOR
                AssetDatabase.SaveAssets();
#endif
            }
            else
            {
                Debug.LogWarning($"Campo {fieldInfo.Name} não encontrado em {name}.");
            }
        }

        /// <summary>
        /// Inicializa o nó. Deve ser implementado pelas classes derivadas.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Finaliza o nó. Deve ser implementado pelas classes derivadas.
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// Atualiza a execução do nó de forma assíncrona e retorna o resultado.
        /// </summary>
        /// <returns>StateResult indicando sucesso ou falha.</returns>
        protected abstract UniTask<StateResult> OnUpdate();
    }
}

using GridSystem;
using Reflex.Attributes;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Centraliza a câmera de acordo com as dimensões da grade.
    /// Calcula a posição ideal com base nas configurações de grid e posiciona a câmera.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraCentralizer : MonoBehaviour
    {
        private Camera _camera;
        [Inject] private GridSettings _settings;

        private const float OFFSET_SIZE_CELL = 0.5f;
        private const float OFFSET_DISTANCE = -8f;

        /// <summary>
        /// Obtém a referência da câmera neste GameObject.
        /// </summary>
        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                Debug.LogError("Câmera não encontrada no GameObject.");
            }
        }

        /// <summary>
        /// Centraliza a câmera utilizando os parâmetros definidos nas configurações do grid.
        /// </summary>
        private void Start()
        {
            if (_settings == null)
            {
                Debug.LogError("GridSettings não injetado em CameraCentralizer.");
                return;
            }
            _camera.transform.position = new Vector3(_settings.GridWidth / 2f - OFFSET_SIZE_CELL,
                                                       _settings.GridHeight / 2f - OFFSET_SIZE_CELL,
                                                       OFFSET_DISTANCE);
        }
    }
}

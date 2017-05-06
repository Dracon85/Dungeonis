namespace RPG.LevelManager
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	public class LoadingScreenManager
		: MonoBehaviour
	{
		[Header("Visual Settings")]
		[SerializeField] private Image _loadingIcon;
		[SerializeField] private Image _loadingDoneIcon;
		[SerializeField] private Image _progressBar;
		[SerializeField] private Image _fadeOverlay;
		[SerializeField] private Text _loadingText;

		[Header("Timing Settings")]
		[SerializeField] private float _waitOnLoadDone = 0.25f;
		[SerializeField] private float _fadeDuration = 0.25f;

		[Header("Loading Settings")]
		[SerializeField] private ThreadPriority loadThreadPriority;

		[Header("Other")]
		[SerializeField] public static int LoadSceneIndex = -1;
		[SerializeField] public static int LoadingSceneIndex = 1; /* This value comes from the Build Settings and is the "LoadingScene" */

		private AsyncOperation operation;

		/// <summary>
		/// Loads the scene.
		/// </summary>
		/// <param name="loadSceneIndex">Index of the load scene.</param>
		public static void LoadScene(int loadSceneIndex)
		{
			if (loadSceneIndex >= SceneManager.sceneCountInBuildSettings)
			{
				Debug.LogWarning("Can't load scene at index " + loadSceneIndex + ". SceneManager only contains " + SceneManager.sceneCountInBuildSettings + " scenes.");
				return;
			}

			Application.backgroundLoadingPriority = ThreadPriority.High;
			LoadSceneIndex                        = loadSceneIndex;
			SceneManager.LoadScene(LoadingSceneIndex);
		}

		private void Start()
		{
			if (LoadSceneIndex < 1&&LoadSceneIndex!=0)
				return;

			_fadeOverlay.gameObject.SetActive(true);
			StartCoroutine(LoadAsync(LoadSceneIndex));
		}

		/// <summary>
		/// Loads the scene asynchronously.
		/// </summary>
		/// <param name="loadSceneIndex">Index of the load scene.</param>
		/// <returns></returns>
		private IEnumerator LoadAsync(int loadSceneIndex)
		{
			ShowVisuals(LoadingState.Loading);

			yield return null;

			FadeIn();
			StartOperation(loadSceneIndex);

			float lastProgress = 0f;

			while (!IsLoadingDone())
			{
				yield return null;

				if (!Mathf.Approximately(operation.progress, lastProgress))
				{
					_progressBar.fillAmount = operation.progress;
					lastProgress            = operation.progress;
				}
			}

			ShowVisuals(LoadingState.FinishedLoading);

			yield return new WaitForSeconds(_waitOnLoadDone);

			operation.allowSceneActivation = true;
		}

		/// <summary>
		/// Shows the visuals.
		/// </summary>
		/// <param name="loadingState">State of the loading.</param>
		private void ShowVisuals(LoadingState loadingState)
		{
			if (loadingState.Equals(LoadingState.Loading))
			{
				_loadingIcon.gameObject.SetActive(true);
				_loadingDoneIcon.gameObject.SetActive(false);

				_progressBar.fillAmount = 0f;
				_loadingText.text       = "LOADING...";
			}
			else
			{
				_loadingIcon.gameObject.SetActive(false);
				_loadingDoneIcon.gameObject.SetActive(true);

				_progressBar.fillAmount = 1f;
				_loadingText.text       = "LOADING DONE";
			}
		}

		/// <summary>
		/// Fades in the loading screen.
		/// </summary>
		private void FadeIn()
		{
			_fadeOverlay.CrossFadeAlpha(0, _fadeDuration, true);
		}

		/// <summary>
		/// Starts the loading operation.
		/// </summary>
		/// <param name="loadSceneIndex">Index of the load scene.</param>
		private void StartOperation(int loadSceneIndex)
		{
			Application.backgroundLoadingPriority = loadThreadPriority;
			operation                             = SceneManager.LoadSceneAsync(loadSceneIndex);
		}

		/// <summary>
		/// Has loading finished.
		/// </summary>
		/// <returns></returns>
		private bool IsLoadingDone()
		{
			return operation.progress >= 0.9f;
		}

		/// <summary>
		/// Loading States
		/// </summary>
		private enum LoadingState
		{
			Loading,
			FinishedLoading
		}
	}
}
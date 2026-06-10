#if UNITY_EDITOR_WIN

using UnityEditor;

namespace EditorWindowMaximizer.Editor
{
	internal static class Maximizer
	{
		private enum Mode { Normal, KeepTaskbar, FullScreen }

		private const string menuItemFullScreen = "Window/Full Screen _F11";

		private static Mode _mode;

		static Maximizer()
		{
			_mode = Window.Current.IsFullScreen ? Mode.FullScreen : Mode.Normal;
			UpdateToggles();
		}

		[MenuItem(menuItemFullScreen, false, 10)]
		private static void SwitchFullScreen()
		{
			_mode = (Mode)(((int)_mode + 1) % 3);

			var window = Window.Current;
			window.IsFullScreen = _mode != Mode.Normal;
			window.FitScreen(_mode == Mode.FullScreen);

			UpdateToggles();
		}

		private static void UpdateToggles()
		{
			Menu.SetChecked(menuItemFullScreen, _mode != Mode.Normal);
		}
	}
}
#endif

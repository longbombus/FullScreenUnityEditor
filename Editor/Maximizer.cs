#if UNITY_EDITOR_WIN

using UnityEditor;

namespace EditorWindowMaximizer.Editor
{
	internal static class Maximizer
	{
		private enum Mode { Normal, KeepTaskbar, FullScreen }

		private const string menuItemCycle       = "Window/Full Screen/Switch Mode _F11";
		private const string menuItemOff         = "Window/Full Screen/Off";
		private const string menuItemKeepTaskbar = "Window/Full Screen/Keep Task Bar";
		private const string menuItemFullScreen  = "Window/Full Screen/Totally Full Screen";

		private static Mode _mode;

		static Maximizer()
		{
			_mode = Window.Current.IsFullScreen ? Mode.FullScreen : Mode.Normal;
			UpdateToggles();
		}

		[MenuItem(menuItemCycle, false, 10)]
		private static void Cycle() => SetMode((Mode)(((int)_mode + 1) % 3));

		[MenuItem(menuItemOff, false, 21)]
		private static void SetOff() => SetMode(Mode.Normal);

		[MenuItem(menuItemKeepTaskbar, false, 22)]
		private static void SetKeepTaskbar() => SetMode(Mode.KeepTaskbar);

		[MenuItem(menuItemFullScreen, false, 23)]
		private static void SetFullScreen() => SetMode(Mode.FullScreen);

		private static void SetMode(Mode mode)
		{
			if (_mode == mode)
				return;

			_mode = mode;

			var window = Window.Current;
			window.IsFullScreen = _mode != Mode.Normal;
			window.FitScreen(_mode == Mode.FullScreen);

			UpdateToggles();
		}

		private static void UpdateToggles()
		{
			Menu.SetChecked(menuItemOff,         _mode == Mode.Normal);
			Menu.SetChecked(menuItemKeepTaskbar, _mode == Mode.KeepTaskbar);
			Menu.SetChecked(menuItemFullScreen,  _mode == Mode.FullScreen);
			Menu.SetChecked(menuItemCycle,       false);
		}
	}
}
#endif

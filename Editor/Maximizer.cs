#if UNITY_EDITOR_WIN

using UnityEditor;

namespace WindowMaximizer.Editor
{
	internal static class Maximizer
	{
		private const string menuItemFullScreen = "Window/Full Screen _F11";

		static Maximizer()
		{
			UpdateToggles();
		}
		
		[MenuItem(menuItemFullScreen, false, 10)]
		private static void SwitchFullScreen()
		{
			var currentWindow = Window.Current;
			currentWindow.IsFullScreen = !currentWindow.IsFullScreen;
			currentWindow.FitScreen();
			
			UpdateToggles();
		}

		private static void UpdateToggles()
		{
			var currentWindow = Window.Current;
			
			Menu.SetChecked(menuItemFullScreen, currentWindow.IsFullScreen);
		}
	}
}
#endif

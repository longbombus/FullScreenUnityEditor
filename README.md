# Unity Editor Window Maximizer

Adds full screen mode to the Unity Editor on Windows.

## Usage

Press **F11** to switch the modes. Also available via menu **Window → Full Screen**.

Available modes:
* Totally full screen (hides window header and taskbar)
* Partially full screen (hides only window header)
* Default window mode (restores default unity editor presence)

## Installation

### Via OpenUPM

1. Open **Edit → Project Settings → Package Manager**
2. Add a scoped registry with URL `https://package.openupm.com`
3. Add scope `com.longbombus`
4. Open **Window → Package Management → Package Manager**, switch to **My Registries**, and install **Editor Window Maximizer**.


### Via Git URL

In Unity, open **Window → Package Management → Package Manager**, click **+** → **Add package from git URL** and enter:

```
https://github.com/longbombus/FullScreenUnityEditor.git
```

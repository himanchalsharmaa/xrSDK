# Display Provider

A custom display provider for Windows Unity made using Unity's XR SDK to support multi-pass and single pass rendering with functions to change projection and view matrix values for each eye.<br>
Used a single camera to get output for both eyes.<br>
[ExampleDisplayProvider.cpp](https://github.com/himanchalsharmaa/xrSDK/blob/master/ExampleDisplayProvider.cpp) is the cpp file used to compile the DLL plugin for display provider.<br>
[RenderToTexture.cs](https://github.com/himanchalsharmaa/xrSDK/blob/master/Assets/Scripts/RenderToTexture.cs) is the script responsible for loading,enabling the display subsystem and displaying color and depth output. <br>
![image](https://github.com/himanchalsharmaa/xrSDK/assets/95272385/d5eb6898-1dbd-4985-a28c-918db3e1fdd0)
Used fragment shader to get depth however it is not in sync with display output.

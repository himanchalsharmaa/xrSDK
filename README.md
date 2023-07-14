# xrSDK

A custom display provider for Windows Unity made using Unity's XR SDK to support multi-pass and single pass rendering with functions to change projection and view matrix values for each eye.
Used a single camera to get output for both eyes.
[ExampleDisplayProvider.cpp](https://github.com/himanchalsharmaa/xrSDK/blob/master/ExampleDisplayProvider.cpp) is the cpp file used to compile the DLL plugin for display provider.
![image](https://github.com/himanchalsharmaa/xrSDK/assets/95272385/d5eb6898-1dbd-4985-a28c-918db3e1fdd0)
Used fragment shader to get depth however it is not in sync with display output.

# BlobTileMap
A project that creates a TileMap in Unity from a webcam. Takes an incoming webcam feed, downsamples it using a RenderTexture in Unity, and then applies a contrast filter, blob generator to the feed. This in turn is used to make and update a Tilemap.

There are 2 scenes, one generates a Tilemap with borders and other 2D Tilemap features. The 2D scene launches navagents that run across the visible pixels from the webcam and the invisible pixels are rendered as nav obstacles. The result is agents try to stay on visible pixels only.

The 3D scene makes 3D objects, sizes them to fit a renderQuad so that you can check alighment. The 3D scene has a very simple implementation, to help understand how this uses renderTextures for fast downsampling of a webcam feed. Both scenes are running over 100FPS.

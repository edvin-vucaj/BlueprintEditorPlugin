# Blueprint Editor
A plugin for [Frosty Editor](https://github.com/CadeEvs/FrostyToolsuite/tree/1.0.6.3) which allows assets such as LogicPrefabBlueprints and SpatialPrefabBlueprints to be edited in a proper graph form, created using [Nodify](https://github.com/miroiu/nodify).

This tool is still unfinished, with many things relating to optimization and additional support needing to be done. PRs are very much so welcomed!

## What is finished:
- Core editing features, so adding and removing connections/objects
- Node extension functionality(so connections for nodes can be mapped out)
- loading blueprints into graphed form
- layout saving
- xml style configs for node mappings
- Transient node functionality
- Layered Graph Drawing algorithm for sorting nodes
- Comment, & Redirect nodes
- Options for customizing the look and functionality of the editor
  
- It would be appreciated if you could at the very least add documentation to any methods/classes you create, though full comments explaining the process of what code is doing is the most strongly preferred. I know I am guilty of not doing this a lot in the code, though it makes managing PRs and code much easier. If I have any problems with how you are doing it, I will likely bring it up, though its not like I will strike you down like Zeus for not having enough comments keep in mind... At least hopefully for your sakes.

- This requires [Prism](https://www.nuget.org/packages/Prism.Wpf/), which is avaliable as a NuGet package.

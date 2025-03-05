# Voxel Chunk Generation Project

A Unity-based system for generating chunks from Blender-imported objects and applying linear transformations dynamically. This project makes chunk generation more engaging with creativity!
This project handles meshes and colliders manipulation, simple transformation updates, making it ideal for terrain systems.

### Parameters are fine-tunable through the World GameObject in the Sample Scene provided! ###
## Demo Pictures ##

No transformations / Padding = 0 

<img src="DemoPics/Demo1.png" alt="Demo Image" width="600"/>

Transformation from Cube to Parallelogram using Linear Transformations / Padding = 1 for visualization.
<img src="DemoPics/Demo2.png" alt="Demo Image 2" width="600"/>

### This Demo was created using the following object created on Blender ###
<img src="DemoPics/BlenderObject.png" alt="Blender Object" width="300"/>
<img src="DemoPics/ObjectTemplate.png" alt="Object Template" width="400"/>

## How to Use ##
1. Create your desired shape for each Voxel [Basic Tutorial](https://www.youtube.com/watch?v=ZYYkdNhfMhw)
2. In the object's Inspect Tab set Read/Write to True
<img src="DemoPics/ConfigReadWrite.png" alt="Configs Template" width="400"/>
3. Change the Chunk's BlockPrefab to your new object and save.
<img src="DemoPics/ConfigObject.png" alt="Configs Object" width="400"/>
4. Fine-tune the padding and vectors according to the object passed


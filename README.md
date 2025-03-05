# Voxel Chunk Generation Project

A Unity-based system for generating chunks from Blender-imported objects and applying linear transformations dynamically. This project makes chunk generation more engaging with creativity!
This project handles meshes and colliders manipulation, simple transformation updates, making it ideal for terrain systems.

### Fine-tunable Parameters ###
<img src="DemoPics/WorldConfigs.png" alt="Parameters Image" width="600"/>
1. Vector1, Vector2, Vector3, represent the linear transformation vectors in the 3D space, such vectors in the example image does not make any changes as they are 1's.
2. X_offset, represents the shift in X-axis as the Y increases, resulting in a Brick-Wall effect.
3. Chuck Size_X,Y,Z represent each chunk dimensions.
4. X,Y,Z_Padding represent the padding between each voxel, This is useful when object passed is not a cube.

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


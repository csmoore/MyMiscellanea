# 2525D Library

This is just a prototype project so I could learn 2525D and test/validate the data provided in the standard. It includes a test app to draw the symbols. 

![Image of Military Features Data](Screenshot.jpg)

## Features

* A sample data model that corresponds to 2525D symbol set
* A sample/prototype search capability
* A sample search and draw UI
    * IMPORTANT: This app depends on a set of SVG image files that are not yet publicly available. The drawing portion of this app will not be functional without these image files. Should the availability of this set of images change, I'll update to provide the link.
    * If you have these images, just update their location at: [./Library2525D/MilitarySymbolToGraphicLayersMaker.cs](./Library2525D/MilitarySymbolToGraphicLayersMaker.cs) and rebuild the project


## Licensing

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and
limitations under the License.

Note: This project also uses the C# SVG Rendering Engine (current Github forked repo below)

The C# SVG Rendering Engine is governed by the Microsoft Public License: https://svg.codeplex.com/license

For more information see the project pages at:

http://svg.codeplex.com/

https://github.com/vvvv/SVG 

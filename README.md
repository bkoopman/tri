tri
===
SDL Tridion Reference Implementation


About
-----
The SDL Tridion Reference Implementation is a reference implementation of SDL Tridion intended to help you create, design and publish an SDL Tridion-based Web site quickly.

You can find more details and a download of the entire release on http://www.sdltridionworld.com/community/2011_extensions/reference_implementation.aspx


Support
---------------
The SDL Tridion Reference Implementation is intended as a toolkit to help the SDL Tridion community and is not an officially supported SDL Tridion product.

If you encounter problems, reach out to the community: http://tridion.stackexchange.com/


Sources
-------

The official v1.0 GA release (downloadable on http://www.sdltridionworld.com/community/2011_extensions/reference_implementation.aspx) contains only the **Site** project in the web-application Visual Studio solution, since only that part of the source is considered public API (as in, you are expected to change that). This repository contains the full source of all the `Sdl.Web.*` DLLs to give you insight in how the solution is built and what is there available for you to extend. You are free to use these sources under the terms and conditions of the license mentioned below, however we suggest you only change the code of the **Site** project and make use of the compiled `Sdl.Web.Common.dll`, `Sdl.Web.DD4T.dll`, `Sdl.Web.Mvc.dll` and `Sdl.Web.Tridion.dll` from SDL Tridion World. 


Hotfix
------

We released a hotfix (stri-1.0.1) on top of the v1.0 GA release, the sources here are updated with the changes in this hotfix, and so is the download package on SDL Tridion World. The hotfix is also separately available to be installed on an existing V1.0 GA Reference Implementation installation. Details about this hotfix and its installation instructions are available inside the zipfile.


License
-------
Copyright (c) 2011-2014 SDL Group.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and limitations under the License.

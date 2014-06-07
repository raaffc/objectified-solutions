#Objectified Solutions

[![Support us][gittip-image]][gittip-url]

## Like what you see?
[Support us via gittip](https://www.gittip.com/agray/)


Solution File Object Modeller

This class library produces an navigatable object graph of an Visual Studio solution (.sln file) and subordinate project(s) (.csproj file(s)).

The information currently gathered is:

- Global properties of the solution
- List of projects in solution
- Global properties of each project
- List of source files in each project
- List of System references that each project depends on
- List of Project references that each project depends on
- Basic properties of each source file


General Notes:
- FileView object is the main object of the SolutionObject
- SolutionView picks up the NestedProjects section of the sln file. If the SolutionView object is null it means that the solution does not have any solution folders defined.


Release Notes:
- 1.0.0.0: Alpha release.
- 1.0.0.1: Added readme.md to NuGet package.
- 1.0.0.2: Corrected description for NuGet package.
- 1.0.0.3: Small tweaks.
- 2.0.0.0: Major release. Altered API. Moved existing API to FileView namespace and added SolutionView namespace. Added support for wixproj files

[gittip-image]: http://img.shields.io/gittip/agray.svg
[gittip-url]: https://www.gittip.com/agray/

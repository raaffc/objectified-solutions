objectified-solutions
=====================

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

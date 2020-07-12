Toolset Dataset --excel=..\ --binary=.\Bin\ --csv=.\CSV\ --csharp=.\Code\CShape --cpp=.\Code\CPP --cache=..\Cache
xcopy .\Code\CPP\* ..\..\..\Server\Include\DataService\automake /S /Y
xcopy .\Bin\* ..\..\..\Server\bin\Config\Bin\ /S /Y
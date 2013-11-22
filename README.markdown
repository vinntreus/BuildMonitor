# Build monitor
Measures build times in Visual Studio

## To install
Download either via searching in Extension Manager in Visual studio or [download it manually](http://visualstudiogallery.msdn.microsoft.com/b0c87e47-f4ee-4935-9a59-f2c81ce692ab)

## How it works
When any solution is opened, the extension is loaded and will start output to a output-window called "Build monitor" 
(Found in View -> Output -> Change "Show output from:" dropdown-list). 
It measures solution total build time and each project built. 
After every build the data is also saved to a file on disk (see output-window for path) as json.

## Notes for contributors
- Install the [Visual Studio 2013 SDK](http://www.microsoft.com/en-us/download/confirmation.aspx?id=40758) to get references needed to be able to open the projects.

## Features and bugs
If you find a bug or desire some feature, please create an issue here in Github.
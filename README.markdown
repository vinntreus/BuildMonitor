# Build monitor
Measures build times in Visual Studio.

This is a fork of the original, that adds an IsRebuildAll flag and a form showing the breakdown by month and solution (under `Tools - Show Solution Build Times`). It also saves build time data to a database so that we can report on it, which it would be better to comment out before using.

## To install
Clone this repo, build, and then double click on the vsix in `BuildMonitorPackage\bin\Release` (or debug output folder).

## How it works
When any solution is opened, the extension is loaded and will start output to a output-window called "Build monitor" 
(Found in View -> Output -> Change "Show output from:" dropdown-list). 

It measures solution total build time and each project built. 

After every build the data is also saved to a file on disk (see output-window for path) as json.

## Features and bugs
If you find a bug or desire some feature, please create an issue here in Github.

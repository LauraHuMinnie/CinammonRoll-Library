# CinammonRoll-Library
Custom media library app that handles your watchlist and your reviews of your favourite TV shows

## Getting Started

1. Clone the repository
2. Setup the library as mentioned below.
3. Run the project by building the solution.

### Setting up the media library
NOTE: As of now, there isn't a reliable api or website to obtain image data for both the TV series poster or panel. 
This feature will be implemented soon whereby the program would try to find possible poster and panel images of the available TV shows within their library.
To setup your media library so the app could register it, follow the structure stated below:
```
+-- TVLibrary
|   +-- TVSeries1
|   |   +-- Episode1.mkv
|   |   +-- Episode2.mkv
|   |   +-- poster.jpg
|   |   +-- panel.jpg
|   +-- TVSeries2
|   |   +-- Episode1.mkv
|   |   +-- Episode2.mkv
```
The media files should have .mkv as their containers. Further implementation on the app would support other container formats.
Both poster and panel image files can either be jpeg/jpg file or a png file.
For best display, the resolution of the poster should be 338 x 500 but any image size with the same ratios are compatible.
The panel could be a simple 1080p based image. This would be displayed within the queue screen and the series details screen.

### Prerequisites

TODO: Check required dependencies
..* Windows10, build version 1809 (required or you won't be able to see the UI within the XAML editor.
..* VLC.MediaElement 3.1.0.1
..* VLC.MediaElement.ClassLibrary 3.1.0.1

#### Downloading VLC Modules
VLC.MediaElement : [![NuGet](https://img.shields.io/nuget/v/VLC.MediaElement.svg)](https://www.nuget.org/packages/VLC.MediaElement)

VLC.MediaElement.ClassLibrary : [![NuGet](https://img.shields.io/nuget/v/VLC.MediaElement.ClassLibrary.svg)](https://www.nuget.org/packages/VLC.MediaElement.ClassLibrary)

## Authors

* **Harith Borhan** - *Initial work* - [habim84](https://github.com/habim84)

## License

This project is licensed under the GNU GPL2 license.

## Acknowledgments

* [Kakone's](https://github.com/kakone) [VLC.MediaElement](https://github.com/kakone/VLC.MediaElement)

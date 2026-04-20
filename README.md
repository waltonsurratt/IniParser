# IniParser
This a new project for a native Windows library (DLL) built using only the .NET 10.0 standard/core. It's purpose is to read & write INI files stored on any computer. It also acts as a drop-in library to parse INI files inside other projects and applications. The latest build supports .NET 10.0 standard/core and C# version 8.0.

This is a clean, integration‑ready INI parser design and implementation built only with .NET Core libraries (no NuGet, no Win32 P/Invoke). The design also operates at the line-level allowing for true preservation of comments (_on the same line and in the same order as loaded into memory_).

# Version: 1.0
The library currently includes the following features:
* Load INI files into memory
* Modify **Key->Value** pairs
* Create/Write new INI files
* Uses only the official delimiter for comments ( **;** )
* Preserves comments in stored files
* Supports **UTF-16 / ANSI compatibility**
* Merge File Support
* Compare File Support
* Safe overwrite behavior

There are some design elements not in place for this version of the library (DLL), such as deterministic ordering (key ordering - sequential or alphabetic), structural difference output, creating new comments, ToString() override definitions for simple key-value and section outputs, and zero‑copy streaming. These are all features being considered for future revisions, but for now the current version should suffice for most INI file scenarios.

# API Calls
Load a new INI file into the program

`var ini = Document.Load("C:\\tmp\\settings.ini");`

Remove a key named "Theme" from the Section "App"

`ini.GetSection("App").RemoveKey("Theme");`

Create a new key named "Server" with an empty/blank value in the Section "App"

`ini.GetSection("App").CreateKey("Server");`

In the section called "Network", create a new Key AND set the value all in one line

`ini.GetSection("Network").CreateKey("URL").SetValue("https://wackywalt.com");`

Save the INI file to the system

`ini.Save("C:\\tmp\\settings.ini");`

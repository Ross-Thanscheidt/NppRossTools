# NppRossTools - A C# Notepad++ Plugin

## Tools

- **Update Ages**

  This will update every line in the current document that starts with a birthdate (in `MM/dd/yyyy` format) that has `(# in yyyy)` somewhere on the line, which should reflect their age for that year.
  
  For example, if you had a document like this:
  
  ```
  Celebrities
  
  05/04/1928 Maynard Ferguson (94 in 2022) (died at 78 on 08/23/2006)
  05/31/1930 Clint Eastwood (92 in 2022)
  06/09/1961 Michael J. Fox (61 in 2022)
  ```
  
  Then after running `Update Ages` in 2023 the document should look like this:
  
  ```
  Celebrities
  
  05/04/1928 Maynard Ferguson (95 in 2023) (died at 78 on 08/23/2006)
  05/31/1930 Clint Eastwood (93 in 2023)
  06/09/1961 Michael J. Fox (62 in 2023)
  ```
  
## Development Environment

  Notepad++ - v8.5.4 (64-bit)  
  Visual Studio 2022 Preview - v17.8 Preview 1  
  [NppPlugin .NET package for VS2019 and beyond - v0.95.00 (Jan 2021)](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net/releases/download/0.95.00/NppPlugin0.95.00.zip)  

  Building the project in Visual Studio should add (or update) `NppRossTools\NppRossTools.dll` under `C:\Program Files\Notepad++\plugins`, which will add `Ross Tools` under `Plugins` when you run Notepad++.  

## References

  https://npp-user-manual.org/docs/plugins/#how-to-develop-a-plugin  
  https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net  

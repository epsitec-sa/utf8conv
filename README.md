# utf8conv

This tool converts text files from CP1252 or UTF-8 with BOM to UTF-8 without BOM.

Usage:

`utf8converter *.cs path/to/my/files`

## Notes

This is my first .NETcore project. I went through following hurdles while developing it:

1. I had to add NuGet packages for `System.Text.Encoding`, `System.Text.Encoding.CodePages`
and `System.Text.Encoding.Extensions` (all in a prerelease rc2 version on May 26th 2016).
2. I had to call `Encoding.RegisterProvider()` so that `GetEncoding(1252)` would work.
3. I had to edit the `project.json` file to remove `"type": "platform"` and add a
`"runtimes"` section (see [issue](https://github.com/dotnet/core/issues/77)).

Publishing the executables is done with `dotnet publish`.

All files found in `utf8conv\Utf8Converter\bin\Debug\netcoreapp1.0\win7-x64` need to be
copied along in order for the tool to work.

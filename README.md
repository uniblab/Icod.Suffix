# Icod.Suffix
Suffix.exe appends the specified string to each line of input.

## Usage
`Suffix.exe (-h | --help | /help)`
Displays this text.

`Suffix.exe (-c | --copyright | /copyright)`
Displays copyright and licensing information.

`Suffix.exe (-s | --suffix | /suffix) theSuffix [(-i | --input | /input) inputFilePathName] [(-o | --output | /output) outputFilePathName] [(-t | --trim | /trim)]`
Suffix.exe appends the specified string to each line of input.
inputFilePathName and outputFilePathName may be relative or absolute paths.
If inputFilePathName is omitted then input is read from StdIn.
If outputFilePathName is omitted then output is written to StdOut.
If trim switch is specified, then input lines are trimmed of all surrounding whitespace and empty strings are ignored.

## Copyright and Licensing
Suffix.exe appends the specified string to each line of input.
Copyright (C) 2025 Timothy J. Bruce

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published 
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.


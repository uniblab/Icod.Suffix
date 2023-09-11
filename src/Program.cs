﻿// Suffix.exe appends the specified string to each line of input.
// Copyright (C) 2023 Timothy J. Bruce

/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

namespace Icod.Suffix {

	public static class Program {

		private const System.Int32 theBufferSize = 16384;


		[System.STAThread]
		public static System.Int32 Main( System.String[] args ) {
			var len = args.Length;
			if ( ( 7 < len ) || ( len < 1 ) ) {
				PrintUsage();
				return 1;
			}

			var processor = new Icod.Argh.Processor(
				new Icod.Argh.Definition[] {
					new Icod.Argh.Definition( "help", new System.String[] { "-h", "--help", "/help" } ),
					new Icod.Argh.Definition( "copyright", new System.String[] { "-c", "--copyright", "/copyright" } ),
					new Icod.Argh.Definition( "input", new System.String[] { "-i", "--input", "/input" } ),
					new Icod.Argh.Definition( "output", new System.String[] { "-o", "--output", "/output" } ),
					new Icod.Argh.Definition( "suffix", new System.String[] { "-s", "--suffix", "/suffix" } ),
					new Icod.Argh.Definition( "trim", new System.String[] { "-t", "--trim", "/trim" } ),
				},
				System.StringComparer.OrdinalIgnoreCase
			);
			processor.Parse( args );

			if ( processor.Contains( "help" ) ) {
				PrintUsage();
				return 1;
			} else if ( processor.Contains( "copyright" ) ) {
				PrintCopyright();
				return 1;
			} else if ( len < 2 ) {
				PrintUsage();
				return 1;
			}

			var trim = processor.Contains( "trim" );
			System.Func<System.String, System.String?> trimmer;
			if ( trim ) {
				trimmer = x => x.TrimToNull();
			} else {
				trimmer = x => x;
			}

			if (
				( !processor.TryGetValue( "suffix", false, out var suffix ) )
				|| System.String.IsNullOrEmpty( suffix )
			) {
				PrintUsage();
				return 1;
			}

			System.Func<System.String?, System.Collections.Generic.IEnumerable<System.String>> reader;
			if ( processor.TryGetValue( "input", true, out var inputPathName ) ) {
				if ( System.String.IsNullOrEmpty( inputPathName ) ) {
					PrintUsage();
					return 1;
				} else {
					reader = a => ReadFile( a!, trimmer );
				}
			} else {
				reader = a => ReadStdIn( trimmer );
			}

			System.Action<System.String?, System.Collections.Generic.IEnumerable<System.String>> writer;
			if ( processor.TryGetValue( "output", true, out var outputPathName ) ) {
				if ( System.String.IsNullOrEmpty( outputPathName ) ) {
					PrintUsage();
					return 1;
				} else {
					writer = ( a, b ) => WriteFile( a!, b );
				}
			} else {
				writer = ( a, b ) => WriteStdOut( b );
			}

			writer(
				outputPathName,
				reader( inputPathName ).Select(
					x => x + suffix
				)
			);
			return 0;
		}

		private static void PrintUsage() {
			System.Console.Error.WriteLine( "No, no, no! Use it like this, Einstein:" );
			System.Console.Error.WriteLine( "Suffix.exe --help" );
			System.Console.Error.WriteLine( "Suffix.exe --copyright" );
			System.Console.Error.WriteLine( "Suffix.exe --suffix theSuffix [--input inputFilePathName] [--output outputFilePathName] [--trim]" );
			System.Console.Error.WriteLine( "Suffix.exe appends the specified string to each line of input." );
			System.Console.Error.WriteLine( "inputFilePathName and outputFilePathName may be relative or absolute paths." );
			System.Console.Error.WriteLine( "If inputFilePathName is omitted then input is read from StdIn." );
			System.Console.Error.WriteLine( "If outputFilePathName is omitted then output is written to StdOut." );
			System.Console.Error.WriteLine( "If --trim switch is specified, then input lines are trimmed of all surrounding whitespace and empty strings are ignored." );
		}
		private static void PrintCopyright() {
			var copy = new System.String[] {
				"Suffix.exe appends the specified string to each line of input.",
				"Copyright (C) 2023 Timothy J. Bruce",
				"",
				"",
				"This program is free software: you can redistribute it and / or modify",
				"it under the terms of the GNU General Public License as published by",
				"the Free Software Foundation, either version 3 of the License, or",
				"( at your option ) any later version.",
				"",
				"This program is distributed in the hope that it will be useful,",
				"but WITHOUT ANY WARRANTY; without even the implied warranty of",
				"MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the",
				"GNU General Public License for more details.",
				"",
				"You should have received a copy of the GNU General Public License",
				"along with this program.If not, see <https://www.gnu.org/licenses/>."
			};
			foreach ( var line in copy ) {
				System.Console.WriteLine( line );
			}
		}

		#region io
		private static System.Collections.Generic.IEnumerable<System.String> ReadStdIn( System.Func<System.String, System.String?> trimFunc ) {
			var line = System.Console.In.ReadLine();
			while ( null != line ) {
				line = trimFunc( line );
				if ( null != line ) {
					yield return line;
				}
				line = System.Console.In.ReadLine();
			}
		}
		private static System.Collections.Generic.IEnumerable<System.String> ReadFile( System.String filePathName, System.Func<System.String, System.String?> trimFunc ) {
			using ( var file = System.IO.File.Open( filePathName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read ) ) {
				using ( var reader = new System.IO.StreamReader( file, System.Text.Encoding.UTF8, true, theBufferSize, true ) ) {
					var line = reader.ReadLine();
					while ( null != line ) {
						line = trimFunc( line );
						if ( null != line ) {
							yield return line;
						}
						line = reader.ReadLine();
					}
				}
			}
		}

		private static void WriteStdOut( System.Collections.Generic.IEnumerable<System.String> data ) {
			foreach ( var datum in data ) {
				System.Console.Out.WriteLine( datum );
			}
		}
		private static void WriteFile( System.String filePathName, System.Collections.Generic.IEnumerable<System.String> data ) {
			using ( var file = System.IO.File.Open( filePathName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.None ) ) {
				_ = file.Seek( 0, System.IO.SeekOrigin.Begin );
				using ( var writer = new System.IO.StreamWriter( file, System.Text.Encoding.UTF8, theBufferSize, true ) ) {
					foreach ( var datum in data ) {
						writer.WriteLine( datum );
					}
					writer.Flush();
				}
				file.Flush();
				file.SetLength( file.Position );
			}
		}
		#endregion io

		private static System.String? TrimToNull( this System.String? @string ) {
			if ( System.String.IsNullOrEmpty( @string ) ) {
				return null;
			}
			@string = @string.Trim();
			return System.String.IsNullOrEmpty( @string )
				? null
				: @string
			;
		}

	}

}
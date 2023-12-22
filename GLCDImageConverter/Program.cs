﻿using System.Drawing;
using System.Text.RegularExpressions;

namespace GLCDImageConverter;

partial class Program {
	static byte[]? ReadPixels(string file, out int imageWidth) {
		Bitmap img = new(file);

		if (img.Width > 128 || img.Height > 64) {
			Console.WriteLine("ERROR: Image must be less or equal than 128x64 pixels.");
			imageWidth = -1;
			return null;
		}

		if (img.Height % 8 != 0) {
			Console.WriteLine("ERROR: The image height must be a multiple of 8.");
			imageWidth = -1;
			return null;
		}
        
		byte[] pixels = new byte[img.Width * img.Height / 8];

		for (int i = 0; i < img.Height / 8; i++) {
			for (int j = 0; j < img.Width; j++) {
				byte px = 0;
				for (int k = i * 8, l = 0; k < (i + 1) * 8; k++, l++) {
					Color pixel = img.GetPixel(j, k);
					px |= Convert.ToByte((pixel.A == 0 ? 0 : 1) << l);
				}
				pixels[i * img.Width + j] = px;
			}
		}

		imageWidth = img.Width;
		return pixels;
	}

	static void ExportImage(IReadOnlyCollection<byte>? pixels, string fileName, int imageWidth) {
		if (pixels == null) {
			Console.WriteLine("An error occurred while exporting the array.");
			return;
		}
		
		using StreamWriter file = File.CreateText($"{fileName}.txt");
		file.WriteLine("#include \"GLCD.h\"\n");
		file.Write($"unsigned const char {fileName}[{pixels.Count}] = {{ ");
		foreach (byte t in pixels) {
			file.Write($"{t}, ");
		}
		file.WriteLine("};\n");
		
		file.WriteLine($"void draw_{fileName}(){{");
		file.WriteLine($"\tfor (int i = 0; i < {pixels.Count}; ++i) {{");
		file.WriteLine($"\t\twriteByte(i / {imageWidth}, i % {imageWidth}, {fileName}[i]);");
		file.WriteLine("\t}");
		file.WriteLine("}");
		
		file.Close();

		Console.WriteLine("Image exported successfully.");
	}
	
	static void Main(string[] args) {
		if(args.Length == 0) {
			Console.WriteLine("ERROR: No image file specified.");
			Console.ReadKey();
			return;
		}

		string fileName = Path.GetFileNameWithoutExtension(args[0]);
		Regex variableCheck = MyRegex();
		if (!variableCheck.IsMatch(fileName)) {
			Console.WriteLine("ERROR: File name is not a valid C variable name.");
			Console.ReadKey();
			return;
		}

		byte[]? pixels = ReadPixels(args[0], out int imageWidth);
		ExportImage(pixels, fileName, imageWidth);
		
		Console.WriteLine("Program execution completed, press any key to exit.");
		Console.ReadKey();
	}

    [GeneratedRegex("^[a-zA-Z_$][\\w$]*$")]
    private static partial Regex MyRegex();
}
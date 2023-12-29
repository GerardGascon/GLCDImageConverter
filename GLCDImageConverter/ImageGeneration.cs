using System.Drawing;

namespace GLCDImageConverter;

static class ImageGeneration {
	public static byte[]? ReadPixels(string file, out int imageWidth) {
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
	
	static void ExportFile(IReadOnlyCollection<byte> pixels, string fileName, int imageWidth) {
		using StreamWriter file = File.CreateText($"{fileName}.h");
		
		file.WriteLine($"#ifndef {fileName.ToUpper()}_H");
		file.WriteLine($"#define {fileName.ToUpper()}_H");
		
		file.WriteLine("#include \"GLCD.h\"\n");
		file.WriteLine($"#include \"{fileName}.h\"\n");
		file.Write($"unsigned const char {fileName}[{pixels.Count}] = {{ ");
		foreach (byte t in pixels) {
			file.Write($"{t}, ");
		}
		file.WriteLine("};\n");
		
		file.WriteLine($"void draw_{fileName}(byte offset_y, byte offset_x){{");
		file.WriteLine($"\tfor (int i = 0; i < {pixels.Count}; ++i) {{");
		file.WriteLine($"\t\twriteByte(i / {imageWidth} + offset_y, i % {imageWidth} + offset_x, {fileName}[i]);");
		file.WriteLine("\t}");
		file.WriteLine("}");
		
		file.WriteLine($"#endif //{fileName.ToUpper()}_H");
		
		file.Close();
	}

	public static void ExportImage(IReadOnlyCollection<byte>? pixels, string fileName, int imageWidth) {
		if (pixels == null) {
			Console.WriteLine($"An error occurred while exporting the array. ({fileName})");
			return;
		}
		
		ExportFile(pixels, fileName, imageWidth);

		Console.WriteLine($"Image exported successfully. ({fileName})");
	}
}
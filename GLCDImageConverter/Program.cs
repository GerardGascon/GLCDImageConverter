using System.Drawing;

namespace GLCDImageConverter;

class Program {
	static byte[]? ReadPixels(string file) {
		Bitmap img = new(file);

		if (img.Width != 128 || img.Height != 64) {
			Console.WriteLine("Image must be 128x64 pixels.");
			return null;
		}
        
		byte[]? pixels = new byte[1024];

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < img.Width; j++) {
				byte px = 0;
				for (int k = i * 8, l = 0; k < (i + 1) * 8; k++, l++) {
					Color pixel = img.GetPixel(j, k);
					px |= Convert.ToByte((pixel.A == 0 ? 0 : 1) << l);
				}
				pixels[i * img.Width + j] = px;
			}
		}

		return pixels;
	}

	static void ExportImage(byte[]? pixels) {
		if (pixels == null) return;
		using StreamWriter file = File.CreateText("fileName.txt");
		file.Write("unsigned const char bitmap [1024] = { ");
		foreach (byte t in pixels) {
			file.Write($"{t}, ");
		}
		file.WriteLine("};");
		file.Close();
	}
	
	static void Main(string[] args) {
		if(args.Length == 0) {
			Console.WriteLine("No image file specified.");
			return;
		}

		byte[]? pixels = ReadPixels(args[0]);
		ExportImage(pixels);
	}
}
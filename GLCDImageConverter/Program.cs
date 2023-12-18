using System.Drawing;

namespace GLCDImageConverter;

class Program {
	static void ReadPixels(string file) {
		Bitmap img = new(file);
		for (int i = 0; i < img.Width; i++) {
			for (int j = 0; j < img.Height; j++) {
				Color pixel = img.GetPixel(i, j);

				Console.Write(pixel.A == 0 ? "0" : "1");
			}
		}
	}
	
	static void Main(string[] args) {
		if(args.Length == 0) {
			Console.WriteLine("No image file specified.");
			return;
		}

		ReadPixels(args[0]);
	}
}
using System.Text.RegularExpressions;

namespace GLCDImageConverter;

partial class Program {
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

		byte[]? pixels = ImageGeneration.ReadPixels(args[0], out int imageWidth);
		ImageGeneration.ExportImage(pixels, fileName, imageWidth);
		
		Console.WriteLine("Program execution completed, press any key to exit.");
		Console.ReadKey();
	}

    [GeneratedRegex("^[a-zA-Z_$][\\w$]*$")]
    private static partial Regex MyRegex();
}
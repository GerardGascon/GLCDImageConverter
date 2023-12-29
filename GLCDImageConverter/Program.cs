using System.Text.RegularExpressions;

namespace GLCDImageConverter;

partial class Program {
	static void Main(string[] args) {
		if(args.Length == 0) {
			Console.WriteLine("ERROR: No image file specified.");
			Console.ReadKey();
			return;
		}

		foreach (string file in args) {
			string fileName = Path.GetFileNameWithoutExtension(file);
			Regex variableCheck = C_CompatibleVariable();
			if (!variableCheck.IsMatch(fileName)) {
				Console.WriteLine($"ERROR: File name of '{fileName}' is not a valid C variable name.");
				Console.ReadKey();
				return;
			}
			
			byte[]? pixels = ImageGeneration.ReadPixels(file, out int imageWidth);
			ImageGeneration.ExportImage(pixels, fileName, imageWidth);
		}
		
		Console.WriteLine("Program execution completed, press any key to exit.");
		Console.ReadKey();
	}

    [GeneratedRegex("^[a-zA-Z_$][\\w$]*$")]
    private static partial Regex C_CompatibleVariable();
}
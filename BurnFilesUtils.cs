namespace Super_Burner
{
	internal static class BurnFilesUtils
	{
		public static BurnFileInfo[] ListBurnableFiles()
		{
			var files = Directory.GetFiles(Constants.BURN_DIR);

			var burnableFiles = files.Select(filePath =>
			{
				var f = new FileInfo(filePath);
				long sizeMb = f.Length / (1024 * 1024);
				short sizeShort = sizeMb > short.MaxValue ? short.MaxValue : (short)sizeMb;

				return new BurnFileInfo()
				{
					name = f.Name,
					size = sizeShort,
					extension = f.Extension,
				};
			});

			return [..burnableFiles];
		}
	}

	public struct BurnFileInfo {
		public string name;
		public short size; // In MB
		public string extension;
	}
}

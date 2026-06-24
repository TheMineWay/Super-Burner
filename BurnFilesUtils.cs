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
				float sizeMb = f.Length / (float)(1024 * 1024);

				return new BurnFileInfo()
				{
					name = f.Name,
					size = sizeMb,
					extension = f.Extension,
				};
			});

			return [..burnableFiles];
		}
	}

	public struct BurnFileInfo {
		public string name;
		public float size; // In MB
		public string extension;
	}
}

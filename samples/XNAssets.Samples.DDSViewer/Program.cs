namespace XNAssets.Samples.DdsViewer
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var game = new ViewerGame())
				game.Run();
		}
	}
}

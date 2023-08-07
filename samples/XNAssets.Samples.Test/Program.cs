namespace XNAssets
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using (var game = new TestGame())
				game.Run();
		}
	}
}

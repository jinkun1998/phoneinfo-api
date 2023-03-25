namespace PhoneInfo.API.Tests
{
	[TestFixture]
	public class SearchTests
	{
		private ISearchService _searchService;

		[SetUp]
		public void SetUp()
		{
			DoSetup.Setup();
			_searchService = DoSetup.GetService<ISearchService>();
		}

		[Test]
		[TestCase("apple")]
		[TestCase("samsung")]
		[TestCase("huawei")]
		[TestCase("sony")]
		public async Task GetGlossaries(string keyword)
		{
			(string response, _) = await _searchService.SearchAll(keyword);
			object result = GlossaryParser.Parse(response, GlossaryParser.GlossaryType.List);
			Assert.That(result, Is.InstanceOf<object>());
		}
	}
}

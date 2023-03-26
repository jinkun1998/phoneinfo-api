
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace PhoneInfo.API.Tests
{
	[TestFixture]
	public class CatalogTests
	{
		private ICatalogService _catalogService;

		[SetUp]
		public void Setup()
		{
			DoSetup.Setup();
			_catalogService = DoSetup.GetService<ICatalogService>();
		}

		[Test]
		public async Task GetBranchsAsync()
		{
			(string html, _) = await _catalogService.GetBrandsAsync();
			object result = CatalogParser.Parse(html, CatalogParser.CatalogType.Branchs);
			Assert.That(result, Is.InstanceOf<object>());
		}

		[Test]
		[TestCase("acer-phones-59", TestName = "acer")]
		[TestCase("allview-phones-88", TestName = "allview")]
		[TestCase("amazon-phones-76", TestName = "amazon")]
		[TestCase("amoi-phones-28", TestName = "amoi")]
		[Parallelizable(ParallelScope.All)]
		public async Task GetBranchAsync(string brand)
		{
			(string html, _) = await _catalogService.GetProductByBrandAsync(brand);
			object result = CatalogParser.Parse(html, CatalogParser.CatalogType.Branch);
			Assert.That(result, Is.InstanceOf<object>());
		}

		[Test]
		[TestCase("apple_iphone_13_pro_max-11089", TestName = "iphone_13_promax")]
		[TestCase("apple_ipad_pro_12_9_(2022)-11939", TestName = "ipad_pro_12.9_2022")]
		[TestCase("apple_iphone_14_pro_max-11773", TestName = "iphone_14_promax")]
		[TestCase("apple_watch_ultra-11827", TestName = "watch_ultra")]
        [Parallelizable(ParallelScope.All)]
		public async Task GetProductDetailAsync(string device)
		{
			(string html, _) = await _catalogService.GetProductDetailAsync(device);
			object result = CatalogParser.Parse(html, CatalogParser.CatalogType.Device);
			Assert.That(result, Is.InstanceOf<object>());
		}
	}
}
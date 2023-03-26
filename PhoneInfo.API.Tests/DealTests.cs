namespace PhoneInfo.API.Tests
{
	[TestFixture]
	public class DealTests
	{
		private IDealService _dealService;

		[SetUp]
		public void SetUp()
		{
			DoSetup.Setup();
			_dealService = DoSetup.GetService<IDealService>();
		}

		[Test]
		public async Task GetDeal()
		{
			(string response, _) = await _dealService.GetDealsAsync();
			object result = DealParser.Parse(response, DealParser.DealType.Deals);
			Assert.That(result, Is.InstanceOf<object>());
		}

		[Test]
		public async Task GetTop10DailyInterest()
		{
			(string response, _) = await _dealService.GetDealsAsync();
			object result = DealParser.Parse(response, DealParser.DealType.Top10DailyInterest);
			Assert.That(result, Is.InstanceOf<object>());
		}

		[Test]
		public async Task GetTop10ByFans()
		{
			(string response, _) = await _dealService.GetDealsAsync();
			object result = DealParser.Parse(response, DealParser.DealType.Top10ByFans);
			Assert.That(result, Is.InstanceOf<object>());
		}
	}
}

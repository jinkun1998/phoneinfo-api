using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneInfo.API.Tests
{
	[TestFixture]
	public class GlossaryTests
	{
		private IGlossaryService? _glossaryService;

		[SetUp]
		public void SetUp()
		{
			DoSetup.Setup();
			_glossaryService = DoSetup.GetService<IGlossaryService>();
		}

		[Test]
		public async Task GetGlossaries()
		{
			(string response, _) = await _glossaryService.GetGlossaries();
			object result = GlossaryParser.Parse(response, GlossaryParser.GlossaryType.List);
			Assert.That(result, Is.InstanceOf<object>());
		}

		[Test]
		[TestCase("apple-airplay-2", TestName = "apple_airplay_2")]
		[TestCase("bada-os", TestName = "bada_os")]
		[TestCase("chipset", TestName = "chipset")]
		[TestCase("firefox-os", TestName = "firefox-os")]
		public async Task GetTop10DailyInterest(string term)
		{
			(string response, _) = await _glossaryService.GetTerm(term);
			object result = GlossaryParser.Parse(response, GlossaryParser.GlossaryType.Term);
			Assert.That(result, Is.InstanceOf<object>());
		}
	}
}

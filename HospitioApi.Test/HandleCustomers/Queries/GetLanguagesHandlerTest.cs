using FakeItEasy;
using HospitioApi.Core.HandleCustomers.Queries.GetLanguages;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Language;
using HospitioApi.Data;
using Xunit;
using ThisTestFixure = HospitioApi.Test.HandleCustomers.Queries.GetLanguagesHandlerFixure;

namespace HospitioApi.Test.HandleCustomers.Queries
{
    public class GetLanguagesHandlerTest : IClassFixture<ThisTestFixure>
    {
        public readonly ThisTestFixure _fix;
        public GetLanguagesHandlerTest(ThisTestFixure fix)
        {
            _fix = fix;
        }
        [Fact]
        public async Task Success()
        {
            var _languageService = A.Fake<ILanguageService>();
            var _dapper = A.Fake<IDapperRepository>();

            A.CallTo(() => _languageService.GetSupportedLanguageAsync(CancellationToken.None)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Languages);

            var result = await _fix.BuildHandler(_dapper, _languageService).Handle(new(), CancellationToken.None);

            Assert.True(result.HasResponse);
            Assert.True(result.Response!.Message == "Get languages successful.");

        }
        [Fact]
        public async Task Not_Found_Error()
        {
            var _languageService = A.Fake<ILanguageService>();
            var _dapper = A.Fake<IDapperRepository>();

            string actualString = _fix.Languages;
            _fix.Languages = null;

            A.CallTo(() => _languageService.GetSupportedLanguageAsync(CancellationToken.None)).WhenArgumentsMatch(x => x.Count() > 0).Returns(_fix.Languages);
            var result = await _fix.BuildHandler(_dapper, _languageService).Handle(new(), CancellationToken.None);

            Assert.True(result.HasFailure);
            Assert.True(result.Failure!.Message == "Languages not found.");

            _fix.Languages = actualString;
        }
    }
    public class GetLanguagesHandlerFixure : DbFixture
    {
        public HandlerResponseFactory Response { get; set; } = new HandlerResponseFactory();
        public string Languages { get; set; } = string.Empty;
        protected override void Seed()
        {
            using var db = new ApplicationDbContext(DbContextOptions, TenantService);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Languages  = @"{
  ""translation"": {
    ""af"": {
      ""name"": ""Afrikaans"",
      ""nativeName"": ""Afrikaans"",
      ""dir"": ""ltr""
    },
    ""am"": {
      ""name"": ""Amharic"",
      ""nativeName"": ""\u12A0\u121B\u122D\u129B"",
      ""dir"": ""ltr""
    },
    ""ar"": {
      ""name"": ""Arabic"",
      ""nativeName"": ""\u0627\u0644\u0639\u0631\u0628\u064A\u0629"",
      ""dir"": ""rtl""
    },
    ""as"": {
      ""name"": ""Assamese"",
      ""nativeName"": ""\u0985\u09B8\u09AE\u09C0\u09AF\u09BC\u09BE"",
      ""dir"": ""ltr""
    },
    ""az"": {
      ""name"": ""Azerbaijani"",
      ""nativeName"": ""Az\u0259rbaycan"",
      ""dir"": ""ltr""
    },
    ""ba"": {
      ""name"": ""Bashkir"",
      ""nativeName"": ""Bashkir"",
      ""dir"": ""ltr""
    },
    ""bg"": {
      ""name"": ""Bulgarian"",
      ""nativeName"": ""\u0411\u044A\u043B\u0433\u0430\u0440\u0441\u043A\u0438"",
      ""dir"": ""ltr""
    },
    ""bho"": {
      ""name"": ""Bhojpuri"",
      ""nativeName"": ""Bhojpuri"",
      ""dir"": ""ltr""
    }
  }
}";

        }
        public GetLanguagesHandler BuildHandler(IDapperRepository _dapper, ILanguageService languageService) => new(_dapper, Response, languageService);

    }
}

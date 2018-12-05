using CandleScraper.ExternalApi.CoinMarketCapPro.Config;
using CandleScraper.ExternalApi.CoinMarketCapPro.Interfaces;
using CandleScraper.ExternalApi.CoinMarketCapPro.Models;
using CandleScraper.ExternalApi.Common;
using CandleScraper.ExternalApi.Common.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace CandleScraper.ExternalApi.CoinMarketCapPro
{
	public class CoinMarketCapProClient : TypedHttpClient, ICoinMarketCapProClient
	{
		private readonly CoinMarketCapProConfig _config;


		public CoinMarketCapProClient(CoinMarketCapProConfig config)
		{
			_config = config;
			SerializerSettings = new JsonSerializerSettings()
			{
				MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
				DateParseHandling = DateParseHandling.None,
				Converters =
				{
					new IsoDateTimeConverter()
					{
						DateTimeStyles = DateTimeStyles.AssumeUniversal
					}
				}
			};
			DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _config.ApiKey);
		}


		// ICoinMarketCapProClient ////////////////////////////////////////////////////////////////
		/// <summary>
		/// Not an official endpoint of CoinMarketCap Pro API
		/// </summary>
		public async Task<CmcpAvailableSupplyApi> GetSupplyData(String slug, Int64 start, Int64 end)
		{
			var urlBuilder = new StringBuilder();
			urlBuilder.Append($"https://graphs2.coinmarketcap.com/currencies/{slug}/{start}/{end}");
			using(var httpResponse = await GetAsync(urlBuilder.ToString()))
			{
				try
				{
					var response = JsonConvert.DeserializeObject<CmcpAvailableSupplyApi>(await httpResponse.Content.ReadAsStringAsync(), SerializerSettings);
					if(!httpResponse.IsSuccessStatusCode)
						throw new RequestExecutionApiException($"CMCP-API error");

					return response;
				}
				catch(Exception ex)
				{
					throw new RequestExecutionApiException($"CMCP-API error: {ex.Message}", ex);
				}
			}
		}

		/// <summary>
		/// Returns a paginated list of all cryptocurrencies by CoinMarketCap ID. 
		/// </summary>
		/// <returns></returns>
		public async Task<CmcpCryptocurrencyMapApi> GetCryptocurrencyMapAsync(GetCryptocurrencyMapRequestApi request)
		{
			var urlBuilder = new StringBuilder();
			urlBuilder.Append($"{_config.BaseUri}/v1/cryptocurrency/map");
			urlBuilder.Append($"?listing_status={request.ListingStatus}");
			urlBuilder.Append($"&start={request.StartPage}");
			urlBuilder.Append($"&limit={request.PageSize}");

			if(request.Symbol != null && request.Symbol.Length != 0)
				urlBuilder.Append($"&symbol={String.Join(",", request.Symbol)}");

			using(var httpResponse = await GetAsync(urlBuilder.ToString()))
			{
				try
				{
					var response = JsonConvert.DeserializeObject<CmcpCryptocurrencyMapApi>(await httpResponse.Content.ReadAsStringAsync(), SerializerSettings);
					if(!httpResponse.IsSuccessStatusCode)
						throw new RequestExecutionApiException($"CMCP-API error: {response.Status.ErrorMessage}");

					return response;
				}
				catch(Exception ex)
				{
					throw new RequestExecutionApiException($"CMCP-API error: {ex.Message}", ex);
				}
			}
		}

		/// <summary>
		/// Returns all static metadata for one or more cryptocurrencies including name, symbol, logo, and its various registered URLs.
		/// </summary>
		/// <param name="id">One or more CoinMarketCap cryptocurrency IDs. Example: "1,2"</param>
		public async Task<CmcpCryptocurrencyInfoApi> GetCryptocurrencyInfoAsync(Int64[] id)
		{
			return await GetCryptocurrencyInfoAsync(id, null);
		}

		/// <summary>
		/// Returns all static metadata for one or more cryptocurrencies including name, symbol, logo, and its various registered URLs.
		/// </summary>
		/// <param name="id">One or more CoinMarketCap cryptocurrency IDs. Example: "1,2"</param>
		/// <param name="symbol">Alternatively pass one or more cryptocurrency symbols. Example: "BTC,ETH". At least one "id" or "symbol" is required.</param>
		public async Task<CmcpCryptocurrencyInfoApi> GetCryptocurrencyInfoAsync(Int64[] id, String[] symbol)
		{
			var urlBuilder = new StringBuilder();
			urlBuilder.Append($"{_config.BaseUri}/v1/cryptocurrency/info?");

			if(id.Length != 0)
				urlBuilder.Append($"&id={String.Join(",", id)}");

			if(symbol != null && symbol.Length != 0)
				urlBuilder.Append($"&symbol={String.Join(",", symbol)}");

			using(var httpResponse = await GetAsync(urlBuilder.ToString()))
			{
				try
				{
					var response = JsonConvert.DeserializeObject<CmcpCryptocurrencyInfoApi>(await httpResponse.Content.ReadAsStringAsync(), SerializerSettings);
					if(!httpResponse.IsSuccessStatusCode)
						throw new RequestExecutionApiException($"CMCP-API error: {response.Status.ErrorMessage}");

					return response;
				}
				catch(Exception ex)
				{
					throw new RequestExecutionApiException($"CMCP-API error: {ex.Message}", ex);
				}
			}
		}

		public async Task<CmcpCryptocurrencyListingsLatestApi> GetCryptocurrencyListingLatest(GetCryptocurrencyListingsLatestRequestApi request)
		{
			var urlBuilder = new StringBuilder();
			urlBuilder.Append($"{_config.BaseUri}/v1/cryptocurrency/listings/latest");
			urlBuilder.Append($"?start={request.StartPage}");
			urlBuilder.Append($"&limit={request.ReturnLimit}");
			urlBuilder.Append($"&convert={request.Convert}");
			urlBuilder.Append($"&sort={request.Sort}");
			urlBuilder.Append($"&sort_dir={request.SortDir}");
			urlBuilder.Append($"&cryptocurrency_type={request.Type}");

			using(var httpResponse = await GetAsync(urlBuilder.ToString()))
			{
				try
				{
					var json = await httpResponse.Content.ReadAsStringAsync();
					var response = JsonConvert.DeserializeObject<CmcpCryptocurrencyListingsLatestApi>(json, SerializerSettings);

					if(!httpResponse.IsSuccessStatusCode)
						throw new RequestExecutionApiException($"CMCP-API error: {response.Status.ErrorMessage}");

					return response;
				}
				catch(Exception ex)
				{
					throw new RequestExecutionApiException($"CMCP-API error: {ex.Message}", ex);
				}
			}
		}
	}
}
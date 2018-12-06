using AutoMapper;
using CandleScraper.Database.Models.Storage;
using CandleScraper.Services.Models;
using MongoDB.Bson;

namespace CandleScraper.Core.AutoMapper.Profiles
{
	public class AssetProfile : Profile
	{
		public AssetProfile()
		{
			CreateMap<AssetDto, AssetDb>()
				.ForMember(p => p.Id, opt => opt.MapFrom(p => new ObjectId(p.Id)))
				.ReverseMap()
				.ForMember(p => p.Id, opt => opt.MapFrom(p => p.Id.ToString()));

			CreateMap<CollectedDailyOhlcDto, DailyOhlcDto>()
				.ForMember(p => p.Id, opt => opt.Ignore())
				.ForMember(p => p.TimeOpen, opt => opt.MapFrom(p => p.HistoricalData.TimeOpen))
				.ForMember(p => p.Open, opt => opt.MapFrom(p => p.HistoricalData.Open))
				.ForMember(p => p.High, opt => opt.MapFrom(p => p.HistoricalData.High))
				.ForMember(p => p.Low, opt => opt.MapFrom(p => p.HistoricalData.Low))
				.ForMember(p => p.Close, opt => opt.MapFrom(p => p.HistoricalData.Close))
				.ForMember(p => p.Volume, opt => opt.MapFrom(p => p.HistoricalData.Volume))
				.ForMember(p => p.MarketCap, opt => opt.MapFrom(p => p.HistoricalData.MarketCap))
				.ForMember(p => p.CirculatingSupply, opt => opt.MapFrom(p => p.CoinSummary.CirculatingSupply))
				.ForMember(p => p.CirculatingSupply, opt => opt.MapFrom(p => p.CoinSummary.CirculatingSupply))
				.ForMember(p => p.TotalSupply, opt => opt.MapFrom(p => p.CoinSummary.TotalSupply))
				.ForMember(p => p.MaxSupply, opt => opt.MapFrom(p => p.CoinSummary.MaxSupply));

			CreateMap<DailyOhlcDto, DailyOhlcDb>()
				.ForMember(p => p.Id, opt => opt.MapFrom(p => new ObjectId(p.Id)))
				.ForMember(p => p.AssetId, opt => opt.MapFrom(p => new ObjectId(p.AssetId)));
		}
	}
}
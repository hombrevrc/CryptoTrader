﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTrading.DAL.Models;

namespace CryptoTrading.Logic.Repositories.Interfaces
{
    public interface ICandleRepository
    {
        Task SaveCandleAsync(string tradingPair, List<CandleDto> candlesDto, long? lastScanId = null);

        Task<Dictionary<long, List<CandleDto>>> GetCandlesAsync(string tradingPair);

        long GetLatestScanId();
    }
}

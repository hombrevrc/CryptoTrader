﻿using System;
using System.Threading.Tasks;
using CryptoTrading.Logic.Indicators.Interfaces;
using CryptoTrading.Logic.Models;
using CryptoTrading.Logic.Options;
using CryptoTrading.Logic.Strategies.Interfaces;
using Microsoft.Extensions.Options;

namespace CryptoTrading.Logic.Strategies
{
    public class LtcMacdStrategy : IStrategy
    {
        private readonly IIndicator _shortEmaIndicator;
        private readonly IIndicator _longEmaIndicator;
        private readonly IIndicator _signalEmaIndicator;

        private TrendDirection _lastTrend = TrendDirection.Short;
        private decimal _lastBuyPrice;
        private decimal _maxOrMinMacd;
        private decimal? _lastMacd;
        private readonly MacdStrategyOptions _options;
        private bool _stopTrading;

        public LtcMacdStrategy(IOptions<MacdStrategyOptions> options, IIndicatorFactory indicatorFactory)
        {
            _options = options.Value;
            _shortEmaIndicator = indicatorFactory.GetEmaIndicator(_options.ShortWeight);
            _longEmaIndicator = indicatorFactory.GetEmaIndicator(_options.LongWeight);
            _signalEmaIndicator = indicatorFactory.GetEmaIndicator(_options.Signal);
        }

        public int DelayInCandlePeriod => 1;

        public async Task<TrendDirection> CheckTrendAsync(string tradingPair, CandleModel currentCandle)
        {
            var shortEmaValue = _shortEmaIndicator.GetIndicatorValue(currentCandle).IndicatorValue;
            var longEmaValue = _longEmaIndicator.GetIndicatorValue(currentCandle).IndicatorValue;
            var emaDiffValue = shortEmaValue - longEmaValue;
            var signalEmaValue = Math.Round(_signalEmaIndicator.GetIndicatorValue(emaDiffValue).IndicatorValue, 4);
            var macdValue = Math.Round(emaDiffValue - signalEmaValue, 4);

            Console.WriteLine($"DateTs: {currentCandle.StartDateTime:s}; " +
                              $"MACD: {macdValue}; " +
                              $"PeekMACD: {_maxOrMinMacd}; " +
                              $"Close price: {currentCandle.ClosePrice}; ");

            if (!_lastMacd.HasValue)
            {
                _lastMacd = macdValue;
                return await Task.FromResult(TrendDirection.None);
            }

            if (_lastTrend == TrendDirection.Short)
            {
                if (macdValue > 0 && _stopTrading)
                {
                    _stopTrading = false;
                }

                if (macdValue < 0 && macdValue < _lastMacd)
                {
                    _maxOrMinMacd = macdValue;
                }

                _lastMacd = macdValue;
                if (_stopTrading == false && macdValue < _options.BuyThreshold)
                {
                    _lastTrend = TrendDirection.Long;
                    _maxOrMinMacd = 0;
                    _lastBuyPrice = currentCandle.ClosePrice;
                }
                else
                {
                    return await Task.FromResult(TrendDirection.None);
                }
            }
            else if (_lastTrend == TrendDirection.Long)
            {
                if (macdValue > 0 && macdValue > _lastMacd)
                {
                    _maxOrMinMacd = macdValue;
                }

                _lastMacd = macdValue;
                if (_maxOrMinMacd > _options.SellThreshold 
                    && currentCandle.ClosePrice > _lastBuyPrice * (decimal)1.01)
                {
                    _lastTrend = TrendDirection.Short;
                    _maxOrMinMacd = 0;
                    _stopTrading = true;
                }
                else
                {
                    return await Task.FromResult(TrendDirection.None);
                }
            }

            return await Task.FromResult(_lastTrend);
        }
    }
}

﻿using CommandLine;
using CryptoTrading.Logic.Models;
using CryptoTrading.Logic.Providers.Models;

namespace TradingTester
{
    public class ArgumentOptions
    {
        [Option('e', "exchange", Required = true, HelpText = "Exchange. Value: Kraken, Poloniex")]
        public ExchangeEnum Exchange { get; set; }

        [Option('t', "tradingpair", Required = true, HelpText = "Trading pair")]
        public string TradingPair { get; set; }

        [Option('b', "backtest", HelpText = "Backtesting imported data")]
        public bool EnableBacktest { get; set; }

        [Option('o', "ordertest", HelpText = "Order testing")]
        public bool EnableOrderTesting { get; set; }

        [Option('i', "import", HelpText = "Imported data")]
        public bool EnableImport { get; set; }

        [Option('h', "import-interval", HelpText = "Import interval in hour")]
        public int ImportIntervalInHour { get; set; }

        [Option('p', "candle-period", HelpText = "Candle period")]
        public CandlePeriod CandlePeriod { get; set; } = CandlePeriod.OneMinute;

        [Option('s', "strategy", HelpText = "Trading strategy")]
        public string Strategy { get; set; } = "Custom";
    }
}

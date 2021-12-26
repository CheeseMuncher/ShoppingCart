namespace Finance.Domain.Prices;

public static class PriceSetExtensions
{
    public static PriceSet AddDates(this PriceSet set, DateTime[] dates)
    {
        foreach(var date in dates.Where(date => !set.Prices.ContainsKey(date)))
            set.Prices[date] = new HashSet<StockPrice>();

        return set;
    }

    public static PriceSet AddPrices(this PriceSet set, PriceSet newSet, string stock)
    {
            foreach (var date in newSet.Prices.Keys)
            {
                if(!set.Prices.ContainsKey(date))
                    set.Prices[date] = new HashSet<StockPrice>();
                
                if (newSet.Prices[date].Any(sp => sp.Stock == stock))
                    set.Prices[date].Add(newSet.Prices[date].Single(sp => sp.Stock == stock));
            }
            return set;
    }

    public static PriceSet Interpolate(this PriceSet prices, string[] stocks)
    {
            foreach(var stock in stocks)
            {
                var stockPrices = prices.Prices
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => new PricePoint 
                        { 
                            Date = kvp.Key,
                            Price = kvp.Value
                                .SingleOrDefault(v => v.Stock == stock)
                                ?.Price ?? 0                                    
                        })
                    .ToArray();
                var newPrices = new HashSet<PricePoint>();
                for (int i = 1; i < stockPrices.Length - 1; i++)
                {
                    if(stockPrices[i].Price == 0
                        && stockPrices.Any(sp => sp.Date < stockPrices[i].Date && sp.Price > 0)
                        && stockPrices.Any(sp => sp.Date > stockPrices[i].Date && sp.Price > 0))
                    {
                        var before = stockPrices.Last(sp => sp.Date < stockPrices[i].Date && sp.Price > 0);
                        var after = stockPrices.First(sp => sp.Date > stockPrices[i].Date && sp.Price > 0);
                        var gap = stockPrices.Count(sp => before.Date < sp.Date && sp.Date < after.Date);
                        var newPrice = before.Price + (after.Price - before.Price) / (1 + gap) * (i - Array.FindIndex(stockPrices, 0, stockPrices.Length, sp => sp == before));
                        newPrices.Add(new PricePoint { Date = stockPrices[i].Date, Price = newPrice });
                    }
                }
                foreach(var item in newPrices)
                    prices.Prices[item.Date].Add(new StockPrice { Stock = stock, Price = item.Price });
            }
            return prices;

    }
}
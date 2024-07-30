using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockSmart.Application.Dto;
using StockSmart.Application.Services;
using StockSmart.Domain.Exceptions;

namespace StockSmart.Infrastructure.Services;

public class DiscountService(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory, IConfiguration configuration) : IDiscountService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger _logger = loggerFactory.CreateLogger<DiscountService>();
    private readonly string _discountServiceUrl =
        configuration.GetValue<string>("Services:Discount:BaseUrl") ?? throw new SettingsNotFoundException("Discount service setting not found", null);

    public async Task<IEnumerable<DiscountDto>> GetDiscountByProducts(IEnumerable<string> productCodes)
    {
        using var client = _httpClientFactory.CreateClient();

        try
        {
            _logger.LogInformation($"Calling discount service for products ({string.Join(", ", productCodes)})");
            var response = await client.GetStringAsync(_discountServiceUrl);

            var discounts = JsonConvert.DeserializeObject<IEnumerable<DiscountDto>>(response);

            var productDiscounts = discounts.Where(x => productCodes.Contains(x.ProductCode)).ToList();
            _logger.LogInformation($"Product discounts found ({string.Join(", ", productDiscounts.Select(x => new { x.ProductCode, x.Value }))}");

            return productDiscounts;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting product discounts", ex);
        }

        return Enumerable.Empty<DiscountDto>();
    }
}

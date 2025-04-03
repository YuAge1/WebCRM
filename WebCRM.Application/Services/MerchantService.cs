using WebCRM.Application.Abstractions;
using WebCRM.Application.Models.Merchants;
using WebCRM.Domain;
using WebCRM.Domain.Entities;

namespace WebCRM.Application.Services;

public class MerchantService(OrdersDbContext context) : IMerchantService
{
    public async Task<MerchantDto> Create(MerchantDto merchant)
    {
        var entity = new MerchantEntity
        {
            Name = merchant.Name,
            Phone = merchant.Phone,
            WebSite = merchant.WebSite
        };
        
        var result = await context.Merchants.AddAsync(entity);
        var resultEntity = result.Entity;
        await context.SaveChangesAsync();

        return new MerchantDto
        {
            Id = resultEntity.Id,
            Name = resultEntity.Name,
            Phone = resultEntity.Phone,
            WebSite = resultEntity.WebSite
        };
    }
}
using WebCRM.Application.Models.Merchants;

namespace WebCRM.Application.Abstractions;

public interface IMerchantService
{
    Task<MerchantDto> Create(MerchantDto merchant);
}
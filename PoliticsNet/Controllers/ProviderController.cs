using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PoliticsNet.Data;
using AutoMapper;
using PoliticsNet.DTO;
using System.Collections.Generic;

namespace PoliticsNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProviderRespository _provider;
        public ProviderController(IMapper mapper, IProviderRespository provider)
        {
            _provider = provider;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProviders()
        {
            var provider = await _provider.GetProviderList();
            var providersList = _mapper.Map<IEnumerable<ProviderToReturn>>(provider);
            return Ok(providersList);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetProvider(string name)
        {
            var provider = await _provider.GetProvider(name);
            var providersList = _mapper.Map<ProviderToReturnProvider>(provider);
            return Ok(providersList);
        }
    }
}
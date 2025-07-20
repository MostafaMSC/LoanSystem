using Loan_System.Modules;
using Loan_System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loan_System.Services.Interface;

[ApiController]
[Route("[controller]")]
public class ContractsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContractsController> _logger;
    private readonly IConract _contractService;

    public ContractsController(ApplicationDbContext context, ILogger<ContractsController> logger, IConract ContractService)
    {
        _context = context;
        _logger = logger;
        _contractService = ContractService;
    }

    [Authorize]
    [HttpPost("AddContract")]
    public async Task<IActionResult> AddNewContract([FromBody] ContractCreateDto contractDto)
    {
        _logger.LogInformation("Received contract creation request for LoanId {LoanId}", contractDto.LoanId);
        var result = await _contractService.AddNewContract(contractDto);
        if (result == "Contract added successfully.")
            return Ok(result);
        return StatusCode(500, result);
    }

    [Authorize]
    [HttpGet("GetAllContracts")]
    public async Task<IActionResult> GetAllContracts(int page = 1, int pageSize = 10)
    {
        var result = await _contractService.GetAllContracts(page, pageSize);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("GetAllContractsByLoan")]
    public async Task<IActionResult> GetAllContractsByLoanId(int LoanId, int page = 1, int pageSize = 10)
    {
        var contracts = await _contractService.GetAllContractsByLoanId(LoanId, page, pageSize);
        return Ok(contracts);
    }

    [Authorize]
    [HttpGet("GetContract/{id}")]
    public async Task<IActionResult> GetContractById(int id)
    {
        var contract = await _contractService.GetContractById(id);
        if (contract == null)
            return NotFound($"Contract with ID {id} not found.");
        return Ok(contract);
    }

    [Authorize]
    [HttpPut("UpdateContract/{id}")]
    public async Task<IActionResult> UpdateContract(int id, [FromBody] ContractCreateDto updatedContract)
    {
        var result = await _contractService.UpdateContract(id, updatedContract);
        if (result.Contains("not found"))
            return NotFound(result);
        return Ok(result);
    }
    [Authorize]
    [HttpPut("AddRevenue/{id}")]

    public async Task<IActionResult> AddRevenue(int id, [FromBody] ContractCreateDto updatedContract)
    {
        var result = await _contractService.UpdateContract(id, updatedContract);
        if (result.Contains("not found"))
            return NotFound(result);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("DeleteContract/{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        var result = await _contractService.DeleteContract(id);
        if (result.Contains("not found"))
            return NotFound(result);
        return Ok(result);
    }


[Consumes("multipart/form-data")]
[HttpPost("UploadContractDocuments")]
public async Task<IActionResult> UploadContractDocuments([FromForm] ContractDocumentUploadDto dto)
{
    var result = await _contractService.UploadContractDocuments(dto);
    return Ok(result);
}

        [Authorize]
[HttpGet("GetDocumentsByContractId/{contractId}")]
public async Task<IActionResult> GetDocumentsByContractId(int contractId)
{
    var documents = await _contractService.GetDocumentsByContractId(contractId);

    var result = documents.Select(d => new
    {
        d.Id,
        d.FileName,
        d.FilePath
    }).ToList();

    return Ok(new { documents = result });
}




}

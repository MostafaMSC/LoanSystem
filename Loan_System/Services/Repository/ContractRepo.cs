using Loan_System.Data;
using Loan_System.Modules;
using Loan_System.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ContractService : IConract
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ContractService> _logger;

    public ContractService(ApplicationDbContext context, ILogger<ContractService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<string> AddNewContract(ContractCreateDto dto)
    {
        try
        {
            var contract = new ContractsModule
            {
                ContractNumber = dto.ContractNumber,
                ContractName = dto.ContractName,
                ContractType = dto.ContractType,
                CompanyName = dto.CompanyName,
                Status = dto.Status,
                LoanId = dto.LoanId,
                ContractSigningDate = dto.ContractSigningDate,
                StartDate = dto.StartDate,
                CompleteDate = dto.CompleteDate,
                DurationInDays = dto.DurationInDays,
                AddedDays = dto.AddedDays,
                ContractAmount = dto.ContractAmount,
                CostPlanMins = dto.CostPlanMins,
                CostChange = dto.CostChange,
                CostAfterChange = dto.CostAfterChange,
                CostToNatiBank = dto.CostToNatiBank,
                TotalCostPaid = dto.TotalCostPaid,
                OperationLoanCost = dto.OperationLoanCost,
                CashPaid = dto.CashPaid,
                TaxesAndBlockedmoney = dto.TaxesAndBlockedmoney,
                PrivateMoneyPaid = dto.PrivateMoneyPaid,
                Notes = dto.Notes,
            };
            contract.CalculateCostAfterChange();

            await _context.ContractTable.AddAsync(contract);
            await _context.SaveChangesAsync();
            return "Contract added successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding contract");
            return "An error occurred while adding the contract.";
        }
    }

    public async Task<object> GetAllContracts(int page, int pageSize)
    {
        var totalContracts = await _context.ContractTable.CountAsync();
        var contracts = await _context.ContractTable
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new { totalCount = totalContracts, contracts };
    }

    public async Task<List<ContractsModule>> GetAllContractsByLoanId(int loanId, int page, int pageSize)
    {
        return await _context.ContractTable
            .Where(a => a.LoanId == loanId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<ContractsModule?> GetContractById(int id)
    {
        return await _context.ContractTable.FindAsync(id);
    }

    public async Task<string> UpdateContract(int id, ContractCreateDto updatedContract)
    {
        var contract = await _context.ContractTable.FindAsync(id);
        if (contract == null) return $"Contract with ID {id} not found.";
        contract.ContractType = updatedContract.ContractType;
        contract.ContractName = updatedContract.ContractName;
        contract.Notes = updatedContract.Notes;
        contract.AddedDays = updatedContract.AddedDays;
        contract.CompanyName = updatedContract.CompanyName;
        contract.CashPaid = updatedContract.CashPaid;
        contract.LoanId = updatedContract.LoanId;
        contract.CompleteDate = updatedContract.CompleteDate;
        contract.ContractAmount = updatedContract.ContractAmount;
        contract.ContractNumber = updatedContract.ContractNumber;
        contract.ContractSigningDate = updatedContract.ContractSigningDate;
        contract.CostAfterChange = updatedContract.CostAfterChange;
        contract.CostChange = updatedContract.CostChange;
        contract.CostPlanMins = updatedContract.CostPlanMins;
        contract.CostToNatiBank = updatedContract.CostToNatiBank;
        contract.DurationInDays = updatedContract.DurationInDays;
        contract.StartDate = updatedContract.StartDate;
        contract.TotalCostPaid = updatedContract.TotalCostPaid;
        contract.OperationLoanCost = updatedContract.OperationLoanCost;
        contract.TaxesAndBlockedmoney = updatedContract.TaxesAndBlockedmoney;
        contract.PrivateMoneyPaid = updatedContract.PrivateMoneyPaid;
        contract.Notes = updatedContract.Notes;
        contract.Status = updatedContract.Status;
        contract.CalculateCostAfterChange();
        await _context.SaveChangesAsync();
        return "Contract updated successfully.";
    }
    public async Task<string> UploadContractDocuments(ContractDocumentUploadDto dto)
    {
        var contract = await _context.ContractTable
            .Include(c => c.Documents)
            .FirstOrDefaultAsync(c => c.Id == dto.ContractId);

        if (contract == null)
            return $"Contract with ID {dto.ContractId} not found.";

        if (dto.Files == null || !dto.Files.Any())
            return "No files provided.";

        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "contracts");
        Directory.CreateDirectory(uploadDir);

        foreach (var file in dto.Files)
        {
            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadDir, uniqueName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            contract.Documents.Add(new ContractDocument
            {
                FileName = file.FileName,
                FilePath = $"/uploads/contracts/{uniqueName}"
            });
        }

        await _context.SaveChangesAsync();
        return "Documents uploaded successfully.";
    }

    public async Task<string> DeleteContract(int id)
    {
        var contract = await _context.ContractTable.FindAsync(id);
        if (contract == null) return $"Contract with ID {id} not found.";

        _context.ContractTable.Remove(contract);
        await _context.SaveChangesAsync();
        return "Contract deleted successfully.";
    }
    public async Task<List<ContractDocument>> GetDocumentsByContractId(int contractId)
{
    return await _context.ContractDocuments
        .Where(d => d.ContractId == contractId)
        .ToListAsync();
}


    
}

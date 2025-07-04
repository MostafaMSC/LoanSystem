using Loan_System.Modules;
using Microsoft.EntityFrameworkCore;

public class ContractCreateDto
{
    public string ContractNumber { get; set; } = string.Empty;
    public string ContractName { get; set; } = string.Empty;
    public ContractType ContractType { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? LoanId { get; set; }

    // Dates
    public DateTime? ContractSigningDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompleteDate { get; set; }

    // Duration
    public int? DurationInDays { get; set; }
    public int? AddedDays { get; set; }

    // Financials
    [Precision(18, 2)]
    public decimal? ContractAmount { get; set; }
    [Precision(18, 2)]
    public decimal? CostChange { get; set; }
    [Precision(18, 2)]

    public decimal? CostPlanMins { get; set; }
    [Precision(18, 2)]

    public decimal? CostAfterChange { get; set; }
    [Precision(18, 2)]
    public decimal? CostToNatiBank { get; set; }
    [Precision(18, 2)]
    public decimal? TotalCostPaid { get; set; }
    [Precision(18, 2)]
    public decimal? OperationLoanCost { get; set; }
    [Precision(18, 2)]
    public decimal? CashPaid { get; set; }
    [Precision(18, 2)]
    public decimal? TaxesAndBlockedmoney { get; set; }
    [Precision(18, 2)]
    public decimal? PrivateMoneyPaid { get; set; }

    // Additional Info
    public string Notes { get; set; } = string.Empty;
    public void CalculateCostAfterChange()
    {
        CostAfterChange = (ContractAmount ?? 0) + (CostChange ?? 0);
    }
        public List<IFormFile>? Documents { get; set; }

}

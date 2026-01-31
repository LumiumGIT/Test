namespace Lumium.Contracts;

public record CompanyInfo(string Identifier, string Name);

public record ValidateCompanyRequest(string CompanyName);

public record ValidateCompanyResponse(string Identifier, string Name);
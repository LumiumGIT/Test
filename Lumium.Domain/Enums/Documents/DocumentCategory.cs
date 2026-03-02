using System.ComponentModel;

namespace Domain.Enums.Documents;

public enum DocumentCategory
{
    // Official 
    [Description("Izvodi")]
    BankStatements = 0,
    
    [Description("Izlazne fakture")]
    OutgoingInvoices = 1,
    
    [Description("Ulazne fakture")]
    IncomingInvoices = 2,
    
    [Description("Ugovori")]
    Contracts = 3,
    
    [Description("Arhivska knjiga")]
    ArchiveBook = 4,
    
    [Description("Poreska rešenja")]
    TaxDecisions = 5,
    
    // Supporting 
    [Description("Prateća dokumentacija")]
    SupportingDocuments = 10,
    
    [Description("Ovlašćenja")]
    Authorizations = 11,
    
    [Description("AML dokumenta")]
    AmlDocuments = 12,
    
    [Description("Dokumenta klijenta")]
    ClientDocuments = 13,
    
    [Description("Ostalo")]
    Other = 99
}
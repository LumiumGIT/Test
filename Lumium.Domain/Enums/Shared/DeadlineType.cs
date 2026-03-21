using System.ComponentModel;

namespace Domain.Enums.Shared;

public enum DeadlineType
{
    [Description("Sertifikat")] Certificate = 0,

    [Description("Ugovor")] Contract = 1
}
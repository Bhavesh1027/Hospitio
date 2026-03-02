namespace HospitioApi.Shared.Enums;

public enum MusementOrderStatusEnum
{
    COMPLETED = 1 ,
    PENDING = 2 ,
    CANCELLED = 3
}

public enum MusementPaymentStatusEnum
{
    COMPLETED = 1,
    PENDING = 2,
    CANCELLED = 3
}

public enum MusementPaymentPlatFormEnum
{
    Musement = 1
}

public enum MusementPaymentMethodEnum
{
    STRIPE =1,
    ADYEN = 2
}

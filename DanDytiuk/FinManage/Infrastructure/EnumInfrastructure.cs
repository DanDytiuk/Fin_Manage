using System;
using System.Collections.Generic;

namespace FinManage.Infrastructure
{
    public class EnumInfrastructure
    {
        public enum TypesOfCurrency
        {
            USD,
            EUR,
            UAH,
            GBP,
            GEL,
            DKK,
            KZT,
            KAD,
            CNY,
            MDL,
            NOK,
            PLN,
            RON,
            BTC,
            TJS,
            TMT,
            TRY,
            UZS,
            CHF,
            SEK,
            JPY
        }

        public enum Themes
        {
            Light,
            Dark
        }

        public enum TypeOperation
        {
            Unknown,
            Income,
            Expenses
        }

        public enum Category
        {
            Food,
            Store,
            Entertainment,
            OnlineStore,
            Games,
            PublicUtilities,
            PhoneTopUp,
            CardTopUp,
            InternetAndTV,
            Security,
            Insurance,
            ETickets,
            Education,
            Transport,
            Charity,
            Commission,
            ProjectSupport,
            Other
        }
        
        public enum Months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        public List<int> Years = new List<int> { 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2031, 2032, 2033, 2034, 2035 };
    }
}


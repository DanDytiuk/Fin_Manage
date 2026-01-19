using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            RUB,
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
            ProjectSupport
        }
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string> 
        { 
            "Food",
            "Store",
            "Entertainment",
            "Online store",
            "Games",
            "Public Utilities",
            "Phone Top Up",
            "Card Top Up",
            "Internet And TV",
            "Security",
            "Insurance",
            "E Tickets",
            "Education",
            "Transport",
            "Charity",
            "Commission",
            "ProjectSupport"
        };
    }
}


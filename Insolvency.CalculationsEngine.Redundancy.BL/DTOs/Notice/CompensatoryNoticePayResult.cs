using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.BL.DTOs.Notice
{
    public class CompensatoryNoticePayResult
    {
        public CompensatoryNoticePayResult()
        { }

        public int WeekNumber { get; set; }

		public decimal EmployerEntitlement { get; set; }

        public decimal BenefitsDeducted { get; set; }

        public decimal NewEmploymentDeducted { get; set; }

        public decimal WageIncreaseDeducted { get; set; }

        public decimal NotionalBenefitDeducted { get; set; }

        public decimal GrossEntitlement { get; set; }
		            
        public bool IsTaxable { get; set; }

        public decimal NotionalTaxDeducted { get; set; }

        public decimal TaxDeducted { get; set; }

        public decimal NIDeducted { get; set; }

        public decimal NetEntitlement { get; set; }
        public decimal PreferentialClaim { get; set; }
        public decimal NonPreferentialClaim { get; set; }

        public bool IsSelected { get; set; }

    }
}



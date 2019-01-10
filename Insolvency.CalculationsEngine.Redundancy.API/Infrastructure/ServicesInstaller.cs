using Insolvency.CalculationsEngine.Redundancy.BL.Services.Implementations;
using Insolvency.CalculationsEngine.Redundancy.BL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Insolvency.CalculationsEngine.Redundancy.API.Infrastructure
{
    internal static class ServicesInstaller
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAPPACalculationService, APPACalculationService>();
            services.AddTransient<IArrearsOfPayCalculationsService, ArrearsOfPayCalculationsService>();
            services.AddTransient<IBasicAwardCalculationService, BasicAwardCalculationService>();
            services.AddTransient<ICompensatoryNoticePayCalculationService, CompensatoryNoticePayCalculationService>();
            services.AddTransient<IHolidayCalculationService, HolidayCalculationService>();
            services.AddTransient<IHolidayTakenNotPaidCalculationService, HolidayTakenNotPaidCalculationService>();
            services.AddTransient<INoticeWorkedNotPaidCalculationService, NoticeWorkedNotPaidCalculationService>();
            services.AddTransient<IProjectedNoticeDateCalculationService, ProjectedNoticeDateCalculationService>();
            services.AddTransient<IProtectiveAwardCalculationService, ProtectiveAwardCalculationService>();
            services.AddTransient<IRedundanyPayCalculationsService, RedundancyPaymentCalculationsService>();
            services.AddTransient<IRefundOfNotionalTaxCalculationService, RefundOfNotionalTaxCalculationService>();
            services.AddTransient<IHolidayPayAccruedCalculationService, HolidayPayAccruedCalculationService>();
            services.AddTransient<IApportionmentCalculationService, ApportionmentCalculationService>();
            services.AddTransient<INoticeCalculationService, NoticeCalculationService>();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using QbSync.QbXml.Objects;
using System.Threading.Tasks;
using WebApplication.Sample.Db;

namespace WebApplication.Sample.Extensions
{
    public static class QbSettingExtensions
    {
        public static async Task SaveIfNewerAsync(this ApplicationDbContext applicationDbContext, string setting, DATETIMETYPE? moment)
        {
            if (moment != null)
            {
                var savedSetting = await applicationDbContext.QbSettings.FirstOrDefaultAsync(m => m.Name == setting);
                var existingDateTimeType = DATETIMETYPE.ParseOrDefault(savedSetting?.Value, DATETIMETYPE.MinValue);
                if (moment > existingDateTimeType)
                {
                    if (savedSetting == null)
                    {
                        savedSetting = new QbSetting
                        {
                            Name = setting
                        };
                        applicationDbContext.QbSettings.Add(savedSetting);
                    }

                    savedSetting.Value = moment.ToString();
                    await applicationDbContext.SaveChangesAsync();
                }
            }
        }
    }
}

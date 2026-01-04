using FlowFeedback.Device.Models;
using SQLite;

namespace FlowFeedback.Device.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    async Task Init()
    {
        if (_database is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "flowfeedback.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        await _database.CreateTableAsync<VotoLocal>();
    }

    public async Task SalvarVotoAsync(VotoLocal voto)
    {
        await Init();
        await _database.InsertAsync(voto);
    }

    public async Task<List<VotoLocal>> ObterVotosPendentesAsync()
    {
        await Init();
        return await _database.Table<VotoLocal>().Where(v => !v.Sincronizado).ToListAsync();
    }

    public async Task MarcarComoSincronizadoAsync(IEnumerable<int> ids)
    {
        await Init();
        foreach (var id in ids)
        {
            var voto = await _database.Table<VotoLocal>().FirstOrDefaultAsync(v => v.Id == id);
            if (voto != null)
            {
                voto.Sincronizado = true;
                await _database.UpdateAsync(voto);
            }
        }
    }
}
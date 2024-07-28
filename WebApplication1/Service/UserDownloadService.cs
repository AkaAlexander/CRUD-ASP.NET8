using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using WebApplication1.Service;

public class UserDownloadService
{
    private readonly IApiService _apiService;
    private readonly DbptContext _context;
    private readonly ILogger<UserDownloadService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public UserDownloadService(ILogger<UserDownloadService> logger, DbptContext context, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task FetchAndUpdateUsersAsync()
    {
        _logger.LogInformation("Starting fetch and update of users at {Time}", DateTimeOffset.Now);

        var users = await FetchUsersFromApi();

        foreach (var user in users)
        {
            var existingUser = await _context.Usuarios
                .Include(u => u.Direccion)
                .ThenInclude(d => d.Geo)
                .Include(u => u.Compania)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser != null)
            {
                _logger.LogInformation("Updating user with ID {UserId}", user.Id);
                // Actualizar el usuario existente
                UpdateExistingUser(existingUser, user);
            }
            else
            {
                _logger.LogInformation("Inserting new user with ID {UserId}", user.Id);
                await _context.Usuarios.AddAsync(user);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Finished fetch and update of users at {Time}", DateTimeOffset.Now);
    }

    private async Task<List<Usuario>> FetchUsersFromApi()
    {
        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);

        return users ?? throw new Exception("Los datos no son validos");
    }

    private void UpdateExistingUser(Usuario existingUser, Usuario newUser)
    {
        existingUser.Name = newUser.Name;
        existingUser.Username = newUser.Username;
        existingUser.Email = newUser.Email;
        existingUser.Phone = newUser.Phone;
        existingUser.Website = newUser.Website;

        if (existingUser.Direccion != null && newUser.Direccion != null)
        {
            existingUser.Direccion.Street = newUser.Direccion.Street;
            existingUser.Direccion.Suite = newUser.Direccion.Suite;
            existingUser.Direccion.City = newUser.Direccion.City;
            existingUser.Direccion.Zipcode = newUser.Direccion.Zipcode;

            if (existingUser.Direccion.Geo != null && newUser.Direccion.Geo != null)
            {
                existingUser.Direccion.Geo.Lat = newUser.Direccion.Geo.Lat;
                existingUser.Direccion.Geo.Lng = newUser.Direccion.Geo.Lng;
            }
        }

        if (existingUser.Compania != null && newUser.Compania != null)
        {
            existingUser.Compania.Name = newUser.Compania.Name;
            existingUser.Compania.CatchPhrase = newUser.Compania.CatchPhrase;
            existingUser.Compania.Bs = newUser.Compania.Bs;
        }

        _context.Usuarios.Update(existingUser);
    }
}


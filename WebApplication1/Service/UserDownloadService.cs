using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

public class UserDownloadService
{
    private readonly DbptContext _context;
    private readonly ILogger<UserDownloadService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    /// <param name="httpClientFactory"></param>
    public UserDownloadService(ILogger<UserDownloadService> logger, DbptContext context, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Metodo que comienza la lectura y actualizacion de usuarios
    /// </summary>
    /// <returns></returns>
    public async Task FetchAndUpdateUsersAsync()
    {
        _logger.LogInformation("Iniciando la descarga y actualización de usuarios a las {Time}", DateTimeOffset.Now);

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
                _logger.LogInformation("Actualizando usuario con ID {UserId}", user.Id);
                UpdateExistingUser(existingUser, user);
            }
            else
            {
                _logger.LogInformation("Insertando usuario con ID {UserId}", user.Id);
                await _context.Usuarios.AddAsync(user);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Finalizada actualización y descarga de los usuarios a las {Time}", DateTimeOffset.Now);
    }

    /// <summary>
    /// Metodo que realiza una llamada a la api para obtener los usuarios
    /// </summary>
    /// <returns>Listado de usuarios</returns>
    /// <exception cref="Exception"></exception>
    private async Task<List<Usuario>> FetchUsersFromApi()
    {
        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/users");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var users = JsonConvert.DeserializeObject<List<Usuario>>(responseBody);

        return users ?? throw new Exception("Los datos no son validos");
    }

    /// <summary>
    /// Metodo que actualiza los usaurios existentes
    /// </summary>
    /// <param name="existingUser"></param>
    /// <param name="newUser"></param>
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


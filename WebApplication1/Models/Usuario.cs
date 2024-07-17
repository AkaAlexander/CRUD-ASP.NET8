using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApplication1.Models;

public partial class Usuario
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; } = null!;

	[JsonProperty("username")]
	public string Username { get; set; } = null!;

	[JsonProperty("email")]
	public string Email { get; set; } = null!;

	[JsonProperty("phone")]
	public string? Phone { get; set; }

	[JsonProperty("website")]
	public string? Website { get; set; }

	[JsonProperty("company")]
    public virtual Compania? Compania { get; set; }

	[JsonProperty("address")]
	public virtual Direccion? Direccion { get; set; }
}

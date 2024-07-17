using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApplication1.Models;

public partial class Direccion
{
	[JsonIgnore]
	public int UserId { get; set; }

    [JsonProperty("street")]
    public string? Street { get; set; }

	[JsonProperty("suite")]
	public string? Suite { get; set; }

	[JsonProperty("city")]
	public string? City { get; set; }

	[JsonProperty("zipcode")]
	public string? Zipcode { get; set; }

	[JsonProperty("geo")]
	public virtual Geo? Geo { get; set; }

	[JsonIgnore]
	public virtual Usuario User { get; set; } = null!;
}

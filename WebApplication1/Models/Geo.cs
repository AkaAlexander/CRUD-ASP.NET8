using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApplication1.Models;

public partial class Geo
{
	[JsonIgnore]
	public int DireccionId { get; set; }

	[JsonProperty("lat")]
	public decimal? Lat { get; set; }

	[JsonProperty("lng")]
	public decimal? Lng { get; set; }

	[JsonIgnore]
	public virtual Direccion Direccion { get; set; } = null!;
}

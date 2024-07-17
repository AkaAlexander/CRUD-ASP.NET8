using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApplication1.Models;

public partial class Compania
{
	[JsonIgnore]
	public int UserId { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }

	[JsonProperty("catchPhrase")]
	public string? CatchPhrase { get; set; }

	[JsonProperty("bs")]
	public string? Bs { get; set; }

	[JsonIgnore]
	public virtual Usuario User { get; set; } = null!;
}
